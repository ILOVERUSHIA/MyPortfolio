using UnityEngine;
using UnityEditor;

public class RockManGameSetup
{
    [MenuItem("RockMan/Setup Game Scene")]
    public static void SetupGameScene()
    {
        // Clear existing objects if needed
        if (EditorUtility.DisplayDialog("Setup RockMan Scene",
            "This will create all necessary GameObjects for the RockMan game.\n\nContinue?",
            "Yes", "No"))
        {
            CreateLayers();
            CreatePlayer();
            CreateGround();
            CreateBulletPrefab();
            
            // Assign bullet prefab to PlayerShooter after prefab is created
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerShooter playerShooter = player.GetComponent<PlayerShooter>();
                if (playerShooter != null)
                {
                    GameObject bulletPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Bullet.prefab");
                    if (bulletPrefab != null)
                    {
                        playerShooter.bulletPrefab = bulletPrefab;
                        playerShooter.shootPoint = player.transform;
                        EditorUtility.SetDirty(playerShooter);
                    }
                }
            }
            
            CreateMainCamera();
            EditorUtility.DisplayDialog("Success", "Scene setup complete!\n\nNext steps:\n1. Assign sprites to Player and Ground (optional)\n2. Press Play to test the game!", "OK");
        }
    }
    
    private static void CreateLayers()
    {
        CreateLayer("Ground");
        CreateLayer("Player");
        CreateLayer("Enemy");
        CreateLayer("Bullet");
    }
    
    private static void CreateLayer(string layerName)
    {
        try
        {
            var assets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if (assets == null || assets.Length == 0)
            {
                Debug.LogWarning($"Could not access TagManager. Layer '{layerName}' must be created manually in Project Settings → Tags and Layers");
                return;
            }
            
            SerializedObject tagManager = new SerializedObject(assets[0]);
            SerializedProperty layers = tagManager.FindProperty("layers");
            
            if (layers == null)
            {
                Debug.LogWarning("Could not find layers property in TagManager");
                return;
            }
            
            bool layerExists = false;
            for (int i = 0; i < layers.arraySize; i++)
            {
                SerializedProperty layerSP = layers.GetArrayElementAtIndex(i).FindPropertyRelative("name");
                if (layerSP != null && layerSP.stringValue == layerName)
                {
                    layerExists = true;
                    break;
                }
            }
            
            if (!layerExists)
            {
                for (int i = 8; i < layers.arraySize; i++)
                {
                    SerializedProperty layerSP = layers.GetArrayElementAtIndex(i).FindPropertyRelative("name");
                    if (layerSP != null && string.IsNullOrEmpty(layerSP.stringValue))
                    {
                        layerSP.stringValue = layerName;
                        tagManager.ApplyModifiedProperties();
                        Debug.Log($"Layer '{layerName}' created successfully");
                        break;
                    }
                }
            }
            else
            {
                Debug.Log($"Layer '{layerName}' already exists");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Error creating layer '{layerName}': {e.Message}\nPlease create it manually in Project Settings → Tags and Layers");
        }
    }
    
    private static int GetLayerOrDefault(string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer == -1)
        {
            Debug.LogWarning($"Layer '{layerName}' not found. Using Default layer (0) instead. Please manually create '{layerName}' layer in Project Settings.");
            return 0;
        }
        return layer;
    }
    
    private static Sprite CreateColoredSprite(Color color, int width = 32, int height = 32)
    {
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        
        Color[] colors = new Color[width * height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
        texture.SetPixels(colors);
        texture.Apply();
        
        // Pivot should be normalized (0-1), not pixel coordinates
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 100);
        return sprite;
    }
    
    private static void CreatePlayer()
    {
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.layer = GetLayerOrDefault("Player");
        
        // Add Sprite Renderer with visible sprite
        SpriteRenderer spriteRenderer = player.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateColoredSprite(new Color(0.2f, 0.8f, 1f), 32, 32); // Light blue
        spriteRenderer.color = Color.white;
        
        // Add Rigidbody2D
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 2f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        // Add Box Collider 2D
        BoxCollider2D collider = player.AddComponent<BoxCollider2D>();
        collider.isTrigger = false;
        // Adjust collider size to match sprite (32x32 at PPU 100)
        collider.size = new Vector2(0.32f, 0.32f);
        
        // Add PlayerController
        PlayerController playerController = player.AddComponent<PlayerController>();
        playerController.moveSpeed = 5f;
        playerController.jumpForce = 5f;
        playerController.slideSpeed = 8f;
        playerController.slideDuration = 0.5f;
        playerController.slideCooldown = 0.3f;
        playerController.groundCheckDistance = 0.1f;
        playerController.groundLayer = (1 << GetLayerOrDefault("Ground"));
        
        // Add PlayerShooter
        PlayerShooter playerShooter = player.AddComponent<PlayerShooter>();
        playerShooter.shootCooldown = 0.3f;
        playerShooter.bulletSpeed = 10f;
        playerShooter.shootPoint = player.transform;
        
        EditorUtility.SetDirty(playerController);
        EditorUtility.SetDirty(playerShooter);
        
        Selection.activeGameObject = player;
        Debug.Log("Player created successfully!");
    }
    
    private static void CreateGround()
    {
        GameObject ground = new GameObject("Ground");
        ground.transform.position = new Vector3(0, -5, 0);
        ground.transform.localScale = new Vector3(20, 1, 1);
        ground.layer = GetLayerOrDefault("Ground");
        
        // Add Sprite Renderer with visible sprite
        SpriteRenderer spriteRenderer = ground.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateColoredSprite(new Color(0.5f, 0.3f, 0.1f), 64, 32); // Brown
        spriteRenderer.color = Color.white;
        
        // Add Rigidbody2D (Static)
        Rigidbody2D rb = ground.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        
        // Add Box Collider 2D
        BoxCollider2D collider = ground.AddComponent<BoxCollider2D>();
        collider.isTrigger = false;
        // Adjust collider size to match sprite (64x32 at PPU 100)
        collider.size = new Vector2(0.64f, 0.32f);
        
        Debug.Log("Ground created successfully!");
    }
    
    private static void CreateBulletPrefab()
    {
        // Check if prefabs directory exists
        string prefabPath = "Assets/Prefabs";
        if (!AssetDatabase.IsValidFolder(prefabPath))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        
        GameObject bullet = new GameObject("Bullet");
        bullet.layer = GetLayerOrDefault("Bullet");
        bullet.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        
        // Add Sprite Renderer with visible sprite
        SpriteRenderer spriteRenderer = bullet.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = CreateColoredSprite(new Color(1f, 1f, 0f), 16, 16); // Yellow
        spriteRenderer.color = Color.white;
        
        // Add Rigidbody2D
        Rigidbody2D rb = bullet.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        // Add Circle Collider 2D
        CircleCollider2D collider = bullet.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        // Adjust collider radius to match sprite (16x16 at PPU 100)
        collider.radius = 0.08f;
        
        // Add Bullet script
        Bullet bulletScript = bullet.AddComponent<Bullet>();
        bulletScript.lifetime = 5f;
        bulletScript.speed = 10f;
        
        EditorUtility.SetDirty(bulletScript);
        
        // Save as prefab
        string prefabPrefixPath = "Assets/Prefabs/Bullet.prefab";
        PrefabUtility.SaveAsPrefabAsset(bullet, prefabPrefixPath);
        
        // Remove from scene after creating prefab
        Object.DestroyImmediate(bullet);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log("Bullet prefab created at " + prefabPrefixPath);
    }
    
    private static void CreateMainCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // Add CameraFollower script to existing Main Camera
            if (mainCamera.GetComponent<CameraFollower>() == null)
            {
                CameraFollower cameraFollower = mainCamera.gameObject.AddComponent<CameraFollower>();
                
                // Find player and assign
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    cameraFollower.player = player.transform;
                    EditorUtility.SetDirty(cameraFollower);
                }
                
                Debug.Log("CameraFollower added to Main Camera");
            }
        }
    }
}

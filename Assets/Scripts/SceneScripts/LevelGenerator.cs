using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Settings")]
    public int levelWidth = 100;
    public int levelHeight = 30;
    public int minCaveHeight = 5;
    public int maxCaveHeight = 10;
    
    [Header("Prefabs")]
    public GameObject[] floorPrefabs;       // Верхний слой земли (по которому ходит игрок)
    public GameObject[] innerWallPrefabs;   // Внутренний слой земли
    public GameObject[] ceilingPrefabs;     // Верх пещеры
    public GameObject[] platformPrefabs;   // Платформы
    public GameObject[] obstaclePrefabs;    // Препятствия
    public GameObject entrancePrefab;
    public GameObject exitPrefab;
    
    [Header("Generation Settings")]
    [Range(0.1f, 0.9f)] public float platformChance = 0.4f;
    [Range(0.1f, 0.9f)] public float obstacleChance = 0.2f;
    public float pathSmoothness = 2f;
    
    private int[,] levelMap;
    private List<Vector2Int> mainPath = new List<Vector2Int>();

    void Start()
    {
        GenerateLevel();
        BuildLevel();
    }

    void GenerateLevel()
    {
        levelMap = new int[levelWidth, levelHeight];
        mainPath.Clear();
        
        // Генерация основного пути через пещеру (Perlin noise для плавности)
        float noiseOffset = Random.Range(0f, 100f);
        int currentHeight = Random.Range(minCaveHeight, levelHeight - minCaveHeight);
        
        for (int x = 0; x < levelWidth; x++)
        {
            // Плавное изменение высоты с помощью шума Перлина
            float noise = Mathf.PerlinNoise(x / pathSmoothness + noiseOffset, 0);
            currentHeight = Mathf.RoundToInt(Mathf.Lerp(minCaveHeight, levelHeight - minCaveHeight, noise));
            currentHeight = Mathf.Clamp(currentHeight, minCaveHeight, levelHeight - minCaveHeight);
            
            // Определяем ширину туннеля в этой точке
            int caveHeight = Random.Range(minCaveHeight, maxCaveHeight);
            int pathStart = currentHeight - caveHeight / 2;
            pathStart = Mathf.Clamp(pathStart, 1, levelHeight - caveHeight - 1);
            
            // Заполняем карту
            for (int y = 0; y < levelHeight; y++)
            {
                if (y >= pathStart && y < pathStart + caveHeight)
                {
                    // Основной проход - оставляем пустым
                    levelMap[x, y] = 0;
                    
                    // Пол (платформы, по которым можно ходить)
                    if (y == pathStart)
                    {
                        levelMap[x, y] = 1; // 1 - пол
                        mainPath.Add(new Vector2Int(x, y));
                    }
                    
                    // Потолок
                    if (y == pathStart + caveHeight - 1)
                    {
                        levelMap[x, y] = 3; // 3 - потолок
                    }
                }
                else if (y < pathStart)
                {
                    levelMap[x, y] = 2; // 2 - земля под полом
                }
                else
                {
                    levelMap[x, y] = 4; // 4 - земля над потолком
                }
            }
            
            // Добавляем платформы для вертикального перемещения
            if (Random.value < platformChance && x > 10 && x < levelWidth - 10)
            {
                CreatePlatformColumn(x, pathStart, pathStart + caveHeight);
            }
            
            // Добавляем препятствия
            if (Random.value < obstacleChance && x > 10 && x < levelWidth - 10)
            {
                CreateObstacle(x, pathStart, pathStart + caveHeight);
            }
        }
        
        // Создаем вход и выход
        CreateEntranceAndExit();
        
        // Добавляем внутренние стены
        AddInnerWalls();
    }

    void CreatePlatformColumn(int x, int pathBottom, int pathTop)
    {
        int platformHeight = Random.Range(pathBottom + 2, pathTop - 2);
        int platformCount = Random.Range(1, 4);
        
        for (int i = 0; i < platformCount; i++)
        {
            int length = Random.Range(1, 4);
            for (int px = x; px < x + length && px < levelWidth; px++)
            {
                if (levelMap[px, platformHeight] == 0)
                {
                    levelMap[px, platformHeight] = 5; // 5 - платформа
                }
            }
            
            // Соединяем платформы "лестницами"
            if (i < platformCount - 1)
            {
                int nextHeight = Random.Range(pathBottom + 1, pathTop - 1);
                int direction = nextHeight > platformHeight ? 1 : -1;
                
                for (int y = platformHeight; y != nextHeight; y += direction)
                {
                    if (levelMap[x, y] == 0) levelMap[x, y] = 5;
                }
                
                platformHeight = nextHeight;
            }
        }
    }

    void CreateObstacle(int x, int pathBottom, int pathTop)
    {
        int obstacleY = Random.Range(pathBottom + 1, pathTop - 1);
        if (levelMap[x, obstacleY] == 0)
        {
            levelMap[x, obstacleY] = 6; // 6 - препятствие
        }
    }

    void AddInnerWalls()
    {
        for (int x = 1; x < levelWidth - 1; x++)
        {
            for (int y = 1; y < levelHeight - 1; y++)
            {
                // Добавляем внутренние стены рядом с основными элементами
                if (levelMap[x, y] == 1 || levelMap[x, y] == 3) // Пол или потолок
                {
                    // Стены под полом
                    if (levelMap[x, y] == 1 && levelMap[x, y + 1] == 0)
                    {
                        levelMap[x, y + 1] = 7; // 7 - внутренняя стена под потолком
                    }
                    
                    // Стены над потолком
                    if (levelMap[x, y] == 3 && levelMap[x, y - 1] == 0)
                    {
                        levelMap[x, y - 1] = 8; // 8 - внутренняя стена над полом
                    }
                }
            }
        }
    }

    void CreateEntranceAndExit()
    {
        // Вход (первые 5 блоков)
        int entranceY = mainPath[5].y;
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                if (y >= entranceY - 2 && y <= entranceY + 2)
                {
                    levelMap[x, y] = 0;
                    if (x == 0 && y == entranceY)
                    {
                        levelMap[x, y] = 9; // 9 - вход
                    }
                }
            }
        }
        
        // Выход (последние 5 блоков)
        int exitY = mainPath[mainPath.Count - 5].y;
        for (int x = levelWidth - 5; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                if (y >= exitY - 2 && y <= exitY + 2)
                {
                    levelMap[x, y] = 0;
                    if (x == levelWidth - 1 && y == exitY)
                    {
                        levelMap[x, y] = 10; // 10 - выход
                    }
                }
            }
        }
    }

    void BuildLevel()
    {
        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                Vector3 position = new Vector3(x, y, 0);
                GameObject prefab = null;
                
                switch (levelMap[x, y])
                {
                    case 1: // Пол (верхний слой земли)
                        prefab = floorPrefabs[Random.Range(0, floorPrefabs.Length)];
                        break;
                        
                    case 2: // Земля под полом
                        prefab = innerWallPrefabs[Random.Range(0, innerWallPrefabs.Length)];
                        break;
                        
                    case 3: // Потолок
                        prefab = ceilingPrefabs[Random.Range(0, ceilingPrefabs.Length)];
                        break;
                        
                    case 4: // Земля над потолком
                        prefab = innerWallPrefabs[Random.Range(0, innerWallPrefabs.Length)];
                        break;
                        
                    case 5: // Платформа
                        prefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
                        break;
                        
                    case 6: // Препятствие
                        prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                        break;
                        
                    case 7: // Внутренняя стена под потолком
                        prefab = innerWallPrefabs[Random.Range(0, innerWallPrefabs.Length)];
                        break;
                        
                    case 8: // Внутренняя стена над полом
                        prefab = innerWallPrefabs[Random.Range(0, innerWallPrefabs.Length)];
                        break;
                        
                    case 9: // Вход
                        prefab = entrancePrefab;
                        break;
                        
                    case 10: // Выход
                        prefab = exitPrefab;
                        break;
                }
                
                if (prefab != null)
                {
                    Instantiate(prefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}
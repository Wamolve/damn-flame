using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public int levelWidth = 100;
    public int levelHeight = 20;
    public GameObject[] platformPrefabs;
    public GameObject[] obstaclePrefabs;
    public float platformChance = 0.7f;
    public float obstacleChance = 0.2f;
    
    private int[,] levelMap;

    void Start()
    {
        GenerateLevel();
        BuildLevel();
    }

    void GenerateLevel()
    {
        levelMap = new int[levelWidth, levelHeight];
        
        // Генерация основного пути
        int currentHeight = Random.Range(3, levelHeight / 2);
        
        for (int x = 0; x < levelWidth; x++)
        {
            // Изменение высоты с некоторой вероятностью
            if (Random.value < 0.1f && currentHeight > 2)
            {
                currentHeight += Random.Range(-1, 2);
                currentHeight = Mathf.Clamp(currentHeight, 2, levelHeight - 3);
            }
            
            // Создание платформы
            levelMap[x, currentHeight] = 1;
            
            // Добавление земли под платформой
            for (int y = 0; y < currentHeight; y++)
            {
                levelMap[x, y] = 2; // 2 - непроходимый тайл (земля)
            }
            
            // Добавление случайных платформ выше
            if (Random.value < platformChance && x % 3 == 0)
            {
                int platformHeight = Random.Range(currentHeight + 1, levelHeight - 1);
                int platformLength = Random.Range(1, 4);
                
                for (int px = x; px < x + platformLength && px < levelWidth; px++)
                {
                    levelMap[px, platformHeight] = 1;
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
                if (levelMap[x, y] == 1) // Платформа
                {
                    GameObject platform = Instantiate(
                        platformPrefabs[Random.Range(0, platformPrefabs.Length)], 
                        new Vector3(x, y, 0), 
                        Quaternion.identity);
                    platform.transform.parent = transform;
                }
                else if (levelMap[x, y] == 2) // Земля/стена
                {
                    Instantiate(
                        obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], 
                        new Vector3(x, y, 0), 
                        Quaternion.identity);
                }
            }
        }
    }
}
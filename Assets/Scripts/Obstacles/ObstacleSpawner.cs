using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Obstacle[] obstaclePrefabs;
    [SerializeField, Min(1)] private int instancesPerObstacle = 3;
    [SerializeField] private GameObject floor;
    [SerializeField, Min(0.1f)] private float obstacleSpawnTime = 2f;

    private Collider2D floorCollider;
    private float timeUntilNextSpawn;
    private List<Obstacle>[] pools;
    private GameManager gameManager;
    private bool spawning;
    public void Initialize(GameManager manager) => gameManager = manager;

    private void Awake()
    {
        floorCollider = floor.GetComponent<Collider2D>();
        pools = new List<Obstacle>[obstaclePrefabs.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<Obstacle>();
            for (int j = 0; j < instancesPerObstacle; j++) CreateObstacle(i);
        }
    }

    private void Update()
    {
        if (!spawning || gameManager == null || !gameManager.IsPlaying || pools.Length == 0) return;
        timeUntilNextSpawn -= Time.deltaTime;
        if (timeUntilNextSpawn <= 0f)
        {
            GetObstacle(Random.Range(0, obstaclePrefabs.Length));
            timeUntilNextSpawn = Mathf.Max(0.1f, obstacleSpawnTime);
        }
    }

    public void StartSpawning()
    {
        timeUntilNextSpawn = Mathf.Max(0.1f, obstacleSpawnTime);
        spawning = true;
    }

    public void StopSpawning()
    {
        spawning = false;
        foreach (var pool in pools)
            foreach (var obstacle in pool) obstacle.StopMovement();
    }

    public void ResetObstacles()
    {
        StopSpawning();
        foreach (var pool in pools)
            foreach (var obstacle in pool) obstacle.gameObject.SetActive(false);
    }

    public void GetObstacle(int typeIndex)
    {
        if (gameManager == null || !gameManager.IsPlaying || typeIndex < 0 || typeIndex >= pools.Length) return;
        Obstacle available = pools[typeIndex].Find(obstacle => !obstacle.gameObject.activeSelf);
        if (available == null) available = CreateObstacle(typeIndex);
        available.Prepare(gameManager, new Vector2(transform.position.x, floorCollider.bounds.max.y));
    }

    private Obstacle CreateObstacle(int typeIndex)
    {
        Obstacle obstacle = Instantiate(obstaclePrefabs[typeIndex], transform);
        obstacle.gameObject.SetActive(false);
        pools[typeIndex].Add(obstacle);
        return obstacle;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public Transform[] spawnPoints;
    public Transform player;
    public int maxWaves = 3;   
    private int currentWave = 0;
    public float despawnDistance = 15f;

    public int enemiesPerWave = 4;
    public float spawnDelay = 0.5f;

    private bool isSpawning = false;
    private bool isWaveSpawning = false;
    private List<GameObject> currentEnemies = new List<GameObject>();

    void Update()
    {
        if (isSpawning)
        {
            CheckWaveStatus();
        }
        // Despawn enemies that are too far from the player
        HandleDespawn();
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnWave());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

  IEnumerator SpawnWave()
{
    
    if (currentWave >= maxWaves)
    {
        Debug.Log("All waves completed");
        yield break;
    }

    currentWave++;
    isWaveSpawning = true;

    Debug.Log($"Starting Wave {currentWave}");

    for (int i = 0; i < enemiesPerWave; i++)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        int enemyIndex = Random.Range(0, enemyPrefab.Length);

        GameObject enemy = Instantiate(enemyPrefab[enemyIndex], spawnPoint.position, Quaternion.identity);
        currentEnemies.Add(enemy);

        yield return new WaitForSeconds(spawnDelay);
    }

    isWaveSpawning = false;
}
void CheckWaveStatus()
{
    currentEnemies.RemoveAll(enemy => enemy == null);

    // Only spawn next wave if limit not reached
    if (currentEnemies.Count == 0 && !isWaveSpawning && currentWave < maxWaves)
    {
        StartCoroutine(SpawnWave());
    }
}

    public void ClearAllEnemies()
{
    foreach (GameObject enemy in currentEnemies)
    {
        if (enemy != null)
        {
            Destroy(enemy);
        }
    }

    currentEnemies.RemoveAll(enemy => enemy == null);
}

void HandleDespawn()
{
    if (player == null) return;

    for (int i = currentEnemies.Count - 1; i >= 0; i--)
    {
        if (currentEnemies[i] == null) continue;

        float distance = Vector2.Distance(player.position, currentEnemies[i].transform.position);

        if (distance > despawnDistance)
        {
            Destroy(currentEnemies[i]);
            currentEnemies.RemoveAt(i);
        }
    }
}
}
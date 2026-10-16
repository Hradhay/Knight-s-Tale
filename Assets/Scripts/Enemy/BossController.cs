using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Enemy_Health health;

    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public bool spawnAroundBoss = true;
    public float spawnRadius = 3f;


    // Reference your EXISTING scripts
    private Enemy_Movements movement;
    private Enemy_Combat combat;

    private float baseSpeed;
    private int baseDamage;

    private bool phase75Triggered = false;
    private bool phase50Triggered = false;
    private bool phase25Triggered = false;
    

    void Start()
    {
        movement = GetComponent<Enemy_Movements>();
        combat = GetComponent<Enemy_Combat>();

        baseSpeed = movement.speed;
        baseDamage = combat.damage;
    }

    void Update()
    {
        float healthPercent = (float)health.currentHealth / health.maxHealth;

        if (!phase75Triggered && healthPercent <= 0.75f)
        {
            phase75Triggered = true;
            SpawnEnemies(5);
            BuffBoss(1.2f);
        }

        if (!phase50Triggered && healthPercent <= 0.5f)
        {
            phase50Triggered = true;
            SpawnEnemies(7);
            BuffBoss(1.4f);
        }

        if (!phase25Triggered && healthPercent <= 0.25f)
        {
            phase25Triggered = true;
            SpawnEnemies(10);
            BuffBoss(1.6f);
        }
    }
       void SpawnEnemies(int count)
{
    StartCoroutine(SpawnWave(count));
}

      IEnumerator SpawnWave(int count)
{
    for (int i = 0; i < count; i++)
    {
        SpawnAroundBoss();
        SpawnAtSpawnPoint();

        yield return new WaitForSeconds(0.2f);
    }
}

    void SpawnAroundBoss()
{
    Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(1.5f, spawnRadius);
    Vector3 spawnPos = transform.position + new Vector3(offset.x, offset.y, 0);

    SpawnEnemyAtPosition(spawnPos);
}

void SpawnAtSpawnPoint()
{
    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
    SpawnEnemyAtPosition(spawnPoint.position);
}

void SpawnEnemyAtPosition(Vector3 pos)
{
    int index = Random.Range(0, enemyPrefabs.Length);

    if (Random.value < 0.6f)
        index = 0;

    GameObject enemy = enemyPrefabs[index];

    Instantiate(enemy, pos, Quaternion.identity);
}

    void BuffBoss(float multiplier)
    {
        movement.speed = baseSpeed * multiplier;
        combat.damage = Mathf.RoundToInt(baseDamage * multiplier);

        Debug.Log("Boss Buffed! Speed & Damage Increased");
    }
}
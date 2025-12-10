using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour
{
    [Header("Enemy Prefabs (easy → hard)")]
    public GameObject[] enemyPrefabs;

    [Header("PowerUp Prefabs")]
    public GameObject powerUpPrefab;

    [Header("Spawning Settings")]
    public Transform[] spawnPoints;
    public Transform[] powerUpSpawnPoints;
    public int baseEnemiesPerRound = 5;
    public float spawnDelay = 0.3f;

    public float powerUpInterval = 20f;

    private int currentRound = 0;
    public int score = 0;

    public bool allowPowerUps = true;

    private GameObject currentPowerUp = null;

    GameUI gui;


    void Start()
    {
        gui = GameObject.FindFirstObjectByType<GameUI>();
        StartNextRound();
        StartCoroutine(PowerUpTimer());
    }

    void Update()
    {
        //check if there are any enemies left
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            StartNextRound();
        }
    }

    public void StartNextRound()
    {
        currentRound++;
        gui.ShowRound(currentRound);

        int enemiesToSpawn = baseEnemiesPerRound + currentRound * 2;
        StartCoroutine(SpawnRound(enemiesToSpawn));
    }

    IEnumerator SpawnRound(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnEnemy()
    {
        // How difficult the enemies allowed this round can be
        int maxIndex = Mathf.Clamp(currentRound / 3, 0, enemyPrefabs.Length - 1);

        GameObject chosen = enemyPrefabs[Random.Range(0, maxIndex + 1)];

        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(chosen, spawn.position, Quaternion.identity);
    }

    IEnumerator PowerUpTimer()
    {
        while (allowPowerUps)
        {
            yield return new WaitForSeconds(powerUpInterval);
            SpawnPowerUp();
        }
    }

    public void SpawnPowerUp()
    {
        // Do NOT spawn if one already exists
        if (currentPowerUp != null)
            return;

        Transform spawn = powerUpSpawnPoints[Random.Range(0, powerUpSpawnPoints.Length)];

        currentPowerUp = Instantiate(powerUpPrefab, spawn.position, Quaternion.identity);

        // When the power-up is destroyed, clear the reference
        StartCoroutine(TrackPowerUpLife(currentPowerUp));
    }

    private IEnumerator TrackPowerUpLife(GameObject pu)
    {
        // Wait until the power-up is destroyed
        while (pu != null)
            yield return null;

        // Once destroyed, clear the reference so a new one can spawn
        currentPowerUp = null;
    }
}
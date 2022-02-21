using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private int enemyCount;
    [SerializeField] private int waveNumber = 1;
    [SerializeField] private int bossRound;

    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] powerupPrefabs;
    [SerializeField] private GameObject[] miniEnemyPrefabs;
    [SerializeField] private GameObject enemyPrefab;    

    private int _randomEnemy;
    private float _spawnRange = 9;

    private void Start()
    {
        int randomPowerup = Random.Range(0, powerupPrefabs.Length);
        Instantiate(powerupPrefabs[randomPowerup], GenerateSpawnPosition(), powerupPrefabs[randomPowerup].transform.rotation);
        SpawnEnemyWave(waveNumber);         
    }

    private void SpawnBossWave(int currentRound)
    {
        int miniEnemysToSpawn;
            if (bossRound !=0)
            {
            miniEnemysToSpawn = currentRound / bossRound;
            }
            else
            {
            miniEnemysToSpawn = 1;
            }
        var boss = Instantiate(bossPrefab, GenerateSpawnPosition(), bossPrefab.transform.rotation);
        boss.GetComponent<Enemy>().miniEnemySpawnCount = miniEnemysToSpawn;
    }
    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {            
            _randomEnemy = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[_randomEnemy], GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    public void SpawnMiniEnemy(int amount)
    {
        for(int i=0; i<amount; i++)
        {
            int randomMini = Random.Range(0, miniEnemyPrefabs.Length);
            Instantiate(miniEnemyPrefabs[randomMini], GenerateSpawnPosition(), 
                miniEnemyPrefabs[randomMini].transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-_spawnRange, _spawnRange);
        float spawnPosZ = Random.Range(-_spawnRange, _spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }

    private void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            waveNumber++;
            //spawn a boss every x number of waves
            if (waveNumber % bossRound == 0)
            {
                SpawnBossWave(waveNumber);
            }
            else
            {
                SpawnEnemyWave(waveNumber);
            }
            //update to select a random powerup prefab for the medium challenge
            int randomPowerup = Random.Range(0, powerupPrefabs.Length);
            Instantiate(powerupPrefabs[randomPowerup], GenerateSpawnPosition(), powerupPrefabs[randomPowerup].transform.rotation);
        }        
    }
}

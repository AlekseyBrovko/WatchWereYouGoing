using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private float spawnInterval;
    [SerializeField] public int miniEnemySpawnCount;

    public bool isBoss = false;

    private Rigidbody _enemyRb;
    private GameObject _player;
    private SpawnManager _spawnManager;
    private float _nextSpawn;

    private void Start()
    {
        _enemyRb = GetComponent<Rigidbody>();
        _player = GameObject.Find("Player");

        if(isBoss)
        {
            _spawnManager = FindObjectOfType<SpawnManager>();
        }
    }

    private void Update()
    {
        Vector3 lookDirection = (_player.transform.position - transform.position).normalized;
        _enemyRb.AddForce(lookDirection * speed);        
        if(isBoss)
        {
            if(Time.time > _nextSpawn)
            {
                _nextSpawn = Time.time + spawnInterval;
                _spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
            }
        }

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}

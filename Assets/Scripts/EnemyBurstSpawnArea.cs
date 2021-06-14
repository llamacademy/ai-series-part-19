using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyBurstSpawnArea : MonoBehaviour
{
    [SerializeField]
    private Collider SpawnCollider;
    [SerializeField]
    private EnemySpawner EnemySpawner;
    [SerializeField]
    private List<EnemyScriptableObject> Enemies = new List<EnemyScriptableObject>();
    [SerializeField]
    private EnemySpawner.SpawnMethod SpawnMethod = EnemySpawner.SpawnMethod.Random;
    [SerializeField]
    private int SpawnCount = 10;
    [SerializeField]
    private float SpawnDelay = 0.5f;

    private Coroutine SpawnEnemiesCoroutine;
    private Bounds Bounds;

    private void Awake()
    {
        if (SpawnCollider == null)
        {
            SpawnCollider = GetComponent<Collider>();
        }
        
        Bounds = SpawnCollider.bounds;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (SpawnEnemiesCoroutine == null)
        {
            SpawnEnemiesCoroutine = StartCoroutine(SpawnEnemies());
        }
    }

    private Vector3 GetRandomPositionInBounds()
    {
        return new Vector3(Random.Range(Bounds.min.x, Bounds.max.x), Bounds.min.y, Random.Range(Bounds.min.z, Bounds.max.z));
    }

    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds Wait = new WaitForSeconds(SpawnDelay);

        for (int i = 0; i < SpawnCount; i++)
        {
            if (SpawnMethod == EnemySpawner.SpawnMethod.RoundRobin)
            {
                EnemySpawner.DoSpawnEnemy(
                    EnemySpawner.Enemies.FindIndex((enemy) => enemy.Equals(Enemies[i % Enemies.Count])),
                    GetRandomPositionInBounds()
                ); ;
            }
            else if (SpawnMethod == EnemySpawner.SpawnMethod.Random)
            {
                int index = Random.Range(0, Enemies.Count);
                EnemySpawner.DoSpawnEnemy(
                    EnemySpawner.Enemies.FindIndex((enemy) => enemy.Equals(Enemies[index])),
                    GetRandomPositionInBounds()
                );
            }

            yield return Wait;
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (SpawnCollider != null)
        {
            Gizmos.DrawWireCube(SpawnCollider.bounds.center, SpawnCollider.bounds.size);
        }
        else
        {
            Collider collider = GetComponent<Collider>();
            Gizmos.DrawWireCube(collider.bounds.center, collider.bounds.size);
        }
    }
}

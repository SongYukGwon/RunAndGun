using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//오브젝트풀 사용 예정
public class EnemySpawn : MonoBehaviour
{
    //소환할 적 오브젝트
    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private int enemyCount;
    private int currentEnemyCount;

    [SerializeField]
    private int stage;

    private bool endStage = false;

    private bool isSpawn = false;

    void Start()
    {
        currentEnemyCount = enemyCount * stage;
    }

    void Update()
    {
        TrySpawnEnemy();
    }

    private void TrySpawnEnemy()
    {
        if (!endStage && !isSpawn)
        {
            StartCoroutine(Spawn());
        }
    }

    private IEnumerator Spawn()
    {
        isSpawn = true;
        for(int j =0; j< currentEnemyCount; j++)
        {
            Vector3 ranPos = GetRandomEnemySpawnPosition();
            Instantiate(enemy, ranPos, Quaternion.identity);
            currentEnemyCount--;
            yield return new WaitForSeconds(3f);
        }
        endStage = true;
        isSpawn = false;
        currentEnemyCount = enemyCount;
    }

    private Vector3 GetRandomEnemySpawnPosition()
    {
        float radius = 3f;
        Vector3 playerPosition = transform.position;

        float a = playerPosition.x;
        float b = playerPosition.z;

        float x = Random.Range(-radius + a, radius + a);
        float z_b = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(x - a, 2));
        z_b *= Random.Range(0, 2) == 0 ? -1 : 1;
        float z = z_b + b;

        Vector3 randomPosition = new Vector3(x, 0, z);

        return randomPosition;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawn : MonoBehaviour
{

    //�ʿ��� ������Ʈ
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

    public void updatedStage(int num)
    {
        stage = num;
    }

    //�����õ� �Լ�
    private void TrySpawnEnemy()
    {
        if (!endStage && !isSpawn)
        {
            StartCoroutine(Spawn());
        }
        else if(endStage)
        {
            currentEnemyCount += 5;
            endStage = false;
        }
    }

    //�����ϴ� �Լ�
    //�ð������� �ѹ��� �����Ǵ� ���� �þ�� ���� ������ ����
    private IEnumerator Spawn()
    {
        isSpawn = true;
        for(int j =0; j< currentEnemyCount; j++)
        {
            for (int i = 0; i < stage*3; i++)
            {
                Vector3 ranPos = GetRandomEnemySpawnPosition();
                GameObject em = EnemyObjectPool.GetObject();
                em.transform.position = ranPos;
                em.GetComponent<ZombieInfo>().SetStatus(stage);
                em.transform.rotation = Quaternion.identity;
                currentEnemyCount--;
            }
            yield return new WaitForSeconds(3f - stage/10);
        }
        yield return new WaitForSeconds(10f+stage);
        endStage = true;
        isSpawn = false;
    }

    //������ġ�� �޾ƿͼ� ����ϴ� �Լ�
    private Vector3 GetRandomEnemySpawnPosition()
    { 
        float radius = 20f;
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

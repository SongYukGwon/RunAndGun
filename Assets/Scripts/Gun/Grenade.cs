using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObject;
    public ParticleSystem effectObject; // ���� ����Ʈ
    public Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        // �ڷ�ƾ -> 3�ʵ� ����
        yield return new WaitForSeconds(3f);

        // �������� �ӵ� ����
        rigid.velocity = Vector3.zero;
        // ȸ�� �ӵ� ����
        rigid.angularVelocity = Vector3.zero;
        effectObject.Play(true);
        meshObject.SetActive(false);


        //  ����ź �ǰ� ó��

        // �ǰݵ� ��ü�� ����
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        foreach (RaycastHit hitObject in rayHits)
        {
            // ����ź ���� ���� �ǰ� ����� �ǰ� �Լ� ȣ��
            hitObject.transform.GetComponent<EnemyController>().Damage(100,transform.position);
        }

        // ���� ����Ʈ�� ���� 5�ʵڿ� ����
        Destroy(gameObject, 5);
    }
}

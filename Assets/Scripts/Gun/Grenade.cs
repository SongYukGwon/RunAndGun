using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObject;
    public ParticleSystem effectObject; // 터짐 이펙트
    public Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        // 코루틴 -> 3초뒤 폭발
        yield return new WaitForSeconds(3f);

        // 굴러가는 속도 제거
        rigid.velocity = Vector3.zero;
        // 회전 속도 제거
        rigid.angularVelocity = Vector3.zero;
        effectObject.Play(true);
        meshObject.SetActive(false);


        //  수류탄 피격 처리

        // 피격된 객체들 모음
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        foreach (RaycastHit hitObject in rayHits)
        {
            // 수류탄 범위 내의 피격 대상의 피격 함수 호출
            hitObject.transform.GetComponent<EnemyController>().Damage(100,transform.position);
        }

        // 폭발 이펙트를 위해 5초뒤에 삭제
        Destroy(gameObject, 5);
    }
}

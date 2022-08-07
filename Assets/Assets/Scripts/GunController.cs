using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //활성화 여부
    public static bool isActivate = true;

    //현재 장착된 총
    [SerializeField]
    private GunInfo currentGun;

    // 현재 연사 속도
    private float currentFireRate;

    // 현재 효과음
    private AudioSource audioSource;


    //상태변수
    private bool isReload = false;

    //충돌 정보 받아옴.
    private RaycastHit hitInfo;

    //본래포지션값
    [SerializeField]
    private Vector3 originPos;

    //필요한 컴포넌트
    [SerializeField]
    private Camera theCam;
    //private CrossHair theCrossHair;

    //피격 이펙트
    //좀비 히트
    [SerializeField]
    private GameObject zombie_hit_prefab;
    //일반 구조물 히트
    [SerializeField]
    private GameObject other_hit_prefab;

    // 특정 레이어 마스크 지정
    int layerMaskEmemy;


    // Start is called before the first frame update
    void Start()
    {
        originPos = Vector3.zero;
        audioSource = currentGun.GetComponent<AudioSource>();
        layerMaskEmemy = (-1) - (1 << LayerMask.NameToLayer("Dead")); // Enemy레이어만 탐색하도록지정
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
        {
            GunFireRateCalc();
            TryFire();
            //TryReload();
        }
    }

    //연사속도 재계산
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime; //1초에 1을 감소시킨다.
    }

    //발사시도
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    //발사전 계산
    private void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
            {
                Shoot();
                audioSource.Play();
            }
        }
    }

    //발사후 계산
    private void Shoot()
    {
        // theCrossHair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; // 연사속도 재계산
        currentGun.muzzleFlash.Play(); // 총구화염
        Hit(); // 히트처리
        StopAllCoroutines(); 
        StartCoroutine(RetroActionCoroutine());
    }

    private void Hit()
    {

        //공격 범위 설정 방법 설정
        //스코프는 동그라미로 할 것.
        /*
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward +
            new Vector3(Random.Range(-theCrossHair.GetAccuracy() - currentGun.accuracy, theCrossHair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-theCrossHair.GetAccuracy() - currentGun.accuracy, theCrossHair.GetAccuracy() + currentGun.accuracy),
                        0),
                        out hitInfo, currentGun.range, layerMask))
        */
        RaycastHit hitInfo;
        GameObject clone;
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward, out hitInfo,Mathf.Infinity, layerMaskEmemy))
        {
            //Enemy일 경우 적 에너미에게 데미지를 줌.
            if (hitInfo.transform.gameObject.CompareTag("Enemy"))
            {
                clone = Instantiate(zombie_hit_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                hitInfo.transform.GetComponent<EnemyController>().Damage(currentGun.damage, transform.position);
                Destroy(clone, 2);
            }
        }
        
    }

    //반동 코루틴
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(originPos.x, originPos.y, -currentGun.retroActionForce);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.findSightOriginPos.y, currentGun.findSightOriginPos.z);

        currentGun.transform.localPosition = originPos;

        //반동 시작
        while (currentGun.transform.localPosition.z <= currentGun.retroActionForce - 0.02f)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
            yield return null;
        }

        //원위치

        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
            yield return null;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunController : MonoBehaviour
{
    //활성화 여부
    public static bool isActivate = true;

    //현재 장착된 총
    [SerializeField]
    private GunInfo currentGun;


    // 현재 연사 속도
    private float currentFireRate;

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
    [SerializeField]
    private CrossHair theCrossHair;
    [SerializeField]
    private GunChanger theGunChanger;

    //피격 이펙트
    //좀비 히트
    [SerializeField]
    private GameObject zombie_hit_prefab;
    //일반 구조물 히트
    [SerializeField]
    private GameObject other_hit_prefab;

    //총알 표시 UI
    [SerializeField]
    private TextMeshProUGUI currentAmmoText;
    [SerializeField]
    private TextMeshProUGUI carryAmmoText;

    // 특정 레이어 마스크 지정
    int layerMaskEmemy;

    //증가한 능력치를 적용하기 위한 컴포넌트
    [SerializeField]
    private PlayerStat thePlayerStat;

    //수류탄 필요 컴포넌트
    private int hasGrenades;
    [SerializeField]
    private GameObject GrenadePrefab;


    void Start()
    {
        originPos = Vector3.zero;
        layerMaskEmemy = (-1) - (1 << LayerMask.NameToLayer("Dead")); // Enemy레이어만 탐색하도록지정
        GunChanger.currentWeapon = currentGun.transform;
        GunChanger.currentWeaponAnim = currentGun.GetComponent<Animator>();
        hasGrenades = 10;
    }

    void Update()
    {
        if (isActivate)
        {
            GunFireRateCalc();
            TryFire();
            TryKeyCode();
            AmmoTextUpdate();
        }
    }


    // 타입에따른 탄약 증가
    public void IncreseAmmo(string type)
    {
        theGunChanger.IncreseAmmo(type);
    }

    // 탄약 소지 개수 업데이트 함수
    private void AmmoTextUpdate()
    {
        currentAmmoText.text = currentGun.currentBulletCount.ToString();
        carryAmmoText.text = currentGun.carryBulletCount.ToString();
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
                currentGun.audioShot.Play();
            }
            else if(currentGun.carryBulletCount > 0)
            {
                StartCoroutine(ReloadCouroutine());
            }
        }
    }

    //재장전 취소
    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }


    //총 바꾸는 함수
    public void GunChange(GunInfo _gun)
    {
        if (currentGun != null)
            currentGun.gameObject.SetActive(false);

        currentGun = _gun;
        GunChanger.currentWeapon = currentGun.GetComponent<Transform>();
        GunChanger.currentWeaponAnim = currentGun.GetComponent<Animator>();

        currentGun.transform.localPosition = Vector3.zero;

        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }

    //발사후 계산
    private void Shoot()
    {
        // theCrossHair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate - (0.01f * thePlayerStat.addAttackSpeed); // 연사속도 재계산
        currentGun.muzzleFlash.Play(); // 총구화염
        StopAllCoroutines();
        theCrossHair.FireAnimation();
        StartCoroutine(Hit()); // 히트처리
        
        StartCoroutine(RetroActionCoroutine());
    }


    //정확도에따라 hit처리하는 함수
    //적에 맞으면 빨간색 효과 아니면 검은색 효과를 냄
    private IEnumerator Hit()
    {

        RaycastHit hitInfo;
        GameObject clone;
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward +
            new Vector3(Random.Range(-theCrossHair.GetAccuracy() - currentGun.accuracy, theCrossHair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-theCrossHair.GetAccuracy() - currentGun.accuracy, theCrossHair.GetAccuracy() + currentGun.accuracy),
                        0),
                        out hitInfo, currentGun.range, layerMaskEmemy))
        {
            if (hitInfo.transform.gameObject.CompareTag("Enemy"))
            {
                clone = HitEffectPool.GetHitObject();
                clone.transform.position = hitInfo.point;
                clone.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
                hitInfo.transform.GetComponent<EnemyController>().Damage(currentGun.damage + thePlayerStat.addAttack, transform.position);
                yield return new WaitForSeconds(2f);
                HitEffectPool.ReturnHitObject(clone);
            }
            else if(!hitInfo.transform.gameObject.CompareTag("Dead") && !hitInfo.transform.gameObject.CompareTag("Player"))
            {
                clone = HitEffectPool.GetOtherObject();
                clone.transform.position = hitInfo.point;
                clone.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
                yield return new WaitForSeconds(2f);
                HitEffectPool.ReturnOtherObject(clone);
            }
        }
    }

    
    //반동 코루틴
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(originPos.x, originPos.y, currentGun.retroActionForce);

        currentGun.transform.position = originPos;

        currentGun.GetComponent<Animator>().SetTrigger("Attack");

        yield return null;
    }

    

    //재장전 시도
    private void TryKeyCode()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            StartCoroutine(ReloadCouroutine());
            
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            ThrowGrenade();
        }

    }
    private void ThrowGrenade()
    {
        if (hasGrenades == 0)
            return;

        if (!isReload )
        {
            Debug.Log(theCam.transform);
            GameObject grenade = Instantiate(GrenadePrefab, theCam.transform);
            Rigidbody grenadeRigidbody = grenade.GetComponent<Rigidbody>();
            grenadeRigidbody.AddForce(theCam.transform.forward * 75);
        }
    }

    //재장전
    IEnumerator ReloadCouroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);
            currentGun.audioReload.Play();
            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("소유한 총알이 없습니다");
        }
    }
}

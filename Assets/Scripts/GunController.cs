using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //Ȱ��ȭ ����
    public static bool isActivate = true;

    //���� ������ ��
    [SerializeField]
    private GunInfo currentGun;

    // ���� ���� �ӵ�
    private float currentFireRate;

    // ���� ȿ����
    private AudioSource audioSource;

    //���º���
    private bool isReload = false;

    //�浹 ���� �޾ƿ�.
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;

    //���������ǰ�
    [SerializeField]
    private Vector3 originPos;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCam;
    //private CrossHair theCrossHair;

    //�ǰ� ����Ʈ
    //[SerializeField]
    //private GameObject hit_effect_prefab;



    // Start is called before the first frame update
    void Start()
    {
        originPos = Vector3.zero;
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

    //����ӵ� ����
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime; //1�ʿ� 1�� ���ҽ�Ų��.
    }

    //�߻�õ�
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    //�߻��� ���
    private void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
            {
                Shoot();
            }
            else
            {
                //CancelFineSight();
                //StartCoroutine(ReloadCouroutine());
            }
        }
    }

    //�߻��� ���
    private void Shoot()
    {
        // theCrossHair.FireAnimation();
        Debug.Log("Shoot");
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate; // ����ӵ� ����
        //PlaySE(currentGun.fire_Sound);
        //currentGun.muzzleFlash.Play();
        Hit();
        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());
    }

    private void Hit()
    {
        /*
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward +
            new Vector3(Random.Range(-theCrossHair.GetAccuracy() - currentGun.accuracy, theCrossHair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-theCrossHair.GetAccuracy() - currentGun.accuracy, theCrossHair.GetAccuracy() + currentGun.accuracy),
                        0),
                        out hitInfo, currentGun.range, layerMask))
        */
        if(Physics.Raycast(theCam.transform.position, theCam.transform.forward, out hitInfo))
        {
            //GameObject clone = Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            //Destroy(clone, 2);
            Debug.Log(hitInfo);

        }
    }

    //�ݵ� �ڷ�ƾ
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.findSightOriginPos.y, currentGun.findSightOriginPos.z);

        currentGun.transform.localPosition = originPos;

        //�ݵ� ����
        while (currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
            yield return null;
        }

        //����ġ

        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
            yield return null;
        }

    }
}

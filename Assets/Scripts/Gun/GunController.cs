using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunController : MonoBehaviour
{
    //Ȱ��ȭ ����
    public static bool isActivate = true;

    //���� ������ ��
    [SerializeField]
    private GunInfo currentGun;


    // ���� ���� �ӵ�
    private float currentFireRate;

    //���º���
    private bool isReload = false;

    //�浹 ���� �޾ƿ�.
    private RaycastHit hitInfo;

    //���������ǰ�
    [SerializeField]
    private Vector3 originPos;

    //�ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCam;
    [SerializeField]
    private CrossHair theCrossHair;

    //�ǰ� ����Ʈ
    //���� ��Ʈ
    [SerializeField]
    private GameObject zombie_hit_prefab;
    //�Ϲ� ������ ��Ʈ
    [SerializeField]
    private GameObject other_hit_prefab;

    //�Ѿ� ǥ�� UI
    [SerializeField]
    private TextMeshProUGUI currentAmmoText;
    [SerializeField]
    private TextMeshProUGUI carryAmmoText;

    // Ư�� ���̾� ����ũ ����
    int layerMaskEmemy;

    //������ �ɷ�ġ�� �����ϱ� ���� ������Ʈ
    [SerializeField]
    private PlayerStat thePlayerStat;

    // Start is called before the first frame update
    void Start()
    {
        originPos = Vector3.zero;
        layerMaskEmemy = (-1) - (1 << LayerMask.NameToLayer("Dead")); // Enemy���̾ Ž���ϵ�������
        GunChanger.currentWeapon = currentGun.transform;
        GunChanger.currentWeaponAnim = currentGun.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            AmmoTextUpdate();
        }
    }

    public void IncreseAmmo(string type)
    {
        FindObjectOfType<GunChanger>().IncreseAmmo(type);
    }

    private void AmmoTextUpdate()
    {
        currentAmmoText.text = currentGun.currentBulletCount.ToString();
        carryAmmoText.text = currentGun.carryBulletCount.ToString();
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
                currentGun.audioShot.Play();
            }
        }
    }

    public void CancelReload()
    {
        if (isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }

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

    //�߻��� ���
    private void Shoot()
    {
        // theCrossHair.FireAnimation();
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate - (0.01f * thePlayerStat.addAttackSpeed); // ����ӵ� ����
        currentGun.muzzleFlash.Play(); // �ѱ�ȭ��
        Hit(); // ��Ʈó��
        StopAllCoroutines(); 
        
        StartCoroutine(RetroActionCoroutine());
    }

    private void Hit()
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
                clone = Instantiate(zombie_hit_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                hitInfo.transform.GetComponent<EnemyController>().Damage(currentGun.damage + thePlayerStat.addAttack, transform.position);
                Destroy(clone, 2);
            }
            else if(!hitInfo.transform.gameObject.CompareTag("Dead") && !hitInfo.transform.gameObject.CompareTag("Player"))
            {
                clone = Instantiate(other_hit_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(clone, 2);
            }
        }
    }

    
    //�ݵ� �ڷ�ƾ
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(originPos.x, originPos.y, currentGun.retroActionForce);

        currentGun.transform.position = originPos;

        currentGun.GetComponent<Animator>().SetTrigger("Attack");

        yield return null;
    }

    

    //������ �õ�
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            StartCoroutine(ReloadCouroutine());
            
        }
    }

    //������
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
            Debug.Log("������ �Ѿ��� �����ϴ�");
        }
    }
}

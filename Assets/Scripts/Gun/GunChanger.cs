using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunChanger : MonoBehaviour
{
    //�����ڿ�, Ŭ���� ���� = ���� ����
    public static bool isChangeWeapon;

    //���� ������ �ִϸ��̼�
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    //���� ��ü ������, ���� ��ü�� ������ ���� ����
    [SerializeField]
    private float changeWeaponDelayTime;
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //���� ������ ���� ����
    [SerializeField]
    private GunInfo[] guns;


    //UI���� ������Ʈ
    [SerializeField]
    private GunUIHandler gunUIHandler;

    //���� �������� ���� ���� ������ �����ϵ��� ����.
    private Dictionary<string, GunInfo> gunDictionary = new Dictionary<string, GunInfo>();


    //�ʿ��� ������Ʈ
    [SerializeField]
    private GunController theGunController;

    [SerializeField]
    private string currentWeaponName;

    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && !currentWeaponName.Equals("pistol"))
            {
                StartCoroutine(ChangeWeaponCoroutine("pistol"));
                gunUIHandler.ChangeGunUI(0);
                currentWeaponName = "pistol";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && !currentWeaponName.Equals("assult"))
            {
                StartCoroutine(ChangeWeaponCoroutine("assult"));
                gunUIHandler.ChangeGunUI(1);
                currentWeaponName = "assult";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && !currentWeaponName.Equals("shotgun"))
            {
                StartCoroutine(ChangeWeaponCoroutine("shotgun"));
                gunUIHandler.ChangeGunUI(2);
                currentWeaponName = "shotgun";
            }
        }
    }

    public void IncreseAmmo(string type)
    {
        Debug.Log(type);
        switch(type)
        {
            case ("all"):
                gunDictionary["assult"].carryBulletCount += 30;
                gunDictionary["shotgun"].carryBulletCount += 14;
                Debug.Log("ALLGET");
                break;
            case ("assult"):
                gunDictionary["assult"].carryBulletCount += 30;
                Debug.Log("ASSULTGET");
                break;
            case ("shotgun"):
                gunDictionary["shotgun"].carryBulletCount += 14;
                Debug.Log("SHOTGUNGET");
                break;
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("WeaponOut");
        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();

        WeaponChange(_name);
        currentWeaponAnim.SetTrigger("WeaponIn");
        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()
    {
        Debug.Log("�ڷ�ƾ ���");
        theGunController.CancelReload();
        GunController.isActivate = false;
    }

    private void WeaponChange(string _name)
    {
        theGunController.GunChange(gunDictionary[_name]);
    }
}

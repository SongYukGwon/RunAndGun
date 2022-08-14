using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunChanger : MonoBehaviour
{
    //공유자원, 클래스 변수 = 정적 변수
    public static bool isChangeWeapon;

    //현재 무기의 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    //무기 교체 딜레이, 무기 교체가 완전히 끝난 시점
    [SerializeField]
    private float changeWeaponDelayTime;
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //무기 종류들 전부 관리
    [SerializeField]
    private GunInfo[] guns;


    //UI조절 컴포넌트
    [SerializeField]
    private GunUIHandler gunUIHandler;

    //관리 차원에서 쉽게 무기 접근이 가능하도록 만듦.
    private Dictionary<string, GunInfo> gunDictionary = new Dictionary<string, GunInfo>();


    //필요한 컴포너트
    [SerializeField]
    private GunController theGunController;

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
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ChangeWeaponCoroutine("pistol"));
                gunUIHandler.ChangeGunUI(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(ChangeWeaponCoroutine("assult"));
                gunUIHandler.ChangeGunUI(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(ChangeWeaponCoroutine("shotgun"));
                gunUIHandler.ChangeGunUI(2);
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
        Debug.Log("코루틴 취소");
        theGunController.CancelReload();
        GunController.isActivate = false;
    }

    private void WeaponChange(string _name)
    {
        theGunController.GunChange(gunDictionary[_name]);
    }
}

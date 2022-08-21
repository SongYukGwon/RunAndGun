using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float jumpForce;

    private float applySpeed;

    //���º���
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;

    //�� ���� ���θ� �˱����� ������Ʈ
    private CapsuleCollider capsuleCollider;

    //ī�޶� �ΰ���
    [SerializeField]
    private float lookSensitivity;

    //ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCamearaRotationX = 0;

    //�ʿ� ������Ʈ
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    [SerializeField]
    private PlayerStat playerStat;
    [SerializeField]
    private CrossHair theCrosshair;

    //��Ʈó�� ������Ʈ
    [SerializeField]
    private Image hitedImage;
    [SerializeField]
    private Vector3 originCameraPos;

    //������ üũ ����
    private Vector3 lastPos;

    private float originPosY;

    private Vector3 originRotation;

    private bool levelUpdate;


    // Start is called before the first frame update
    void Start()
    {
        originRotation = theCamera.transform.eulerAngles;
        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        levelUpdate = false;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        Move();
        if(!levelUpdate)
        {
            CameraRotation();
            CharacterRotation();
            MoveCheck();
            TryHeal();
        }
    }

    //levelUpdate ���� ���� �Լ�
    public void ChangeLevelUpdate(bool flag)
    {
        levelUpdate = flag;
    }

    //�÷��̾��� ü�� ȸ�� Ű �Է� Ȯ��
    private void TryHeal()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerStat.Heal();
        }
    }

    
    //������ ó��
    public void Damaged(int dma)
    {
        StartCoroutine(DamageAnim());
        playerStat.Damaged(dma);
    }

    //������ ������ ī�޶� ȿ��
    IEnumerator DamageAnim()
    {
        Color a = new Color(1, 0, 0, 0.2f);
        Color originColor = new Color(0,0,0,0);
        hitedImage.color = a;

        float shakeTime = 0.5f;
        while(shakeTime > 0.0f)
        {
            float x = 0;
            float y = 0;
            float z = Random.Range(-1f, 1f);

            shakeTime -= 0.1f;

            yield return new WaitForSeconds(0.1f);
        }

        //yield return new WaitForSeconds(1f);
        hitedImage.color = originColor;
    }


    //�����̴��� Ȯ��
    private void MoveCheck()
    {
        if (!isRun && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
                isWalk = true;
            else
                isWalk = false;

            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }

    //���� �ִ��� Ȯ��
    private void IsGround()
    {
        //Vector�� ���� ������ǥ�� �����ش�. transform�� ������Ʈ ����
        //bounds.extends.y ������.�ݻ�������.yũ�� -> ������Ʈ ������ y�� ���� ũ�⸸ŭ �������� ��
        //��ȯ�� bool
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        theCrosshair.JumpAnimation(!isGround);
    }

    //�����õ�
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    //�޸��� �õ�
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && isGround && playerStat.GetCurrentSP() != 0)
        {
            Running();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || isGround || isGround && playerStat.GetCurrentSP() != 0)
        {
            RunningCancel();
        }
    }

    //�޸���
    private void Running()
    {
        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        playerStat.DecreaseStamina(10*Time.deltaTime);
        applySpeed = runSpeed;
    }
    //�޸��� ���
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
        theCrosshair.RunningAnimation(isRun);
    }

    //����
    private void Jump()
    {
        playerStat.DecreaseStamina(50);
        myRigid.velocity = transform.up * jumpForce;
    }

    //ī�޶� ȸ��
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCamearaRotationX -= _cameraRotationX;
        currentCamearaRotationX = Mathf.Clamp(currentCamearaRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCamearaRotationX, 0f, 0f);
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        //100
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        // 001
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        // 101 �հ谡 2�ΰ��� �հ踦 1�� �ٲ��� 0.5 0 0.5 1�ʿ� �̵��ϴ� ���� ����ȭ��Ű�� ��
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }


    //���Ʒ� ī�޶� ����
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");

        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}

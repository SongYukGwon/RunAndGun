using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //private StatusController theStatusController;

    //������ üũ ����
    private Vector3 lastPos;
    

    private float originPosY;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        Move();
        CameraRotation();
        CharacterRotation();
        MoveCheck();
    }

    private void MoveCheck()
    {
        if (!isRun && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
                isWalk = true;
            else
                isWalk = false;

            //theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }

    private void IsGround()
    {
        //Vector�� ���� ������ǥ�� �����ش�. transform�� ������Ʈ ����
        //bounds.extends.y ������.�ݻ�������.yũ�� -> ������Ʈ ������ y�� ���� ũ�⸸ŭ �������� ��
        //��ȯ�� bool
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        //theCrosshair.JumpAnimation(!isGround);
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
        if (Input.GetKey(KeyCode.LeftShift) && isGround)
        {
            Running();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || isGround )
        {
            RunningCancel();
        }
    }

    //�޸���
    private void Running()
    {
        //theGunController.CancelFineSight();

        isRun = true;
        //theCrosshair.RunningAnimation(isRun);
        //theStatusController.DecreaseStamina(5);
        applySpeed = runSpeed;
    }
    //�޸��� ���
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
        //theCrosshair.RunningAnimation(isRun);
    }

    //����
    private void Jump()
    {
        //theStatusController.DecreaseStamina(100);
        //up 010
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

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

    //상태변수
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;

    //땅 착지 여부를 알기위한 컴포넌트
    private CapsuleCollider capsuleCollider;

    //카메라 민감도
    [SerializeField]
    private float lookSensitivity;

    //카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCamearaRotationX = 0;

    //필요 컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    //private StatusController theStatusController;

    //움직임 체크 변수
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
        //Vector는 월드 기준좌표로 보여준다. transform은 오브젝트 기준
        //bounds.extends.y 영역의.반사이즈의.y크기 -> 오브젝트 영역의 y의 반의 크기만큼 레이저를 쏨
        //반환은 bool
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        //theCrosshair.JumpAnimation(!isGround);
    }

    //점프시도
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    //달리기 시도
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

    //달리기
    private void Running()
    {
        //theGunController.CancelFineSight();

        isRun = true;
        //theCrosshair.RunningAnimation(isRun);
        //theStatusController.DecreaseStamina(5);
        applySpeed = runSpeed;
    }
    //달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
        //theCrosshair.RunningAnimation(isRun);
    }

    //점프
    private void Jump()
    {
        //theStatusController.DecreaseStamina(100);
        //up 010
        myRigid.velocity = transform.up * jumpForce;

    }

    //카메라 회전
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

        // 101 합계가 2인것을 합계를 1로 바꿔줌 0.5 0 0.5 1초에 이동하는 값을 일정화시키는 것
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);



    }


    //위아래 카메라 설정
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");

        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}

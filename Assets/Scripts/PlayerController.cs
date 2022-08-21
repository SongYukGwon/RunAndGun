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
    [SerializeField]
    private PlayerStat playerStat;
    [SerializeField]
    private CrossHair theCrosshair;

    //히트처리 컴포넌트
    [SerializeField]
    private Image hitedImage;
    [SerializeField]
    private Vector3 originCameraPos;

    //움직임 체크 변수
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

    //levelUpdate 변수 설정 함수
    public void ChangeLevelUpdate(bool flag)
    {
        levelUpdate = flag;
    }

    //플레이어의 체력 회복 키 입력 확인
    private void TryHeal()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerStat.Heal();
        }
    }

    
    //데미지 처리
    public void Damaged(int dma)
    {
        StartCoroutine(DamageAnim());
        playerStat.Damaged(dma);
    }

    //데미지 받을때 카메라 효과
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


    //움직이는지 확인
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

    //땅에 있는지 확인
    private void IsGround()
    {
        //Vector는 월드 기준좌표로 보여준다. transform은 오브젝트 기준
        //bounds.extends.y 영역의.반사이즈의.y크기 -> 오브젝트 영역의 y의 반의 크기만큼 레이저를 쏨
        //반환은 bool
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        theCrosshair.JumpAnimation(!isGround);
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
        if (Input.GetKey(KeyCode.LeftShift) && isGround && playerStat.GetCurrentSP() != 0)
        {
            Running();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || isGround || isGround && playerStat.GetCurrentSP() != 0)
        {
            RunningCancel();
        }
    }

    //달리기
    private void Running()
    {
        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        playerStat.DecreaseStamina(10*Time.deltaTime);
        applySpeed = runSpeed;
    }
    //달리기 취소
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
        theCrosshair.RunningAnimation(isRun);
    }

    //점프
    private void Jump()
    {
        playerStat.DecreaseStamina(50);
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

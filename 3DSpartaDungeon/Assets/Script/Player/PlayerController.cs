using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 키입력및 동작 관리
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;  // 현재 입력 값
    public float jumpPower;
    public LayerMask groundLayerMask;  // 레이어 정보

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;  // 최소 시야각
    public float maxXLook;  // 최대 시야각
    private float camCurXRot;
    public float lookSensitivity; // 카메라 민감도

    private Vector2 mouseDelta;  // 마우스 변화값

    [HideInInspector]
    public bool canLook = true;
    public Action inventory;
    private Rigidbody rigidbody;

    [Header("Jump")]
    public float maxSlopeAngle;
    bool canJump = true;
    public float JumpCoolTime;

    [Header("caching")]
    private Player player;
        Vector3 test;
    public LayerMask moveObstacleLayer;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>(); //해당 스크립트를 가진 오브젝트의 컴포넌트 가져오기

    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//커서 중앙고정및 Hide
        player = CharacterManager.Instance.Player;//캐싱
    }

    // 물리 연산
    private void FixedUpdate()//
    {

    if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.2f, moveObstacleLayer))
    {
            Debug.Log("충돌함!");
            test = hit.collider.GetComponent<Rigidbody>().velocity;
    }

    Move();
    }

    // 카메라 연산 -> 모든 연산이 끝나고 카메라 움직임
    private void LateUpdate()
    {
        if (canLook) //toggle
        {
            CameraLook();
        }
    }
    //OnLook(),Onmove(),OnJump() 입력 처리
    //Input System에서 (Behavior)전달 방식이 SendMessage일 경우 On+Action이름으로 명명규칙을 지켜야 하고
    //Invoke Unity Events일 경우 자유롭게 명명이 가능하다.
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (player.P_condition.uiCondition.stamina.curValue >= 20)
        {
            if (context.phase == InputActionPhase.Started && IsGrounded() && canJump)
            {
                rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
                player.P_condition.StaminaMinus(20);
                StartCoroutine(JumpCooldown());
            }
        }
    }

    IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(JumpCoolTime);  // JumpCoolTime동안 점프 불가
        canJump = true;
    }

    //Move(),CameraLook() 물리연산처리



    private void Move()
    {
        // 현재 입력의 y 값은 z축(forward, 앞뒤)에 곱한다,Unity에서 높이는 Y축이다.
        // 현재 입력의 x 값은 x축(right, 좌우)에 곱한다.
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;  // 방향에 속력을 곱해준다.
        dir.y = rigidbody.velocity.y;  // y값은 velocity(변화량)의 y 값을 넣어준다.

            rigidbody.velocity = dir+test;  // 연산된 속도를 velocity(변화량)에 넣어준다.
    }

    void CameraLook()
    {
        // 마우스 입력으로 회전 각도 누적
        camCurXRot -= mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        float yaw = transform.eulerAngles.y + mouseDelta.x * lookSensitivity;

        // 쿼터니언 회전값 계산
        Quaternion rotation = Quaternion.Euler(camCurXRot, yaw, 0);

        // 카메라 거리 설정
        float cameraDistance = 1f;
        Vector3 cameraOffset = rotation * new Vector3(0, 0, -cameraDistance);

        // 회전값을 적용한 카메라의 위치,플레이어쪽으로 방향 돌리기
        cameraContainer.position = transform.position + cameraOffset;
        cameraContainer.LookAt(transform.position);
        //플레이어가 카메라따라 y축 회전
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
    {
        //1개의 Ray로는 (경사,낭떠러지등)판별오류가 있기 때문에 4개의 면 느낌의 Ray를 쏨
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        // 4개의 Ray 중 groundLayerMask에 해당하는 오브젝트가 충돌했는지 조회한다.
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.2f, groundLayerMask))
            {
                RaycastHit hit;
                if (Physics.Raycast(rays[i], out hit, 0.2f))
                {
                    float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
                    if (slopeAngle < maxSlopeAngle)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }
    //커서고정,해제
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("JumpHold"))
        {
            rigidbody.AddForce(Vector2.up * 600, ForceMode.Impulse);
        }

    }
}
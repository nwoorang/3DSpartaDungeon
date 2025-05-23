using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 해당 스크립트를 부착한 오브젝트와 다른 오브젝트를 상호작용을 할 수 있게 해줌
/// </summary>
public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;    // 상호작용 오브젝트 체크 시간
    private float lastCheckTime;       // 마지막 상호작용 체크 시간
    public float maxCheckDistance;     // 최대 체크 거리
    public LayerMask layerMask;         //상호작용하고싶은 레이어 인스펙터에서 선택

    public GameObject curInteractGameObject;  // 현재 상호작용 게임오브젝트
    public GameObject StartUI;
    private bool isUI;

    public Stage stage;
    private IInteractable curInteractable;    //ItemObject <- IInteractable 인터페이스

    public TextMeshProUGUI promptText; //출력메시지
    private Camera Cam;

    public GameObject DesciptionUI;
    void Start()
    {
        Cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            Ray ray = Cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                int hitLayer = hit.collider.gameObject.layer;
                if (hitLayer == LayerMask.NameToLayer("Interactable"))
                {
                    if (hit.collider.gameObject != curInteractGameObject) //중복을 방지하기 위해 탐색한게 같은 오브젝트면 패스
                    {
                        Debug.Log("curInteractGameObject맞음");
                        curInteractGameObject = hit.collider.gameObject;
                        curInteractable = hit.collider.GetComponent<IInteractable>();
                        SetPromptText();
                    }
                }
                else if (hitLayer == LayerMask.NameToLayer("UI"))
                {
                    if (hit.collider.gameObject == StartUI)//콜라이더 계속 호출하는 문제 해결해야함
                    {
                        isUI = true;
                        DesciptionUI.SetActive(true);
                    }
                }
            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptText.gameObject.SetActive(false);

                isUI = false;
                DesciptionUI.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context) //E키 누르면 아이템 수집
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }

    public void StartStage(InputAction.CallbackContext callbackContext)//F키 누르면 UI 상호작용
    {
        if (callbackContext.phase == InputActionPhase.Started && isUI)
        {
            stage.StartCoroutine("StageRoutine");
            StartUI.SetActive(false);
        }
    }
}
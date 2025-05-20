using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJTargetChase : MonoBehaviour
{

    public GameObject target;
    public float velocity;
    Vector3 direction;

    public LayerMask layerMask;         //상호작용하고싶은 레이어 인스펙터에서 선택

    [Header("JumpObstacle")]
    RaycastHit hit;
    private Rigidbody rb;


    public Vector3 lastPosition;
    public Vector3 deltaMove;//
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = (target.transform.position - transform.position).normalized;//
        lastPosition = transform.position;//
    }

    void FixedUpdate()
    {
        transform.position += direction * velocity;
        deltaMove = transform.position - lastPosition;//
        lastPosition = transform.position;//
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("TriggerObstacle")) //여러 레이어마스크는 이런식으로 비트계산해줘야함
        {
            Debug.Log("충돌");
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{

    public GameObject target;
    public float velocity;
    Vector3 direction;

    public LayerMask layerMask;         //상호작용하고싶은 레이어 인스펙터에서 선택

    [Header("JumpObstacle")]

    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = (target.transform.position - transform.position).normalized;//
        transform.LookAt(target.transform.position);
    }

    void FixedUpdate()
    {
        // rb.MovePosition(rb.position + direction * velocity * Time.fixedDeltaTime);
               rb.velocity = direction*velocity;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("TriggerObstacle"))//외곽 충돌시 사라지게 함
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{

    public GameObject Target;
    public float Velocity;
    Vector3 Direction;

    public LayerMask layerMask;         //상호작용하고싶은 레이어 인스펙터에서 선택

    [Header("JumpObstacle")]

    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
                        Direction = (Target.transform.position - transform.position).normalized;
        transform.LookAt(Target.transform.position);
    }

    public void InitSet(GameObject target, Vector3 pos, float velocity)
    {
        Target = target;
        transform.position = pos;
        Velocity = velocity;
                        Direction = (Target.transform.position - transform.position).normalized;
        transform.LookAt(Target.transform.position);
    }


    void FixedUpdate()
    {
        // rb.MovePosition(rb.position + direction * velocity * Time.fixedDeltaTime);
               rb.velocity = Direction*Velocity;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("TriggerObstacle"))//외곽 충돌시 사라지게 함
        {
            Destroy(gameObject);
        }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))//jumpHold오브젝트를 밟으면 위로 날림
        {
            collision.rigidbody.AddForce(Direction*Velocity, ForceMode.Impulse);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{

    public GameObject Target;
    public float originVelocity;
    public float Velocity;
    Vector3 Direction;

    public LayerMask layerMask;         //상호작용하고싶은 레이어 인스펙터에서 선택

    [Header("JumpObstacle")]

    private Rigidbody rb;
    public string SerialName;
    public float lifetime;

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

    void OnEnable()
    {
        StartCoroutine("LifeTime");
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifetime);
        PlatformObjectPool.Instance.Release(SerialName, gameObject);
    }
    void FixedUpdate()
    {
        // rb.MovePosition(rb.position + direction * velocity * Time.fixedDeltaTime);
        rb.velocity = Direction * Velocity*originVelocity;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("TriggerObstacle"))//외곽 충돌시 사라지게 함
        {
            PlatformObjectPool.Instance.Release(SerialName, gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))//jumpHold오브젝트를 밟으면 위로 날림
        {
            collision.rigidbody.AddForce(Direction * Velocity*originVelocity, ForceMode.Impulse);
        }

    }
}

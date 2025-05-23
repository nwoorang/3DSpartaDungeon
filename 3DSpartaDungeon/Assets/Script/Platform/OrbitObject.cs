using UnityEngine;

public class RigidbodyOrbitVelocity : MonoBehaviour
{
    public Transform centerPoint;   // 중심 오브젝트
    public float orbitSpeed = 2f;   // 선속도 (유닛/초)
    public float radius = 2f;       // 반지름

    private Rigidbody rb;
    private float currentAngle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        // 시작 위치 설정
        Vector3 startOffset = new Vector3(radius, 0f, 0f);
        rb.position = centerPoint.position + startOffset;
        currentAngle = 0f;
    }

    void FixedUpdate()
    {
        // 현재 각도 증가 (초당 몇 라디안 도는지)
        float angularSpeed = orbitSpeed / radius; // (rad/s)
        currentAngle += angularSpeed * Time.fixedDeltaTime;

        // 현재 위치 → 중심과의 벡터
        Vector3 offset = transform.position - centerPoint.position;

        // 현재 각도에 맞는 방향 계산 (수직 방향)
        Vector3 tangentDir = Vector3.Cross(Vector3.up, offset).normalized;

        // velocity 방향 설정
        rb.velocity = tangentDir * orbitSpeed;
    }
}

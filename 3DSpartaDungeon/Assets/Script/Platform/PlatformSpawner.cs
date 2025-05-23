using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public WeightedTable weightedTable;
    public float spawnDelay;

    public PlatformObjectPool platformObjectPool;
    public float radius;
    //원밖 랜덤 위치에서 생성 1초마다 코루틴써서 update문으로 계속돌리기
    public GameObject Target;
    public float Velocity;
    void OnEnable()
    {
        StartCoroutine("DelaySpawn");
    }

    IEnumerator DelaySpawn()
    {
        while (true)
        {

            try
            {
                float angleDeg = Random.Range(0f, 360f); // y축 기준으로 90도 회전 (오른쪽)
                Quaternion rotation = Quaternion.Euler(0, angleDeg, 0);
                Vector3 dir = rotation * Vector3.forward * radius;
                Vector3 Pos = dir + transform.position;

                GameObject PlatForm = platformObjectPool.Get(weightedTable.GetRandom().name);//오브젝트풀에서 랜덤으로 프리팹과 동일이름인 프리팹 가져오기
                PlatForm.GetComponent<MovePlatform>().InitSet(Target, Pos, Velocity);
                //   MovePlatform moveScript = PlatForm.GetComponent<MovePlatform>();
                //   moveScript.InitSet(Target, Pos);
            }
            catch (System.Exception e)
            {
             //   Debug.LogError("코루틴 예외 발생: " + e);
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}

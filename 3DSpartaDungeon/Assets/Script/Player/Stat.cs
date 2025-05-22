using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //클래스,구조체 직렬화
public class PlayerStatus
{
    public float hp;
    public float hunger;
    public float stamina;
    public float moveSpeed;
    public float jumpPower;
    public float atk;
}


public class Stat : MonoBehaviour
{
    [SerializeField] //클래스내 변수 직렬화
    private PlayerStatus status;

    public PlayerStatus GetStatus()
    {
        return status;
    }
}

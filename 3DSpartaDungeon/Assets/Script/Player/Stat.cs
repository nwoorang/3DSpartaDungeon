using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
    [SerializeField]
    private PlayerStatus status;
}

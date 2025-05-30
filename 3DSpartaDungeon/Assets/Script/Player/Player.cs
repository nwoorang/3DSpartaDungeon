using System;
using UnityEngine;

/// <summary>
/// 플레이어에 각 기능을 부착
/// </summary>
public class Player : MonoBehaviour
{
    [Header("caching")]
    public PlayerController P_controller;
    public PlayerCondition P_condition;

    public Stat P_stat;
        public Stage P_stage;
    public ItemData itemData;
    public Action addItem;
    public Transform dropPosition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        P_controller = GetComponent<PlayerController>();
        P_condition = GetComponent<PlayerCondition>();
        P_stat = GetComponent<Stat>();
        
    }
}
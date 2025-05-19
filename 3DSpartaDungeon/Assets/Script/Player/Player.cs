using System;
using UnityEngine;

/// <summary>
/// 플레이어에 각 기능을 부착
/// </summary>
public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;

    public ItemData itemData;
    public Action addItem;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }
}
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Condition(상태 UI_SlideBar)을 외부에서 변경 가능하게 해주는 클래스
/// </summary>
public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;   // hunger가 0일때 사용할 값 (value > 0)
    public event Action onTakeDamage;   // Damage 받을 때 호출할 Action (6강 데미지 효과 때 사용)

    [Header("caching")]
    Player player;
    PlayerController playerController;
    void Start()
    {
        player = CharacterManager.Instance.Player;
        health.curValue = player.P_stat.GetStatus().hp;
        hunger.curValue = player.P_stat.GetStatus().hunger;
        stamina.curValue = player.P_stat.GetStatus().stamina;
        playerController = player.P_controller;
                        
    }
    private void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue <= 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount) //HP회복
    {
        health.Add(amount);
    }

    public void Eat(float amount) //배고픔 회복
    {
        hunger.Add(amount);
    }

    public void StaminaMinus(float amount)
    {
        stamina.Subtract(amount);
    }

    public void SpeedUp(float amount)
    {
        playerController.moveSpeed += amount;
        StartCoroutine(SpeedUpTimeEnd(amount));
    }

    IEnumerator SpeedUpTimeEnd(float amount)
    {
        yield return new WaitForSeconds(5f);
               playerController.moveSpeed -= amount; 
    }
    public void Die()
    {
        //   Debug.Log("플레이어가 죽었다.");
    }
}
using System;
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

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Die()
    {
     //   Debug.Log("플레이어가 죽었다.");
    }
}
using UnityEngine;

/// <summary>
/// 여러Condition(상태 UI_SlideBar)를 Player에 연결하기 위한 클래스
/// </summary>
public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition= this;
    }
}
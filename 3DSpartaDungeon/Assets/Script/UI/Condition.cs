using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 상태 UI_SlideBar에 대한 스크립트
/// </summary>
public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public float passiveValue;
    public Image uiBar;

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amount)
    {
        // maxValue까지 제한
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        // minValue(0.0f)까지 제한
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    //FillBar채우기 위한 퍼센테이지
    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // UI 컴포넌트 쓸 거면 필요
using TMPro;
using UnityEngine.InputSystem;
public class Stage : MonoBehaviour
{
    //난이도를 만들거임 
    //필요재료-발판맵,스포너,시간,시간ui,클리어ui

    //start 오브젝트에 다가가면 f키를 누르면 시작 이라는 문구 뜨게 해주기
    //시작 후 스포너 위치,시간,시간ui 세팅
    //시간내에 클리어 하면 클리어 ui 세팅,대포 안에 들어가서 R키 누르라고 문구 뜨게 해주기

    //위에 올라가면 2단계 맵이 있고 똑같은 방식 반복
    //가능하다면 여기는 일정 위치 움직이는 플랫폼(여기에 매달리기 추가할듯)+레이저 트랩 맵
    //레이저 트랩-레이가 플레이어를 감지하면 경고 UI 띄우고 문구로 발사속도가 빨라지게함
    // 발판맵 오브젝트 (ex. PlatformMap 부모 오브젝트)

    public GameObject platformMap;

    // 스포너 오브젝트 (ex. 발판 생성기)
    public GameObject spawner;

    // 제한 시간 (초 단위)
    public float timeLimit;

    // 시간 UI (Text 또는 TMP_Text)
    public TMP_Text TitleText;

    public TMP_Text timeText;


    // 클리어 UI 오브젝트 (ex. 클리어했을 때 띄울 패널)
    public GameObject clearUI;

    // 나중에 private 변수도 추가 가능
    private float currentTime;
    private bool isTimerRunning = false;

    public GameObject Canon;
    public GameObject StartUI;
      public AudioSource bgmSource;     // 배경음 오디오소스
    public GameObject FirstGuideDescription;
    private IEnumerator StageRoutine()
    {
        FirstGuideDescription.SetActive(false);
        bgmSource.Play();
        isTimerRunning = true;
        TitleText.text = "Survival Time:";
        spawner.SetActive(true);

        while (timeLimit > 0f)
        {
            timeLimit -= Time.deltaTime;

            // 시간 UI 갱신
            timeText.text = timeLimit.ToString("F2");

            yield return null;
        }

        // 시간 다 되면 처리
        timeLimit = 0f;
        isTimerRunning = false;
        timeText.text = "0.00";

        TitleText.text = "";
        spawner.SetActive(false);
        clearUI.SetActive(true);
        Canon.SetActive(true);
        bgmSource.Stop();
    }

    public void StageUp()
    {
        transform.position = new Vector3(0,121.3f, 0);
        clearUI.SetActive(false);
        Canon.SetActive(false);
        StartUI.SetActive(true);
                     timeLimit = 20f;
 }
}

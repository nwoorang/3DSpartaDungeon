using UnityEngine;

/// <summary>
/// 캐릭터생성,정보전달 해주는 매니저 
/// </summary>
public class CharacterManager : Singleton<CharacterManager>
{

    public Player Player //플레이어 정보 전달
    {
        get { return _player; }
        set { _player = value; }
    }
    private Player _player; //플레이어 생성

}
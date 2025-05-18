using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{

    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }
    private Player _player;

}
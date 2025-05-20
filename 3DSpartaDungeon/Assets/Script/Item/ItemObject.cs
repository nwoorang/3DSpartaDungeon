using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data; //실질적인 데이터가 담겨있는 ScriptableObject

    public string GetInteractPrompt() //아이템정보 출력
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()//아이템 넘겨주고 삭제
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
using UnityEngine;


[CreateAssetMenu(fileName = "NewWeightedTable", menuName = "Game/Weighted Table")]
public class WeightedTable : ScriptableObject
{
    public WeightedGameObject[] objects;

    public GameObject GetRandom()
    {
        float totalWeight = 0f;
        foreach (var obj in objects)
        {
            totalWeight += obj.weight; 
        }

        float rand = Random.Range(0f, totalWeight);
        float current = 0f;

        foreach (var obj in objects)
        {
            current += obj.weight;
            if (rand < current)
            {
                return obj.go;
            }
        }

        return null;
    }
}

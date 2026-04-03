using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "Scriptable Objects/FishData")]
public class FishData : ScriptableObject
{
    [SerializeField] public int id;
    [SerializeField] public string species;
    public enum rarity
    {
        common,
        rare,
        epic,
        legendary,
    }
    public rarity currentRarity;
    [SerializeField] public int price;
    [SerializeField] public Sprite icon;

}

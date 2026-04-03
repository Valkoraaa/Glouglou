using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFishDatabase", menuName = "FishingGame/Fish Database")]
public class FishDatabaseSO : ScriptableObject
{
    [Header("50%")]
    public List<Fish> commonFish = new List<Fish>();

    [Header("30%")]
    public List<Fish> rareFish = new List<Fish>();

    [Header("15%")]
    public List<Fish> epicFish = new List<Fish>();

    [Header("5%")]
    public List<Fish> legendaryFish = new List<Fish>();

}
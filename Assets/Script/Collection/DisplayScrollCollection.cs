using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScrollCollection : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Transform content;   
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private List<FishData> allFishes;

    public void RefreshCollection()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        foreach (FishData fish in allFishes)
        {
            GameObject slot = Instantiate(slotPrefab, content);
            bool isCaught = FishingBookManager.Instance.IsFishCaught(fish.id);
            slot.GetComponent<Collection_Slot>().SetUp(fish, isCaught);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
    }
}
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BookGenerator : MonoBehaviour
{
    [SerializeField] private List<FishData> allFishes;
    [SerializeField] private Transform gridLayoutGroup;
    [SerializeField] private GameObject slotPrefab;

    public static BookGenerator Instance;
    void Awake() { Instance = this; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        RefreshBook();
    }

    public void RefreshBook()
    {
        // On nettoie la grille pour éviter les doublons
        foreach (Transform child in gridLayoutGroup)
        {
            Destroy(child.gameObject);
        }

        // On génère la liste à jour
        foreach (FishData fish in allFishes)
        {
            GameObject newSlot = Instantiate(slotPrefab, gridLayoutGroup);
            Collection_Slot slotScript = newSlot.GetComponent<Collection_Slot>();
            bool caught = FishingBookManager.Instance.IsFishCaught(fish.id);
            slotScript.SetUp(fish, caught);
        }
    }

}

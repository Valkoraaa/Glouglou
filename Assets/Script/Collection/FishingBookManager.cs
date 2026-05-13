using System.Collections.Generic;
using UnityEngine;

public class FishingBookManager : MonoBehaviour
{
    public static FishingBookManager Instance { get; private set; }
    private HashSet<int> caughtFishId = new HashSet<int>();

    [Header("Debug View")]
    [SerializeField] private List<int> caughtFishIdDisplay = new List<int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        ResetData();

        DontDestroyOnLoad(gameObject);
    }

    public void ResetData()
    {
        caughtFishId.Clear();
        caughtFishIdDisplay.Clear();
        Debug.Log("Données du livre de pêche réinitialisées.");
    }

    public HashSet<int> getCaughtFishId()
    {
        return this.caughtFishId;
    }

    public void RegisterCatch(int id, float finalWeight, float finalSize)
    {
        if (!caughtFishId.Contains(id))
        {
            caughtFishId.Add(id);
            caughtFishIdDisplay.Add(id);
            Debug.Log($"ID {id} ajouté au livre !");
        }
    }

    public bool IsFishCaught(int id)
    {
        return caughtFishId.Contains(id);
    }
}

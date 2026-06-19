using System.Collections.Generic;
using UnityEngine;

public class FishingBookManager : MonoBehaviour
{
    public static FishingBookManager Instance { get; private set; }
    private HashSet<int> caughtFishId = new HashSet<int>();

    [Header("Debug View")]
    [SerializeField] private List<int> caughtFishIdDisplay = new List<int>();

    private Dictionary<int, float> bestWeight = new Dictionary<int, float>();
    private Dictionary<int, float> bestSize = new Dictionary<int, float>();

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
        bestWeight.Clear();
        bestSize.Clear();
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
        }

        // Garder le meilleur score
        if (!bestWeight.ContainsKey(id) || finalWeight > bestWeight[id])
            bestWeight[id] = finalWeight;

        if (!bestSize.ContainsKey(id) || finalSize > bestSize[id])
            bestSize[id] = finalSize;
    }

    public float GetBestWeight(int id) => bestWeight.ContainsKey(id) ? bestWeight[id] : 0f;
    public float GetBestSize(int id) => bestSize.ContainsKey(id) ? bestSize[id] : 0f;

    public bool IsFishCaught(int id)
    {
        return caughtFishId.Contains(id);
    }
}

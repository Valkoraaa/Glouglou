using System.Collections.Generic;
using UnityEngine;

public class FishingBookManager : MonoBehaviour
{
    public static FishingBookManager Instance { get; private set; }
    private HashSet<int> caughtFishId = new HashSet<int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // On vérifie si une instance existe déjà pour éviter les doublons
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        // Optionnel : permet au manager de survivre au changement de scène
        DontDestroyOnLoad(this.gameObject);
    }

    public HashSet<int> getCaughtFishId()
    {
        return this.caughtFishId;
    }

    public void RegisterCatch(int id)
    {
        if (!caughtFishId.Contains(id))
        {
            caughtFishId.Add(id);
            Debug.Log($"ID {id} ajouté au livre !");
        }
    }

    public bool IsFishCaught(int id)
    {
        return caughtFishId.Contains(id);
    }
}

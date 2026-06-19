using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class SpawnFish : MonoBehaviour
{
    public static SpawnFish Instance { get; private set; }
    [SerializeField]
    private FishDatabaseSO fishDatabase;

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 zoneSize;
    [SerializeField] private int fishNumber;

    [Header("NavMesh Reference")]
    [SerializeField] private Transform navMeshSurface;

    [Header("Aura Fish")]
    private GameObject aura;
    [SerializeField] private GameObject commonAura;
    [SerializeField] private GameObject rareAura;
    [SerializeField] private GameObject epicAura;
    [SerializeField] private GameObject lengendaryAura;



    void Start()
    {
        Instance = this;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, zoneSize);
    }

    public void SpawningFish()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Fish>() != null)
            {
                Destroy(child.gameObject);
            }
        }
        ResetFishDatabase();

        for (int i = 0; i < fishNumber; i++)
        {
            Fish fishToInstantiate = GetRandomFish();

            Vector3 randomLocalPos = new Vector3(
                Random.Range(-zoneSize.x / 2f, zoneSize.x / 2f),
                0f,
                Random.Range(-zoneSize.z / 2f, zoneSize.z / 2f)
            );

            Vector3 spawnPos = transform.TransformPoint(randomLocalPos);

            // On force la hauteur du NavMesh
            spawnPos.y = navMeshSurface.position.y;

            Fish instantiated = Instantiate(fishToInstantiate,spawnPos,Quaternion.identity,this.transform);
            instantiated.IsBadForToday = fishToInstantiate.IsBadForToday;
            instantiated.FishEffect = fishToInstantiate.FishEffect;
            GameObject tempAura = Instantiate(aura, new Vector3(instantiated.transform.position.x, instantiated.transform.position.y, instantiated.transform.position.z + 0.1f ), Quaternion.identity, instantiated.transform);
            //tempAura.transform.SetParent(instantiated.transform);
            NavMeshAgent agent = instantiated.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(spawnPos);
            }
            
        }
    }

    public void ResetFishDatabase()
    {
        foreach (var f in fishDatabase.commonFish) { f.IsBadForToday = false; f.FishEffect = "none"; }
        foreach (var f in fishDatabase.rareFish) { f.IsBadForToday = false; f.FishEffect = "none"; }
        foreach (var f in fishDatabase.epicFish) { f.IsBadForToday = false; f.FishEffect = "none"; }
        foreach (var f in fishDatabase.legendaryFish) { f.IsBadForToday = false; f.FishEffect = "none"; }
    }

    private Fish GetRandomFish()
    {
        int randRarity = Random.Range(0, 100);
        List<Fish> targetList;

        if (randRarity < 50)
        {
            targetList = fishDatabase.commonFish;
            aura = commonAura;
        }
        else if (randRarity < 80)
        {
            targetList = fishDatabase.rareFish; 
            aura = rareAura;
        }
        else if (randRarity < 95)
        { 
            targetList = fishDatabase.epicFish;
            aura = epicAura;
        }
        else
        { 
            targetList = fishDatabase.legendaryFish;
            aura = lengendaryAura;
        }
        return targetList[Random.Range(0, targetList.Count)];
    }
}

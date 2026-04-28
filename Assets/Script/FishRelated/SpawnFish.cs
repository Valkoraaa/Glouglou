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

            Fish instantiated = Instantiate(
                fishToInstantiate,
                spawnPos,
                Quaternion.identity,
                this.transform
            );
            NavMeshAgent agent = instantiated.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(spawnPos);
            }
        }
    }

    private Fish GetRandomFish()
    {
        int randRarity = Random.Range(0, 100);
        List<Fish> targetList;

        if (randRarity < 50)
        {
            targetList = fishDatabase.commonFish;
        }
        else if (randRarity < 80)
        {
            targetList = fishDatabase.rareFish; 
        }
        else if (randRarity < 95)
        { 
            targetList = fishDatabase.epicFish;
        }
        else
        { 
            targetList = fishDatabase.legendaryFish;
        }
        return targetList[Random.Range(0, targetList.Count)];
    }
}

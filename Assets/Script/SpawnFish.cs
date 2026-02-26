using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class SpawnFish : MonoBehaviour
{
    //50% to spawn
    [SerializeField]
    private GameObject commonFish1;
    [SerializeField]
    private GameObject commonFish2;
    [SerializeField]
    private GameObject commonFish3;
    [SerializeField]
    private GameObject commonFish4;
    [SerializeField]
    private GameObject commonFish5;
    [SerializeField]
    private GameObject commonFish6;
    [SerializeField]
    private GameObject commonFish7;
    //30% to spawn
    [SerializeField]
    private GameObject rareFish1;
    [SerializeField]
    private GameObject rareFish2;
    [SerializeField]
    private GameObject rareFish3;
    [SerializeField]
    private GameObject rareFish4;
    //15% to spawn
    [SerializeField]
    private GameObject epicFish1;
    [SerializeField]
    private GameObject epicFish2;
    [SerializeField]
    private GameObject epicFish3;
    //5% to spawn
    [SerializeField]
    private GameObject legendaryFish;

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 zoneSize;
    [SerializeField] private int fishNumber;

    [Header("NavMesh Reference")]
    [SerializeField] private Transform navMeshSurface;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, zoneSize);
    }

    private void Start()
    {
        for (int i = 0; i < fishNumber; i++)
        {
            GameObject fishToInstantiate = GetRandomFish();

            Vector3 randomLocalPos = new Vector3(
                Random.Range(-zoneSize.x / 2f, zoneSize.x / 2f),
                0f,
                Random.Range(-zoneSize.z / 2f, zoneSize.z / 2f)
            );

            Vector3 spawnPos = transform.TransformPoint(randomLocalPos);

            // On force la hauteur du NavMesh
            spawnPos.y = navMeshSurface.position.y;

            GameObject instantiated = Instantiate(fishToInstantiate, spawnPos, Quaternion.identity);

            NavMeshAgent agent = instantiated.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(spawnPos);
            }
        }
    }

    private GameObject GetRandomFish()
    {
        int randRarity = Random.Range(0, 100);

        if (randRarity <= 50)
        {
            GameObject[] commons = {
                commonFish1, commonFish2, commonFish3,
                commonFish4, commonFish5, commonFish6, commonFish7
            };
            return commons[Random.Range(0, commons.Length)];
        }
        else if (randRarity <= 80)
        {
            GameObject[] rares = {
                rareFish1, rareFish2, rareFish3, rareFish4
            };
            return rares[Random.Range(0, rares.Length)];
        }
        else if (randRarity <= 95)
        {
            GameObject[] epics = {
                epicFish1, epicFish2, epicFish3
            };
            return epics[Random.Range(0, epics.Length)];
        }
        else
        {
            return legendaryFish;
        }
    }
}

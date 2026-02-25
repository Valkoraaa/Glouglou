using System.Collections;
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

    [SerializeField]
    private Vector3 zoneSize;

    [SerializeField]
    private int fishNumber;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }

    private void Start()
    {
        GameObject fishToInstantiate;

        for(int i = 0; i < fishNumber; i++)
        {
            int randRarity = Random.Range(0, 100);
            if (randRarity <= 50)
            {
                int randFishCommon = Random.Range(0, 100);
                switch (randFishCommon)
                {
                    case <= 14:
                        fishToInstantiate = commonFish1;
                        break;
                    case <= 28:
                        fishToInstantiate = commonFish2;
                        break;
                    case <= 42:
                        fishToInstantiate = commonFish3;
                        break;
                    case <= 56:
                        fishToInstantiate = commonFish4;
                        break;
                    case <= 70:
                        fishToInstantiate = commonFish5;
                        break;
                    case <= 84:
                        fishToInstantiate = commonFish6;
                        break;
                    case <= 98:
                        fishToInstantiate = commonFish7;
                        break;
                    default:
                        fishToInstantiate = commonFish1;
                        break;
                }
            }
            else if (randRarity <= 80)
            {
                int randFish = Random.Range(0, 100);
                switch (randFish)
                {
                    case <= 25:
                        fishToInstantiate = rareFish1;
                        break;
                    case <= 50:
                        fishToInstantiate = rareFish2;
                        break;
                    case <= 75:
                        fishToInstantiate = rareFish3;
                        break;
                    case <= 100:
                        fishToInstantiate = rareFish4;
                        break;
                    default:
                        fishToInstantiate = rareFish1;
                        break;
                }
            }
            else if (randRarity <= 95)
            {
                int randFish = Random.Range(0, 100);
                switch (randFish)
                {
                    case <= 33:
                        fishToInstantiate = epicFish1;
                        break;
                    case <= 66:
                        fishToInstantiate = epicFish2;
                        break;
                    case <= 99:
                        fishToInstantiate = epicFish3;
                        break;
                    default:
                        fishToInstantiate = epicFish1;
                        break;
                }
            }
            else
            {
                fishToInstantiate = legendaryFish;
            }
            GameObject instantiated = Instantiate(fishToInstantiate);

            Vector3 spawnPos = new Vector3(
                Random.Range(transform.position.x - zoneSize.x / 2, transform.position.x + zoneSize.x / 2),
                Random.Range(transform.position.y - zoneSize.y / 2, transform.position.y + zoneSize.y / 2),
                Random.Range(transform.position.z - zoneSize.z / 2, transform.position.z + zoneSize.z / 2)
            );

            instantiated.transform.position = spawnPos;

            NavMeshAgent agent = instantiated.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.Warp(spawnPos);
            }
        }
    }
}

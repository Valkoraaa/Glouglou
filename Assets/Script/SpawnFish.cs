using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnFish : MonoBehaviour
{
    //50% to spawn
    [SerializeField]
    private GameObject commonFish;
    //30% to spawn
    [SerializeField]
    private GameObject rareFish;
    //15% to spawn
    [SerializeField]
    private GameObject epicFish;
    //5% to spawn
    [SerializeField]
    private GameObject legendaryFish;

    [SerializeField]
    private Vector3 zoneSize;

    [SerializeField]
    private int fishNumber;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, zoneSize);
    }

    private void Start()
    {
        GameObject fishToInstantiate;

        for(int i = 0; i < fishNumber; i++)
        {
            int rand = Random.Range(0, 100);
            if(rand <= 50)
            {
                fishToInstantiate = commonFish;
            }
            else if (rand <= 70)
            {
                fishToInstantiate = rareFish;
            }
            else if (rand <= 95)
            {
                fishToInstantiate = epicFish;
            }
            else
            {
                fishToInstantiate = legendaryFish;
            }
            GameObject instantiated = Instantiate(fishToInstantiate);
            instantiated.transform.position = new Vector3(
                Random.Range(transform.position.x - zoneSize.x / 2, transform.position.x + zoneSize.x / 2),
                Random.Range(transform.position.y - zoneSize.y / 2, transform.position.y + zoneSize.y / 2),
                Random.Range(transform.position.z - zoneSize.z / 2, transform.position.z + zoneSize.z / 2)
                );
        }
    }
}

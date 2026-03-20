using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
public class FishPatrol : MonoBehaviour
{

    private NavMeshAgent fish;
    private Vector3 destination;
    private bool walkPointSet;
    [SerializeField]
    private float range;
    [SerializeField]
    private float fishSpeed = 3f;
    private float timerIdle;
    private bool isIdle = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fish = GetComponent<NavMeshAgent>();
        fish.speed = fishSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isIdle == false)
        {
            Patrol();
        }
        if (isIdle == true)
        {
            timerIdle -= Time.deltaTime;
            if(timerIdle <= 0)
            {
                isIdle = false;
                fish.isStopped = false;
                Patrol();
            }
        }
    }

    void Patrol()
    {
        if (fish == null) return;

        if (!fish.isOnNavMesh) return;
        if (!walkPointSet)
        {
            SearchForDest();
        }
        if (walkPointSet)
        {
            fish.SetDestination(destination);
        }
        if (fish.remainingDistance < 0.5)
        {
            timerIdle = Random.Range(0, 2.5f);
            fish.isStopped = true;
            walkPointSet = false;
            isIdle = true;
        }
    }

    void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);

        destination = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        if (Physics.Raycast(destination, Vector3.down))
        {
            walkPointSet = true;
        }
    }
}

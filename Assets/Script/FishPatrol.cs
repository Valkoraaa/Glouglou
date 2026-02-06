using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
public class FishPatrol : MonoBehaviour
{

    public NavMeshAgent fish;
    public Vector3 destination;
    public bool walkPointSet;
    [SerializeField]
    float WalkRange;
    public float range;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fish = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (!walkPointSet)
        {
            SearchForDest();
        }
        if(walkPointSet)
        {
            fish.SetDestination(destination);
        }
        if (Vector3.Distance(transform.position, destination) < 10) walkPointSet = false;
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

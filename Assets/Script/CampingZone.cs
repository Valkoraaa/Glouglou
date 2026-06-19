using UnityEngine;

public class CampingZone : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //ThrowLasso.Instance.canThrow = false;
            Debug.Log("playerInCamping");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //ThrowLasso.Instance.canThrow = true;
            Debug.Log("playerOutCamping");
        }
    }
}

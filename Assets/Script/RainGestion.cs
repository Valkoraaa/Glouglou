using UnityEngine;

public class RainGestion : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    float yOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = playerTransform.position + Vector3.up/* yOffset*/;
    }
}

using UnityEngine;

public class lightInstance : MonoBehaviour
{
    public static lightInstance Instance { get; private set; }
    public Light light;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

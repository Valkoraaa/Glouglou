using UnityEngine;

public class TextInteract : MonoBehaviour
{
    public static TextInteract Instance { get; private set; }

    void Start()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        //animation
    }
}

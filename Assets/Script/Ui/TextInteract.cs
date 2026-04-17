using UnityEngine;
using TMPro;

public class TextInteract : MonoBehaviour
{
    public static TextInteract Instance { get; private set; }
    public TextMeshPro txtInteract;
    public GameObject txtObject;

    void Start()
    {
        Instance = this;
        txtInteract = GetComponent<TextMeshPro>();
        txtObject = GetComponent<GameObject>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        //animation
    }
}

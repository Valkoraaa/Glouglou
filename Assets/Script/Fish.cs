using UnityEngine;

[System.Serializable]
public class Fish : MonoBehaviour
{
    [Header ("Caracteritique")]
    [SerializeField] private string species;
    [SerializeField] private float size;
    [SerializeField] private float weight;
    [SerializeField] private string temporaryEffect;
    [SerializeField] private string permanentEffect;
    [SerializeField] private int isPermanent;
    [SerializeField] private int speed;

    [SerializeField] private bool isFind;
    [SerializeField] private bool isHooked; 
    [SerializeField] private Material baseMaterial;
    [SerializeField] private Material HookedMaterial;

    /*private GameObject fishObject;

    private void Awake()
    {
        fishObject = GetComponent<GameObject>();
    }*/

    private void Update()
    {
        Physics.Raycast(Vector3.zero, Vector3.up);
    }
   /* void OnDrawGizmos()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * 50f);
    } */
    

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public float Size
    {
        get { return size; }
        set { size = value; }
    }

    public float Weight
    {
        get { return weight; }
        set { weight = value; }
    }

    public string TemporaryEffect
    {
        get { return temporaryEffect; }
        set { temporaryEffect = value; }
    }

    public string PermanentEffect
    {
        get { return permanentEffect; }
        set { permanentEffect = value; }
    }

    public int IsPermanent
    {
        get { return isPermanent; }
        set { isPermanent = value; }
    }

    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }
}

using UnityEngine;
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * 500f);
    }


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

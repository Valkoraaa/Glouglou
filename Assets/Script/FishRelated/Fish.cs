using UnityEngine;
public class Fish : MonoBehaviour
{
    [Header ("Caracteritique")]
    
    [SerializeField] private float size;
    [SerializeField] private float weight;
    //wind, drunk, ...
    [SerializeField] private string temporaryEffect;
    [SerializeField] private string permanentEffect;
    [SerializeField] private int isPermanent;
    [SerializeField] private int speed;
    [SerializeField] private bool isBadForToday;
    [SerializeField] public Material baseMaterial;
    [SerializeField] private Material HookedMaterial;
    public FishData data;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.up * 500f);
    }


    public bool IsBadForToday
    {
        get { return isBadForToday; }
        set { isBadForToday = value; }
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

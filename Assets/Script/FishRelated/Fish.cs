using UnityEngine;
public class Fish : MonoBehaviour
{
    [Header ("Caracteritique")]
    
    [SerializeField] private float size;
    [SerializeField] private float weight;
    //wind, drunk, ...
    [SerializeField] private string effect;
    [SerializeField] private int isPermanent;
    [SerializeField] private int speed;
    [SerializeField] private bool isBadForToday;
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

    public string Effect
    {
        get { return effect; }
        set { effect = value; }
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

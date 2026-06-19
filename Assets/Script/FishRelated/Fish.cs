using UnityEngine;
public class Fish : MonoBehaviour
{
    [Header ("Caracteritique")]
    
    [SerializeField] private float size;
    [SerializeField] private float weight;
    //wind, drunk, ...
    [SerializeField] private string fishEffect;
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

    public string FishEffect
    {
        get { return fishEffect; }
        set { fishEffect = value; }
    }
}

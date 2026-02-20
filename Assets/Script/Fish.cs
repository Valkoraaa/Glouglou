using UnityEngine;

[System.Serializable]
public class Fish
{
    [Header ("Caracteritique")]
    [SerializeField] private string name;
    [SerializeField] private float size;
    [SerializeField] private float weight;
    [SerializeField] private string temporaryEffect;
    [SerializeField] private string permanentEffect;
    [SerializeField] private int isPermanent;
    [SerializeField] private int speed;

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

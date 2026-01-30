using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private bool[] effects = { false, false, false, false };
    public static EffectManager Instance { get; private set; }


    void Awake()
    {
        Instance = this;
    }
    public void ResetEffect()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i] = false;
        }
    }
    public void ChooseEffect(string effect)
    {
        switch (effect)
        {
            case "wind":
                if (Random.value <= 0.2f)
                {
                    effects[0] = true;
                }
                break;
            case "drunk":
                if (Random.value <= 0.2f)
                {
                    effects[1] = true;
                }
                break;
            case "exhaust":
                if (Random.value <= 0.2f)
                {
                    effects[2] = true;
                }
                break;
            case "sick":
                if (Random.value <= 0.2f)
                {
                    effects[3] = true;
                }
                break;
        }   
    }

    public void ApplyEffect()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i])
            {
                Debug.Log("Effect " + i + " is active");
            }
        }
    }
}

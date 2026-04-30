using UnityEngine;
using UnityEngine.UI;
using TMPro; // Important si tu utilises TextMeshPro

public class Collection_Slot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image displayIcon;
    [SerializeField] private TMP_Text displayName; 

    [Header("Settings")]
    [SerializeField] private Sprite unknownSprite;

    public void SetUp(FishData data, bool isCaught)
    {
        if (isCaught)
        {
            displayIcon.sprite = data.icon;
            displayName.text = data.species;
            displayIcon.color = Color.white;
        }
        else
        {
            displayIcon.sprite = unknownSprite;
            displayName.text = "???";
        }
    }
}
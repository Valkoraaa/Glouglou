using UnityEngine;
using UnityEngine.UI;

public class Collection_Slot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image displayIcon;

    [Header("Settings")]
    [SerializeField] private Sprite unknownSprite;

    public void SetUp(FishData data, bool isCaught)
    {
        displayIcon.sprite = isCaught ? data.icon : unknownSprite;
        displayIcon.color = Color.white;
    }
}
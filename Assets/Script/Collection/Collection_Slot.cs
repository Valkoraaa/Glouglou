using UnityEngine;
using UnityEngine.UI;

public class Collection_Slot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image displayIcon;
    [Header("Settings")]
    [SerializeField] private Sprite unknownSprite;

    public void SetUp(FishData data, bool isCaught, CollectionGestion manager)
    {
        displayIcon.sprite = isCaught ? data.icon : unknownSprite;
        displayIcon.color = Color.white;

        Button btn = GetComponent<Button>();
        if (isCaught)
        {
            btn.onClick.AddListener(() => manager.DisplayFishInfo(data));
        }
    }
}
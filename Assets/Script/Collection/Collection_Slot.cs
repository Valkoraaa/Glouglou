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

        RectTransform iconRect = displayIcon.GetComponent<RectTransform>();
        if (isCaught)
        {
            iconRect.sizeDelta = new Vector2(30f, 30f); // taille fixe, ajuste ce chiffre
            iconRect.localScale = Vector3.one;
            displayIcon.preserveAspect = true;
        }

        Button btn = GetComponent<Button>();
        if (isCaught)
        {
            btn.onClick.AddListener(() => manager.DisplayFishInfo(data));
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class SickEffect : MonoBehaviour
{
    [Tooltip("Vitesse de défilement en unités UV par seconde (X = horizontal, Y = vertical)")]
    public Vector2 scrollSpeed = new Vector2(0f, -0.08f);

    [Tooltip("Transparence globale de l'effet (0 = invisible, 1 = opaque)")]
    [Range(0f, 1f)]
    public float alpha = 0.4f;

    private RawImage rawImage;
    private Rect uvRect;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
        uvRect = rawImage.uvRect;
    }

    void Update()
    {
        uvRect.position += scrollSpeed * Time.deltaTime;
        rawImage.uvRect = uvRect;
    }
}
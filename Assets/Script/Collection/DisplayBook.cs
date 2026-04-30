using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBook : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private List<Transform> pages;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private List<FishData> allFishes;

    [Header("Réglages")]
    [SerializeField] private int slotsPerPage = 9;
    [SerializeField] private float pageSpeed = 2f;

    [Header("UI Buttons")]
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject nextButton;

    private int currentPageIndex = 0; // On suit l'index de la PAGE (0, 1, 2...)
    private bool rotating = false;

    private void Start()
    {

    }

    public void RefreshBook()
    {
        currentPageIndex = 0;

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].rotation = Quaternion.identity;

            Transform fb = pages[i].Find("FaceBefore");
            Transform fa = pages[i].Find("FaceAfter");

            if (fb) fb.gameObject.SetActive(true);
            if (fa)
            {
                fa.gameObject.SetActive(false);
                fa.localScale = new Vector3(-1, 1, 1);
            }

            PopulateFace(fb, i * 2);
            PopulateFace(fa, (i * 2) + 1);
        }

        UpdateButtons();
    }



    private void PopulateFace(Transform face, int faceIdx)
    {
        if (face == null) return;
        Transform grid = face.Find("GridContainer");
        if (grid == null) return;

        foreach (Transform child in grid) Destroy(child.gameObject);

        int startIdx = faceIdx * slotsPerPage;
        for (int i = startIdx; i < startIdx + slotsPerPage && i < allFishes.Count; i++)
        {
            GameObject slot = Instantiate(slotPrefab, grid);
            bool isCaught = FishingBookManager.Instance.IsFishCaught(allFishes[i].id);
            slot.GetComponent<Collection_Slot>().SetUp(allFishes[i], isCaught);
        }
    }

    public void RotateNext()
    {
        if (rotating || currentPageIndex >= pages.Count) return;

        StartCoroutine(AnimateRotation(pages[currentPageIndex], 0f, 180f, true));
        currentPageIndex++;
        UpdateButtons();
    }

    public void RotateBack()
    {
        if (rotating || currentPageIndex <= 0) return;

        currentPageIndex--;
        StartCoroutine(AnimateRotation(pages[currentPageIndex], 180f, 0f, false));
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        backButton.SetActive(currentPageIndex > 0);
        nextButton.SetActive(currentPageIndex < pages.Count);
    }

    IEnumerator AnimateRotation(Transform page, float startAngle, float endAngle, bool forward)
    {
        rotating = true;
        page.SetAsLastSibling(); // Met la page au-dessus des autres pendant qu'elle tourne

        Transform fb = page.Find("FaceBefore");
        Transform fa = page.Find("FaceAfter");

        float elapsed = 0f;
        float duration = 1f / pageSpeed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float angle = Mathf.Lerp(startAngle, endAngle, t);
            page.rotation = Quaternion.Euler(0, angle, 0);

            // LE FIX : Au milieu de la rotation (90°), on switch les faces
            if (t >= 0.5f)
            {
                fb.gameObject.SetActive(!forward);
                fa.gameObject.SetActive(forward);
            }
            yield return null;
        }

        page.rotation = Quaternion.Euler(0, endAngle, 0);
        rotating = false;
    }
}
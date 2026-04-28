using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;

public class DisplayBook : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private List<Transform> pages;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private List<FishData> allFishes;

    [Header("Réglages")]
    [SerializeField] private int slotsPerPage = 9;
    [SerializeField] private float pageSpeed = 1f;

    [Header("UI Buttons")]
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject nextButton;

    private int index = 0;
    private bool rotating = false;

    private void Start()
    {
        pages[0].SetAsLastSibling();
        PopulatePage(0);
        UpdateButtons();
    }

    public void DisplayMouse(bool open)
    {
        Cursor.visible = open;
        if (open)
        {
            Cursor.lockState = CursorLockMode.None;
            Character.Instance.canMove = false;
            Character.Instance.canMoveCam = false;
        }
        else
        {
            Character.Instance.canMove = true;
            Character.Instance.canMoveCam = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void PopulatePage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= pages.Count) return;

        Transform grid = pages[pageIndex].Find("FaceBefore/GridContainer");
        if (grid == null)
        {
            Debug.LogError($"GridContainer introuvable sur {pages[pageIndex].name}");
            return;
        }

        foreach (Transform child in grid) Destroy(child.gameObject);

        int startIdx = pageIndex * slotsPerPage;
        for (int i = startIdx; i < startIdx + slotsPerPage; i++)
        {
            if (i >= allFishes.Count) break;
            GameObject slot = Instantiate(slotPrefab, grid);
            bool isCaught = FishingBookManager.Instance.IsFishCaught(allFishes[i].id);
            slot.GetComponent<Collection_Slot>().SetUp(allFishes[i], isCaught);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(grid.GetComponent<RectTransform>());
    }

    public void RotateNext()
    {
        if (rotating || index >= pages.Count - 1) return;
        index++;
        PopulatePage(index);
        pages[index].SetAsLastSibling();
        StartCoroutine(Rotate(pages[index], true));
        UpdateButtons();
    }

    public void RotateBack()
    {
        if (rotating || index <= 0) return;
        PopulatePage(index - 1);
        pages[index].SetAsLastSibling(); 
        StartCoroutine(Rotate(pages[index], false));
        index--;
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        backButton.SetActive(index > 0);
        nextButton.SetActive(index < pages.Count - 1); 
    }

    IEnumerator Rotate(Transform pageToRotate, bool forward)
    {
        rotating = true;
        float halfDuration = (1.0f / pageSpeed) / 2f;
        float elapsed = 0f;

        Quaternion startRot = pageToRotate.rotation;
        Quaternion midRot = Quaternion.Euler(0, 90, 0);

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            pageToRotate.rotation = Quaternion.Slerp(startRot, midRot,
                                        Mathf.Clamp01(elapsed / halfDuration));
            yield return null;
        }
        pageToRotate.rotation = midRot;


        float scaleX = forward ? -1f : 1f;

        pageToRotate.localScale = new Vector3(
            scaleX, pageToRotate.localScale.y, pageToRotate.localScale.z);

        Transform faceBefore = pageToRotate.Find("FaceBefore");
        if (faceBefore != null)
            faceBefore.localScale = new Vector3(
                scaleX, faceBefore.localScale.y, faceBefore.localScale.z);


        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            pageToRotate.rotation = Quaternion.Slerp(midRot, Quaternion.identity,
                                        Mathf.Clamp01(elapsed / halfDuration));
            yield return null;
        }

        pageToRotate.rotation = Quaternion.identity;
        rotating = false;
    }
}
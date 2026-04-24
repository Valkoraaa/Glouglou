using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class DisplayBook : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private List<Transform> pages; // Glisse Page1, Page2... ici
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private List<FishData> allFishes;

    [Header("Réglages")]
    [SerializeField] private int slotsPerPage = 9;
    [SerializeField] private float pageSpeed;

    [Header("UI Buttons")]
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject nextButton;

    private int index = 0;
    private bool rotate = false;

    public void Start()
    {
        Debug.Log("--- Démarrage DisplayBook ---");
        backButton.SetActive(false);

        if (pages.Count > 0)
        {
            Debug.Log("Start: Peuplement de la page 0");
            PopulatePage(0);
        }
        else
        {
            Debug.LogError("Start: La liste 'pages' est vide !");
        }
        DisplayMouse(true);
    }
    public void DisplayMouse(bool open) //true to open, false to close
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
        Transform grid = pages[pageIndex].Find("PageSprite/GridContainer");

        if (grid == null)
        {
            Debug.LogError($"PopulatePage: GridContainer introuvable sur {pages[pageIndex].name}. Vérifie le chemin 'PageSprite/GridContainer'");
            return;
        }

        Debug.Log($"PopulatePage: Remplissage de {pages[pageIndex].name} (Index: {pageIndex})");

        foreach (Transform child in grid) Destroy(child.gameObject);

        int startIdx = pageIndex * slotsPerPage;
        int endIdx = startIdx + slotsPerPage;

        Debug.Log($"PopulatePage: Génération des slots de {startIdx} à {Mathf.Min(endIdx, allFishes.Count)}");

        for (int i = startIdx; i < endIdx; i++)
        {
            if (i >= allFishes.Count) break;

            GameObject slot = Instantiate(slotPrefab, grid);
            bool isCaught = FishingBookManager.Instance.IsFishCaught(allFishes[i].id);
            slot.GetComponent<Collection_Slot>().SetUp(allFishes[i], isCaught);
            Debug.Log($"Slot ajouté : {allFishes[i].species} (Caught: {isCaught})");
        }
    }

    private void PopulateCurrentPage()
    {
        Transform grid = pages[index].Find("PageSprite/GridContainer");
        if(grid == null)
        {
            Debug.LogError("Erreur : GridContainer non trouvé");
        }
        if (grid != null)
        {
            PopulatePage(index);
        }
        else
        {
            Debug.LogError("GridContainer introuvable sur la page " + index);
        }
    }
    public void RotateNext()
    {
        if (rotate || index >= pages.Count - 1) return;

        index++;
        PopulateCurrentPage();
        pages[index].SetAsLastSibling();
        StartCoroutine(Rotate(pages[index], 180, true));

        UpdateButtons();
    }

    public void ForwardButtonActions()
    {
        if (backButton.activeInHierarchy == false)
        {
            backButton.SetActive(true);
        }
        if (index == pages.Count - 1)
        {
            nextButton.SetActive(false);
        }
    }

    public void RotateBack()
    {
        if (rotate || index < 0) return;

        // On affiche le contenu de la page qu'on révèle
        PopulatePage(index);

        pages[index + 1].SetAsLastSibling(); // La page qui revient vers l'arrière
        StartCoroutine(Rotate(pages[index + 1], 0, false));
        index--;
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        backButton.SetActive(index >= 0);
        nextButton.SetActive(index < pages.Count - 2);
    }

    public void BackButtonActions()
    {
        if (nextButton.activeInHierarchy == false)
        {
            nextButton.SetActive(true);
        }
        if (index - 1 == 1)
        {
            backButton.SetActive(false);
        }
    }

    IEnumerator Rotate(Transform pageToRotate, float targetYAngle, bool forward)
    {
        rotate = true;
        float duration = 1.0f / pageSpeed;
        float elapsed = 0f;
        Quaternion startRotation = pageToRotate.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, targetYAngle, 0);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            pageToRotate.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        pageToRotate.rotation = targetRotation;

        rotate = false;
    }
}

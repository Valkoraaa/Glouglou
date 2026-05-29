using System.Collections;
using UnityEngine;

public class MarchandController : MonoBehaviour
{
    private Animator myAnimator;

    private void Start()
    {
        myAnimator = GetComponentInChildren<Animator>();

        // On lance la routine d'attente pour l'enregistrement
        StartCoroutine(AttendreEtSEnregistrer());
    }

    private IEnumerator AttendreEtSEnregistrer()
    {
        // Tant que le Shop n'est pas chargé ou initialisé, on attend la frame suivante
        while (Shop.Instance == null)
        {
            yield return null;
        }

        // Une fois que le Shop existe enfin, on s'enregistre !
        Shop.Instance.InitialiserBoutique(this);
        Debug.Log("marchand charger");
    }

    public void AskEmote(EmoteType emote)
    {
        if (AnimationManager.Instance != null && myAnimator != null)
        {
            AnimationManager.Instance.DeclencherAnimation(myAnimator, emote);
        }
    }
}
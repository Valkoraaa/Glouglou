using UnityEngine;

public enum EmoteType
{
    Idle = 0,
    Discussion = 1,
    Rire = 2,
    Coucou = 3,
    SautDeJoie = 4
}

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }

    private void Awake()
    {
        // Sécurité Singleton pour les scènes additives
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void DeclencherAnimation(Animator targetAnimator, EmoteType emote)
    {
        targetAnimator.SetInteger("AnimationID", (int)emote);
        targetAnimator.SetTrigger("Action");
    }
}
using UnityEngine;

public class MarchandController : MonoBehaviour
{
    private Animator myAnimator;

    private void Start()
    {
        myAnimator = GetComponentInChildren<Animator>();
    }
    public void AskEmote(EmoteType emote)
    {
        if (AnimationManager.Instance != null)
        {
            AnimationManager.Instance.DeclencherAnimation(myAnimator, emote);
        }
    }
}
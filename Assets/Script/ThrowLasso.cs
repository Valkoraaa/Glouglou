using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowLasso : MonoBehaviour
{
    //public bool isFishing;

    [Header("Références")]
    [SerializeField] private GameObject lasso;
    [SerializeField] private Animator animator; // ANIMATION : Référence de l'animator des bras
    public Rigidbody rb;
    public Camera cam;
    private BoxCollider boxCollider;
    private Rigidbody rbPlayer;
    private CharacterController chaControll;
    private bool waitLag = false;

    [Header("Paramètres")]
    public float force = 15f;

    public bool hasThrown;
    public bool canThrow;
    public bool recallRope;
    private bool isChild;
    public static ThrowLasso Instance { get; private set; }
    private int layerMask;
    public AudioSource lassoAudio;
    [SerializeField] private AudioClip plouf1;
    [SerializeField] private AudioClip plouf2;
    [SerializeField] private AudioClip throw1;
    [SerializeField] private AudioClip throw2;
    [SerializeField] private AudioClip noThrow;
    public AudioClip getLasso;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        lasso.gameObject.SetActive(false);
        rbPlayer = Character.Instance.gameObject.GetComponent<Rigidbody>();
        force = 15; //usefull ? 
        rb = lasso.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        boxCollider = lasso.GetComponent<BoxCollider>();
        //canThrow = true;
        chaControll = GetComponent<CharacterController>();
        layerMask = ~LayerMask.GetMask("Bordure", "Player");
        StartCoroutine(WaitForLag());
        // ANIMATION : Sécurité si tu oublies de glisser l'animator dans l'inspecteur
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        if (chaControll.isGrounded && !DialogueManager.Instance.isInDialogue && !Character.Instance.cinematic && waitLag) { canThrow = true; }
        else { canThrow = false; }

        // if (Keyboard.current.rKey.wasPressedThisFrame) //temp
        // {
        //     hasLasso();
        //     // Character.Instance.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //     // Character.Instance.canMove = true;
        // }
        if (!hasThrown && !isChild)
        {
            GetLasso();
            
        }


        if (!hasThrown && Mouse.current.leftButton.wasPressedThisFrame && canThrow && PauseManager.Instance.canPause && DayManager.Instance.numberOfFails < DayManager.Instance.numberOfFailsAllowed && !CollectionGestion.Instance.collectionCanva.gameObject.activeSelf)//Mouse.current.leftButton.isPressed)
        {
            // ANIMATION : On déclenche le lancer visuel des bras
            if (animator != null)
            {
                lasso.gameObject.SetActive(true);
                animator.SetTrigger("TriggerLasso");
            }

            Character.Instance.stopChara = true;
            rbPlayer.linearVelocity = Vector3.zero;
            rbPlayer.isKinematic = true;
            Character.Instance.canMove = false;
            //Character.Instance.canMoveCam = false; ?
            lasso.transform.SetParent(null);

            rb.isKinematic = false;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            hasThrown = true;

            // Raycast depuis le centre de l'écran
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            Vector3 targetPoint;

            if (Physics.Raycast(ray, out hit, 50f, layerMask))
            {
                targetPoint = hit.point;

            }
            else
            {
                targetPoint = ray.GetPoint(50f);
            }

            // Direction corrigée depuis la position réelle du lasso
            Vector3 direction = (targetPoint - lasso.transform.position).normalized;
            rb.AddForce(direction * force, ForceMode.Impulse);
            boxCollider.enabled = true;
            PlayRandomThrow();
            StartCoroutine(TimerLasso());
            //isFishing = false;
        }
        else if (!hasThrown && Mouse.current.leftButton.wasPressedThisFrame && canThrow && PauseManager.Instance.canPause && DayManager.Instance.numberOfFails >= DayManager.Instance.numberOfFailsAllowed && !CollectionGestion.Instance.collectionCanva.gameObject.activeSelf)
        {
            lassoAudio.PlayOneShot(noThrow);
        }

    }


    public void hasLasso()
    {
        /* ANIMATION : On prévient l'animator que le lasso est réinitialisé/rangé
        if (animator != null)
        {
            animator.SetTrigger("TriggerRetour");
        }*/
        StopAllCoroutines();
        Character.Instance.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        Character.Instance.canMove = true;
        recallRope = false;
        hasThrown = false;
        rb.isKinematic = true;
        lasso.gameObject.SetActive(false);
        isChild = false;
        boxCollider.enabled = false;
        Character.Instance.stopChara = false;
        FishingLasso.Instance.hasToPlaySound = true;
    }

    public void GetLasso()
    {
        lasso.transform.SetParent(cam.transform);
        lasso.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        lasso.transform.localPosition = new Vector3(0.61f, -0.24f, 0.6f);
        isChild = true;
        boxCollider.enabled = false;
        lasso.gameObject.SetActive(false);
    }

    void OnDrawGizmos()
    {
        if (cam == null) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * 50f);
    }

    public void PlayRandomPlouf()
    {
        if (Random.value < 0.5f)
        {
            lassoAudio.PlayOneShot(plouf1, 1.5f);
        }
        else
        {
            lassoAudio.PlayOneShot(plouf2, 1.5f);
        }
    }

    private void PlayRandomThrow()
    {
        if (Random.value < 0.5f)
        {
            lassoAudio.PlayOneShot(throw1, 1.5f);
        }
        else
        {
            lassoAudio.PlayOneShot(throw2, 1.5f);
        }
    }

    private IEnumerator TimerLasso()
    {
        yield return new WaitForSeconds(3.5f);
        if (hasThrown && isChild)
        {
            FishingLasso.Instance.LaunchMissedThrow(false);
        }
        
    }

    private IEnumerator WaitForLag()
    {
        yield return new WaitForSeconds(1);
        waitLag = true;
    }

}
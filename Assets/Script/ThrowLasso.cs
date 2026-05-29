using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowLasso : MonoBehaviour
{
    //public bool isFishing;

    [Header("Références")]
    [SerializeField] private GameObject lasso;
    public Rigidbody rb;
    public Camera cam;
    private BoxCollider boxCollider;
    private Rigidbody rbPlayer;
    private CharacterController chaControll;

    [Header("Paramètres")]
    public float force = 15f;

    public bool hasThrown;
    public bool canThrow;
    public bool recallRope;
    private bool isChild;
    public static ThrowLasso Instance { get; private set; }
    private int layerMask;
    [SerializeField] private AudioSource lassoAudio;
    [SerializeField] private AudioClip plouf1;
    [SerializeField] private AudioClip plouf2;
    [SerializeField] private AudioClip throw1;
    [SerializeField] private AudioClip throw2;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        rbPlayer = Character.Instance.gameObject.GetComponent<Rigidbody>();
        force = 15; //usefull ? 
        rb = lasso.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        boxCollider = lasso.GetComponent<BoxCollider>();
        //canThrow = true;
        chaControll = GetComponent<CharacterController>();
        layerMask = ~LayerMask.GetMask("Bordure");
    }

    void Update()
    {
        if(chaControll.isGrounded && !DialogueManager.Instance.isInDialogue && !Character.Instance.cinematic) {canThrow = true;}
        else { canThrow = false; }
        if(Keyboard.current.rKey.wasPressedThisFrame) //temp
        {
            hasLasso();
            // Character.Instance.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            // Character.Instance.canMove = true;
        }
        if(!hasThrown && !isChild)
        {
            GetLasso();
        }

        
        if (!hasThrown && Keyboard.current.eKey.wasPressedThisFrame && canThrow)//Mouse.current.leftButton.isPressed)
        {
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
            //isFishing = false;
        }
    }


    public void hasLasso()
    {
        
        Character.Instance.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        Character.Instance.canMove = true;
        recallRope = false;
        hasThrown = false;
        rb.isKinematic = true;
        isChild = false;
        boxCollider.enabled = false;
        Character.Instance.stopChara = false;
        FishingLasso.Instance.hasToPlaySound = true;
    }

    public void GetLasso()
    {
        lasso.transform.SetParent(cam.transform);
        lasso.transform.localRotation = Quaternion.identity;
        lasso.transform.localPosition = new Vector3(0.65f, -0.2f, 0.4f);
        isChild = true;
        boxCollider.enabled = false;
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
            lassoAudio.PlayOneShot(plouf1);
        }
        else
        {
            lassoAudio.PlayOneShot(plouf2);
        }
    }

    private void PlayRandomThrow()
    {
        if (Random.value < 0.5f)
        {
            lassoAudio.PlayOneShot(throw1);
        }
        else
        {
            lassoAudio.PlayOneShot(throw2);
        }
    }
    
}

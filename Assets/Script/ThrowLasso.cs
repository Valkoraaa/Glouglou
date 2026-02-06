using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowLasso : MonoBehaviour
{
    public bool isFishing;

    [Header("Références")]
    [SerializeField] private GameObject lasso;
    public Rigidbody rb;
    [SerializeField] private Camera cam;
    private BoxCollider boxCollider;

    [Header("Paramètres")]
    public float force = 15f;

    public bool hasThrown;
    public bool recallRope;
    private bool isChild;
    public static ThrowLasso Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        rb = lasso.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        boxCollider = lasso.GetComponent<BoxCollider>();
    }

    void Update()
    {
        if(Keyboard.current.rKey.wasPressedThisFrame)
        {
            hasLasso();
        }
        if(!hasThrown && !isChild)
        {
            lasso.transform.SetParent(cam.transform);
            lasso.transform.localRotation = Quaternion.identity;
            lasso.transform.localPosition = new Vector3(0.65f, -0.2f, 0.4f);
            isChild = true;
            boxCollider.enabled = false;
        }

        
        if (isFishing && Keyboard.current.eKey.wasPressedThisFrame)//Mouse.current.leftButton.isPressed)
        {
            lasso.transform.SetParent(null);
            rb.isKinematic = false;
            hasThrown = true;
            boxCollider.enabled = true;
            // Raycast depuis le centre de l'écran
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            Vector3 targetPoint;

            if (Physics.Raycast(ray, out hit, 50f))
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
            //isFishing = false;
        }
    }


    public void hasLasso()
    {
        recallRope = false;
        hasThrown = false;
        rb.isKinematic = true;
        isChild = false;
        boxCollider.enabled = false;
    }

    void OnDrawGizmos()
    {
        if (cam == null) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * 50f);
    }
}

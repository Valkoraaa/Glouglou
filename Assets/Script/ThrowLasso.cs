using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowLasso : MonoBehaviour
{
    public bool isFishing;

    [Header("Références")]
    [SerializeField] private GameObject lasso;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera cam;

    [Header("Paramètres")]
    [SerializeField] private float force = 1f;

    void Start()
    {
        rb = lasso.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (isFishing && Mouse.current.leftButton.isPressed)
        {
            rb.isKinematic = false;

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
            // isFishing = false;
        }
    }

    void OnDrawGizmos()
    {
        if (cam == null) return;

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray.origin, ray.direction * 50f);
    }
}

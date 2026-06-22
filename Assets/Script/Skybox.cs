using UnityEngine;

public class ciel : MonoBehaviour
{
    public Light soleil;
    public float rotationSpeed;
    public Material materiauCiel;

    [Header("Couleurs du Ciel")]
    public Color cielJour = new Color(0.3f, 0.6f, 0.9f);
    public Color cielNuit = new Color(0.02f, 0.05f, 0.1f);

    [Header("Couleurs des Nuages")]
    public Color nuageJour = Color.white;
    public Color nuageNuit = new Color(0.1f, 0.15f, 0.2f);

    private void Update()
    {
        if (soleil != null && materiauCiel != null)
        {

            soleil.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);


            float hauteurSoleil = soleil.transform.forward.y;
            float curseurNuit = Mathf.InverseLerp(0.2f, -0.2f, hauteurSoleil);

            Color couleurCielActuelle = Color.Lerp(cielJour, cielNuit, curseurNuit);
            Color couleurNuageActuelle = Color.Lerp(nuageJour, nuageNuit, curseurNuit);


            materiauCiel.SetColor("_CouleurCiel", couleurCielActuelle);
            materiauCiel.SetColor("_CouleurNuage", couleurNuageActuelle);
        }
    }
}
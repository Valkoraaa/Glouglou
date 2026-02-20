using System.Collections;
using UnityEngine;

public class DayManager : MonoBehaviour
{
    [SerializeField] private int dayLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartOfDay(); ?
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DayPassing()
    {
        yield return new WaitForSeconds(dayLight);
        EndOfDay();


        //temp
        yield return new WaitForSeconds(10f);
        StartOfDay();
    }

    private void EndOfDay()
    {
        //empeche le joueur de pecher et le sors de la zone
        EffectManager.Instance.ResetEffect();
    }

    public void StartOfDay()
    {
        EffectManager.Instance.ApplyEffect();
    }
}

using UnityEngine;

public class Reproduccion : MonoBehaviour
{
    Monigote monigote;
    void Start()
    {
        gameObject.GetComponent<Monigote>();
    }

    // Update is called once per frame
    void Update()
    {
        if (monigote.impulsoReproductivo)
        {

        }
    }
}

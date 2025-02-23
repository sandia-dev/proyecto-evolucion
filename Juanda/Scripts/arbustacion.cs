using Unity.VisualScripting;
using UnityEngine;

public class arbustacion : MonoBehaviour
{
    public int comida = 5;
    float tiempo;
    public float tiempoRecuperacion = 5;
    public GameObject[] frutos;
    public float tiemposexo;
    public GameObject arbustofollador;

    public bool reproduccion;
    void Start()
    {
        
    }

   
    void Update()
    {
        if (comida < 5) 
        {
            tiempo += Time.deltaTime;
            if (tiempo > tiempoRecuperacion )
            {
                comida++;
                tiempo = 0;
            }
        }
        for (int i = 0; i < 5; i++) 
        {

            if (i < 5 - comida)
            {
                frutos[i].SetActive(false);
            }
            else 
            {
                frutos[i].SetActive(true);
            }
        }
        if (comida == 5 && reproduccion) 
        {
            tiemposexo += Time.deltaTime;
            if (tiemposexo > 10)
            {
                GameObject hijo;
                
                hijo = Instantiate(arbustofollador, new Vector3 (
                    transform.position.x + Random.Range(-5,5), 
                    transform.position.y + Random.Range(-5,5), 0), 
                    transform.rotation);
                hijo.GetComponent<arbustacion>().tiemposexo = 0;
                tiemposexo = 0;
            }
            
        }
        else
        {
            tiemposexo = 0;
        }
    }

    public void consumir() 
    { 
        if (comida > 0)
        {
            comida--;
        }

        if (comida == 0 && reproduccion)
        {
            Destroy(gameObject);
            print("destruido");
        }
    }



}

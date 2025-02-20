using UnityEngine;

public class arbustacion : MonoBehaviour
{
    public int comida = 5;
    public float tiempo;
    public GameObject[] frutos;
    public float tiemposexo;
    public GameObject arbustofollador;
    void Start()
    {
        
    }

   
    void Update()
    {
        if (comida < 5) 
        {
            tiempo += Time.deltaTime;
            if (tiempo > 10 )
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
        if (comida == 5) 
        {
            tiemposexo += Time.deltaTime;
            if (tiemposexo > 10)
            {
                Instantiate(arbustofollador, new Vector3 (
                    transform.position.x + Random.Range(-5,5), 
                    transform.position.y + Random.Range(-5,5), 0), 
                    transform.rotation);
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

        if (comida == 1)
        {
           Destroy(gameObject);
        }
    }



}

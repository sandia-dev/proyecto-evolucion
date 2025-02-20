using UnityEngine;

public class arbustacion : MonoBehaviour
{
    public int comida = 5;
    public float tiempo;
    public GameObject[] frutos;
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

    }

    public void consumir() 
    { 
        if (comida > 0)
        {
            comida--;
        }

        if (comida == 0)
        {
            Destroy(gameObject);
            print("destruido");
        }
    }



}

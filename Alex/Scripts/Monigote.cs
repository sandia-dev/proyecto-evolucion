using UnityEngine;

public class Monigote : MonoBehaviour
{
    public float velocidad;
    public int hambre = 10;
    public int sed;

    public float consumoEnergetico;
    float delayConsumo;

    public Transform target;
    public Transform comida;


    public float velocidadComer = 2;
    float delayComer;

    public string estado = "explorando";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delayConsumo += Time.deltaTime;
        if (delayConsumo > consumoEnergetico)
        {
            hambre--;
            delayConsumo = 0;
            if (hambre < 5)
            {
                estado = "hambriento";
            }
        }

        


        else if (estado == "explorando")
        {
            float distancia = Vector2.Distance(target.position, transform.position);
            if (distancia > 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, velocidad * Time.deltaTime);

            }
            else
            {
                target.position = new Vector3(Random.Range(-5,5), Random.Range(-5,5), 0);
            }
        }


        //////////////////////////////
        if (estado == "hambriento" && comida != null)
        {
            float distancia = Vector2.Distance(transform.position, comida.position);
            print(distancia);
            if (distancia > 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, comida.position, velocidad * Time.deltaTime);

            }
            else
            {
                if (comida.CompareTag("arbusto"))
                {
                    delayComer += Time.deltaTime;
                    if (delayComer > velocidadComer)
                    {
                        comida.GetComponent<arbustacion>().consumir();
                        hambre++;
                        print("comer");
                        delayComer = 0;
                    }
                }
            }
            
        }
        else
        {
            estado = "explorando";
        }
        

        
            
        





        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("arbusto"))
        {
            comida = collision.gameObject.transform;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("arbusto"))
        {
            comida = null;
        }
    }
}

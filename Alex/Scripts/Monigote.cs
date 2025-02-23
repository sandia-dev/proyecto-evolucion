using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monigote : MonoBehaviour
{
    public float velocidad;
    public int hambre = 10;
    public int sed = 10;
    public float radius = 5;

    public float consumoEnergetico;
    float delayConsumo;

    public Transform target;
    public Transform comida;


    public float velocidadComer = 2;

    float delayComer;
    float delaySed = 0;

    public List<Transform> vision = new List<Transform>();

    public List<string> tareas = new List<string>();

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
            if (hambre <= 5)
            {
                
                if (!tareas.Contains("hambriento"))
                {
                    AddTask("hambriento");
                }
            }
            else if (hambre == 10)
            {
                FinishTask("hambriento");
            }
            if (hambre + sed == 0)
            {
                Destroy(gameObject);
            }
        }

        if (delayComer == 0) // La unica ocasion en la que está quieto es cuando está comiendo, así que uso DelayComer para detectar cuando está en movimiento
        {
            
            delaySed += Time.deltaTime;
            if (delaySed > 3)
            {
                sed--;
                delaySed = 0;
            }
        }

        estado = tareas[0];
        if (!tareas.Contains("hambriento") && !tareas.Contains("sediento"))
        {
            estado = "explorando";
        }

        if (estado == "explorando")
        {
            float distancia = Vector2.Distance(target.position, transform.position);
            if (distancia > 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, velocidad * Time.deltaTime);

            }
            else
            {
                target.position = GetRandomPosition2D(transform.position, radius);
            }
        }

        Transform nearest = GetNearestTransform();
        if (nearest != null && nearest.CompareTag("arbusto"))
        {
            comida = nearest;
        }
        else
        {
            comida = null;
        }




        //////////////////////////////
        if (estado == "hambriento" && comida != null /*&& tareas[0] == "hambriento"*/)
        {

            print(estado +", " + comida.name);
            float distancia = Vector2.Distance(transform.position, comida.position);
            if (distancia > 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, comida.position, velocidad * Time.deltaTime);

            }
            else
            {

                delayComer += Time.deltaTime;
                if (delayComer > velocidadComer)
                {
                    comida.GetComponent<arbustacion>().consumir();
                    hambre++;

                    delayComer = 0;
                }
                if (comida.GetComponent<arbustacion>().comida == 0)
                {
                    estado = "explorando";
                }

            }

        }
        else
        {
            estado = "explorando";
        }

        CleanVisionList();
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("obstaculo"))
        {
            Vector3 normal = target.position-transform.position;
            target.position = -normal.normalized * radius;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!vision.Contains(collision.transform) && collision != null)
        {
            vision.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        vision.Remove(collision.transform);
    }

    // Limpiar referencias a objetos destruidos
    void CleanVisionList()
    {
        vision.RemoveAll(item => item == null);
    }

    void AddTask(string task)
    {
        tareas.Add(task);
    }

    void FinishTask(string task)
    {
        tareas.Remove(task);
    }



    Vector3 GetRandomPosition2D(Vector3 center, float distance)
    {
        float angle = Random.value * 360f; // Ángulo aleatorio en grados
        float radians = angle * Mathf.Deg2Rad; // Convertimos a radianes

        // Calculamos la posición en un círculo
        Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * distance;

        return center + offset;
    }


    Transform GetNearestTransform()
    {
        Transform nearest = null;
        float minDist = Mathf.Infinity; // Distancia inicial muy grande
        Vector2 currentPos = transform.position;

        foreach (Transform t in vision)
        {
            float dist = Vector2.Distance(currentPos, t.position);

            if (dist < minDist)
            {
                minDist = dist;
                nearest = t;
            }
        }

        return nearest;
    }
}

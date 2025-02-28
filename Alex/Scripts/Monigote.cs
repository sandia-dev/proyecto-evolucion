using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class Monigote : MonoBehaviour
{
    public float velocidad;
    public int hambre = 10;
    public int sed = 10;
    public float radius = 5;
    public bool impulsoReproductivo;
    public float madurezReproductiva;

    float edad;

    public float consumoEnergetico;
    float delayConsumo;

    
    public Transform comida;
    public Transform agua;
    public Transform pareja;

    float velocidadComer = 2;

    float delayComer;
    float delayBeber;
    float delaySed = 0;

    CircleCollider2D fov;

    public bool isMoving;

    public List<Transform> vision = new List<Transform>();

    public List<string> tareas = new List<string>();

    public string estado = "explorando";


    public Vector3 pos;

    public GameObject children;




    public float energia;
    void Start()
    {
        fov = GetComponent<CircleCollider2D>();
        fov.radius = radius;
        //consumoEnergetico = velocidad;
    }

    void Update()
    {
        edad += Time.deltaTime;
        energia = hambre + sed;
        consumoEnergetico = velocidad * -2 + 20;

        if (edad > madurezReproductiva && energia > 15)
        {
            impulsoReproductivo = true;
            AddTask("reproducir");
        }
        else
        {
            impulsoReproductivo = false;
            FinishTask("reproducir");
        }

        delayConsumo += Time.deltaTime;
        if (delayConsumo > consumoEnergetico && isMoving )
        {
            if (hambre > 0)
            {
                hambre--;
                delayConsumo = 0;
            }
           

            if (hambre <= 5)
            {
                AddTask("hambriento");
            }
            
        }

        if (hambre > 9)
        {
            FinishTask("hambriento");
            estado = "explorando";
            isMoving = true;
            delayComer = 0;
        }

        if (energia < 6)
        {
            Destroy(gameObject);
        }

        if (isMoving)
        {
            delaySed += Time.deltaTime;
            if (delaySed > consumoEnergetico && sed > 0)
            {
                sed--;
                delaySed = 0;
            }
            if (sed <= 5)
            {
                AddTask("sediento");
            }
            else if (sed > 9)
            {
                FinishTask("sediento");
                isMoving = true;
            }
        }

        // Buscar comida y agua en la visión
        comida = GetNearestTransform("arbusto");
        agua = GetNearestTransform("agua");
        pareja = GetNearestTransform("monigote");

        // Si hay tareas, actualizar el estado
        if (tareas.Any())
        {
            estado = tareas[0];
        }
        else
        {
            estado = "explorando";
        }

        
        if (comida == null && tareas.Contains("hambriento"))
        {
            FinishTask("hambriento");
        }
        if (agua == null && tareas.Contains("sediento"))
        {
            FinishTask("sediento");
        }

        if (estado == "reproducir" && pareja != null && pareja.GetComponent<Monigote>().energia > 15)
        {
            MoveToPartner();
        }
        else if (estado == "hambriento" && comida != null)
        {
            MoveToFood();
        }
        else if (estado == "sediento" && agua != null)
        {
            MoveToWater();
        }
        else
        {
            estado = "explorando";
            isMoving = true;  
        }

        // Movimiento cuando esta explorando
        if (estado == "explorando")
        {
            
            if (Vector2.Distance(transform.position, pos) < 1)
            {
                pos = GetRandomPosition2D(transform.position, radius);
            }

            transform.position = Vector3.MoveTowards(transform.position, pos, velocidad * Time.deltaTime);
        }

        CleanVisionList();
    }


    

    // Mover hacia la comida

    void MoveToPartner()
    {
        float distancia = Vector2.Distance(transform.position, pareja.position);
        if (distancia > 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, pareja.position, velocidad * Time.deltaTime);
        }
        else
        {

            Reproducirse();

            FinishTask("reproducir");
            estado = "explorando";
            comida = null;

           
            isMoving = false;
        }
    }

    Color ObtenerColorMedio(Color c1, Color c2)
    {
        float r = (c1.r + c2.r) / 2;
        float g = (c1.g + c2.g) / 2;
        float b = (c1.b + c2.b) / 2;

        return new Color(r, g, b);
    }

    void Reproducirse()
    {
        Monigote partner = pareja.GetComponent<Monigote>();
        if (partner.energia < energia)
        {
            partner.hambre -= 3;
            partner.sed -= 3;
            hambre -= 3;
            sed -= 3;
            print(gameObject.name + " :" + energia + " " + partner.energia);

            Monigote hijo = Instantiate(children, transform.position, transform.rotation).GetComponent<Monigote>();
            hijo.hambre = 10;
            hijo.sed = 10;
            hijo.velocidad = (velocidad + partner.velocidad) / 2;
            hijo.radius = (radius + partner.radius) / 2;
            hijo.madurezReproductiva = (madurezReproductiva + partner.madurezReproductiva) / 2;
            hijo.GetComponent<SpriteRenderer>().color = ObtenerColorMedio(partner.GetComponent<SpriteRenderer>().color, GetComponent<SpriteRenderer>().color);
        }
    }

    void MoveToFood()
    {
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
                FinishTask("hambriento");
                estado = "explorando";
                comida = null;
            }
            isMoving = false;
        }
    }

    // Mover hacia el agua
    void MoveToWater()
    {
        float distancia = Vector2.Distance(transform.position, agua.position);
        if (distancia > 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, agua.position, velocidad * Time.deltaTime);
        }
        else
        {
            delayBeber += Time.deltaTime;
            if (delayBeber > velocidadComer)
            {
                sed++;
                delayBeber = 0;
            }
            if (sed > 9)
            {
                FinishTask("sediento");
            }
            isMoving = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("obstaculo"))
        {
            Vector3 normal = pos - transform.position;
            pos = -normal.normalized * radius;
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

    void CleanVisionList()
    {
        vision.RemoveAll(item => item == null);
    }

    void AddTask(string task)
    {
        if (!tareas.Contains(task))
        {
            tareas.Add(task);
        }
    }

    void FinishTask(string task)
    {
        if (tareas.Contains(task))
        {
            tareas.Remove(task);
            
        }
    }

    Vector3 GetRandomPosition2D(Vector3 center, float distance)
    {
        float angle = Random.value * 360f;
        float radians = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * distance;

        return center + offset;
    }

    Transform GetNearestTransform(string tag = "")
    {
        Transform nearest = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = transform.position;

        foreach (Transform t in vision)
        {
            
            if (!string.IsNullOrEmpty(tag) && !t.CompareTag(tag))
            {
                continue;
            }

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

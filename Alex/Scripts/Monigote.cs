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

    public float consumoEnergetico;
    float delayConsumo;

    public Transform target;
    public Transform comida;
    public Transform agua;

    public float velocidadComer = 2;

    float delayComer;
    float delayBeber;
    float delaySed = 0;

    public bool isMoving;

    public List<Transform> vision = new List<Transform>();

    public List<string> tareas = new List<string>();

    public string estado = "explorando";


    public Vector3 pos;
    void Start()
    {

    }

    void Update()
    {

        target.position = pos;

        delayConsumo += Time.deltaTime;
        if (delayConsumo > consumoEnergetico)
        {
            hambre--;
            delayConsumo = 0;

            if (hambre <= 5)
            {
                AddTask("hambriento");
            }
            else if (hambre > 9)
            {
                FinishTask("hambriento");
                isMoving = true;
                delayComer = 0;
            }

            if (hambre + sed == 0)
            {
                Destroy(gameObject);
            }
        }

        if (isMoving)
        {
            delaySed += Time.deltaTime;
            if (delaySed > 3)
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

        // Si hay tareas, actualizar el estado
        if (tareas.Any())
        {
            estado = tareas[0];
        }
        else
        {
            estado = "explorando";
        }

        // 🟢 Si no hay comida ni agua disponibles, eliminar tareas de hambre/sed y volver a explorar
        if (comida == null && tareas.Contains("hambriento"))
        {
            FinishTask("hambriento");
        }
        if (agua == null && tareas.Contains("sediento"))
        {
            FinishTask("sediento");
        }

        if (estado == "hambriento" && comida != null)
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
            isMoving = true;  // 🟢 Asegurar que vuelve a moverse
        }

        // 🟢 Movimiento cuando está explorando
        if (estado == "explorando")
        {
            
            if (Vector2.Distance(transform.position, target.position) < 1)
            {
                pos = GetRandomPosition2D(transform.position, radius);
            }

            transform.position = Vector3.MoveTowards(transform.position, pos, velocidad * Time.deltaTime);
        }

        CleanVisionList();
    }


    // Mover hacia la comida
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
                comida = null; // Eliminar la referencia a la comida si ya no hay más
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
            // Si se especifica un tag y el objeto no lo tiene, lo ignoramos
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

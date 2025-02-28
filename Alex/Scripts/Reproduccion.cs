using UnityEngine;

public class Reproduccion : MonoBehaviour
{
    public Monigote monigote;
    public Vector2 size;
    public Color[] colores;
    void Start()
    {
        for (int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
            {
                Monigote newMonigote = Instantiate(monigote, new Vector2(x,y),transform.rotation);
                newMonigote.velocidad = Random.Range(1, 9);
                newMonigote.madurezReproductiva = Random.Range(10, 30);
                newMonigote.radius = Random.Range(5,10);
                newMonigote.GetComponent<SpriteRenderer>().color = colores[Random.Range(0,colores.Length)];
            }
        }
    }

    
}

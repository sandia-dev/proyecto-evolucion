using UnityEngine;

public class aguacion : MonoBehaviour
{
    public GameObject awa;
    void Start()
    {
        for (int i = 0; i < Random.Range(1, 5); i++) 
        {
            Instantiate(awa, new Vector3(Random.Range(-14, 14), Random.Range(-14, 14), 0), transform.rotation);
        } 
    }

    
    void Update()
    {
        
    }
}

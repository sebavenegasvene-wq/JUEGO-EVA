using UnityEngine;

public class Moneda : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        




    }

    // Cambiado a OnCollisionEnter2D y Collision2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Moneda recogida!");
            Destroy(gameObject);
        }
    }
}
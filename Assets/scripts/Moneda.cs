using UnityEngine;

public class Moneda : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Moneda recogida!");
            
            if (GameManager.instance != null)
            {
                GameManager.instance.SumarMoneda();
            }

            Destroy(gameObject);
        }
    }
}
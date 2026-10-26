using UnityEngine;

public class Cofre : MonoBehaviour
{
    public GameObject prefabMoneda; // Aquí arrastraremos el Prefab de la moneda
    public int cantidadMonedas = 10;
    public float fuerzaSalto = 5f; // Qué tan alto saltan

    private bool yaAbierto = false;

    // Esta función la llamaremos desde el script del jugador cuando golpee
    public void AbrirCofre()
    {
        if (yaAbierto) return; // Si ya está abierto, no hacer nada
        yaAbierto = true;

        Debug.Log("¡Cofre abierto!");

        for (int i = 0; i < cantidadMonedas; i++)
        {
            // 1. Crear la moneda
            GameObject nuevaMoneda = Instantiate(prefabMoneda, transform.position, Quaternion.identity);

            // 2. Hacer que salte
            Rigidbody2D rbMoneda = nuevaMoneda.GetComponent<Rigidbody2D>();
            
            if (rbMoneda != null)
            {
                // Elegimos una dirección aleatoria hacia arriba y los lados
                float direccionX = Random.Range(-1f, 1f); 
                float direccionY = Random.Range(0.5f, 1f); // Siempre hacia arriba
                
                Vector2 direccionSalto = new Vector2(direccionX, direccionY).normalized;

                // Aplicamos la fuerza
                rbMoneda.AddForce(direccionSalto * fuerzaSalto, ForceMode2D.Impulse);
            }
        }

        // Opcional: Cambiar el sprite del cofre a abierto o destruirlo
        // Destroy(gameObject); 
    }
}
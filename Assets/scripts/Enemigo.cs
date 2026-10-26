using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 2f;
    private int direccion = -1; // -1 es izquierda, 1 es derecha

    [Header("Vida y Recompensa")]
    public int vida = 1; // <--- Muere de 1 espadazo
    public GameObject prefabMoneda; // <--- Arrastra aquí tu Prefab de moneda en el Inspector

    void Start()
    {
        // Hacemos que el enemigo traspase todo excepto Paredes y el Piso
        Collider2D[] misColliders = GetComponents<Collider2D>();
        Collider2D[] todosLosColliders = FindObjectsOfType<Collider2D>();

        foreach (Collider2D miCol in misColliders)
        {
            foreach (Collider2D otroCol in todosLosColliders)
            {
                if (otroCol == miCol || otroCol.CompareTag("Player")) continue;

                if (!otroCol.CompareTag("Pared") && !otroCol.gameObject.layer.Equals(LayerMask.NameToLayer("Piso")))
                {
                    Physics2D.IgnoreCollision(miCol, otroCol, true);
                }
            }
        }
    }

    void Update()
    {
        transform.Translate(Vector2.right * direccion * velocidad * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si choca con una pared, gira
        if (collision.gameObject.CompareTag("Pared"))
        {
            direccion *= -1; 
            Vector3 escala = transform.localScale;
            escala.x *= -1;
            transform.localScale = escala;
        }
        // Si toca al jugador, le hace daño
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController scriptPlayer = collision.gameObject.GetComponent<PlayerController>();
            if (scriptPlayer != null)
            {
                scriptPlayer.RecibirDaño(); 
            }
        }
    }

    // Función que llama el jugador cuando golpea
    // Función que llama el jugador cuando golpea
    public void RecibirDaño(int cantidadDaño)
    {
        // Al recibir cualquier daño, muere instantáneamente
        Morir();
    }

    void Morir()
    {
        Debug.Log("¡Enemigo derrotado!");
        
        // Soltar 1 moneda
        if (prefabMoneda != null)
        {
            GameObject monedaSoltada = Instantiate(prefabMoneda, transform.position, Quaternion.identity);
            
            // Le damos un pequeño salto a la moneda
            Rigidbody2D rbMoneda = monedaSoltada.GetComponent<Rigidbody2D>();
            if (rbMoneda != null)
            {
                rbMoneda.AddForce(new Vector2(0, 3f), ForceMode2D.Impulse); 
            }
        }

        Destroy(gameObject); 
    }
}
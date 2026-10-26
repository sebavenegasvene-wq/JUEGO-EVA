using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;
    public Animator animator;

    public float fuerzaSalto = 10f;
    public float distanciaLineaSuelo = 0.1f;
    public LayerMask capaSuelo;
    public bool tocandoElSuelo = true;
    private Rigidbody2D rb;

    [Header("Configuración de Golpe")]
    public Transform puntoDeGolpe; 
    public float radioDeGolpe = 0.5f; 
    public LayerMask capaGolpeable; 
    private bool estaGolpeando = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>(); 
    }

    void Update()
    {
        // Pequeña comprobación de seguridad por si acaso
        if (!gameObject.activeInHierarchy) return;

        float velocidadH = Input.GetAxis("Horizontal") * velocidad * Time.deltaTime;

        animator.SetFloat("isMove", Mathf.Abs(velocidadH));

        if (velocidadH > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (velocidadH < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        Vector3 movimiento = transform.position;
        transform.position = new Vector3(movimiento.x + velocidadH, movimiento.y, movimiento.z);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanciaLineaSuelo, capaSuelo);
        tocandoElSuelo = hit.collider != null;

        if (Input.GetKeyDown(KeyCode.Space) && tocandoElSuelo)
        {
            rb.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
        }

        // --- LÓGICA DE GOLPE ---
        if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
        {
            Golpear();
        }
    }

    void Golpear()
    {
        animator.SetTrigger("golpe"); 
        estaGolpeando = true;

        if (puntoDeGolpe != null)
        {
            Collider2D[] objetosGolpeados = Physics2D.OverlapCircleAll(puntoDeGolpe.position, radioDeGolpe, capaGolpeable);

            foreach (Collider2D objeto in objetosGolpeados)
            {
                if (objeto.CompareTag("Moneda"))
                {
                    Destroy(objeto.gameObject);
                    if (GameManager.instance != null) GameManager.instance.SumarMoneda();
                }
                
                if (objeto.CompareTag("Cofre")) 
                {
                    Cofre scriptCofre = objeto.GetComponent<Cofre>();
                    if (scriptCofre != null) scriptCofre.AbrirCofre();
                }

                if (objeto.CompareTag("Enemigo"))
                {
                    Enemigo scriptEnemigo = objeto.GetComponent<Enemigo>();
                    if (scriptEnemigo != null) scriptEnemigo.RecibirDaño(1);
                }
            }
        }

        Invoke("TerminarGolpe", 0.5f);
    }

    void TerminarGolpe()
    {
        estaGolpeando = false;
    }

    public void RecibirDaño()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.RecibirDaño(1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoDeGolpe != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(puntoDeGolpe.position, radioDeGolpe);
        }
    }
}
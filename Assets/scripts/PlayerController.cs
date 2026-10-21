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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // (Opcional) Si olvidas asignarlo en el inspector, esto lo busca solo:
        if (animator == null) animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {


        float velocidadH = Input.GetAxis("Horizontal") * velocidad * Time.deltaTime;

        animator.SetFloat("isMove", Mathf.Abs(velocidadH));
        
        Debug.Log(velocidadH);

        if (velocidadH > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (velocidadH < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        Vector3 movimiento = transform.position;// Movimiento horizontal, vertical y de profundidad
        transform.position = new Vector3(movimiento.x + velocidadH, movimiento.y, movimiento.z);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distanciaLineaSuelo, 
        capaSuelo);
        tocandoElSuelo = hit.collider != null;

        if (Input.GetKeyDown(KeyCode.Space) && tocandoElSuelo)
        {
            rb.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
        }

    }//fin Update

}
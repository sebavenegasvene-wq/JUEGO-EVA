using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int monedas = 0;
    private Text textoContador;

    [Header("Configuración de Vida")]
    public int vidaMaxima = 2; 
    private int vidaActual;
    private Image rellenoBarraVida; 
    private bool juegoTerminado = false; 

    [Header("Configuración de Audio")]
    public AudioClip musicaFondo; 
    public AudioClip sonidoMuerte; 
    public AudioClip sonidoVictoria; // <--- NUEVO: Arrastra aquí el audio "Victoria"
    
    private AudioSource reproductorMusica; 

    [Header("Meta del Juego")]
    public int monedasParaGanar = 49; // <--- NUEVO: Cuántas monedas hacen falta

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        vidaActual = vidaMaxima;
        juegoTerminado = false;
        Time.timeScale = 1f; 
        CrearUI();

        // Crear la música por código
        if (musicaFondo != null)
        {
            GameObject audioGO = new GameObject("MusicaFondoTemporal");
            reproductorMusica = audioGO.AddComponent<AudioSource>();
            reproductorMusica.clip = musicaFondo;
            reproductorMusica.loop = true;
            reproductorMusica.volume = 0.5f;
            reproductorMusica.Play();
        }
    }

    void CrearUI()
    {
        // 1. CREAR EL CANVAS
        GameObject canvasGO = new GameObject("CanvasContador");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>(); 
        canvasGO.AddComponent<GraphicRaycaster>();

        // 2. CREAR EL TEXTO DE MONEDAS
        GameObject textoGO = new GameObject("TextoMonedas");
        textoGO.transform.SetParent(canvasGO.transform, false); 

        textoContador = textoGO.AddComponent<Text>();
        textoContador.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); 
        textoContador.fontSize = 36;
        textoContador.color = Color.white;
        textoContador.alignment = TextAnchor.UpperLeft; 

        RectTransform rectTexto = textoGO.GetComponent<RectTransform>();
        rectTexto.anchorMin = new Vector2(0, 1); 
        rectTexto.anchorMax = new Vector2(0, 1);
        rectTexto.pivot = new Vector2(0, 1);
        rectTexto.anchoredPosition = new Vector2(20, -20); 
        rectTexto.sizeDelta = new Vector2(300, 100); 

        // 3. CREAR FONDO DE LA BARRA DE VIDA
        GameObject fondoVidaGO = new GameObject("FondoBarraVida");
        fondoVidaGO.transform.SetParent(canvasGO.transform, false);
        
        Image fondoImage = fondoVidaGO.AddComponent<Image>();
        fondoImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f); 

        RectTransform rectFondo = fondoVidaGO.GetComponent<RectTransform>();
        rectFondo.anchorMin = new Vector2(0, 1); 
        rectFondo.anchorMax = new Vector2(0, 1);
        rectFondo.pivot = new Vector2(0, 1);
        rectFondo.anchoredPosition = new Vector2(20, -80); 
        rectFondo.sizeDelta = new Vector2(250, 30); 

        // 4. CREAR RELLENO DE LA BARRA DE VIDA
        GameObject rellenoVidaGO = new GameObject("RellenoBarraVida");
        rellenoVidaGO.transform.SetParent(fondoVidaGO.transform, false); 

        rellenoBarraVida = rellenoVidaGO.AddComponent<Image>();
        rellenoBarraVida.color = new Color(0.2f, 0.8f, 0.2f); 

        RectTransform rectRelleno = rellenoVidaGO.GetComponent<RectTransform>();
        rectRelleno.anchorMin = new Vector2(0, 0); 
        rectRelleno.anchorMax = new Vector2(1, 1);
        rectRelleno.pivot = new Vector2(0, 0.5f); 
        rectRelleno.offsetMin = new Vector2(0, 0); 
        rectRelleno.offsetMax = new Vector2(0, 0);

        ActualizarTexto();
    }

    public void SumarMoneda()
    {
        if (juegoTerminado) return; // Si ya ganaste o perdiste, no sumar más
        
        monedas++;
        ActualizarTexto();

        // --- NUEVO: Comprobar si llegamos a la meta ---
        if (monedas >= monedasParaGanar)
        {
            Victoria();
        }
    }

    void ActualizarTexto()
    {
        if (textoContador != null)
        {
            // Mostramos las monedas actuales y cuántas faltan para ganar
            textoContador.text = "Monedas: " + monedas.ToString() + " / " + monedasParaGanar.ToString();
        }
    }

    public void RecibirDaño(int cantidad)
    {
        if (juegoTerminado) return; 

        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        if (rellenoBarraVida != null)
        {
            float porcentajeVida = (float)vidaActual / vidaMaxima;
            rellenoBarraVida.rectTransform.localScale = new Vector3(porcentajeVida, 1, 1);
        }

        if (vidaActual <= 0)
        {
            GameOver();
        }
    }

    // ==========================================
    // NUEVA FUNCIÓN DE VICTORIA
    // ==========================================
    void Victoria()
    {
        juegoTerminado = true;
        Debug.Log("¡Has ganado!");

        // 1. Detener la música de fondo
        if (reproductorMusica != null)
        {
            reproductorMusica.Stop(); 
        }

        // 2. Reproducir sonido de victoria
        if (sonidoVictoria != null)
        {
            GameObject audioVictoriaGO = new GameObject("AudioVictoria");
            AudioSource audioSource = audioVictoriaGO.AddComponent<AudioSource>();
            audioSource.clip = sonidoVictoria;
            audioSource.volume = 1f; 
            audioSource.Play();
            Destroy(audioVictoriaGO, sonidoVictoria.length);
        }

        // 3. Crear el texto de "¡GANASTE!"
        GameObject textoVictoriaGO = new GameObject("TextoVictoria");
        textoVictoriaGO.transform.SetParent(GameObject.Find("CanvasContador").transform, false);
        
        Text textoVictoria = textoVictoriaGO.AddComponent<Text>();
        textoVictoria.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textoVictoria.fontSize = 72;
        textoVictoria.color = Color.yellow; // Color amarillo/dorado para la victoria
        textoVictoria.alignment = TextAnchor.MiddleCenter;
        textoVictoria.text = "¡GANASTE!";

        RectTransform rectGO = textoVictoriaGO.GetComponent<RectTransform>();
        rectGO.anchorMin = new Vector2(0.5f, 0.5f); 
        rectGO.anchorMax = new Vector2(0.5f, 0.5f);
        rectGO.pivot = new Vector2(0.5f, 0.5f);
        rectGO.anchoredPosition = Vector2.zero;
        rectGO.sizeDelta = new Vector2(600, 200);

        // 4. (Opcional) Hacer desaparecer al jugador o congelar el juego
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null) jugador.SetActive(false); 

        Time.timeScale = 0f; // Congelamos el juego
        // No reiniciamos automáticamente, dejamos que se quede en la pantalla de victoria
    }

    void GameOver()
    {
        juegoTerminado = true;
        Debug.Log("¡Has muerto!");

        if (reproductorMusica != null) reproductorMusica.Stop(); 

        if (sonidoMuerte != null)
        {
            GameObject audioMuerteGO = new GameObject("AudioMuerte");
            AudioSource audioSource = audioMuerteGO.AddComponent<AudioSource>();
            audioSource.clip = sonidoMuerte;
            audioSource.volume = 1f; 
            audioSource.Play();
            Destroy(audioMuerteGO, sonidoMuerte.length);
        }

        GameObject textoGameOverGO = new GameObject("TextoGameOver");
        textoGameOverGO.transform.SetParent(GameObject.Find("CanvasContador").transform, false);
        
        Text textoGameOver = textoGameOverGO.AddComponent<Text>();
        textoGameOver.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textoGameOver.fontSize = 72;
        textoGameOver.color = Color.red;
        textoGameOver.alignment = TextAnchor.MiddleCenter;
        textoGameOver.text = "¡PERDISTE!";

        RectTransform rectGO = textoGameOverGO.GetComponent<RectTransform>();
        rectGO.anchorMin = new Vector2(0.5f, 0.5f); 
        rectGO.anchorMax = new Vector2(0.5f, 0.5f);
        rectGO.pivot = new Vector2(0.5f, 0.5f);
        rectGO.anchoredPosition = Vector2.zero;
        rectGO.sizeDelta = new Vector2(600, 200);

        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null) jugador.SetActive(false); 

        Time.timeScale = 0f;
        Invoke("ReiniciarNivel", 2f); 
    }

    void ReiniciarNivel()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
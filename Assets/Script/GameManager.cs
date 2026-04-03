using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Pantallas")]
    [SerializeField] private GameObject pantallaInicio;
    [SerializeField] private GameObject pantallaJuego;
    [SerializeField] private GameObject pantallaFinPartida;
    [SerializeField] private GameObject pantallaPausa;

    [Header("Textos fin de partida")]
    [SerializeField] private TextMeshProUGUI textoPuntuacionFinal;
    [SerializeField] private TextMeshProUGUI textoRecordFinal;

    private ScoreManager scoreManager;
    private bool pausado = false;

    void Awake()
    {
        // Singleton: solo puede existir un GameManager
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

        Time.timeScale = 0f;
        // Al iniciar solo se ve la pantalla de inicio
        MostrarSolo(pantallaInicio);
    }
    
    void Update()
    {
        // Pausar / reanudar con la tecla Q
        if (Input.GetKeyDown(KeyCode.Q))
            Pausa();
    }

    // ── Botón "Jugar" de la pantalla de inicio ────────────────────────────────

    public void Jugar()
    {
        MostrarSolo(pantallaJuego);
        Time.timeScale = 1f;
    }
    
    // ── Botón "Volver Atras" de la pantalla de pausa ────────────────────────────────
    
    public void VoverAtras()
    {
        Time.timeScale = 0f ;

        // Recargamos la escena entera para resetear todo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ── Botón "Reintentar" de la pantalla de fin de partida ───────────────────

    public void Reintentar()
    {
        Time.timeScale = 1f ;

        // Recargamos la escena entera para resetear todo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ── Llamar a este método cuando el pingüino caiga ─────────────────────────

    public void GameOver()
    {
        scoreManager.GameOver();

        Time.timeScale = 0f ;
        
        // Mostramos la puntuación y el récord en la pantalla de fin de partida
        textoPuntuacionFinal.text = "" + Mathf.FloorToInt(scoreManager.GetScore());
        textoRecordFinal.text = "" + Mathf.FloorToInt(scoreManager.GetRecord());

        MostrarSolo(pantallaFinPartida);
    }

    // ── Botón de pausa durante la partida ─────────────────────────────────────

    public void Pausa()
    {
        pausado = !pausado;

        // Time.timeScale a 0 congela el juego, a 1 lo reanuda
        Time.timeScale = pausado ? 0f : 1f;

        pantallaPausa.SetActive(pausado);
    }

    // ── Muestra solo una pantalla y oculta el resto ───────────────────────────

    private void MostrarSolo(GameObject pantalla)
    {
        pantallaInicio.SetActive(false);
        pantallaJuego.SetActive(false);
        pantallaFinPartida.SetActive(false);
        pantallaPausa.SetActive(false);

        pantalla.SetActive(true);
    }
}
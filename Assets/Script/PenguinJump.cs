using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinJump : MonoBehaviour
{
    // ── Variables ajustables desde el Inspector ──────────────────────────────

    [Header("Salto")] [SerializeField] private float limiteCaida = 6f;
    [SerializeField] private float jumpForce = 12f; // Fuerza del salto normal
    [SerializeField] private float trampolineForce = 22f; // Fuerza extra al pisar un trampolín (plataforma trampolin)

    
    [Header("Movimiento horizontal")] [SerializeField]
    private float moveSpeed = 6f; // Velocidad de desplazamiento lateral

    
    [Header("Efectos de Sonido")] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip trampolineSound;
    [SerializeField] private AudioClip destructibleSound;
    [SerializeField] private AudioClip disparoHieloSonido;


    [Header("Cohete Power Up")] [SerializeField]
    private float fuerzaCohete = 20f; // Velocidad de subida
    [SerializeField] private float duracionCohete = 3f; // Segundos que dura
    private bool coheteActivo = false;
    
    
    [Header("Hielo Power Up")]
    [SerializeField] private GameObject prefabBalaHielo; 
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float duracionHielo = 5f;  // Cuánto tiempo puede disparar
    [SerializeField] private float tiempoEntreDisparos = 0.3f;
    private bool HieloActivo = false;
    private bool puedeDisparar = false;



    // ── Referencias internas ─────────────────────────────────────────────────

    private Rigidbody2D rb;
    private bool isAlive = true;



    // ── Inicialización ───────────────────────────────────────────────────────

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

    }


    // ── Cada frame: movimiento horizontal ────────────────────────────────────

    void Update()
    {
        if (!isAlive) return;

        // Leer input horizontal (teclado o mando)
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // Si sale por un borde, aparece por el contrario
        WrapAroundScreen();

        //Se comprueba si el jugador se ha salido del rango de la camara
        ComprobarCaida();
    }


    // ── Colisión con plataforma: aquí ocurre el rebote ───────────────────────

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Solo rebotamos si el pingüino está cayendo (velocidad Y negativa)
        // Esto evita que rebote al rozar una plataforma por el lado mientras sube
        if (rb.linearVelocity.y > 0.1f) return;

        // Plataforma normal → salto estándar
        if (collision.gameObject.CompareTag("Platform"))
        {
            Bounce(jumpForce);
            audioSource.PlayOneShot(jumpSound);
        }

        // Plataforma con trampolín → salto extra
        if (collision.gameObject.CompareTag("Trampoline"))
        {
            Bounce(trampolineForce);
            audioSource.PlayOneShot(trampolineSound);
        }

        // Plataforma destructible → salta una vez y desaparece
        if (collision.gameObject.CompareTag("Destructible"))
        {
            Bounce(jumpForce);
            audioSource.PlayOneShot(destructibleSound);
            Destroy(collision.gameObject);
        }
    }


    // ── Aplica el impulso hacia arriba ────────────────────────────────────────

    private void Bounce(float force)
    {
        // Reseteamos la velocidad Y antes del impulso para que siempre sea consistente
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);

    }


    // ── El pingüino reaparece en el lado contrario al salir por un borde ─────

    private void WrapAroundScreen()
    {
        Vector3 pos = transform.position;
        float halfWidth = Camera.main.orthographicSize * Camera.main.aspect;

        if (pos.x > halfWidth)
            pos.x = -halfWidth;
        else if (pos.x < -halfWidth)
            pos.x = halfWidth;

        transform.position = pos;
    }

    // ── Fin de partida si cae fuera de la pantalla ────────────────────────────

    private void ComprobarCaida()
    {
        float bordeInferiorCamara = Camera.main.transform.position.y - Camera.main.orthographicSize;

        // Si el jugador cae por debajo del borde inferior de la cámara más el límite → Game Over
        if (transform.position.y < bordeInferiorCamara - limiteCaida)
        {
            isAlive = false;
            Debug.Log("Game Over");
            // GameManager.Instance.GameOver();
        }
    }


    // ── Llama al metodo que aplica los power ups al jugador ────────────────────────────

    public void ActivarCohete()
    {
        if (!coheteActivo)
            StartCoroutine(VolarConCohete());
    }

    public void ActivarPoderHielo()
    {
        if (!coheteActivo)
            StartCoroutine(DispararHielo());
    }


    // ── Aplica el Power Up del cohete al jugador por un lapso de tiempo ────────────────────────────

    private IEnumerator VolarConCohete()
    {
        coheteActivo = true;

        GetComponent<Collider2D>().enabled = false;

        float tiempoFin = Time.time + duracionCohete;
        // Time.time → segundos desde que arrancó el juego

        while (Time.time < tiempoFin)
        {
            // Cada frame forzamos la velocidad Y hacia arriba
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaCohete);
            yield return null; // Pausa hasta el siguiente frame
        }

        GetComponent<Collider2D>().enabled = true;

        coheteActivo = false; // Al salir del while, el cohete terminó
    }

    
    // ── Aplica el Power Up del hielo al jugador por un lapso de tiempo ────────────────────────────
    
    private IEnumerator DispararHielo()
    {
        // Si ya tiene el poder activo, no hacemos nada 
        if (puedeDisparar) yield break;

        puedeDisparar = true;
        float tiempoFin = Time.time + duracionHielo; //Establecemos limite de tiempo

        
        while (Time.time < tiempoFin)
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                audioSource.PlayOneShot(disparoHieloSonido);
                Instantiate(prefabBalaHielo, puntoDisparo.position, Quaternion.identity);
            
                // Esperamos un poco para que no salgan ráfagas infinitas
                yield return new WaitForSeconds(tiempoEntreDisparos);
            }
        
            yield return null;
        }

        puedeDisparar = false;
    }

}
using UnityEngine;

public class PenguinJump : MonoBehaviour
{
    // ── Variables ajustables desde el Inspector ──────────────────────────────

    [Header("Salto")]
    [SerializeField] private float limiteCaida = 6f;
    [SerializeField] private float jumpForce = 12f;       // Fuerza del salto normal
    [SerializeField] private float trampolineForce = 22f; // Fuerza extra al pisar un trampolín (plataforma trampolin)

    [Header("Movimiento horizontal")]
    [SerializeField] private float moveSpeed = 6f;        // Velocidad de desplazamiento lateral

    [Header("Efectos de Sonido")] 
        [SerializeField] private AudioClip jumpSound; 
        private AudioSource audioSource;
    
    // ── Referencias internas ─────────────────────────────────────────────────

    private Rigidbody2D rb;
    private bool isAlive = true;

    // ── Inicialización ───────────────────────────────────────────────────────

    void Awake()
    {
        // Recogemos el RigidBody del jugador
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
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
        }

        // Plataforma con trampolín → salto extra
        if (collision.gameObject.CompareTag("Trampoline"))
        {
            Bounce(trampolineForce);
        }
    }

    // ── Aplica el impulso hacia arriba ────────────────────────────────────────

    private void Bounce(float force)
    {
        // Reseteamos la velocidad Y antes del impulso para que siempre sea consistente
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        
        if (jumpSound != null) {
            audioSource.PlayOneShot(jumpSound);
        }
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
}
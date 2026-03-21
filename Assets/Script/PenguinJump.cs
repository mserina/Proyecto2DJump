using UnityEngine;

public class PenguinJump : MonoBehaviour
{
    // ── Variables ajustables desde el Inspector ──────────────────────────────

    [Header("Salto")]
    [SerializeField] private float jumpForce = 12f;       // Fuerza del salto normal
    [SerializeField] private float trampolineForce = 22f; // Fuerza extra al pisar un trampolín

    [Header("Movimiento horizontal")]
    [SerializeField] private float moveSpeed = 6f;        // Velocidad de desplazamiento lateral

    // ── Referencias internas ─────────────────────────────────────────────────

    private Rigidbody2D rb;
    private bool isAlive = true;

    // ── Inicialización ───────────────────────────────────────────────────────

    void Awake()
    {
        // Recogemos el RigidBody del jugador
        rb = GetComponent<Rigidbody2D>();
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

    void OnBecameInvisible()
    {
        // Solo finaliza si cae por ABAJO (por debajo de la cámara)
        if (transform.position.y < Camera.main.transform.position.y)
        {
            isAlive = false;
            Debug.Log("Game Over");
            // Aquí llamarías a tu GameManager: GameManager.Instance.GameOver();
        }
    }
}
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // =============================================
    //            VARIABLES PÚBLICAS
    // =============================================
    [Header("Movimiento Horizontal")]
    public float velocidad = 6f;  
    
    [Header("Salto")]
    public float fuerzaSalto = 12f;
    public float gravedad = -20f; 
    
    
    // =============================================
    //            VARIABLES PRIVADAS
    // =============================================
    private Rigidbody2D rb; // Referencia al componente físico del personaje





    #region Movimiento

    void Start()
    {
        // Obtenemos los componentes adjuntos al mismo GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoverHorizontal();   // 1. Leer input y mover izquierda/derecha
    }
    
    
    void MoverHorizontal()
    {
        // GetAxisRaw devuelve exactamente -1 (izquierda), 0 (quieto) o 1 (derecha)
        // Sin suavizado, lo que da una respuesta inmediata y precisa (ideal para arcade)
        float input = Input.GetAxisRaw("Horizontal");

        // Cambiamos solo la velocidad X, mantenemos la Y intacta
        // Si usáramos rb.velocity.x = ... sin la Y, interrumpiríamos el salto
        rb.linearVelocity = new Vector2(input * velocidad, rb.linearVelocity.y);
    }
    
    
    
    void AplicarGravedad()
    {
        // Fórmula física: velocidad += aceleración * tiempo
        // gravedad es negativo (-20), así que la velocidad Y baja cada frame
        // Time.deltaTime asegura que sea igual sin importar los FPS
        float nuevaVelocidadY = rb.linearVelocity.y + gravedad * Time.deltaTime;

        // Aplicamos la nueva velocidad vertical manteniendo la horizontal
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, nuevaVelocidadY);
    }

    #endregion
    
    
   
}

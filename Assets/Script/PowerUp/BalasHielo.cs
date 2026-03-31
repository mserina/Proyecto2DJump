using UnityEngine;

public class BalasHielo : MonoBehaviour
{
    public float velocidad = 15f;
    public float tiempoVida = 3f;

    void Start()
    {
        // Se destruye automáticamente tras unos segundos para no llenar la memoria
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        // Se mueve hacia arriba constantemente
        transform.Translate(Vector3.up * velocidad * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si choca con un enemigo, lo destruye y desaparece la bala
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
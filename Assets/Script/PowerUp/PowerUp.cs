using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // Definimos los tipos posibles
    public enum TipoEfecto { Cohete, Hielo }
    
    [Header("Configuración")]
    [SerializeField] private TipoEfecto tipoDeEstePowerUp; // Lo eliges en el Prefab

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PenguinJump player = other.GetComponent<PenguinJump>();

            // El condicional que pedías para distinguir el power up
            if (tipoDeEstePowerUp == TipoEfecto.Cohete)
            {
                player.ActivarCohete();
            }
            else if (tipoDeEstePowerUp == TipoEfecto.Hielo)
            {
                player.ActivarPoderHielo(); // Asegúrate de que este método existe en tu PenguinJump
            }

            Destroy(gameObject);
        }
    }
}

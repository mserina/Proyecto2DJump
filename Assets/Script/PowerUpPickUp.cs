using UnityEngine;

public class PowerUpPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Contacto");
        if (other.CompareTag("Player"))
        {
            PenguinJump player = other.GetComponent<PenguinJump>();

            player.ActivarCohete();
            Destroy(gameObject);
        }
    }
}

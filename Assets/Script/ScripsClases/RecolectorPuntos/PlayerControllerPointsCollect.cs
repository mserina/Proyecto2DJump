using UnityEngine;

public class PlayerControllerPointsCollect : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pickup10"))
        {
            Debug.Log(collision.name);
            GameManager.instance.AddPoints(10);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Pickup20"))
        {
            Debug.Log(collision.name);
            GameManager.instance.AddPoints(20);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Pickup50"))
        {
            Debug.Log(collision.name);
            GameManager.instance.AddPoints(50);
            Destroy(collision.gameObject);
        }
    }
}

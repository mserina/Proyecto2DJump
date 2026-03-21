using UnityEditor;
using UnityEngine;

public class PlayerControllerPointsCollectMejorado : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PickUp"))
        {
            Debug.Log(collision.name);
             Item item = collision.GetComponent<Item>();
             if (item != null)
             {
                GameManager.instance.AddPoints(item.PointsValue);   
             }
             Destroy(collision.gameObject);
        }
    }
}

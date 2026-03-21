using System.Collections;
using UnityEngine;

namespace ObjectPicker
{
    public class ItemInventory : MonoBehaviour, IPickable
    {
        [SerializeField] private ItemType itemType;
        
        [SerializeField] private Sprite sprite; // Sprite to represent the item in the inventory (can be set in the inspector)

        private void Start()
        {
            // Set the sprite of the item (if needed, you can also set it in the inspector)
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (sprite == null)
            {
                // If no sprite is assigned, use the current sprite of the SpriteRenderer
                sprite = spriteRenderer.sprite;
            }
        }

        public string GetItemType()
        {
            return itemType.ToString();
        }

        public Sprite GetItemSprite()
        {
            return sprite;
        }

        public void PickUp()
        {
            // "Disable" the item in the scene (e.g., make it invisible, disable its collider, etc.)
            GetComponent<Collider2D>().enabled = false; // Disable the collider to prevent further interactions
            // Add visual effect to show that the item has been picked up (e.g., change color, play animation, etc.)
            // start a coroutine to destroy the item after a short delay to allow the visual effect to play
            StartCoroutine(DestroyAfterDelay(0.5f)); // Example: destroy the item after 0.5 seconds
        }

        private IEnumerator DestroyAfterDelay(float delay)
        {
            // change color progressively to red to show that the item is being picked up
            float elapsedTime = 0f;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color originalColor = spriteRenderer.color;
            while (elapsedTime < delay)
            {
                spriteRenderer.color = Color.Lerp(originalColor, Color.red, elapsedTime / delay);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}

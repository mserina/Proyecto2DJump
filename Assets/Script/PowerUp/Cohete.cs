using UnityEngine;

public class Cohete : MonoBehaviour
{
    [Header("Sonido")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip powerUpSound;
    
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    // Al tocar el pingüino, le activamos el poder y desaparecemos
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(powerUpSound);
            other.GetComponent<PenguinJump>().ActivarCohete();
            Destroy(gameObject);
        }
    }
}
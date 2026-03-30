using UnityEngine;

public class PlataformaCohete : MonoBehaviour
{
    [SerializeField] private GameObject cohetePrefab;  
    [SerializeField] private float probabilidad = 0.2f; // 20% de probabilidad de que aparezca

    void Start()
    {
        Debug.Log("PlataformaCohete ejecutado, probabilidad: " + probabilidad);
        // Generamos un número aleatorio entre 0 y 1
        // Si cae dentro de la probabilidad, spawneamos el cohete encima de la plataforma
        if (Random.value < probabilidad)
        {
            // Lo instanciamos ligeramente por encima de la plataforma
            Vector3 posicionCohete = transform.position + Vector3.up * 1f;
            Instantiate(cohetePrefab, posicionCohete, Quaternion.identity);
        }
    }
}
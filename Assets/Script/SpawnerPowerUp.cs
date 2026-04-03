using UnityEngine;

public class SpawnerPowerUp : MonoBehaviour
{
   [Header("Power Ups")]
   [SerializeField] private GameObject cohetePrefab;

    [Header("Probabilidades")]
    [SerializeField] private float probabilidadCohete = 0.15f;    // 15%

    
    void Start()
    {
        // Generamos un número aleatorio entre 0 y 1
        float random = Random.value; 
       
        // Si cae dentro de la probabilidad, spawneamos el cohete encima de la plataforma
        if (random < probabilidadCohete)
        {
            Spawnear(cohetePrefab);
        }
        
    }
    
    private void Spawnear(GameObject prefab)
    {
        if (prefab == null)
        {
            return;
        }
        Vector3 pos = transform.position + Vector3.up * 1f;
        GameObject nuevoPowerUp = Instantiate(prefab, pos, Quaternion.identity, transform);

    }
    
    
}
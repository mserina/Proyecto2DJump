using UnityEngine;

public class PlataformaPowerUp : MonoBehaviour
{
   [Header("Power Ups")]
    [SerializeField] private GameObject cohetePrefab;
    [SerializeField] private GameObject poderHieloPrefab;

    [Header("Probabilidades")]
    [SerializeField] private float probabilidadCohete = 0.15f;    // 15%
    [SerializeField] private float probabilidadHielo = 0.10f;     // 10%

    
    void Start()
    {
        // Generamos un número aleatorio entre 0 y 1
        float random = Random.value; 
       
        // Si cae dentro de la probabilidad, spawneamos el cohete encima de la plataforma
        if (random < probabilidadCohete)
        {
            Spawnear(cohetePrefab);
        }
        
        // Sumamos las posibilidades de ambos power up para que las posibilidades no coincidan y por ende se solapen  
        else if (random < probabilidadCohete + probabilidadHielo)
        {
            Spawnear(poderHieloPrefab);
        }
    }
    
    private void Spawnear(GameObject prefab)
    {
        if (prefab == null)
        {
            return;
        }
        Vector3 pos = transform.position + Vector3.up * 1f;
        Instantiate(prefab, pos, Quaternion.identity);
    }
}
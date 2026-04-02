using System.Collections.Generic;
using UnityEngine;

public class GeneradorPlataformas : MonoBehaviour
{
    [Header("Prefabs de plataformas")]
    [SerializeField] private GameObject plataformaNormal;
    [SerializeField] private GameObject plataformaDestructible;
    [SerializeField] private GameObject plataformaTrampolin;

    [Header("Configuración de generación")]
    [SerializeField] private float separacionMinima = 1.5f;   // Distancia mínima entre plataformas
    [SerializeField] private float separacionMaxima = 2.5f;   // Distancia máxima entre plataformas
    [SerializeField] private float margenX = 3f;              // Rango horizontal donde pueden aparecer
    [SerializeField] private int plataformasIniciales = 10;   // Cuántas generar al inicio

    [Header("Dificultad")]
    [SerializeField] private float incrementoSeparacion = 0.01f; // Cuánto aumenta la separación al subir
    [SerializeField] private float alturaIncrementoDificultad = 10f; // Cada cuántas unidades sube la dificultad

    [Header("Probabilidades")]
    [SerializeField] private float probNormal = 0.55f;        // 55%
    [SerializeField] private float probDestructible = 0.15f;  // 15%
    [SerializeField] private float probTrampoline = 0.20f;         // 20%

    private float alturaUltimaPlataforma;   // Y de la última plataforma generada
    private Transform jugador;
    private List<GameObject> plataformasActivas = new List<GameObject>(); // Lista para controlar las plataformas vivas

    
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;

        // Generamos las plataformas iniciales desde Y=0 hacia arriba
        alturaUltimaPlataforma = 0f;
        for (int i = 0; i < plataformasIniciales; i++)
            GenerarSiguientePlataforma();
    }

    void Update()
    {
        // Si el jugador se acerca a la última plataforma, generamos más
        if (jugador.position.y + 10f > alturaUltimaPlataforma)
            GenerarSiguientePlataforma();

        // Eliminamos las plataformas que quedaron muy por debajo de la cámara
        EliminarPlataformasViejas();
    }

    
    private void GenerarSiguientePlataforma()
    {
        // Calculamos la separación según la altura actual (más altura = más separación)
        float nivelDificultad = alturaUltimaPlataforma / alturaIncrementoDificultad;   //Cuanto más alto, mayor el nivel. P ej: Y= 50 / Dificultad= 10 = nivel 5
        float separacionActual = Mathf.Min( separacionMinima + nivelDificultad * incrementoSeparacion, separacionMaxima);  //Calcula la separación sumando al mínimo un extra según el nivel

        // Posición aleatoria en X y la Y es la anterior más la separación calculada
        float x = Random.Range(-margenX, margenX);
        float y = alturaUltimaPlataforma + separacionActual;

        GameObject prefab = ElegirPrefab();
        GameObject nueva = Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);

        plataformasActivas.Add(nueva);
        alturaUltimaPlataforma = y;
    }

    private GameObject ElegirPrefab()
    {
        // Mismo sistema de rangos que el SpawneadorPowerUp
        float random = Random.value;

        if (random < probNormal)
        {
            return plataformaNormal;
        }
        else if (random < probNormal + probDestructible)
        {
            return plataformaDestructible;
        }
        else
        {
            return plataformaTrampolin;
        }
        
    }

    private void EliminarPlataformasViejas()
    {
        // Borde inferior de la cámara
        float bordeInferior = Camera.main.transform.position.y - Camera.main.orthographicSize;

        // Recorremos la lista y eliminamos las que quedaron por debajo
        for (int i = plataformasActivas.Count - 1; i >= 0; i--)
        {
            if (plataformasActivas[i] == null)
            {
                // La plataforma ya fue destruida (ej: destructible), la sacamos de la lista
                plataformasActivas.RemoveAt(i);
                continue;
            }

            if (plataformasActivas[i].transform.position.y < bordeInferior - 2f)
            {
                Destroy(plataformasActivas[i]);
                plataformasActivas.RemoveAt(i);
            }
        }
    }
}
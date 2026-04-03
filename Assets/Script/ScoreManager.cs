using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI textoScore;   // Texto de puntuación en partida
    [SerializeField] private TextMeshProUGUI textoRecord;  // Texto de récord en partida

    // Clave con la que se guarda el récord en el dispositivo
    private const string CLAVE_RECORD = "Record";

    private float score = 0f;
    private float record = 0f;
    private Transform jugador;
    private float alturaMáxima = 0f; // Altura más alta que ha alcanzado el jugador

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;

        // Cargamos el récord guardado del dispositivo
        record = PlayerPrefs.GetFloat(CLAVE_RECORD, 0f);
    }

    void Update()
    {
        // Solo actualizamos si el jugador supera su altura máxima
        if (jugador.position.y > alturaMáxima)
        {
            alturaMáxima = jugador.position.y;
            score = alturaMáxima; // La puntuación es directamente la altura
        }

        // Actualizamos los textos en pantalla
        textoScore.text = "Score: " + Mathf.FloorToInt(score);
        textoRecord.text = "Record: " + Mathf.FloorToInt(record);
    }

    // Llamar a este método cuando el jugador pierda
    public void GameOver()
    {
        // Si la puntuación supera el récord, lo guardamos
        if (score > record)
        {
            record = score;
            PlayerPrefs.SetFloat(CLAVE_RECORD, record);
            PlayerPrefs.Save(); // Guardamos en el dispositivo
        }
    }

    // Devuelve la puntuación y el récord para mostrarlos en la pantalla de fin de partida
    public float GetScore() => score;
    public float GetRecord() => record;
}
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private TextMeshProUGUI _scoreText = null;
    private int _score = 0;

    private void Awake()
    {
        instance =  this;
    }
    
    private void Start()
    {
        _score = 0;
    }

    private void UpdateScore()
    {
        _scoreText.text = _score.ToString("000000");
        
    }

    public void AddPoints(int points)
    {
        _score += points;
        UpdateScore();
    }
}
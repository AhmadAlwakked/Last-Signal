using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;  // Singleton patroon om ScoreManager te benaderen
    public Text scoreText;  // De tekst om de score weer te geven
    public Text highscoreText;  // De tekst om de highscore weer te geven

    private int score = 0;  // Huidige score van de speler
    private int highscore = 0;  // Hoogste score

    void Awake()
    {
        // Zorg ervoor dat er maar 1 instantie van ScoreManager is
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        // Laad de opgeslagen highscore van PlayerPrefs
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = score.ToString() + " POINTS";
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
    }

    // Verhoog de score met de waarde die meegegeven wordt
    public void AddPoint(int points)
    {
        score += points;  // Voeg de punten toe
        scoreText.text = score.ToString() + " POINTS";  // Update de score weer

        // Als de huidige score de highscore overtreft, update de highscore
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("highscore", highscore);  // Sla de nieuwe highscore op
            highscoreText.text = "HIGHSCORE: " + highscore.ToString();  // Update highscore tekst
        }
    }
}

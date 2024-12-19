using System.Collections;
using UnityEngine;
using TMPro;
using System.Threading;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] private float timerScore, highScore;

    [SerializeField] private TextMeshProUGUI scoreText, highScoreText, scoreGameText;
    [SerializeField] private GameObject pausePanel, gameOverPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highScore = PlayerPrefs.GetFloat("HighScore");
    }

    // Update is called once per frame
    void Update()
    {
        timerScore += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timerScore/60);
        int seconds = Mathf.FloorToInt(timerScore%60);
        scoreGameText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        if(timerScore > highScore){
            PlayerPrefs.SetFloat("HighScore", timerScore);
            PlayerPrefs.Save();
        }
    }

    public void Pause(){
        pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue(){
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameOver(){
        highScore = PlayerPrefs.GetFloat("HighScore");
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        int minutes = Mathf.FloorToInt(timerScore/60);
        int seconds = Mathf.FloorToInt(timerScore%60);
        scoreText.text = string.Format("Score: {0:00}:{1:00}", minutes, seconds);
        int minutesHigh = Mathf.FloorToInt(highScore/60);
        int secondsHigh = Mathf.FloorToInt(highScore%60);
        highScoreText.text = string.Format("HighScore: {0:00}:{1:00}", minutesHigh, secondsHigh);
    }

    public void Menu(){
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Restart(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Reset(){
        PlayerPrefs.DeleteKey("HighScore");
    }
    
}

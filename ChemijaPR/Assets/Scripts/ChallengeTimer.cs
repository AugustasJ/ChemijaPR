using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChallengeTimer : MonoBehaviour
{
    [SerializeField]
    public Text scoreText;

    [SerializeField]
    public Text finalScoreText;

    [SerializeField]
    public Text timerText;

    [SerializeField]
    public Text finalTimerText;

    GameObject finishContainer;
    private Transform finishBoard;

    bool timerActive = false;
    public float timeStart;

    void Start()
    {
        finalScoreText.text = scoreText.text;
        timerText.text = timeStart.ToString("F2") + "s";
        finalTimerText.text = timerText.text;
    }

    void Update()
    {
        finalScoreText.text = scoreText.text;

        if (timerActive)
        {
            timeStart += Time.deltaTime;
            timerText.text = timeStart.ToString("F2") + "s";
            finalTimerText.text = timerText.text;
        }
    }

    public void StartTimer()
    {
        timerActive = true;
    }

    public void StopTimer()
    {
        timerActive = false;
    }

    public void OpenFinishBoard()
    {
        SetFinalScore();
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenChallengeAR()
    {
        SceneManager.LoadScene(3);
    }

    private void SetFinalScore()
    {
        finalScoreText.text = scoreText.text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MiniGame1 : MonoBehaviour
{
    public TextMeshProUGUI timeTxt;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI time321Txt;
  //  public GameObject button;
    public GameObject playgame;
    public GameObject time321;
    public float timeRemaining = 10;
    public int score = 0;
    float seconds;


    public bool timerIsRunning = false;

    private void Start()

    {
        StartCoroutine(TimerEnum());
        playgame.SetActive(true);
        time321.SetActive(false);
        timerIsRunning = true;

    }

    public void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                
                seconds = Mathf.FloorToInt(timeRemaining % 60);
                
                
            }

            else
            {
                playgame.SetActive(false);
                time321Txt.text = score.ToString();
                time321.SetActive(true);
                
                timeRemaining = 0;
                timerIsRunning = false;
            }
            
        }
        if (seconds >= 0)
        {
            timeTxt.text = seconds.ToString();
        }
    }
    public void scoreButton()
    {
        score += 1;
        scoreTxt.text = score.ToString();
    }

    IEnumerator TimerEnum()
    {
        time321Txt.text = Time.time.ToString();
        yield return new WaitForSeconds(3f);
        Debug.Log(Time.time);
        Time.timeScale = 1;
    }
}

using UnityEngine;
using TMPro; // ←追加

public class Timer : MonoBehaviour
{
    public float timeLeft = 60f;
    public TextMeshProUGUI timerText; // ←ここ変更

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            timeLeft = 0;
            timerText.text = "0:00";
            OnTimerEnd();
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = minutes + ":" + seconds.ToString("00");
    }

    void OnTimerEnd()
    {
        Debug.Log("タイマー終了！");
    }
}
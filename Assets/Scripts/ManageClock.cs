using TMPro;
using UnityEngine;

public class ManageClock : MonoBehaviour
{
    private int hour = 6;
    private int minute = 0;
    private int second = 0;
    public int day = 1;
    private float elapsedTime = 0f;
    public float speed = 10000;
    public TextMeshProUGUI textClock;
    private bool StopTimer;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 1f / speed) { UpdateClock(); elapsedTime -= 1f / speed; }
    }

    private void UpdateClock()
    {
        if (!StopTimer) 
        {
            second++;

            if (second >= 60)
            {
                minute++;
                second = 0;
            }

            if (minute >= 60)
            {
                hour++;
                minute = 0;
            }

            if (hour >= 24)
            {
                hour = 0;
                day++;
            }

            UpdateClockText();
        }
    }

    private void UpdateClockText()
    {
        string clock = $"Day: {day}\n";

        if (hour < 10) { clock += $"0{hour}:"; } else { clock += $"{hour}:"; }

        if (minute < 10) { clock += $"0{minute}"; } else { clock += $"{minute}"; };

        textClock.text = clock;
    }

    public int GetTime() // Renvoit l'heure actuelle en minutes
    {
        return hour * 60 + minute;
    }

    public float GetRealSecondsPerInGameHour()
    {
        return 3600f / speed; // Real-world seconds for one in-game hour
    }

    public void PauseGameClock()
    {
        StopTimer = true;
    }

    public void ResumeGameClock()
    {
        StopTimer = false;
    }
}

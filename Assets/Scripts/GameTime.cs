using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    public Text timerText; 

    private float startTime;
    private bool isPlaying = true;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (isPlaying)
        {
            float t = Time.time - startTime;

            string minutes = ((int)t / 60).ToString("00");
            string seconds = (t % 60).ToString("00");
            string milliseconds = ((t * 100) % 100).ToString("00");

            timerText.text = "Game Time " + minutes + ":" + seconds + ":" + milliseconds;
        }
    }

    public void StopTimer()
    {
        isPlaying = false;
    }

    public void StartTimer()
    {
        isPlaying = true;
        startTime = Time.time;
    }
}

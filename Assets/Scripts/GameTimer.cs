using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float startingTime = 300f;
    public bool Timerticking = false;

    public TMP_Text timerText;

    private float remainingTime;

    void Start()
    {
        remainingTime = startingTime;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (Timerticking && remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                Timerticking = false;

                UpdateTimerDisplay();

                SceneManager.LoadScene("YouDiedScreen");
                return;
            }

            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }
}

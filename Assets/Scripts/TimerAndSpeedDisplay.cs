using UnityEngine;
using UnityEngine.UI;

public class TimerAndSpeedDisplay : MonoBehaviour
{
    public Text timerText;
    public Text speedText;
    public GameObject car;  // Arabanın GameObject'ini referans olarak ekleyin

    private float startTime;
    private bool isRunning;

    void Start()
    {
        startTime = Time.time;
        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
        {
            UpdateTimer();
            UpdateSpeed();
        }
    }

    private void UpdateTimer()
    {
        float timeSinceStart = Time.time - startTime;
        string minutes = ((int)timeSinceStart / 60).ToString();
        string seconds = (timeSinceStart % 60).ToString("f2");

        timerText.text = minutes + ":" + seconds;
    }

   private void UpdateSpeed()
{
    if (car != null) // Car referansını kontrol edin
    {
        Rigidbody2D carRigidbody = car.GetComponent<Rigidbody2D>();
        if (carRigidbody != null) // Rigidbody2D var mı kontrol edin
        {
            float speed = carRigidbody.linearVelocity.magnitude * 3.6f;  // m/s to km/h
            speedText.text = "Speed: " + speed.ToString("f2") + " km/h";
        }
        else
        {
            speedText.text = "Rigidbody2D not found!";
        }
    }
    else
    {
        speedText.text = "Car reference is null!";
    }
}


    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }
}

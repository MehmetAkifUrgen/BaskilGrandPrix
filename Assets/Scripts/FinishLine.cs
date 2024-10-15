using UnityEngine;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    public Text finishTimeText;
    public TimerAndSpeedDisplay timerAndSpeedDisplay;  // TimerAndSpeedDisplay scriptine referans ekleyin

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Car"))  // Arabanın Tag'ını "Player" olarak ayarlayın
        {
            timerAndSpeedDisplay.StopTimer();
            finishTimeText.gameObject.SetActive(true);
            finishTimeText.text = "Finish Time: " + timerAndSpeedDisplay.timerText.text;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Countdown : MonoBehaviour
{
    public Text countdownText;
    public GameObject car;

    private void Start()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1);
        countdownText.text = "2";
        yield return new WaitForSeconds(1);
        countdownText.text = "1";
        yield return new WaitForSeconds(1);
        countdownText.text = "GO!";
        car.GetComponent<Driver>().enabled = true; // Araba kontrol scriptini aktif et
        yield return new WaitForSeconds(1);
        countdownText.gameObject.SetActive(false);
    }
}

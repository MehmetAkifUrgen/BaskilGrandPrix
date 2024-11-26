using UnityEngine;
using UnityEngine.UI;

public class LapCounter : MonoBehaviour
{
    public Text lapText;              // Tur sayısını gösterecek Text
    public RaceManager raceManager;   // RaceManager referansı

    private int totalLaps;

    private void Start()
    {
        // Toplam tur sayısını RaceManager'dan alın
        totalLaps = raceManager.totalLaps;
        UpdateLapText(0); // Oyunun başında tur sayısını sıfırdan başlatın
    }

    public void UpdateLapText(int currentLap)
    {
        // Mevcut ve toplam tur sayısını göster
        lapText.text = "Lap: " + currentLap + "/" + totalLaps;
    }
}

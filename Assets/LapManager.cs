using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine.UI;  // UI bileşenleri için ekleyin

public class LapManager : MonoBehaviour
{
    public int totalLaps = 3; // Toplam tur sayısı
    private Dictionary<Photon.Realtime.Player, int> playerLaps = new Dictionary<Photon.Realtime.Player, int>();
    private int finishedPlayers = 0;
    private List<Photon.Realtime.Player> finishOrder = new List<Photon.Realtime.Player>();

    // UI Text referansı
    public Text lapText;  // Ekranda tur sayısını gösterecek UI Text

    private void Start()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            playerLaps[player] = 0;  // Her oyuncunun tur sayısını 0 ile başlat
        }

        // İlk tur durumunu göster
        UpdateLapUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Oyuncu bitiş çizgisine ulaştığında
        if (other.CompareTag("Car"))
        {
            Photon.Realtime.Player player = other.GetComponent<PhotonView>().Owner;

            // Oyuncunun tur sayısını artır
            playerLaps[player]++;

            // UI Text ile ekranda tur bilgisini güncelle
            UpdateLapUI();

            if (playerLaps[player] >= totalLaps)
            {
                if (!finishOrder.Contains(player)) // Eğer oyuncu daha önce bitirmediyse
                {
                    finishOrder.Add(player);  // Oyuncuyu bitirenler listesine ekle
                    finishedPlayers++;

                    // Oyuncuya sırasını göster
                    other.GetComponent<PlayerController>().ShowFinishPosition(finishedPlayers);

                    // Tüm oyuncular bitirdiyse oyunu sonlandır
                    if (finishedPlayers == PhotonNetwork.PlayerList.Length)
                    {
                        EndRace();
                    }
                }
            }
        }
    }

    // UI'daki tur bilgisini güncelleyen fonksiyon
    private void UpdateLapUI()
    {
        Photon.Realtime.Player player = PhotonNetwork.LocalPlayer;
        int currentLap = playerLaps[player];

        // "Lap: 1/3" formatında ekranda göster
        lapText.text = "Lap: " + currentLap + "/" + totalLaps;
    }

    private void EndRace()
    {
        // Yarış bittiğinde ilk sahneye dön
        PhotonNetwork.LoadLevel("Start");
    }
}

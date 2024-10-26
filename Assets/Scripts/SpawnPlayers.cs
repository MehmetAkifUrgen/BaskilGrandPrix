using UnityEngine;
using Photon.Pun;
using Photon.Realtime;  // Photon Player class'ı için gerekli

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;  // Oyuncu prefab'ı
    public Sprite[] carSprites;      // Tüm araba sprite'ları
    public TimerAndSpeedDisplay timerAndSpeedDisplay; // TimerAndSpeedDisplay referansı

    // Farklı oyuncular için başlangıç pozisyonlarını tanımla
    private Vector3[] spawnPositions = {
        new Vector3(-75.46f, -8.77f, -0.52f),   // 1. oyuncu için pozisyon
        new Vector3(-69.96f, -8.77f, -0.52f),   // 2. oyuncu için pozisyon
        new Vector3(-70.15f, -13.83f, -0.52f),  // 3. oyuncu için pozisyon
        new Vector3(-75.11f, -13.83f, -0.52f)   // 4. oyuncu için pozisyon
    };

    private void Start()
    {
        // Her oyuncu için ActorNumber'a göre bir index hesaplayalım
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber % spawnPositions.Length;

        // Oyuncuyu uygun pozisyonda spawnla
        Vector3 spawnPosition = spawnPositions[playerIndex];
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);

        // Kameraya sahip olan cihazın sadece kendi oyuncusunu takip etmesini sağla
        Camera.main.GetComponent<FollowCamera>().target = player.transform;

        // Seçilen araba index'ini Photon Custom Properties'ten al
        object selectedCarIndex;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("SelectedCar", out selectedCarIndex))
        {
            // Player prefab'ındaki SpriteRenderer bileşenini bul
            SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();

            // Seçilen sprite'ı prefab'a uygula
            spriteRenderer.sprite = carSprites[(int)selectedCarIndex];
        }

        // TimerAndSpeedDisplay'de car referansını ayarlayın
        timerAndSpeedDisplay.car = player;
    }
}

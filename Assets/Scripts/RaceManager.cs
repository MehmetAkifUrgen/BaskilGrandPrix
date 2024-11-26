using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement; // Sahne yönetimi için eklendi
using UnityEngine.UI;

public class RaceManager : MonoBehaviourPun
{
    public int totalLaps = 3;
    private Dictionary<int, int> playerLaps = new Dictionary<int, int>();
    private List<int> finishedPlayers = new List<int>();
    private Dictionary<int, string> playerNames = new Dictionary<int, string>();

    public static RaceManager Instance { get; private set; }
    public static List<string> playerFinishOrder = new List<string>();

    public Transform finishLine;  // Bitiş çizgisinin konumu
    public Text positionDisplay;  // Oyuncunun sıralamasını gösterecek UI öğesi

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerLaps[player.ActorNumber] = 0;
            string playerName = $"Player {player.ActorNumber}"; // Google Play Games kullanıcı adını alabiliriz
            playerNames[player.ActorNumber] = playerName;
        }
    }

    private void Update()
    {
        UpdatePlayerPositions();
    }

    // Oyuncu sıralamasını hesapla ve UI'da güncelle
    private void UpdatePlayerPositions()
    {
        // Tüm oyuncuları, bitiş çizgisine olan mesafelerine göre sıralar
        List<KeyValuePair<int, float>> playerDistances = new List<KeyValuePair<int, float>>();
        
        foreach (GameObject playerCar in GameObject.FindGameObjectsWithTag("Player"))
        {
            PhotonView playerView = playerCar.GetComponent<PhotonView>();
            if (playerView != null)
            {
                int playerID = playerView.Owner.ActorNumber;
                float distance = Vector2.Distance(playerCar.transform.position, finishLine.position);
                playerDistances.Add(new KeyValuePair<int, float>(playerID, distance));
            }
        }

        // Oyuncuları mesafelerine göre sıralar (en yakın olan 1. sırada olur)
        playerDistances.Sort((x, y) => x.Value.CompareTo(y.Value));

        // Her oyuncunun sıralamasını al ve ekranda güncelle
        int rank = 1;
        foreach (var player in playerDistances)
        {
            int playerID = player.Key;
            if (playerID == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                positionDisplay.text = $"{rank}. sıradasınız";
                break;
            }
            rank++;
        }
    }

    // Oyuncu tur tamamladığında çağrılacak metod
    public void CompleteLap(int playerID)
    {
        if (playerLaps.ContainsKey(playerID))
        {
            playerLaps[playerID]++;
            UpdateLapDisplay(playerID);
            
            if (playerLaps[playerID] >= totalLaps && !finishedPlayers.Contains(playerID))
            {
                finishedPlayers.Add(playerID);
                photonView.RPC("ShowFinishPosition", RpcTarget.All, playerID, finishedPlayers.Count);
            }

            if (finishedPlayers.Count == PhotonNetwork.PlayerList.Length)
            {
                photonView.RPC("EndRace", RpcTarget.All);
            }
        }
    }

    private void UpdateLapDisplay(int playerID)
    {
        if (playerID == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            int currentLap = playerLaps[playerID];
            FindObjectOfType<LapCounter>().UpdateLapText(currentLap);
        }
    }

    [PunRPC]
    private void ShowFinishPosition(int playerID, int position)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == playerID)
        {
            string playerName = playerNames[playerID];
            Debug.Log($"{playerName} yarışı {position}. sırada bitirdi!");
            var playerCar = FindPlayerCar(playerID);
            if (playerCar != null) playerCar.GetComponent<Delivery>().enabled = false;

            playerFinishOrder.Add($"{position}. sırada: {playerName}");
        }
    }

    [PunRPC]
    private void EndRace()
    {
        Debug.Log("Yarış tamamlandı! Sonuç ekranına geçiliyor.");
        
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("LoadEndGameScene", RpcTarget.All);
        }
    }

    [PunRPC]
    private void LoadEndGameScene()
    {
        SceneManager.LoadScene("EndGame");
    }

    private GameObject FindPlayerCar(int playerID)
    {
        foreach (GameObject car in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (car.GetComponent<PhotonView>().Owner.ActorNumber == playerID)
                return car;
        }
        return null;
    }
}

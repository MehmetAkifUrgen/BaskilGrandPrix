using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class WaitingRoom : MonoBehaviourPunCallbacks
{
    public Text playerCountText;
    public Button startGameButton;
  
    public Transform playerListContainer; // Oyuncu isimlerinin gösterileceği container
    public GameObject playerNamePrefab; // Oyuncu ismini gösterecek prefab

    private List<GameObject> playerNameObjects = new List<GameObject>();

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        UpdatePlayerCount();
        startGameButton.onClick.AddListener(StartGame);
        startGameButton.interactable = PhotonNetwork.LocalPlayer.IsMasterClient; // Sadece kurucuya izin ver
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdatePlayerCount();
        AddPlayerName(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdatePlayerCount();
        RemovePlayerName(otherPlayer);
    }

  void UpdatePlayerCount()
{
    playerCountText.text = "Current Players: " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
}


    void AddPlayerName(Player player)
    {
        GameObject playerNameObj = Instantiate(playerNamePrefab, playerListContainer);
        playerNameObj.GetComponent<Text>().text = player.NickName; // Oyuncunun adını göster
        playerNameObjects.Add(playerNameObj);
    }

    void RemovePlayerName(Player player)
    {
        foreach (GameObject obj in playerNameObjects)
        {
            if (obj.GetComponent<Text>().text == player.NickName)
            {
                playerNameObjects.Remove(obj);
                Destroy(obj);
                break;
            }
        }
    }

    void StartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2) // En az 2 oyuncu ile oyuna başla
        {
            PhotonNetwork.LoadLevel("bubir"); // Oyun sahnesi ismi
        }
        else
        {
            Debug.Log("Yeterli oyuncu yok!");
            // Toast mesajı eklemek istersen buraya ekleyebilirsin
        }
    }
}

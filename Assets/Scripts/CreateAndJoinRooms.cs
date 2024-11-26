using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    public Text errorText; // Hata mesajı için Text UI öğesi

    public void CreateRoom()
    {
        if (createInput.text.Length < 3)
        {
            errorText.text = "Please enter at least 3 characters.";
            return;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4; // Maksimum 4 oyuncu

        PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        errorText.text = ""; // Hata mesajını temizle
    }

    public void JoinRoom()
    {
        if (joinInput.text.Length < 3)
        {
            errorText.text = "Please enter at least 3 characters.";
            return;
        }

        PhotonNetwork.JoinRoom(joinInput.text);
        errorText.text = ""; // Hata mesajını temizle
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("SelectCar");
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("start"); // Burada önceki sahne adını girin
    }
}

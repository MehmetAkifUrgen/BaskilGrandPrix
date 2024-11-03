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


public void CreateRoom()
{
    RoomOptions roomOptions = new RoomOptions();
    roomOptions.MaxPlayers = 4; // Maksimum 4 oyuncu

    PhotonNetwork.CreateRoom(createInput.text, roomOptions);
}


    public void JoinRoom(){
        PhotonNetwork.JoinRoom(joinInput.text);
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

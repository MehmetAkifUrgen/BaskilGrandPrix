using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PingDisplay : MonoBehaviour
{
    public Text pingText; // Ping değerini göstereceğin Text UI elemanı

    void Update()
    {
        // Photon'dan ping değerini al ve ekranda göster
        int ping = PhotonNetwork.GetPing();
        pingText.text = "Ping: " + ping.ToString() + " ms";
    }
}

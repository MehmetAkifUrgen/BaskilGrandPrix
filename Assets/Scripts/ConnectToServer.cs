using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    // Bağlantı hatası olursa çalışır
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        // Hatayı loga yazdır
        Debug.Log("Bağlantı hatası: " + cause.ToString());

        // Mobilde toast mesajı göster
        ShowAndroidToastMessage("Bağlantı hatası: " + cause.ToString());
    }

    // Android'de Toast mesajı göstermek için
    void ShowAndroidToastMessage(string message)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // Android ortamında çalışıp çalışmadığını kontrol edin
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");

            currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", currentActivity, message, toastClass.GetStatic<int>("LENGTH_SHORT"));
                toastObject.Call("show");
            }));
        }
    }
}

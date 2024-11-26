using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;


public class CarSelection : MonoBehaviour
{
    public Sprite[] carSprites;    // Tüm araba sprite'ları (7 adet)
    private int selectedCarIndex = 0; // Seçilen arabanın index'i


    // Bu fonksiyon, butona tıklanarak çalıştırılır
    public void SelectCar(int carIndex)
    {
        selectedCarIndex = carIndex;  // Seçilen arabanın index'ini kaydet

        // Seçilen araba bilgisini Photon Custom Properties'de sakla
        ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
        playerProperties["SelectedCar"] = selectedCarIndex;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        // Waiting Room sahnesine geçiş yap
        SceneManager.LoadScene("WaitingRoom");
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("Lobby"); // Burada önceki sahne adını girin
    }
}

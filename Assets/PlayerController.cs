using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public Text finishPositionText; // Ekranda gösterilecek sıralama

    // Yarışı bitiren oyuncuya sıralama gösterilecek
    public void ShowFinishPosition(int position)
    {
        finishPositionText.gameObject.SetActive(true);
        finishPositionText.text = "Position: " + position;
    }
}

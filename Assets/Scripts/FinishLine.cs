using UnityEngine;
using Photon.Pun;

public class FinishLine : MonoBehaviourPun
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Arabalarla çarpışmayı kontrol et
        if (other.CompareTag("Player")) // Oyuncuların tag'ini "Player" yapın
        {
            // Oyuncunun PhotonView'ını alın
            PhotonView playerView = other.GetComponent<PhotonView>();

            // Sadece kendi oyuncumuz için CompleteLap'i çağır
            if (playerView != null && playerView.IsMine)
            {
                int playerID = playerView.Owner.ActorNumber;
                RaceManager.Instance.CompleteLap(playerID);
            }
        }
    }
}

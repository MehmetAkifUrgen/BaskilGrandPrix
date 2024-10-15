using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player; // Arabanızın transformunu buraya ekleyin
    public Vector3 offset; // Minimap kamerasının oyuncuya göre konumu

    void LateUpdate()
    {
        Vector3 newPosition = player.position + offset;
        newPosition.z = transform.position.z; // Z pozisyonunu sabit tutarak sadece X ve Y pozisyonlarını değiştiriyoruz
        transform.position = newPosition;
    }
}

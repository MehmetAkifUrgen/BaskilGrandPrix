using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10); // Kameranın arabanın arkasında olması için z ekseni ötelemesi
    public float smoothSpeed = 0.125f; // Yumuşak hareket hızı
    public float rotationSmoothSpeed = 0.1f; // Yumuşak rotasyon hızı

    void Start()
    {
        // PhotonView kullanarak yalnızca kendi arabasını takip et
        if (PhotonNetwork.LocalPlayer.TagObject != null)
        {
            target = ((GameObject)PhotonNetwork.LocalPlayer.TagObject).transform;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Kameranın konumunu yumuşatarak arabaya göre ayarla
            Vector3 desiredPosition = target.position + target.rotation * offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Kameranın rotasyonunu yumuşatarak arabanın rotasyonuna yaklaştır
            Quaternion targetRotation = target.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed);
        }
    }
}

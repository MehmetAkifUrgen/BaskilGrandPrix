using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FollowCamera : MonoBehaviour
{ 
    
    public Transform target;

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
            // Kameranın konumunu arabaya ayarla
            Vector3 newPosition = target.position;
            newPosition.z = transform.position.z; // Z eksenini sabit tut
            transform.position = newPosition;
        }
    }
}

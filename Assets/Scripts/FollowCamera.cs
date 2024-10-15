using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private SpawnPlayers spawnPlayers; // SpawnPlayers referansı
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
    [SerializeField] private float followSpeed = 0.125f;
    private GameObject thingToFollow; // Takip edilecek nesne

    void Start()
    {
        // Oyun başladığında, SpawnPlayers'dan arabayı al
        thingToFollow = spawnPlayers.GetSpawnedCar();
    }

    void LateUpdate()
    {
        if (thingToFollow == null) return; // Eğer araba bulunmazsa kamerayı hareket ettirme

        Vector3 desiredPosition = thingToFollow.transform.position + thingToFollow.transform.TransformDirection(offset);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed);
        transform.position = smoothedPosition;

        transform.rotation = Quaternion.Lerp(transform.rotation, thingToFollow.transform.rotation, followSpeed);
    }
}

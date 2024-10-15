using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject spawnedCar; // Spawn edilen araba

    private void Start()
    {
        Vector3 position = new Vector3(-71.51f, -8.46f, -0.52f);
        spawnedCar = PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity);
        spawnedCar.tag = "Car"; // Tag'ı ekle
    }

    public GameObject GetSpawnedCar() // Spawn edilen arabayı döndüren bir metod
    {
        return spawnedCar;
    }
}

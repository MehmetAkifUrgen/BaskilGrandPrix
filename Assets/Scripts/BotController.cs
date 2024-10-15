using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    public GameObject waypointsParent;  // Waypoints'i içeren ana GameObject
    public float speed = 15f;           // Başlangıç hızı
    public float maxSpeed = 60f;        // Maksimum hız
    public float acceleration = 10f;    // Hızlanma oranı
    public float deceleration = 0.0002f; // Yavaşlama oranı
    public float steerSpeed = 5f;       // Dönüş hızı

    private Rigidbody2D rb;
    private List<Transform> waypoints;  // Waypoints listesini dinamik olarak toplayacağız
    private int currentWaypointIndex = 0;
    private float currentSpeed = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Waypoints GameObject'inin içindeki tüm waypoint'leri al
        waypoints = new List<Transform>();
        foreach (Transform waypoint in waypointsParent.transform)
        {
            waypoints.Add(waypoint);
        }
    }

    void FixedUpdate()
    {
        MoveTowardsWaypoint();
    }

    void MoveTowardsWaypoint()
    {
        if (waypoints.Count == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector2 direction = (targetWaypoint.position - transform.position).normalized;

        // Botun yönünü waypointe doğru çevirme
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        transform.Rotate(0, 0, -rotateAmount * steerSpeed * Time.deltaTime);

        // Hızlanma ve yavaşlama
        currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.deltaTime, 0f, maxSpeed);

        // Hareket et
        rb.velocity = transform.up * currentSpeed;

        // Eğer waypoint'e yakınsa bir sonraki waypoint'e geç
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Photon.Pun;

public class Driver : MonoBehaviour
{
    public float maxSpeed = 60f;             // Normal yoldaki maksimum hız
    public float gravelMaxSpeed = 30f;       // Çakıldaki maksimum hız (daha düşük)
    public float acceleration = 30f;         // Normal hızlanma
    public float gravelAcceleration = 15f;   // Çakıldaki hızlanma (daha düşük)
    public float deceleration = 10f;         // Normal yavaşlama
    public float gravelDeceleration = 20f;   // Çakıldaki yavaşlama (daha yüksek)
    public float steerSpeed = 5f;            // Direksiyon açısı

    private float currentSpeed = 0f;         // Mevcut hız
    private bool hasCollided = false;        // Çarpma kontrolü
    private bool onGravel = false;           // Çakıl kontrolü

    private Rigidbody2D rb;
    private InputAction gas;
    private InputAction brake;
    private PhotonView view;

    // SteeringWheel bileşenine referans
    private SimpleInputNamespace.SteeringWheel steeringWheel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gas = new InputAction("Vertical", InputActionType.Value, "<Gamepad>/rightTrigger");
        brake = new InputAction("Geri", InputActionType.Value, "<Gamepad>/leftTrigger");
        gas.Enable();
        brake.Enable();
        view = GetComponent<PhotonView>();

        // SteeringWheel bileşenini bul ve atama yap
        steeringWheel = FindObjectOfType<SimpleInputNamespace.SteeringWheel>();
        if (steeringWheel == null)
        {
            Debug.LogError("SteeringWheel bileşeni sahnede bulunamadı!");
        }
    }

    void FixedUpdate()
    {
        if (view.IsMine)
        {
            // Gaz ve fren girişlerini oku
            float input = gas.ReadValue<float>() - brake.ReadValue<float>();

            if (hasCollided)
            {
                currentSpeed = 0f; // Çarpma sonrası hızı sıfırla
                if (input <= 0) return; // Gaz verilmeden hareket etme
                hasCollided = false; // Gaz verilirse çarpma durumunu sıfırla
            }

            // Zemin durumuna göre hızlanma ve maksimum hız limitlerini ayarla
            float targetMaxSpeed = onGravel ? gravelMaxSpeed : maxSpeed;
            float currentAcceleration = onGravel ? gravelAcceleration : acceleration;
            float currentDeceleration = onGravel ? gravelDeceleration : deceleration;

            // Gaz verildiğinde hızlanma
            if (input > 0)
            {
                currentSpeed = Mathf.Clamp(currentSpeed + currentAcceleration * Time.deltaTime, 0f, targetMaxSpeed);
            }
            else
            {
                // Gazdan çekildiğinde yavaşlama
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, currentDeceleration * Time.deltaTime);
            }

            // Direksiyon değerini `SteeringWheel`'den al ve hız ile orantılı azalt
            float horizontal = steeringWheel != null ? steeringWheel.Value : 0f;
            float adjustedSteerSpeed = steerSpeed * (1 - (currentSpeed / maxSpeed));
            float angle = horizontal * adjustedSteerSpeed;
            transform.Rotate(0, 0, -angle);

            // Hızı arabaya uygula
            rb.linearVelocity = transform.up * currentSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("cakil"))
        {
            onGravel = true; // Çakıl alanına girildiğinde çakıl etkilerini aktif et
        }
        else if (other.CompareTag("engel"))
        {
            hasCollided = true; // Engelle çarpışma olduğunda çarpma durumunu aktif et
            currentSpeed = 0f;  // Hızı sıfırla
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("cakil"))
        {
            onGravel = false; // Çakıdan çıkınca normal yavaşlamaya dön
        }
    }
}

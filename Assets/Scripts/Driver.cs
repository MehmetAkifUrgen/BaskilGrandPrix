using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Photon.Pun;

public class Driver : MonoBehaviour
{
    public float maxSpeed = 60f;          // Maksimum hız
    public float acceleration = 30f;      // Formula 1 tarzı hızlanma
    public float deceleration = 10f;      // Normal yavaşlama
    public float gravelDeceleration = 20f; // Çakıl üzerindeki yavaşlama
    public float steerSpeed = 5f;         // Direksiyon açısı
    private float currentSpeed = 0f;      // Mevcut hız
    private bool hasCollided = false;     // Çarpma kontrolü

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
                // Çarpma sonrası hızı sıfırla
                currentSpeed = 0f;
                // Gaz verilmeden hareket etme
                if (input <= 0)
                {
                    return;
                }
                else
                {
                    // Gaz verilirse çarpma durumunu sıfırla
                    hasCollided = false;
                }
            }

            // Hızlanma ve yavaşlama
            if (input > 0)
            {
                // Gaz verildiğinde hızlanma
                currentSpeed += acceleration * (1 - (currentSpeed / maxSpeed)) * Time.deltaTime;

                // Maksimum hızı aşma
                if (currentSpeed > maxSpeed)
                    currentSpeed = maxSpeed;
            }
            else
            {
                // Gazdan çekildiğinde yavaşlama
                float currentDeceleration = deceleration;
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
            // Çakıl alanına girildiğinde yavaşlamayı artır
            deceleration = gravelDeceleration;
        }
        else if (other.CompareTag("engel")) // Engelle çarpışma olduğunda
        {
            hasCollided = true; // Çarpma durumunu aktif et
            currentSpeed = 0f;  // Hızı sıfırla
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("cakil"))
        {
            // Çakıldan çıkınca normal yavaşlamaya dön
            deceleration = 10f; // Varsayılan yavaşlama değerine geri dön
        }
    }
}

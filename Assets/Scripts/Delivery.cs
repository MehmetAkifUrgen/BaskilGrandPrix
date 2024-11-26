using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Delivery : MonoBehaviour
{
    public float maxSpeed = 60f;          // Normal yoldaki maksimum hız
    public float gravelMaxSpeed = 15f;    // Çakıldaki maksimum hız (daha düşük)
    public float acceleration = 10f;      // Normal yolda hızlanma
    public float gravelAcceleration = 5f; // Çakıldaki hızlanma (daha düşük)
    public float deceleration = 5f;       // Normal yolda yavaşlama
    public float gravelDeceleration = 10f; // Çakıldaki yavaşlama (daha hızlı)

    private Rigidbody2D rb;
    private float currentSpeed = 0f;
    private bool isOnGravel = false;

    private InputAction gas;
    private InputAction brake;

    private SimpleInputNamespace.SteeringWheel steeringWheel;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gas = new InputAction("Vertical", InputActionType.Value, "<Gamepad>/rightTrigger");
        brake = new InputAction("Geri", InputActionType.Value, "<Gamepad>/leftTrigger");
        gas.Enable();
        brake.Enable();

        // SteeringWheel bileşenini bul ve atama yap
        steeringWheel = FindObjectOfType<SimpleInputNamespace.SteeringWheel>();
        if (steeringWheel == null)
        {
            Debug.LogError("SteeringWheel bileşeni sahnede bulunamadı!");
        }
    }

    void FixedUpdate()
    {
        // Gaz ve fren girişlerini oku
        float input = gas.ReadValue<float>() - brake.ReadValue<float>();

        // Hızlanma ve yavaşlama değerlerini zemin durumuna göre ayarla
        float targetMaxSpeed = isOnGravel ? gravelMaxSpeed : maxSpeed;
        float currentAcceleration = isOnGravel ? gravelAcceleration : acceleration;
        float currentDeceleration = isOnGravel ? gravelDeceleration : deceleration;

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

        // Direksiyon değerini `SteeringWheel`'den al
        float horizontal = steeringWheel != null ? steeringWheel.Value : 0f;
        float steerSpeed = Mathf.Lerp(1f, 0.5f, currentSpeed / targetMaxSpeed); // Hız arttıkça daha az dönüş
        float angle = horizontal * steerSpeed;

        // Arabayı döndür
        transform.Rotate(0, 0, -angle);

        // Hızı arabaya uygula
        rb.linearVelocity = transform.up * currentSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Çakıla girildiğinde isOnGravel durumunu ayarla
        if (other.CompareTag("cakil"))
        {
            isOnGravel = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Çakıdan çıkıldığında isOnGravel durumunu sıfırla
        if (other.CompareTag("cakil"))
        {
            isOnGravel = false;
        }
    }
}

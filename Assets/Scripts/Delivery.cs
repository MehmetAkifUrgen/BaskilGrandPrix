using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Delivery : MonoBehaviour
{
  
    public float speed = 15f;
    public float maxSpeed = 60f;
    public float acceleration = 10f;
    public float deceleration = 0.0002f;
    public float reverseSpeed = 0.0002f; // Geri hareket hızı

    private Rigidbody2D rb;
    private float currentSpeed = 0f;
    private float steerSpeed = 5f;
    private InputAction gas;
    private InputAction brake;
    private InputAction steering;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gas = new InputAction("Vertical", InputActionType.Value, "<Gamepad>/rightTrigger");
        brake = new InputAction("Geri", InputActionType.Value, "<Gamepad>/leftTrigger");
        steering = new InputAction("Horizontal", InputActionType.Value, "<Gamepad>/leftStick/x");
        gas.Enable();
        brake.Enable();
        steering.Enable();
     
    }

    void FixedUpdate()
    {
 
         // Arabayı ileri veya geri hareket ettir
        float input = gas.ReadValue<float>() - brake.ReadValue<float>();
        float horizontal = Input.acceleration.x;
        horizontal = Mathf.Clamp(horizontal, -1f, 1f); // Input değerini -1 ile 1 arasında normalleştirin
        float angle = horizontal * steerSpeed;
        transform.Rotate(0, 0, -angle);
        float accelerationAmount = input * acceleration * Time.deltaTime;

        if (input > 0)
        {
            currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.deltaTime, 0f, maxSpeed);
        }
        /*   else if (input < 0)
           {
               // Arabayı geriye hareket ettir
               currentSpeed = Mathf.Clamp(currentSpeed - reverseSpeed * Time.deltaTime, -maxSpeed, 0f);
           }*/
        else
        {
            // Gaz pedalından ayağını çektiğinde veya fren yaparken arabanın hızını azalt
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        rb.linearVelocity = transform.up * currentSpeed;
       

    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Eğer araba Square colliderı üzerinden geçerse, hızını yavaşlat
        if (other.CompareTag("cakil"))
        {
            currentSpeed = 5f;
            maxSpeed = 20f;
        }
    }


}
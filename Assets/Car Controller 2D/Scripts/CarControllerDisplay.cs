using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarControllerDisplay : MonoBehaviour {

    public CarController CarOptions;

    private WheelJoint2D[] wheels;
    private WheelJoint2D frontWheel;
    private WheelJoint2D rearWheel;
    private JointMotor2D motorFront;
    private JointMotor2D motorRear;

    private Text speedText;
    private Text gearText;

    private float carSpeed;
    private float carMotorForce;
    private float currentSpeed = 0f;
    private float targetSpeed = 0f;
    private float acceleration = 5f;  // Hızlanma katsayısı
    private float deceleration = 5f;  // Yavaşlama katsayısı
    private float mySpeed;
    private float myMotorForce;
    private bool isSlowingDown = false;

    void Start()
    {
        CarOptions.currentTransmission = 0;
        wheels = GetComponents<WheelJoint2D>();
        if (wheels.Length >= 2)
        {
            frontWheel = wheels[0];
            rearWheel = wheels[1];
        }
        GetComponent<Rigidbody2D>().mass = CarOptions.CarMass / 10f;
        frontWheel.connectedBody.mass = CarOptions.FrontWheelMass;
        rearWheel.connectedBody.mass = CarOptions.RearWheelMass;

        speedText = GameObject.Find(CarOptions.SpeedTextName)?.GetComponent<Text>();
        gearText = GameObject.Find(CarOptions.GearTextName)?.GetComponent<Text>();
    }

    void FixedUpdate()
    {
        if (CarOptions == null)
        {
            Debug.LogWarning("Assets -> Create -> Car Controller -> Create Car Controller");
            return;
        }

        HUD();
        HandleTransmissions();
        HandleMovement();
    }

    void HUD()
    {
        float mySpeedCar = GetComponent<Rigidbody2D>().velocity.magnitude * 3.6f;
        float speedCar = Mathf.Round(mySpeedCar);

        if (gearText != null)
        {
            gearText.text = "Gear: " + CarOptions.currentTransmission.ToString();
        }
        if (speedText != null)
        {
            speedText.text = "Speed: " + speedCar.ToString();
        }
    }

    void HandleTransmissions()
    {
        int currentTransmission = CarOptions.currentTransmission;

        for (int i = 0; i < CarOptions.Transmission.Length; i++)
        {
            CarOptions.Transmission[i] = i == currentTransmission;
        }

        if (currentTransmission == 0)
        {
            mySpeed = 0;
            myMotorForce = 0;
        }
        else if (currentTransmission > 0 && currentTransmission <= CarOptions.gearRatio.Length)
        {
            mySpeed = carSpeed / CarOptions.gearRatio[currentTransmission - 1];
            myMotorForce = carMotorForce * CarOptions.gearRatio[currentTransmission - 1];
        }
    }

    void HandleMovement()
    {
        if (!CarOptions.FWD && !CarOptions.RWD)
        {
            CarOptions.FWD = true;
            CarOptions.RWD = true;
        }

        carSpeed = CarOptions.MaxSpeed * 11.53846153846154f;
        carMotorForce = CarOptions.motorForce;

        if (Input.GetKeyDown(KeyCode.A) && CarOptions.currentTransmission < CarOptions.gearRatio.Length)
        {
            CarOptions.currentTransmission++;
        }
        if (Input.GetKeyDown(KeyCode.Z) && CarOptions.currentTransmission > 0)
        {
            CarOptions.currentTransmission--;
        }

        float verticalInput = Input.GetAxisRaw("Vertical");

        if (verticalInput > 0)
        {
            targetSpeed = mySpeed;
        }
        else if (verticalInput < 0)
        {
            targetSpeed = -mySpeed;
        }
        else
        {
            targetSpeed = 0;
        }

        // Hızlanma ve yavaşlama
        if (isSlowingDown)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.fixedDeltaTime);
            if (currentSpeed < 0.1f)
            {
                currentSpeed = 0;
                isSlowingDown = false;
            }
        }
        else
        {
            if (currentSpeed < targetSpeed)
            {
                currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.fixedDeltaTime, targetSpeed);
            }
            else if (currentSpeed > targetSpeed)
            {
                currentSpeed = Mathf.Max(currentSpeed - deceleration * Time.fixedDeltaTime, targetSpeed);
            }
        }

        motorFront.motorSpeed = -currentSpeed;
        motorRear.motorSpeed = -currentSpeed;

        if (CarOptions.FWD)
        {
            motorFront.maxMotorTorque = verticalInput > 0 ? myMotorForce : 0;
        }
        if (CarOptions.RWD)
        {
            motorRear.maxMotorTorque = verticalInput > 0 ? myMotorForce : 0;
        }

        frontWheel.motor = motorFront;
        rearWheel.motor = motorRear;
    }

    public void ReduceSpeed()
    {
        isSlowingDown = true;
    }
}

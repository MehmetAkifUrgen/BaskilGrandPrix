using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollider : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            CarControllerDisplay carController = collision.gameObject.GetComponent<CarControllerDisplay>();
            if (carController != null)
            {
                carController.ReduceSpeed();
            }
        }
    }
}

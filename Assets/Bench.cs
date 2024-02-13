using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bench : MonoBehaviour
{
    bool playerEntered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerLocomotion>() != null)
        {
            playerEntered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerLocomotion>())
        {
            playerEntered = false;
        }
    }

    private void Update()
    {
        if (playerEntered)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Destroy(gameObject);
            }
        }
    }
}

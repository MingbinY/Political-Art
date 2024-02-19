using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondEndingTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerLocomotion>() != null)
        {
            GameManager.Instance.RunAway();
        }
    }
}

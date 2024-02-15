using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bench : MonoBehaviour
{
    bool playerEntered = false;

    public List<Sprite> hostileSprites;
    SpriteRenderer spriteRenderer;

    public Sprite goodBenchSprite;

    private void Awake()
    {
        //Assign random sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = hostileSprites[Random.Range(0, hostileSprites.Count)];
    }

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
                spriteRenderer.sprite = goodBenchSprite;
            }
        }
    }
}

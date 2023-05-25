using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private bool isGameRunning;
    private float scrollSpeed = 70;
    [SerializeField] public float xSpawn;
    [SerializeField] public float xTrigger;
    [SerializeField] public float xDestroy;

    public float groundHeight;
    private BoxCollider2D col;

    bool calledGround = false;

    private void OnEnable()
    {
        GameManager.StopGame += StopGround;
    }

    private void OnDisable()
    {
        GameManager.StopGame -= StopGround;
    }

    private void Awake()
    {
        isGameRunning = true;
        col = GetComponent<BoxCollider2D>();
        groundHeight = transform.position.y + col.bounds.extents.y;
    }

    private void FixedUpdate()
    {
        if (isGameRunning)
        {
            float velocity = scrollSpeed / 10;
            Vector2 pos = transform.position;
            pos.x -= velocity * Time.deltaTime;
            transform.position = pos;

            if (transform.position.x <= xDestroy)
            {
                Destroy(gameObject);
                return;
            }

            if (!calledGround)
            {
                if (transform.position.x <= xTrigger) callGround();
            }
        }
    }

    private void callGround()
    {
        calledGround = true;
        GameObject gr = Instantiate(gameObject);
        gr.name = this.name;
        Vector2 pos = new(xSpawn,transform.position.y);
        gr.transform.position = pos;
    }

    private void StopGround()
    {
        isGameRunning = false;
    }
}

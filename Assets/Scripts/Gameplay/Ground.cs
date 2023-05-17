using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private float scrollSpeed = 70;
    [SerializeField] public float xSpawn;
    [SerializeField] public float xTrigger;
    [SerializeField] public float xDestroy;
    bool calledGround = false;

    private void FixedUpdate()
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

    void callGround()
    {
        calledGround = true;
        GameObject gr = Instantiate(gameObject);
        gr.name = this.name;
        // BoxCollider2D grCol = gr.GetComponent<BoxCollider2D>();
        Vector2 pos = new(xSpawn,transform.position.y);
        gr.transform.position = pos;
    }
}

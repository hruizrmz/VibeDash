using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float scrollSpeed = 50;
    [SerializeField] private float spriteDepth = 1;
    [SerializeField] private float spawnX = 1;

    void Update()
    {
        float velocity = scrollSpeed / spriteDepth;
        Vector2 pos = transform.position;
        pos.x -= velocity * Time.deltaTime;
        if (pos.x <= -spawnX) pos.x = spawnX;
        transform.position = pos;
    }
}

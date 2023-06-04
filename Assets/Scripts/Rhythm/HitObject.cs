using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject : MonoBehaviour
{
    public float lifetime = 0.5f;

    private void Update()
    {
        Destroy(gameObject, lifetime);
    }
}
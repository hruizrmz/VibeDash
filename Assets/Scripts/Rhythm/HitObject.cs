using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject : MonoBehaviour
{
    private float lifetime = 0.5f;

    private void Update()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnApplicationQuit()
    {
        Destroy(gameObject);
    }
}
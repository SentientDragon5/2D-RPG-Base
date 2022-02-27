using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    public float lifetime = 1;
    private void Awake()
    {
        Destroy(this.gameObject, lifetime);
    }
}

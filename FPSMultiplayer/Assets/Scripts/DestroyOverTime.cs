using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float lifetim = 1.5f;
    void Start()
    {
        Destroy(gameObject, lifetim);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

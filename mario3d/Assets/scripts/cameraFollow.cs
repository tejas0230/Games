using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    GameObject target;
    public Vector3 offset;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        
    }

    private void LateUpdate()
    {
        if (target == null)
            return;
        transform.position = target.transform.position + offset;
    }
}

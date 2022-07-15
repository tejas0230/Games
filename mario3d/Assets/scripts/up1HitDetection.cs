using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class up1HitDetection : MonoBehaviour
{
    public Rigidbody up1RB;
    public Transform hitDetection;
    public LayerMask hitLayers;

    public float moveSpeed;
    bool hit;

    float direction = 1;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        hit = Physics.CheckSphere(hitDetection.position, 0.05f,hitLayers);
        if(hit&&up1RB.useGravity==true)
        {
            direction *= -1f;
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, -gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
    }

    private void FixedUpdate()
    {
        if (up1RB.useGravity == true)
        {
            up1RB.velocity = new Vector3(up1RB.velocity.x, up1RB.velocity.y, direction * moveSpeed);
        }
    }
}

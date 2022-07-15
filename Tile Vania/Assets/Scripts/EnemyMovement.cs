using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 1f;

    Rigidbody2D enemyRb;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myBoxCollider;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myBoxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFacingRight())
        {
            enemyRb.velocity = new Vector2(moveSpeed, 0);
        }
        else
        {
            enemyRb.velocity = new Vector2(-moveSpeed, 0f);
        }
        
    }

    bool isFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRb.velocity.x)), 1f);
    }


}

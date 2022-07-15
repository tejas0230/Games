using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class up1Object : MonoBehaviour
{
    public Transform playerHit;
    public LayerMask playerLayer;
    bool up1Gained = false;
    bool isDone = false;
    // Start is called before the first frame update
    public float radius = 0.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!up1Gained)
        {
            up1Gained = Physics.CheckSphere(playerHit.transform.position, radius, playerLayer);
            
        }

        if(up1Gained && !isDone)
        {
            playerStatManager.instance.playerLives += 1;
            Destroy(gameObject);
            isDone = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(playerHit.transform.position, radius);
    }

}

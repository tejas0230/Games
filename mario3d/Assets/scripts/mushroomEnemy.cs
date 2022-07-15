using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mushroomEnemy : MonoBehaviour
{
    public Rigidbody mushroomEnemyRB;
    public Transform hitDetection;
    public Transform playerDetection1;
    public Transform playerDetection2;
    public LayerMask hitLayers;
    public LayerMask playerLayer;
    public float moveSpeed;
    bool hit;
    bool hitPlayer;

    public BoxCollider headCollider;
    
    float direction = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        hit = Physics.CheckSphere(hitDetection.position, 0.05f, hitLayers);
        hitPlayer = Physics.CheckCapsule(playerDetection1.transform.position,playerDetection2.transform.position,0.05f,playerLayer);
        if (hit)
        {
            direction *= -1f;
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, -gameObject.transform.localScale.z);
        }
        if(hitPlayer)
        {
            playerStatManager.instance.die();
        }
    }

    private void FixedUpdate()
    {
        
            mushroomEnemyRB.velocity = new Vector3(mushroomEnemyRB.velocity.x, mushroomEnemyRB.velocity.y, direction * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 150f,ForceMode.Impulse);
            playerStatManager.instance.score += 100;
            audioManager.Instance.PlaySound("stomp");
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, 0.2f, gameObject.transform.localScale.z);
            mushroomEnemyRB.constraints = RigidbodyConstraints.FreezePosition;
            headCollider.enabled = false;
            Destroy(gameObject, 0.2f);
        }
    }

   

}

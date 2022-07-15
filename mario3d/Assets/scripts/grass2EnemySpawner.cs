using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grass2EnemySpawner : MonoBehaviour
{
    public GameObject mushRoomEnemyPrefab;
    public Transform[] spawnPoints;
    bool isSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")&&!isSpawned)
        {
            isSpawned = true;
            foreach (Transform s in spawnPoints)
            {
                Instantiate(mushRoomEnemyPrefab,s.transform.position,Quaternion.Euler(0,-180,0));
            }
        }
        
    }
}

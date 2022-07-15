using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStatManager : MonoBehaviour
{
    public Transform marioSpawn;
    public int playerLives = 3;
    public int score=0;
    public GameObject mario;
    public bool canAttack = false;
    public static playerStatManager instance;
    public GameObject spawnedMario;

   
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }    
    }
    
    void Start()
    {
        spawnMario();
    }

    
    void Update()
    {
        
    }

    public void die()
    {
        
        playerLives -= 1;
        Destroy(spawnedMario);
        Invoke("spawnMario", 2f);
    }

    public void spawnMario()
    {
        if(spawnedMario==null)
        {
            spawnedMario = Instantiate(mario, marioSpawn.transform.position, Quaternion.identity);
        }
        
    }
}

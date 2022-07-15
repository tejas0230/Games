using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brickContoller : MonoBehaviour
{
    public GameObject brickBreakParticalPrefab;

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
        if(other.gameObject.CompareTag("Player"))
        {
            audioManager.Instance.PlaySound("break");
            GameObject partical=  Instantiate(brickBreakParticalPrefab, gameObject.transform.position, Quaternion.Euler(-90,0,0)) ;
            Destroy(gameObject);
            Destroy(partical, 2f);
        }
    }
}

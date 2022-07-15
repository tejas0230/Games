using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mysteryBoxHitDetection : MonoBehaviour
{
    public float numberOfMysteryObjects = 1f;
    public GameObject mysterObjectPrefab;
    public Animator mysteryBoxAnimator;
    Animator mysteryObjectAnimator;
   

    public Renderer boxRendere;
    public GameObject questionMarks;
    public Material boxExhausted;

    public Transform mysteryObjectSpawn;
    GameObject spawnedMysteryItem;
    // Start is called before the first frame update
    void Start()
    {
        
       spawnedMysteryItem =  Instantiate(mysterObjectPrefab,mysteryObjectSpawn);
       mysteryObjectAnimator = spawnedMysteryItem.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if(numberOfMysteryObjects!=0)
        {
            if(mysterObjectPrefab.gameObject.name == "1up")
            {
                spawnedMysteryItem.GetComponent<Rigidbody>().useGravity = true;
                spawnedMysteryItem.GetComponent<SphereCollider>().enabled = true;
            }
            mysteryBoxAnimator.SetBool("boxHit", true);
            mysteryObjectAnimator.SetBool("boxHit", true);
           // coinAnimator.SetBool("boxHit", true);
            audioManager.Instance.PlaySound("coin");
            numberOfMysteryObjects--;
        }
        

    }

    private void OnTriggerExit(Collider other)
    {
        mysteryBoxAnimator.SetBool("boxHit", false);
        mysteryObjectAnimator.SetBool("boxHit", false);
    }

    public void changeBoxModel()
    {
        if (numberOfMysteryObjects == 0)
        {
            boxRendere.material = boxExhausted;
            questionMarks.SetActive(false);
        }
    }

}

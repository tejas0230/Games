using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class up1Box : MonoBehaviour
{
    public float numberOfMysteryObjects = 1f;
    public Animator coinBoxAnimator;
    public GameObject up1;
    public MeshRenderer up1BoxRenderer;
    MeshRenderer up1Renderer;
    SphereCollider up1Collider;
    Rigidbody up1RB;
    public Renderer boxRendere;
    
    public Material boxExhausted;
    // Start is called before the first frame update
    void Start()
    {
        up1Renderer = up1.GetComponent<MeshRenderer>();
        up1Collider = up1.GetComponent<SphereCollider>();
        up1RB = up1.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfMysteryObjects == 0)
        {
            boxRendere.material = boxExhausted;
           
            up1BoxRenderer.enabled = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (numberOfMysteryObjects != 0)
        {
            coinBoxAnimator.SetBool("boxHit", true);
            audioManager.Instance.PlaySound("PUappear");
            up1Collider.enabled = true;
            up1RB.useGravity = true;
            up1Renderer.enabled = true;
            numberOfMysteryObjects--;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        coinBoxAnimator.SetBool("boxHit", false);
        
    }
    
}

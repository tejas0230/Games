using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBox : MonoBehaviour
{
    public float numberOfMysteryObjects = 1f;
    public Animator coinBoxAnimator;
    public Animator coinAnimator;

    public Renderer boxRendere;
    public GameObject questionMarks;
    public Material boxExhausted;

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
        if (numberOfMysteryObjects != 0&&other.gameObject.CompareTag("Player"))
        {
            playerStatManager.instance.score += 100;
            coinBoxAnimator.SetBool("boxHit", true);
            coinAnimator.SetBool("boxHit", true);
            audioManager.Instance.PlaySound("coin");
            numberOfMysteryObjects--;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        coinBoxAnimator.SetBool("boxHit", false);
        coinAnimator.SetBool("boxHit", false);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    public Animator doorAnim;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            doorAnim.SetBool("doorOpen",true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            doorAnim.SetBool("doorOpen", false);
        }
    }
}

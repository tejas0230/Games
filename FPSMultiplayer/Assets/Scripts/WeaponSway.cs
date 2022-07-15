using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class WeaponSway : MonoBehaviourPunCallbacks
{
    [Header("Sway settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;

    private void Update()
    {
        if(photonView.IsMine)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

            Quaternion rotationX = Quaternion.AngleAxis(-mouseX, Vector3.up);
            Quaternion rotationY = Quaternion.AngleAxis(mouseY, Vector3.right);

            Quaternion targetRotation = rotationX * rotationY;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
        

    }
}

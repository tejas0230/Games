using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
public class roomButton : MonoBehaviour
{
    public TMP_Text buttonText;
    private RoomInfo info;

    public void setButtonDetails(RoomInfo inputInfo)
    {
        info = inputInfo;
        buttonText.text = info.Name;
    }

    public void openRoom()
    {
        Launcher.instance.joinRoom(info);
    }
}

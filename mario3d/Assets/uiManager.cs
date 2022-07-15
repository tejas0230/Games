using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class uiManager : MonoBehaviour
{
    public GameObject livesPanel;
    public GameObject inGamePanle;
    public Animator fader;

    bool gamePlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        livesPanel.SetActive(true);
        inGamePanle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Return) && !gamePlaying)
        {
            livesPanel.SetActive(false);
            inGamePanle.SetActive(true);
            fader.SetTrigger("changeScene");
            gamePlaying = true;
        }

    }


}

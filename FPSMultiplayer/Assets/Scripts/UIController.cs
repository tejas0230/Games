using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    public static UIController instance;
    public TMP_Text overheatedText;
    public Slider weaponTempSlider;
    public GameObject deathScreen;

    public TMP_Text killedText;

    public Slider playerHealth;

    public TMP_Text kills;
    public TMP_Text deaths;

    public GameObject Leaderboard;
    public leaderboardPlayer leaderboardPlayerDisplay;

    public GameObject endScreen;
    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}

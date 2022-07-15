using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class leaderboardPlayer : MonoBehaviour
{
    public TMP_Text playernameText;
    public TMP_Text kills;
    public TMP_Text deaths;

    public void SetDetails(string name,int Totalkills,int Totaldeaths)
    {
        playernameText.text = name;
        kills.text = Totalkills.ToString();
        deaths.text = Totaldeaths.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class MatchManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static MatchManager instance;

    public List<PlayerInfo> allPlayers = new List<PlayerInfo>();
    private int index;


    private List<leaderboardPlayer> lBoardPlayers = new List<leaderboardPlayer>();

    
    public enum EventCodes : byte
    {
        NewPlayer,
        ListPlayers,
        UpdateStat,
        NextMatch
    }
    private void Awake()
    {
        instance = this;
    }

   public enum GameState
    {
        waiting,
        playing,
        ending
    }
    public bool perpetual;

    public int killsToWin = 3;
    public GameState state = GameState.waiting;
    public float waitAfterEnding = 5f;
    void Start()
    {
        if(!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            NewPlayerSend(PhotonNetwork.NickName);
            state = GameState.playing;
        }
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && state != GameState.ending)
        {
            if(UIController.instance.Leaderboard.activeInHierarchy)
            {
                    UIController.instance.Leaderboard.SetActive(false);
            }
            else
            {
                showLeaderBoard();
            }
            
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code<200)
        {
            EventCodes theEvent = (EventCodes) photonEvent.Code;
            
            object[] data = (object[])photonEvent.CustomData;

            Debug.Log("received eveny" + theEvent);
            switch(theEvent)
            {
                case EventCodes.NewPlayer:
                    NewPlayerReceive(data);
                break;

                case EventCodes.ListPlayers:
                    ListPlayerReceive(data);
                    break;

                case EventCodes.UpdateStat:
                    UpdateStatsReceive(data);
                    break;

                case EventCodes.NextMatch:
                    nextMatchRecieve();
                    break;
            }
        }
    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this); 
    }

    public void NewPlayerSend(string username)
    {
        object[] package = new object[4];
        package[0] = username;
        package[1] = PhotonNetwork.LocalPlayer.ActorNumber;
        package[2] = 0;
        package[3] = 0;

        PhotonNetwork.RaiseEvent((byte)EventCodes.NewPlayer,package,new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient},new SendOptions { Reliability=true});
    }

    public void NewPlayerReceive(object[] dataReceived)
    {
        PlayerInfo player = new PlayerInfo((string)dataReceived[0],(int)dataReceived[1], (int)dataReceived[2], (int)dataReceived[3]);

        allPlayers.Add(player);
        ListPlayerSend();
    }

    public void ListPlayerSend()
    {
        object[] package = new object[allPlayers.Count+1];
        package[0] = state;
        for(int i=0;i<allPlayers.Count;i++)
        {
            object[] piece = new object[4];
            piece[0] = allPlayers[i].name;
            piece[1] = allPlayers[i].actor;
            piece[2] = allPlayers[i].kills;
            piece[3] = allPlayers[i].deaths;

            package[i+1] = piece;
        }
        PhotonNetwork.RaiseEvent((byte)EventCodes.ListPlayers, package, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });
    }

    public void ListPlayerReceive(object[] dataReceived)
    {
        allPlayers.Clear();
        state = (GameState)dataReceived[0];
        for(int i=1;i<dataReceived.Length;i++)
        {
            object[] piece = (object[])dataReceived[i];
            PlayerInfo player = new PlayerInfo((string)piece[0],(int)piece[1],(int)piece[2],(int)piece[3]);

            allPlayers.Add(player);
            if(PhotonNetwork.LocalPlayer.ActorNumber == player.actor)
            {
                index = i-1;
            }
        }
        StateCheck();
    }

    //statToUpdate 0 = kill, 1=death
    public void UpdateStatsSend(int actorSending, int statToUpdate, int amoutToChange)
    {
        object[] package = new object[] { actorSending, statToUpdate, amoutToChange };

        PhotonNetwork.RaiseEvent((byte)EventCodes.UpdateStat, package, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });
    }

    public void UpdateStatsReceive(object[] dataReceived)
    {
        int actor = (int)dataReceived[0];
        int statType = (int)dataReceived[1];
        int amount = (int)dataReceived[2];

        for(int i=0;i<allPlayers.Count;i++)
        {
            if(allPlayers[i].actor == actor)
            {
               switch(statType)
                {
                    case 0:
                        allPlayers[i].kills += amount;
                        break;

                    case 1:
                        allPlayers[i].deaths += amount;
                        break;
                }
                if(i==index)
                {
                    updateStatDisplay();
                }
                if(UIController.instance.Leaderboard.activeInHierarchy)
                {
                    showLeaderBoard();
                }
                break;
            }
        }
        ScoreCheck();
    }
    public void updateStatDisplay()
    {
        if(allPlayers.Count>index)
        {
            UIController.instance.kills.text = "Kills :" + allPlayers[index].kills;
            UIController.instance.deaths.text = "Deaths :" + allPlayers[index].deaths;
        }
        else
        {
            UIController.instance.kills.text = "Kills : 0";
            UIController.instance.deaths.text = "Deaths : 0" ;
        }
        
    }

    void showLeaderBoard()
    {
        UIController.instance.Leaderboard.SetActive(true);
        foreach(leaderboardPlayer lp in lBoardPlayers)
        {
            Destroy(lp.gameObject);
        }
        lBoardPlayers.Clear();
        UIController.instance.leaderboardPlayerDisplay.gameObject.SetActive(false);

        List<PlayerInfo> sorted = SortPlayer(allPlayers);
        foreach(PlayerInfo player in sorted)
        {
            leaderboardPlayer newplayerDisplay= Instantiate(UIController.instance.leaderboardPlayerDisplay, UIController.instance.leaderboardPlayerDisplay.transform.parent);

            newplayerDisplay.SetDetails(player.name,player.kills,player.deaths);
            newplayerDisplay.gameObject.SetActive(true);

            lBoardPlayers.Add(newplayerDisplay);
        }
    }

    private List<PlayerInfo>SortPlayer(List<PlayerInfo> player)
    {
        List<PlayerInfo> sorted = new List<PlayerInfo>();

        while (sorted.Count < player.Count)
        {
            int heigest = -1;
            PlayerInfo selectedPlayer = player[0];
            foreach(PlayerInfo pla in player)
            {
                if(!sorted.Contains(pla))
                {
                    if (pla.kills > heigest)
                    {
                        selectedPlayer = pla;
                        heigest = pla.kills;
                    }
                }
            }
            sorted.Add(selectedPlayer);
        }
        return sorted;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene(0);
    }

    void ScoreCheck()
    {
        bool winnerFound = false;
        foreach(PlayerInfo player in allPlayers)
        {
            if(player.kills >= killsToWin && killsToWin>0)
            {
                winnerFound = true;
                break;
            }
        }
        if(winnerFound)
        {
            if(PhotonNetwork.IsMasterClient && state!=GameState.ending)
            {
                state = GameState.ending;
                ListPlayerSend();
            }
        }
    }

    void StateCheck()
    {
        if(state==GameState.ending)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        state = GameState.ending;
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.DestroyAll();
        }

        UIController.instance.endScreen.SetActive(true);
        showLeaderBoard();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(EndCo());
    }

    private IEnumerator EndCo()
    {
        yield return new WaitForSeconds(waitAfterEnding);
        if(!perpetual)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            if(PhotonNetwork.IsMasterClient)
            {
                nextMatchSend();
            }
        }
       
    }

    public void nextMatchSend()
    {
        PhotonNetwork.RaiseEvent((byte)EventCodes.NextMatch, null, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions { Reliability = true });
    }
    public void nextMatchRecieve()
    {
        state = GameState.playing;
        UIController.instance.endScreen.SetActive(false);
        UIController.instance.Leaderboard.SetActive(false);

        foreach(PlayerInfo player in allPlayers)
        {
            player.kills = 0;
            player.deaths = 0;
        }
        updateStatDisplay();
        PlayerSpawner.instance.spawnPlayer();
    }
}


[System.Serializable]
public class PlayerInfo
{
    public string name;
    public int actor, kills, deaths;

   public PlayerInfo(string _name, int _actor,int _kills,int _deaths)
    {
        name = _name;
        actor = _actor;
        kills = _kills;
        deaths = _deaths;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;

    private void Awake()
    {
        instance = this; 
    }
    public GameObject loadingScreen;
    public GameObject creatRoomScreen;
    public GameObject roomScreen;
    public GameObject menuButtons;
    public GameObject ErrorPanel;
    public GameObject roomBrowers;
    public GameObject nameInputScreen;


    private static bool hasSetNickName;
    public roomButton theRoomButton;
    private List<roomButton> allRoomButtons = new List<roomButton>();

    public TMP_Text playerNameLable;
    private List<TMP_Text> allPlayerName = new List<TMP_Text>();

    public TMP_Text loadingText;
    public TMP_Text roomNameText;
    public TMP_Text errorText;
    public TMP_InputField roomNameInput;
    public TMP_InputField nameInput;

    public GameObject startButton;
    public string levelToPlay;

    void Start() 
    {
        CloseMenus();

        loadingScreen.SetActive(true);
        loadingText.text = "Connecting to network....";
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    void CloseMenus()
    {
        loadingScreen.SetActive(false);
        menuButtons.SetActive(false);
        creatRoomScreen.SetActive(false);
        roomScreen.SetActive(false);
        ErrorPanel.SetActive(false);
        roomBrowers.SetActive(false);
        nameInputScreen.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        loadingText.text = "Joining Lobby.....";
    }

    public override void OnJoinedLobby()
    {
        CloseMenus();
        menuButtons.SetActive(true);
      
        if(!hasSetNickName)
        {
            CloseMenus();
            nameInputScreen.SetActive(true);
            if(PlayerPrefs.HasKey("PlayerName"))
            {
                nameInput.text = PlayerPrefs.GetString("PlayerName");
            }
        }
        else
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");
        }
    }
    void Update()
    {
        
    }

   public  void onMenuCreatRoomPressed()
    {
        CloseMenus();
        creatRoomScreen.SetActive(true);
    }

    public void onCreateRoomPressed()
    {
       if(!string.IsNullOrEmpty(roomNameInput.text))
        {
            RoomOptions options = new RoomOptions();

            options.MaxPlayers = 8;
            PhotonNetwork.CreateRoom(roomNameInput.text, options);

            CloseMenus();
            loadingText.text = "Creating Room....";
            loadingScreen.SetActive(true);
        }
    }

    public override void OnJoinedRoom()
    {
        CloseMenus();
        roomScreen.SetActive(true);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        listAllPlayers();
        if(PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }

    private void listAllPlayers()
    {
        foreach(TMP_Text player in allPlayerName )
        {
            Destroy(player.gameObject);
        }
        allPlayerName.Clear();
        playerNameLable.gameObject.SetActive(false);

        Player[] players = PhotonNetwork.PlayerList;
        for(int i=0;i<players.Length;i++)
        {
            TMP_Text newPlayerLable = Instantiate(playerNameLable, playerNameLable.transform.parent);

            newPlayerLable.text = players[i].NickName;
            newPlayerLable.gameObject.SetActive(true);

            allPlayerName.Add(newPlayerLable);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        TMP_Text newPlayerLable = Instantiate(playerNameLable, playerNameLable.transform.parent);

        newPlayerLable.text = newPlayer.NickName;
        newPlayerLable.gameObject.SetActive(true);

        allPlayerName.Add(newPlayerLable);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        listAllPlayers();
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CloseMenus();
        errorText.text = "Failed to Create room: " + message;
        ErrorPanel.SetActive(true);
    }

    public void CloseErrorScreen()
    {
        CloseMenus();
        menuButtons.SetActive(true);
    }

    public void onQuitPressed()
    {
        Application.Quit();
       // PhotonNetwork.LeaveLobby();
    }

    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        CloseMenus();
        loadingText.text = "Leaving Room ....";
        loadingScreen.SetActive(true);
    }
    public override void OnLeftRoom()
    {
        CloseMenus();
        menuButtons.SetActive(true);
    }

    public void openRoomBrowser()
    {
        CloseMenus();
        roomBrowers.SetActive(true);
    }

    public void CloseRoomBrowser()
    {
        CloseMenus();
        menuButtons.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(roomButton rb in allRoomButtons)
        {
            Destroy(rb.gameObject);
        }
        allRoomButtons.Clear();
        theRoomButton.gameObject.SetActive(false);
        for(int i=0;i<roomList.Count;i++)
        {
            if(roomList[i].PlayerCount!= roomList[i].MaxPlayers && !roomList[i].RemovedFromList)
            {
                roomButton newButton = Instantiate(theRoomButton, theRoomButton.transform.parent);

                newButton.setButtonDetails(roomList[i]);
                newButton.gameObject.SetActive(true);

                allRoomButtons.Add(newButton);
            }
        }
    }

    public void joinRoom(RoomInfo inputInfo)
    {
        PhotonNetwork.JoinRoom(inputInfo.Name);
        CloseMenus();
        loadingText.text = "Joining Room....";
        loadingScreen.SetActive(true);
    }

    public void setNickName()
    {
        if(!string.IsNullOrEmpty(nameInput.text))
        {
            PhotonNetwork.NickName = nameInput.text;
            PlayerPrefs.SetString("PlayerName", nameInput.text);
            CloseMenus();
            menuButtons.SetActive(true);
            hasSetNickName = true;
        }
    }

    public void startGame()
    {
        PhotonNetwork.LoadLevel(levelToPlay);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(true);
        }
        else
        {
            startButton.gameObject.SetActive(false); 
        }
    }
}

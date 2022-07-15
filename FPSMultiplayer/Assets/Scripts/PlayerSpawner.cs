using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner instance;
    public GameObject deathEffect;
    private void Awake()
    {
        instance = this;
    }

    public GameObject playerPrefab;
    private GameObject player;
    private void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            spawnPlayer();
        }
    }
    public void spawnPlayer()
    {
        Transform spawnPoint = SpawnManager.instance.GetSpawnPoint();
       player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);

       
    }

    public void die(string damager)
    {
        
        UIController.instance.killedText.text = "You were killed by "+ damager;
        MatchManager.instance.UpdateStatsSend(PhotonNetwork.LocalPlayer.ActorNumber,1,1);
        if(player!=null)
        {
            StartCoroutine(DieCo());
        }

    }


    public IEnumerator DieCo()
    {
        PhotonNetwork.Instantiate(deathEffect.name, player.transform.position, Quaternion.identity);

        PhotonNetwork.Destroy(player);
        player   = null;
        UIController.instance.deathScreen.SetActive(true);

        yield return new WaitForSeconds(3f);

        
        UIController.instance.deathScreen.SetActive(false);
        if(MatchManager.instance.state == MatchManager.GameState.playing && player == null)
        {
            spawnPlayer();
        }
      
    }
}   
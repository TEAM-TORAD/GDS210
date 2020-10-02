using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MPManager : MonoBehaviourPunCallbacks
{
    public GameObject[] enableObjectsOnConnect;
    public GameObject[] disableObjectsOnConnect;
    string serverName = "";
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        foreach (GameObject o in enableObjectsOnConnect)
        {
            o.SetActive(true);
        }
        foreach (GameObject o in disableObjectsOnConnect)
        {
            o.SetActive(false);
        }
        Debug.Log("We are now connected to Photon.");
        //PhotonNetwork.ConnectToRegion("au");
        //PhotonNetwork.ConnectUsingSettings();
        //PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void JoinNinjaVillageServer()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + "\n" + message);
        CreateNinjaVillageServer();
    }
    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(serverName);
    }
    public void CreateNinjaVillageServer()
    {
        Debug.Log("Server created");
        PhotonNetwork.AutomaticallySyncScene = true;
        RoomOptions RO = new RoomOptions { MaxPlayers = 10, IsOpen = true, IsVisible = true };
        PhotonNetwork.CreateRoom("defaultFFA", RO, TypedLobby.Default);
        //Set the name of the server to be loaded once the player has joined a room
        serverName = "Village_GreyBox";

    }
}

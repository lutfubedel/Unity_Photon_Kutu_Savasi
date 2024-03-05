using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ServerManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        DontDestroyOnLoad(gameObject);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Servere Baðlandý");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobiye girdi");
    }

    public void MyRandomLogin()
    {
        print("RandomLogin");
        PhotonNetwork.LoadLevel(1);
        PhotonNetwork.JoinRandomRoom();
    }

    public void MyCreateRoom()
    {
        print("CreateRoom");
        PhotonNetwork.LoadLevel(1);
        string roomName = "Room_" + Random.Range(0, 1000).ToString();
        PhotonNetwork.JoinOrCreateRoom(roomName,new RoomOptions { MaxPlayers=2,IsOpen=true,IsVisible=true},TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        InvokeRepeating(nameof(MyCheckInfo), 0, 1f);
        GameObject myPlayer = PhotonNetwork.Instantiate("Player",Vector3.zero,Quaternion.identity,0,null);
        myPlayer.GetComponent<PhotonView>().Owner.NickName = PlayerPrefs.GetString("UserName");

        if(PhotonNetwork.PlayerList.Length == 2)
        {
            myPlayer.gameObject.tag = "Player_2";
            GameObject.FindWithTag("GameManager").gameObject.GetComponent<PhotonView>().RPC("StartPrizeSpawner", RpcTarget.All);
        }
    }


    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        InvokeRepeating(nameof(MyCheckInfo), 0, 1f);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }


    public void MyCheckInfo()
    {
        if(PhotonNetwork.PlayerList.Length == 2)
        {
            GameObject.FindGameObjectWithTag("PlayersWaiting").SetActive(false);
            GameObject.FindGameObjectWithTag("Player1Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindGameObjectWithTag("Player2Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[1].NickName;
            CancelInvoke();
        }
        else
        {
            GameObject.FindGameObjectWithTag("PlayersWaiting").SetActive(true);
            GameObject.FindGameObjectWithTag("Player1Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[0].NickName;
            GameObject.FindGameObjectWithTag("Player2Name").GetComponent<TextMeshProUGUI>().text = "......";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ServerManager : MonoBehaviourPunCallbacks
{
    TMP_Text serverInfo;
    GameObject buttonSaveName;
    GameObject buttonFastGame;
    GameObject buttonCreateRoom;

    public bool isButton;



    private void Start()
    {
        serverInfo = GameObject.FindWithTag("ServerInfo").gameObject.GetComponent<TMP_Text>();
        buttonSaveName = GameObject.FindWithTag("Button_SaveName");
        buttonFastGame = GameObject.FindWithTag("Button_FastGame");
        buttonCreateRoom = GameObject.FindWithTag("Button_CreateRoom");

        PhotonNetwork.ConnectUsingSettings();
        DontDestroyOnLoad(gameObject);
    }

    public override void OnConnectedToMaster()
    {
        serverInfo.text = "Successful Server Connection";
        Debug.Log("Servere Baðlandý");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        serverInfo.text = "Successful Loby Connection";

        if (!PlayerPrefs.HasKey("UserName"))
        {
            buttonSaveName.gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            buttonCreateRoom.gameObject.GetComponent<Button>().interactable = true;
            buttonFastGame.gameObject.GetComponent<Button>().interactable = true;
        }

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

        if (PhotonNetwork.PlayerList.Length == 2)
        {
            Debug.Log("player_2 girdi");
            myPlayer.gameObject.tag = "Player_2";
            GameObject.FindWithTag("GameManager").GetComponent<PhotonView>().RPC("StartPrizeSpawner", RpcTarget.All);
        }
    }


    public override void OnLeftRoom()
    {
        if(isButton)
        {
            Time.timeScale = 1;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();

            Time.timeScale = 1;
            PlayerPrefs.SetInt("TotalMatch", (PlayerPrefs.GetInt("TotalMatch") + 1));
            PlayerPrefs.SetInt("Lose", (PlayerPrefs.GetInt("Lose") + 1));
            PlayerPrefs.SetInt("Score", (PlayerPrefs.GetInt("Score") - 10));


        }
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

        if (isButton)
        {
            PhotonNetwork.ConnectUsingSettings();
            Time.timeScale = 1;

        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            Time.timeScale = 1;


            PlayerPrefs.SetInt("TotalMatch", (PlayerPrefs.GetInt("TotalMatch") + 1));
            PlayerPrefs.SetInt("Win", (PlayerPrefs.GetInt("Win") + 1));
            PlayerPrefs.SetInt("Score", (PlayerPrefs.GetInt("Score") + 100));
        }

        InvokeRepeating(nameof(MyCheckInfo), 0, 1f);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        serverInfo.text = "Failed To Join The Room";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        serverInfo.text = "Failed To Join The Room";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        serverInfo.text = "Failed To Create Room";
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

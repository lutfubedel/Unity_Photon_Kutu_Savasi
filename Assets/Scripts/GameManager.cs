using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("Players Health")]
    [SerializeField] private Image player1_healthBar;
    [SerializeField] private Image player2_healthBar;
    [SerializeField] private float player1_health = 100f;
    [SerializeField] private float player2_health = 100f;

    PhotonView pw;

    private bool isPrizeSpawnerStarted = false;
    private int limit = 5;
    private float waitingTime = 20f;
    private int spawnedPrizeCount;

    [SerializeField] private GameObject[] prizeSpawnPoints;

    [SerializeField] private TMP_Text player1_Name;
    [SerializeField] private TMP_Text player2_Name;

    [SerializeField] private bool isGameOver = false;

    private void Start()
    {
        pw = GetComponent<PhotonView>();
    }

    IEnumerator PrizeSpawner()
    {
        spawnedPrizeCount = 0;

        while (true && isPrizeSpawnerStarted)
        {
            if (limit == spawnedPrizeCount)
            {
                isPrizeSpawnerStarted = false;
            }

            yield return new WaitForSeconds(waitingTime);

            int value = Random.Range(0, prizeSpawnPoints.Length);
            PhotonNetwork.Instantiate("Prize", prizeSpawnPoints[value].transform.position, prizeSpawnPoints[value].transform.rotation,0,null);
            spawnedPrizeCount++;
        }
    }

    [PunRPC]
    public void StartPrizeSpawner()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            isPrizeSpawnerStarted = true;
            StartCoroutine(PrizeSpawner());
        }
    }



    [PunRPC]
    public void TowerDamage(int choice, float damagePower)
    {
        switch (choice)
        {
            case 1:
                player1_health -= damagePower;
                player1_healthBar.fillAmount = player1_health / 100;

                if (player1_health <= 0)
                {
                    foreach (GameObject item in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if(item.CompareTag("Panel_End"))
                        {
                            item.SetActive(true);
                            GameObject.FindWithTag("EndGameText").GetComponent<Text>().text = player2_Name.text + " WINS";
                        }
                    }

                    Winner(2);
                }
                break;

            case 2:
                player2_health -= damagePower;
                player2_healthBar.fillAmount = player2_health / 100;

                if (player2_health <= 0)
                {
                    foreach (GameObject item in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
                    {
                        if (item.CompareTag("Panel_End"))
                        {
                            item.SetActive(true);
                            GameObject.FindWithTag("EndGameText").GetComponent<Text>().text = player1_Name.text + " WINS";
                        }
                    }

                    Winner(1);
                }
                break;
        }
    }

    [PunRPC]
    public void RenewHealth(int playerNo)
    {
        switch (playerNo)
        {
            case 1:
                player1_health += 30;

                if (player1_health >= 100)
                {
                    player1_health = 100;
                    player1_healthBar.fillAmount = player1_health / 100;
                }
                else
                {
                    player1_healthBar.fillAmount = player1_health / 100;
                }
                break;

            case 2:
                player2_health += 30;

                if (player2_health >= 100)
                {
                    player2_health = 100;
                    player2_healthBar.fillAmount = player2_health / 100;
                }
                else
                {
                    player2_healthBar.fillAmount = player2_health / 100;
                }
                break;
        }
    }



    public void Winner(int value)
    {
        if(!isGameOver)
        {
            GameObject.FindWithTag("Player_1").GetComponent<Player>().WhoIsWon(value);
            GameObject.FindWithTag("Player_2").GetComponent<Player>().WhoIsWon(value);
            isGameOver = true;
        }
    }




    public void MainMenu()
    {
        GameObject.FindWithTag("ServerManager").GetComponent<ServerManager>().isButton = true;
        print(GameObject.FindWithTag("ServerManager").GetComponent<ServerManager>().isButton);
        PhotonNetwork.LoadLevel(0);
    }

    public void ExitButton()
    {
        print(GameObject.FindWithTag("ServerManager").GetComponent<ServerManager>().isButton);
        PhotonNetwork.LoadLevel(0);
    }





}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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
    private float waitingTime =5f;
    private int spawnedPrizeCount;

    private void Start()
    {
        pw = GetComponent<PhotonView>();
    }

    IEnumerator PrizeSpawner()
    {
        spawnedPrizeCount = 0;

        while (true && isPrizeSpawnerStarted)
        {
            if(limit==spawnedPrizeCount)
            {
                isPrizeSpawnerStarted = false;
            }

            yield return new WaitForSeconds(waitingTime);
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
                if(PhotonNetwork.IsMasterClient)
                {
                    player1_health -= damagePower;
                    player1_healthBar.fillAmount = player1_health / 100;
                }
                break;

            case 2:
                if (PhotonNetwork.IsMasterClient)
                {
                    player2_health -= damagePower;
                    player2_healthBar.fillAmount = player2_health / 100;
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





}

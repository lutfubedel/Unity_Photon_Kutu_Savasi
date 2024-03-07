using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class CanonBall : MonoBehaviour
{

    [SerializeField] private float damagePower = 20f;
    [SerializeField] private int playerNo;

    AudioSource canonHitSound;
    GameManager gameManager;
    Player player;
    PhotonView pw;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        pw = GetComponent<PhotonView>();
        canonHitSound = GetComponent<AudioSource>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.CompareTag("OtherBoxes"))
        {
            collision.gameObject.GetComponent<PhotonView>().RPC("takeDamage", RpcTarget.All, damagePower);
            player.PowerBarMoveAgain();

            if(pw.IsMine)
            {
                PhotonNetwork.Instantiate("Destruction_2", transform.position, transform.rotation, 0, null);
                canonHitSound.Play();
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("FlatBoard") || collision.gameObject.CompareTag("CanonBall"))
        {
            player.PowerBarMoveAgain();

            PhotonNetwork.Instantiate("Destruction_1", transform.position, transform.rotation, 0, null);
            canonHitSound.Play();

            if (pw.IsMine)
            {         
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Player_2_Tower"))
        {
            if(playerNo != 2)
            {
                gameManager.GetComponent<PhotonView>().RPC("TowerDamage", RpcTarget.All, 2, damagePower);
            }

            player.PowerBarMoveAgain();

            if (pw.IsMine)
            {
                PhotonNetwork.Instantiate("Destruction_1", transform.position, transform.rotation, 0, null);
                canonHitSound.Play();
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Player_1_Tower"))
        {
            if(playerNo != 1)
            {
                gameManager.GetComponent<PhotonView>().RPC("TowerDamage", RpcTarget.All, 1, damagePower);
            }

            player.PowerBarMoveAgain();

            if (pw.IsMine)
            {
                PhotonNetwork.Instantiate("Destruction_1", transform.position, transform.rotation, 0, null);
                canonHitSound.Play();
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Prize"))
        {
            gameManager.GetComponent<PhotonView>().RPC("RenewHealth", RpcTarget.All,playerNo);
            player.PowerBarMoveAgain();

            PhotonNetwork.Instantiate("Destruction_1", transform.position, transform.rotation, 0, null);
            canonHitSound.Play();

            PhotonNetwork.Destroy(collision.transform.gameObject);
            if (pw.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }


    [PunRPC]
    public void SendTag(string tagComing)
    {
        player = GameObject.FindWithTag(tagComing).GetComponent<Player>();

        if(tagComing == "Player_1")
        {
            playerNo = 1;
        }
        else
        {
            playerNo = 2;
        }
    }


}

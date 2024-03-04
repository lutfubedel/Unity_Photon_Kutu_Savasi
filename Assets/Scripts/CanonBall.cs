using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CanonBall : MonoBehaviour
{

    [SerializeField] private float damagePower = 20f;

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
            collision.gameObject.GetComponent<OtherBoxes>().takeDamage(damagePower);
            player.PowerBarMoveAgain();

            if(pw.IsMine)
            {
                PhotonNetwork.Instantiate("Destruction_1", transform.position, transform.rotation, 0, null);
                canonHitSound.Play();
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            player.PowerBarMoveAgain();

            if (pw.IsMine)
            {
                PhotonNetwork.Instantiate("Destruction_1", transform.position, transform.rotation, 0, null);
                canonHitSound.Play();
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Player_2_Tower"))
        {
            gameManager.TowerDamage(2, damagePower);
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
            gameManager.TowerDamage(1, damagePower);
            player.PowerBarMoveAgain();

            if (pw.IsMine)
            {
                PhotonNetwork.Instantiate("Destruction_1", transform.position, transform.rotation, 0, null);
                canonHitSound.Play();
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }


    [PunRPC]
    public void SendTag(string tagComing)
    {
        player = GameObject.FindWithTag(tagComing).GetComponent<Player>();
    }


}

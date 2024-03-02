using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBall : MonoBehaviour
{
    [SerializeField] private float damagePower=20f;

    GameManager gameManager;
    Player player;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player_1").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("OtherBoxes"))
        {
            collision.gameObject.GetComponent<OtherBoxes>().takeDamage(damagePower);
            gameManager.EffectSoundCreater(1,collision.gameObject);

            player.PowerBarMoveAgain();
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            player.PowerBarMoveAgain();
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player_2_Tower"))
        {
            gameManager.EffectSoundCreater(1, collision.gameObject);
            gameManager.TowerDamage(2, damagePower);
            player.PowerBarMoveAgain();

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player_1_Tower"))
        {
            gameManager.EffectSoundCreater(1, collision.gameObject);
            gameManager.TowerDamage(1, damagePower);
            player.PowerBarMoveAgain();

            Destroy(gameObject);
        }
    }


}

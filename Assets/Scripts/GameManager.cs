using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    [Header("OtherBoxes Effects and Sounds")]
    [SerializeField] private GameObject boxesDestroyEffect;
    [SerializeField] private AudioSource boxesHitSound;

    [Header("Players Health")]
    [SerializeField] private Image player1_healthBar;
    [SerializeField] private Image player2_healthBar;
    [SerializeField] private float player1_health = 100f;
    [SerializeField] private float player2_health = 100f;





    public void EffectSoundCreater(int choice , GameObject hitObject)
    {
        switch (choice)
        {
            case 2:
                Instantiate(boxesDestroyEffect, hitObject.transform.position, hitObject.transform.rotation);
                boxesHitSound.Play();
                break;
        }
    }

    public void TowerDamage(int choice, float damagePower)
    {
        switch (choice)
        {
            case 1:
                player1_health -= damagePower;
                player1_healthBar.fillAmount = player1_health / 100;
                break;
            case 2:
                player2_health -= damagePower;
                player2_healthBar.fillAmount = player2_health / 100;
                break;
        }
    }



}

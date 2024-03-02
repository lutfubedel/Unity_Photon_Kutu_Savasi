using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherBoxes : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject healthCanvas;


    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }


    public void takeDamage(float damagePower)
    {
        health -= damagePower;
        healthBar.fillAmount = health / 100;

        if (health <= 0)
        {
            gameManager.EffectSoundCreater(2, this.gameObject);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(ShowCanvas());
        }
        
    }

    IEnumerator ShowCanvas()
    {
        if(!healthCanvas.activeInHierarchy)
        {
            healthCanvas.SetActive(true);
            yield return new WaitForSeconds(2);
            healthCanvas.SetActive(false);
        }
    }
}

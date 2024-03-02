using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public GameObject canonBall;
    public GameObject firePoint;
    public GameObject fireBallSpawnEffect;

    private float powerBarValue;
    public bool isOver = false;

    public AudioSource throwingBallSound;
    public Image powerBar;

    Coroutine powerBarLoop;

    private void Start()
    {
        powerBarLoop = StartCoroutine(PowerBarSystem());
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject instantiatedBall = Instantiate(canonBall, firePoint.transform.position, firePoint.transform.rotation);
            Rigidbody2D rb = instantiatedBall.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(1,0) * 12 * powerBar.fillAmount, ForceMode2D.Impulse);

            Instantiate(fireBallSpawnEffect, firePoint.transform.position, firePoint.transform.rotation);
            throwingBallSound.Play();

            StopCoroutine(powerBarLoop);

        }
    }


    public void PowerBarMoveAgain()
    {
        powerBarLoop = StartCoroutine(PowerBarSystem());
    }

    IEnumerator PowerBarSystem()
    {
        isOver = false;
        powerBar.fillAmount = 0;

        while (true)
        {
            if(powerBar.fillAmount < 1 && !isOver)
            {
                powerBarValue = 0.0075f;
                powerBar.fillAmount += powerBarValue;
                yield return new WaitForSeconds(0.001f *Time.deltaTime);
            }
            else
            {
                isOver = true;
                powerBarValue = 0.0075f;
                powerBar.fillAmount -= powerBarValue;
                yield return new WaitForSeconds(0.001f * Time.deltaTime);

                if(powerBar.fillAmount == 0)
                {
                    isOver = false;
                }
            }
        }
    }
}

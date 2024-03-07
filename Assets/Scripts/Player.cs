using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;


public class Player : MonoBehaviour
{
    [Header("CanonBall Values")]
    [SerializeField] private GameObject canonBall;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject fireBallSpawnEffect;


    private float powerBarValue;
    private float shootingDirection;
    public bool isOver = false;
    public bool canFire = false;

    public AudioSource throwingBallSound;
    
    [SerializeField] Image powerBar;
    Coroutine powerBarLoop;
    PhotonView pw;

    

    private void Start()
    {
        pw = GetComponent<PhotonView>();

        if(pw.IsMine)
        {
            powerBar = GameObject.FindWithTag("PowerBar").GetComponent<Image>();

            if (PhotonNetwork.IsMasterClient)
            {
                transform.position = GameObject.FindWithTag("SpawnPoint_1").transform.position;
                transform.rotation = GameObject.FindWithTag("SpawnPoint_1").transform.rotation;
                shootingDirection = 1f;
            }
            else
            {
                transform.position = GameObject.FindWithTag("SpawnPoint_2").transform.position;
                transform.rotation = GameObject.FindWithTag("SpawnPoint_2").transform.rotation;
                shootingDirection = -1f;
            }
        }

        InvokeRepeating(nameof(IsGameStarted), 0, 0.5f);

        powerBarLoop = StartCoroutine(PowerBarSystem());
        


    }
    private void Update()
    {

        print(PhotonNetwork.PlayerList.Length);
        if(pw.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space) && canFire)
            {
                GameObject instantiatedBall = PhotonNetwork.Instantiate("CanonBall", firePoint.transform.position, firePoint.transform.rotation, 0, null);
                instantiatedBall.GetComponent<PhotonView>().RPC("SendTag",RpcTarget.All,gameObject.tag);

                PhotonNetwork.Instantiate("Explosion", firePoint.transform.position, firePoint.transform.rotation, 0, null);

                Rigidbody2D rb = instantiatedBall.GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(shootingDirection, 0.5f) * 15 * powerBar.fillAmount, ForceMode2D.Impulse);

                throwingBallSound.Play();

                StopCoroutine(powerBarLoop);

            }

            //if(Input.touchCount>0 && canFire)
            //{
            //    GameObject instantiatedBall = PhotonNetwork.Instantiate("CanonBall", firePoint.transform.position, firePoint.transform.rotation, 0, null);
            //    instantiatedBall.GetComponent<PhotonView>().RPC("SendTag", RpcTarget.All, gameObject.tag);

            //    PhotonNetwork.Instantiate("Explosion", firePoint.transform.position, firePoint.transform.rotation, 0, null);

            //    Rigidbody2D rb = instantiatedBall.GetComponent<Rigidbody2D>();
            //    rb.AddForce(new Vector2(shootingDirection, 0.5f) * 15 * powerBar.fillAmount, ForceMode2D.Impulse);

            //    throwingBallSound.Play();

            //    canFire = false;
            //    StopCoroutine(powerBarLoop);
            //}
        }
      
    }

    public void IsGameStarted()
    {
        if(PhotonNetwork.PlayerList.Length == 2)
        {
            if(pw.IsMine)
            {
                canFire = true;
                powerBarLoop = StartCoroutine(PowerBarSystem());
                CancelInvoke(nameof(IsGameStarted));
            }

        }
        else
        {
            StopAllCoroutines();
        }
    }

    public void PowerBarMoveAgain()
    {
        powerBarLoop = StartCoroutine(PowerBarSystem());
    }

    IEnumerator PowerBarSystem()
    {
        powerBar.fillAmount = 0f;
        isOver = false;

        while (true)
        {
            if(powerBar.fillAmount < 1 && !isOver)
            {
                powerBarValue = 0.01f;
                powerBar.fillAmount += powerBarValue;
                yield return new WaitForSeconds(0.001f *Time.deltaTime);
            }
            else
            {
                isOver = true;
                powerBarValue = 0.01f;
                powerBar.fillAmount -= powerBarValue;
                yield return new WaitForSeconds(0.001f * Time.deltaTime);

                if(powerBar.fillAmount == 0)
                {
                    isOver = false;
                }
            }
        }
    }


    public void WhoIsWon(int value)
    {
        if(pw.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (value == 1)
                {
                    PlayerPrefs.SetInt("TotalMatch", (PlayerPrefs.GetInt("TotalMatch") + 1));
                    PlayerPrefs.SetInt("Win", (PlayerPrefs.GetInt("Win") + 1));
                    PlayerPrefs.SetInt("Score", (PlayerPrefs.GetInt("Score") + 100));
                }
                else
                {
                    PlayerPrefs.SetInt("TotalMatch", (PlayerPrefs.GetInt("TotalMatch") + 1));
                    PlayerPrefs.SetInt("Lose", (PlayerPrefs.GetInt("Lose") + 1));
                    PlayerPrefs.SetInt("Score", (PlayerPrefs.GetInt("Score") - 20));
                }
            }
            else
            {
                if (value == 2)
                {
                    PlayerPrefs.SetInt("TotalMatch", (PlayerPrefs.GetInt("TotalMatch") + 1));
                    PlayerPrefs.SetInt("Win", (PlayerPrefs.GetInt("Win") + 1));
                    PlayerPrefs.SetInt("Score", (PlayerPrefs.GetInt("Score") + 100));
                }
                else
                {
                    PlayerPrefs.SetInt("TotalMatch", (PlayerPrefs.GetInt("TotalMatch") + 1));
                    PlayerPrefs.SetInt("Lose", (PlayerPrefs.GetInt("Lose") + 1));
                    PlayerPrefs.SetInt("Score", (PlayerPrefs.GetInt("Score") - 20));
                }
            }
        }

        Time.timeScale = 0;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class Player : MonoBehaviour
{
    [Header("CanonBall Values")]
    [SerializeField] private GameObject canonBall;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject fireBallSpawnEffect;


    private float powerBarValue;
    private float shootingDirection;
    public bool isOver = false;

    public AudioSource throwingBallSound;
    private Image powerBar;

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
        if(pw.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject instantiatedBall = PhotonNetwork.Instantiate("CanonBall", firePoint.transform.position, firePoint.transform.rotation, 0, null);
                instantiatedBall.GetComponent<PhotonView>().RPC("SendTag",RpcTarget.All,gameObject.tag);

                PhotonNetwork.Instantiate("Explosion", firePoint.transform.position, firePoint.transform.rotation, 0, null);

                Rigidbody2D rb = instantiatedBall.GetComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(shootingDirection, 1) * 20 * powerBar.fillAmount, ForceMode2D.Impulse);

                throwingBallSound.Play();

                StopCoroutine(powerBarLoop);

            }
        }
      
    }

    public void IsGameStarted()
    {
        if(PhotonNetwork.PlayerList.Length == 2)
        {
            if(pw.IsMine)
            {
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
        isOver = false;
        powerBar.fillAmount = 0f;

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

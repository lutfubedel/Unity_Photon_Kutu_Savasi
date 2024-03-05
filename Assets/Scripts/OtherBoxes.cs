using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class OtherBoxes : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject healthCanvas;

    AudioSource hitSound;
    PhotonView pw;


    private void Start()
    {
        hitSound = GetComponent<AudioSource>();
        pw = GetComponent<PhotonView>();
    }


    [PunRPC]
    public void takeDamage(float damagePower)
    {
        health -= damagePower;
        healthBar.fillAmount = health / 100;

        if (health <= 0)
        {
            PhotonNetwork.Instantiate("Destruction_1", transform.position, transform.rotation, 0, null);
            hitSound.Play();
            PhotonNetwork.Destroy(gameObject);
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

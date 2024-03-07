using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Prize : MonoBehaviour
{
    PhotonView pw;

    private void Start()
    {
        pw = GetComponent<PhotonView>();
        StartCoroutine(DestroyItSelf());
    }


    IEnumerator DestroyItSelf()
    {
        yield return new WaitForSeconds(10f);
        if(pw.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);   
        }
    }
}

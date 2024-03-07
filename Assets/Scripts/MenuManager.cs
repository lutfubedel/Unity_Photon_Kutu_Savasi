using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject panel_userName;
    [SerializeField] private GameObject panelEntry;
    [SerializeField] private GameObject panelInfo;

    [Header("UI")]
    [SerializeField] private TMP_InputField userNameInputField;
    [SerializeField] private TMP_Text userNameText;

    [SerializeField] private TMP_Text[] infoTexts;

    GameObject buttonFastGame;
    GameObject buttonCreateRoom;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("UserName"))
        {
            PlayerPrefs.SetInt("TotalMatch", 0);
            PlayerPrefs.SetInt("Lose", 0);
            PlayerPrefs.SetInt("Win", 0);
            PlayerPrefs.SetInt("Score", 0);

            panel_userName.SetActive(true);
            panelEntry.SetActive(false);
            panelInfo.SetActive(false);
            InfoWriter();
        }
        else
        {
            panel_userName.SetActive(false);
            panelEntry.SetActive(true);
            panelInfo.SetActive(true);
            userNameText.text = "username : " + PlayerPrefs.GetString("UserName");
            InfoWriter();
        }
    }


    public void SaveUserName()
    {
        PlayerPrefs.SetString("UserName", userNameInputField.text);
        userNameText.text = "username : " + PlayerPrefs.GetString("UserName");

        panel_userName.SetActive(false);
        panelEntry.SetActive(true);
        panelInfo.SetActive(true);

        buttonFastGame = GameObject.FindWithTag("Button_FastGame");
        buttonCreateRoom = GameObject.FindWithTag("Button_CreateRoom");

        buttonCreateRoom.gameObject.GetComponent<Button>().interactable = true;
        buttonFastGame.gameObject.GetComponent<Button>().interactable = true;
    }

    public void InfoWriter()
    {
        infoTexts[0].text = PlayerPrefs.GetInt("TotalMatch").ToString();
        infoTexts[1].text = PlayerPrefs.GetInt("Lose").ToString();
        infoTexts[2].text = PlayerPrefs.GetInt("Win").ToString();
        infoTexts[3].text = PlayerPrefs.GetInt("Score").ToString();

        if(PlayerPrefs.GetInt("Score") <= 0)
        {
            PlayerPrefs.SetInt("Score", 0);
        }
    }

}

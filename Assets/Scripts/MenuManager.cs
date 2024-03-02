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

    [Header("UI")]
    [SerializeField] private TMP_InputField userNameInputField;
    [SerializeField] private TMP_Text userNameText;

    private void Start()
    {
        if(!PlayerPrefs.HasKey("UserName"))
        {
            panel_userName.SetActive(true);
        }
        else
        {
            panelEntry.SetActive(true);
            userNameText.text = "username : " + PlayerPrefs.GetString("UserName");
        }
    }


    public void SaveUserName()
    {
        PlayerPrefs.SetString("UserName", userNameInputField.text);
        userNameText.text = "username : " + PlayerPrefs.GetString("UserName");

        panel_userName.SetActive(false);
        panelEntry.SetActive(true);
    }


}

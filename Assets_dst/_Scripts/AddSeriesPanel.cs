using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSeriesPanel : MonoBehaviour
{
    string name;
    string password;

    [SerializeField]
    InputField nameIF;

    [SerializeField]
    InputField commentIF;

    [SerializeField]
    InputField explanations;

    [SerializeField]
    Text message;

    public JsonData loginJsonResponse;

    public void onRegisterBtnClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Registration);
    }
    public void onRecoveryBtnClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Recover);
    }

    public void onLoginButtonClick()
    {
        //email = emailIF.text;
        //password = passwordIF.text;
        if (!NetworkingManager.Instance.validateEmail("sagargurjar59@gmail.com"))
        {
            Debug.Log("Email is not valid");
            message.text = "Email is not valid";
            return;
        }
        if (string.IsNullOrEmpty(password))
        {
            message.text = "Password  is null or empty";
            Debug.Log("Password field is empty");
            return;
        }
        //PlayerData.Instance.PlayerName = name;
        //PlayerData.Instance.PlayerGUID = System.Guid.NewGuid().ToString();
        //StartCoroutine(CallLogin("Sa","GA"));
        //UIMenu.Instance.changeMode(UIMenu.ePanel.Welcome);
        Debug.Log("Login");
        PostLoginData();

    }
    public void PostLoginData()
    {
        NetworkConst.series data = new NetworkConst.series();
        //data.email = email;
        //data.password = password;

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIPostLoginData(json, (string data) =>
        {
            Debug.Log("God Login PLayer Response" + data);
            loginJsonResponse = JsonMapper.ToObject(data);
            if ((bool)loginJsonResponse["status"])
            {
                Debug.Log("Status is True");
                UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
            }
            else
            {
                message.text = loginJsonResponse["message"].ToString();
            }

            ////PlayerData.Instance.PlayerName = name;
            ////PlayerData.Instance.PlayerGUID = playerRowKey;

        });
    }
}

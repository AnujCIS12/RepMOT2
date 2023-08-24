using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class LoginPanel : MonoBehaviour
{
    string email;
    string password;

    [SerializeField]
    InputField emailIF;

    [SerializeField]
    InputField passwordIF;

    [SerializeField]
    Text message;

    NetworkConst.loginRes _loginRes;

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
        email = emailIF.text;
        password = passwordIF.text;
        if (!NetworkingManager.Instance.IsValidEmail(email))
        {
            Debug.Log("Email is not valid");
            message.text = "Email is not valid";
            return;
        }if (string.IsNullOrEmpty(password))
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
        string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postLogin data = new NetworkConst.postLogin();
        data.email = email;
        data.password = password;

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIPostLoginData(json, (string data) =>
        {
            
            loginJsonResponse = JsonMapper.ToObject(data);
            Debug.Log("God Login PLayer Response" + loginJsonResponse["data"]);
            if ((bool)loginJsonResponse["status"])
            {
                Debug.Log("Status is True");
                this._loginRes = JsonUtility.FromJson<NetworkConst.loginRes>(data);
                Debug.Log(this._loginRes.data.token);
                CreatorData.Instance.setCreatorData(this._loginRes.data);
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

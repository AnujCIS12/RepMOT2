using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class LoginPanel : MonoBehaviour
{

    [SerializeField]
    InputField emailIF;

    [SerializeField]
    InputField passwordIF;

    [SerializeField]
    Text message;   

    public void onRegisterBtnClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Registration);
    }
    public void onRecoveryBtnClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Recover);
    }
    private void OnEnable()
    {
        Debug.Log("Load Credentials");
        loadCredentials();
    }

    public void onLoginButtonClick()
    {
        if (!NetworkingManager.Instance.IsValidEmail(emailIF.text))
        {
            Debug.Log("Email is not valid");
            message.text = "Email is not valid";
            return;
        }if (string.IsNullOrEmpty(passwordIF.text))
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
    public void saveCredentials()
    {
        PlayerPrefs.SetString("EmailID", emailIF.text);
        PlayerPrefs.SetString("Password", passwordIF.text);
    }
    public void loadCredentials()
    {
        emailIF.text = PlayerPrefs.GetString("EmailID");
        passwordIF.text = PlayerPrefs.GetString("Password");
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void PostLoginData()
    {
        string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postLogin data = new NetworkConst.postLogin();
        data.email = emailIF.text;
        data.password = passwordIF.text;

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIPostLoginData(json, (string data) =>
        {
            JsonData loginJsonResponse;
            loginJsonResponse = JsonMapper.ToObject(data);
            Debug.Log("God Login PLayer Response" + loginJsonResponse["data"]);
            if ((bool)loginJsonResponse["status"])
            {
                saveCredentials();
                Debug.Log("Status is True");
                NetworkConst.loginRes _loginRes;
                _loginRes = JsonUtility.FromJson<NetworkConst.loginRes>(data);
                CreatorData.Instance.setCreatorData(_loginRes.data);
                UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
            }
            else
            {
                message.text = loginJsonResponse["message"].ToString();
            }

        });
    }
}

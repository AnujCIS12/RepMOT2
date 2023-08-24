using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecoveryPanel : MonoBehaviour
{
    string name;
    string email;

    [SerializeField]
    InputField nameIF;

    [SerializeField]
    InputField emailIF;

    [SerializeField]
    Text message;

    public void onCancelBtnClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Login);
    }

    public void onSendButtonClick()
    {
        name = nameIF.text;
        email = emailIF.text;
        if (!NetworkingManager.Instance.IsValidEmail(email))
        {
            Debug.Log("Email is not valid");
            message.text = "Email is not valid";
            return;
        }
        //if (string.IsNullOrEmpty(name))
        //{
        //    message.text = "Name  Field is empty";
        //    Debug.Log("Name  Field is empty");
        //    return;
        //}
        //PlayerData.Instance.PlayerName = name;
        //PlayerData.Instance.PlayerGUID = System.Guid.NewGuid().ToString();
        //StartCoroutine(CallLogin("Sa","GA"));
        //UIMenu.Instance.changeMode(UIMenu.ePanel.Welcome);
        Debug.Log("Recover");
        PostRecoverData();

    }

    public void PostRecoverData()
    {
        NetworkConst.postRevover data = new NetworkConst.postRevover();
        data.name = name;
        data.email = email;

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIPostRecoverData(json, (string data) =>
        {
            Debug.Log("God Login PLayer Response");

            JsonData jsonResponse;
            jsonResponse = JsonMapper.ToObject(data);
            if ((bool)jsonResponse["status"])
            {
                Debug.Log("Status is True");
                UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Login);
            }
            else
            {
                message.text = jsonResponse["message"].ToString();
            }
        });
    }
}

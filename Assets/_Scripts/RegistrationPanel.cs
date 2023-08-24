using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationPanel : MonoBehaviour
{
    string uName;
    string email;
    string password;
    string mobile;
    string company_name;
    string city;
    string country;

    [SerializeField]
    InputField uNameIF;
    [SerializeField]
    InputField emailIF;
    [SerializeField]
    InputField passwordIF;
    [SerializeField]
    InputField mobileIF;
    [SerializeField]
    InputField cNameIF;
    [SerializeField]
    InputField cityIF;
    [SerializeField]
    InputField countryIF;


    [SerializeField]
    Text message;


    public void onLoginButtonClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Login);
    }
    public void onRegisterButtonClick()
    {
        uName = uNameIF.text;
        email = emailIF.text;
        password = passwordIF.text;
        mobile = mobileIF.text;
        company_name = cNameIF.text;
        city = cityIF.text;
        country = countryIF.text;


        if (string.IsNullOrEmpty(uName))
        {
            message.text = "Name field is  empty";
            Debug.Log("Password is null or empty");
            return;
        }
        
        if (!NetworkingManager.Instance.IsValidEmail(email))
        {
            Debug.Log("Email is not valid");
            message.text = "Email is not valid";
            return;
        }
        if (string.IsNullOrEmpty(password))
        {
            message.text = "Password  field is empty";
            Debug.Log("Password is null or empty");
            return;
        }
        Debug.Log("Register");
        PostRegisterData();

    }

    public void PostRegisterData()
    {
        string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.author data = new NetworkConst.author();
        data.email = email;
        data.name = name;
        data.password = password;
        //Int32.TryParse(mobile, out data.mobile);
        data.mobile = mobile;
        data.companyName = company_name;
        data.city = city;
        data.country = country;
        data.user_type = 3;//2 User 3 Author

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIPostRegisterData(json, (string data) =>
        {
            Debug.Log("God Register PLayer Response");
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

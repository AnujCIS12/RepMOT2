using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddLevelPopup : MonoBehaviour
{
    [SerializeField]
    InputField nameIF, commentIF, explanationIF;

    [SerializeField]
    Toggle enableToggle;

    [SerializeField]
    Text message;

    JsonData seriesJsonData;
    public static string seriesVersion;
    void Start()
    {

    }
    public static void setSeriesVersion(string data)
    {
        Debug.Log("Series Version is " + data);
        seriesVersion = data;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoneBtnClicked()
    {
        PostLevelData();
    }
    public void closePopup()
    {
        setDefaultField();
        gameObject.SetActive(false);
    }
    public void setDefaultField()
    {
        nameIF.text = "";
        commentIF.text = "";
        explanationIF.text = "";
        enableToggle.isOn = false;
    }

    public void PostLevelData()
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.level data = new NetworkConst.level();
        //data.ID = "1";
        data.name = nameIF.text;
        data.levelVersion = "1.0";
        data.seriesID = seriesVersion;
        data.threshold = "a";
        data.version = "a";
        data.creatorAccountID = CreatorData.Instance.CreatorID;
        //data.creatorAccountID = 1;
        //data.dateCreated = DateTime.Now.Date;
        //data.sinceDate = DateTime.Now.Date;
        data.comment = commentIF.text;
        data.settings = "a";
        data.explanations = explanationIF.text;


        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIAddLevelData(json, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            seriesJsonData = JsonMapper.ToObject(resData);
            if ((bool)seriesJsonData["status"])
            {
                Debug.Log("Status is True");
                LevelManager.Instance.createNewLevel(data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(seriesJsonData["message"].ToString());
                //message.text = seriesJsonData["message"].ToString();
            }

            ////PlayerData.Instance.PlayerName = name;
            ////PlayerData.Instance.PlayerGUID = playerRowKey;

        });
    }
}

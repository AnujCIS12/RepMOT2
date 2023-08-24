using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddSeriesPopup : MonoBehaviour
{
    [SerializeField]
    InputField nameIF, commentIF, explanationIF, tagIF;

    [SerializeField]
    Dropdown availModeDD;

    [SerializeField]
    Toggle enableToggle;

    [SerializeField]
    Text message;

    JsonData seriesJsonData;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoneBtnClicked()
    {
        PostSeriesData();
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
        tagIF.text = "";
        enableToggle.isOn = false;
        availModeDD.value = availModeDD.options.FindIndex(option => option.text == "Public");
    }

    public void PostSeriesData()
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.series data = new NetworkConst.series();
        //data.ID = "1";
        data.name = nameIF.text;
        data.serieVersion = "1.0";
        //data.creatorAccountID = CreatorData.Instance.CreatorID;
        data.creatorAccountID = 1;
        data.price = 0;
        data.enabled = enableToggle.isOn;
        data.availabilityMode = availModeDD.options[availModeDD.value].text;
        //data.dateCreated = DateTime.Now.Date;
        data.comment = commentIF.text;
        data.settings = "a";
        data.explanations = explanationIF.text;
        data.level = 0;
        data.category = "a";
        data.subCategory = "a";
        data.tag = tagIF.text;
        data.previewPicture = null;
        data.previewText = "a";

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIAddSeriesData(json, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            seriesJsonData = JsonMapper.ToObject(resData);
            if ((bool)seriesJsonData["status"])
            {
                Debug.Log("Status is True");
                SeriesManager.Instance.createNewSeries(data);
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

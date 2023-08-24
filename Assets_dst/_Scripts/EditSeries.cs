using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditSeries : MonoBehaviour
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
    Series series;

    public void InitializeData(Series _series)
    {
        this.series = _series;
        NetworkConst.series tmpSeries;
        tmpSeries = _series.getSeries();
        nameIF.text = tmpSeries.name;
        commentIF.text = tmpSeries.comment;
        explanationIF.text = tmpSeries.explanations;
        tagIF.text = tmpSeries.tag;
        enableToggle.isOn = tmpSeries.enabled;
        availModeDD.value = availModeDD.options.FindIndex(option => option.text == tmpSeries.availabilityMode);
        showPopup();
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
    public void showPopup()
    {
        gameObject.SetActive(true);
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
    public void updateSeries()
    {
        this.series.setSeries(data);
    }
    NetworkConst.series data;
    public void PostSeriesData()
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        data = new NetworkConst.series();
        data.id = this.series.ID;
        data.name = nameIF.text;
        data.serieVersion = "1.0";
        data.creatorAccountID = CreatorData.Instance.CreatorID;
        //data.creatorAccountID = 1;
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
        NetworkingManager.Instance.APIEditSeriesData(json, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            seriesJsonData = JsonMapper.ToObject(resData);
            if ((bool)seriesJsonData["status"])
            {
                Debug.Log("Status is True");
                //SeriesManager.Instance.createNewSeries(data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                updateSeries();
                closePopup();
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


using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddTaskPopup : MonoBehaviour
{
    [SerializeField]
    InputField nameIF, maxScorePointIF, instructionIF, threshholdIF, explainationIF;

    [SerializeField]
    Dropdown recordDD,musicDD;

    [SerializeField]
    Toggle enableToggle;

    [SerializeField]
    Text message;

    JsonData jsonData;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoneBtnClicked()
    {
        PostTaskData();
    }
    public void closePopup()
    {
        setDefaultField();
        gameObject.SetActive(false);
    }
    public void setDefaultField()
    {
        nameIF.text = "";
        maxScorePointIF.text = "";
        instructionIF.text = "";
        threshholdIF.text = "";
        explainationIF.text = "";
        enableToggle.isOn = false;
        recordDD.value = recordDD.options.FindIndex(option => option.text == "null");
        musicDD.value = musicDD.options.FindIndex(option => option.text == "null");
    }

    public void PostTaskData()
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.task data = new NetworkConst.task();
        //data.ID = "1";
        data.name = nameIF.text;
        //data.serieVersion = "1.0";
        //data.creatorAccountID = CreatorData.Instance.CreatorID;
        //data.creatorAccountID = 1;
        //data.price = 0;
        //data.enabled = enableToggle.isOn;
        //data.availabilityMode = availModeDD.options[availModeDD.value].text;
        //data.dateCreated = DateTime.Now.Date;
        //data.comment = commentIF.text;
        //data.settings = "a";
        //data.explanations = explanationIF.text;
        //data.level = 0;
        //data.category = "a";
        //data.subCategory = "a";
        //data.tag = tagIF.text;
        //data.previewPicture = null;
        //data.previewText = "a";

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIAddTaskData(json, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonData = JsonMapper.ToObject(resData);
            if ((bool)jsonData["status"])
            {
                Debug.Log("Status is True");
                TaskManager.Instance.createNewTask(data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(jsonData["message"].ToString());
                //message.text = seriesJsonData["message"].ToString();
            }

            ////PlayerData.Instance.PlayerName = name;
            ////PlayerData.Instance.PlayerGUID = playerRowKey;

        });
    }
}

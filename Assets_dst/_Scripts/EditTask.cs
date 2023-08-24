using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditTask : MonoBehaviour
{
    [SerializeField]
    InputField nameIF, commentIF, explanationIF;

    [SerializeField]
    Toggle enableToggle;

    [SerializeField]
    Text message;

    JsonData levelJsonData;
    Task task;

    public void InitializeData(Task _task)
    {
        this.task = _task;
        NetworkConst.task tmpTask;
        tmpTask = _task.getTask();
        nameIF.text = tmpTask.name;
        showPopup();
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
    public void showPopup()
    {
        gameObject.SetActive(true);
    }
    public void setDefaultField()
    {
        nameIF.text = "";
        commentIF.text = "";
        explanationIF.text = "";
        enableToggle.isOn = false;
    }
    public void updateLevel()
    {
        this.task.setTask(data);
    }
    NetworkConst.task data;
    public void PostTaskData()
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        data = new NetworkConst.task();
        data.id = this.task.ID;
        data.name = nameIF.text;
        //data.levelVersion = "1.0";
        //data.creatorAccountID = CreatorData.Instance.CreatorID;
        //data.creatorAccountID = 1;
        //data.enabled = enableToggle.isOn;
        //data.dateCreated = DateTime.Now.Date;
        //data.comment = commentIF.text;
        //data.settings = "a";
        //data.explanations = explanationIF.text;

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIEditLevelData(json, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            levelJsonData = JsonMapper.ToObject(resData);
            if ((bool)levelJsonData["status"])
            {
                Debug.Log("Status is True");
                //LevelManager.Instance.createNewLevel(data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                updateLevel();
                closePopup();
            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(levelJsonData["message"].ToString());
                //message.text = levelJsonData["message"].ToString();
            }

        });
    }
}

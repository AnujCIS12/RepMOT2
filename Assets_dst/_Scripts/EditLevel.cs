using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditLevel : MonoBehaviour
{
    [SerializeField]
    InputField nameIF, commentIF, explanationIF;

    [SerializeField]
    Toggle enableToggle;

    [SerializeField]
    Text message;

    JsonData levelJsonData;
    Level level;

    public void InitializeData(Level _level)
    {
        this.level = _level;
        NetworkConst.level tmpLevel;
        tmpLevel = _level.getLevel();
        tmpLevel = _level.getLevel();
        nameIF.text = tmpLevel.name;
        commentIF.text = tmpLevel.comment;
        explanationIF.text = tmpLevel.explanations;
        enableToggle.isOn = tmpLevel.enabled;
        showPopup();
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
        this.level.setLevel(data);
    }
    NetworkConst.level data;
    public void PostLevelData()
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        data = new NetworkConst.level();
        data.levelID = this.level.ID;
        data.name = nameIF.text;
        data.levelVersion = "1.0";
        data.creatorAccountID = CreatorData.Instance.CreatorID;
        //data.creatorAccountID = 1;
        data.enabled = enableToggle.isOn;
        //data.dateCreated = DateTime.Now.Date;
        data.comment = commentIF.text;
        data.settings = "a";
        data.explanations = explanationIF.text;

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

            ////PlayerData.Instance.PlayerName = name;
            ////PlayerData.Instance.PlayerGUID = playerRowKey;

        });
    }
}

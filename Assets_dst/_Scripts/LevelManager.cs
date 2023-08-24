using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>, IPanel
{
    [SerializeField]
    Level prefabLevel;

    [SerializeField]
    GameObject parentContent;

    [SerializeField]
    GameObject levelViewPanel;

    List<Level> levellist = new List<Level>();
    Level selectedLevel;

    NetworkConst.allLevelRes _allLevelRes;//need to change this
    public JsonData jsonResponse;

    string seriesVersion;

    public void Initialize(string data)
    {
        seriesVersion = data;
        loadAllLevel();
    }
    public void addLevelBtnClicked()
    {
        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.AddLevel);
        AddLevelPopup.setSeriesVersion(seriesVersion);
    }

    public void createNewLevel(NetworkConst.level tempLevel)
    {

        //Deck tempDeck = new Deck();
        //tempDeck.ID = deckID;
        //tempDeck.Name = name;
        //tempDeck.TeacherID = Constants.teacherID;
        //tempDeck.TeacherName = Constants.teacherName;
        //teacher.AddDeck(tempDeck);

        //UIDeck tempUIDeck;
        //tempUIDeck = Instantiate(uIDeck, parentContent.transform) as UIDeck;
        //tempUIDeck.gameObject.SetActive(true);
        //tempUIDeck.InitializeData(tempDeck);
        //AddUIDeck(tempUIDeck);


        Level tmpLevel;
        tmpLevel = Instantiate(prefabLevel, parentContent.transform) as Level;
        tmpLevel.gameObject.SetActive(true);
        tmpLevel.initializeData(tempLevel);
        addLevel(tmpLevel);
        UIPopupManager.Instance.HideSelectedPopUp();

    }
    public void addLevel(Level level)
    {
        levellist.Add(level);
    }
    public void deleteLevel(Level _level)
    {
        levellist.Remove(_level);
        Destroy(_level.gameObject);
    }

    public void hideDeleteConfirmPopup()
    {
        UIPopupManager.Instance.HideSelectedPopUp();
    }
    public void setSelectedLevel(Level _level)
    {
        this.selectedLevel = _level;
        Debug.Log("Selected Series ID is " + _level.ID);
    }
    public void calldeleteLevelAPI()
    {
        deleteLevel(selectedLevel.ID);
    }

    public void loadAllLevel()
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postLevel data = new NetworkConst.postLevel();
        data.limit = 10;
        data.page_no = 1;

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APILoadAllLevelData(json, (string data) =>
        {
            this._allLevelRes = JsonUtility.FromJson<NetworkConst.allLevelRes>(data);
            jsonResponse = JsonMapper.ToObject(data);
            Debug.Log("God Login PLayer Response" + jsonResponse["data"]);
            Debug.Log(jsonResponse);
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                Debug.Log("Name " + this._allLevelRes.data[0].name);
                foreach (NetworkConst.level tmpSeries in this._allLevelRes.data)
                {
                    createNewLevel(tmpSeries);
                }
                levelViewPanel.gameObject.SetActive(true);
            }
            else
            {
                //message.text = jsonResponse["message"].ToString();
                Debug.Log("Message is " + this._allLevelRes.message);
            }

        });
    }
    public void deleteLevel(int id)
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.level data = new NetworkConst.level();
        data.levelID = id;
        Debug.Log("ID is " + id);

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIDeleteLevelData(json, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonResponse = JsonMapper.ToObject(resData);
            if ((bool)jsonResponse["status"])
            {
                Debug.Log("Status is True");
                //SeriesManager.Instance.createNewSeries(data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                deleteLevel(selectedLevel);
                UIPopupManager.Instance.HideSelectedPopUp();

            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(jsonResponse["message"].ToString());
                //message.text = seriesJsonData["message"].ToString();
            }

        });
    }

    public void editLevel(int id)
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.level data = new NetworkConst.level();
        data.levelID = id;

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIEditLevelData(json, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            //seriesJsonData = JsonMapper.ToObject(resData);
            //if ((bool)seriesJsonData["status"])
            //{
            //    Debug.Log("Status is True");
            //    SeriesManager.Instance.createNewSeries(data);
            //    //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
            //}
            //else
            //{
            //    Debug.Log("Message ");
            //    Debug.Log(seriesJsonData["message"].ToString());
            //    //message.text = seriesJsonData["message"].ToString();
            //}

            ////PlayerData.Instance.PlayerName = name;
            ////PlayerData.Instance.PlayerGUID = playerRowKey;

        });
    }
}

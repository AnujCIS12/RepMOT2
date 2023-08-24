using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeriesManager: SingletonMonoBehaviour<SeriesManager>, IPanel
{
    [SerializeField]
    Series prefabSeries;
    [SerializeField]
    GameObject parentContent;

    [SerializeField]
    GameObject seriesViewPanel;

    List<Series> serieslist = new List<Series>();
    Series selectedSeries;

    
    public JsonData jsonResponse;

    public void Initialize(string data)
    {
        loadAllSeries();
    }
    public void addSeriesBtnClicked()
    {
        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.AddSeries);
    }

    public void createNewSeries(NetworkConst.series tempSeries)
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


        Series tmpSeries;
        tmpSeries = Instantiate(prefabSeries, parentContent.transform) as Series;
        tmpSeries.gameObject.SetActive(true);
        tmpSeries.initializeData(tempSeries);
        addSeries(tmpSeries);
        UIPopupManager.instance.HideSelectedPopUp();

    }
    public void addSeries(Series series)
    {
        serieslist.Add(series);
    }
    public void deleSeries(Series _series)
    {
        serieslist.Remove(_series);
        Destroy(_series.gameObject);
    }

    public void hideDeleteConfirmPopup()
    {
        UIPopupManager.Instance.HideSelectedPopUp();
    }
    public void setSelectedSeries(Series _series)
    {
        this.selectedSeries = _series;
        Debug.Log("Selected Series ID is " + _series.ID);
    }
    public void calldeleteSeriesAPI()
    {
        deleteSeries(selectedSeries.ID);
    }

    public void loadAllSeries()
    {
        NetworkConst.allSeriesRes _allSeriesRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postSeries data = new NetworkConst.postSeries();
        data.limit = 10;
        data.page_no = 1;

        string json = JsonUtility.ToJson(data);
        
        NetworkingManager.Instance.APILoadAllSeriesData(json, (string data) =>
        {
            _allSeriesRes = JsonUtility.FromJson<NetworkConst.allSeriesRes>(data);
            jsonResponse = JsonMapper.ToObject(data);
            Debug.Log("God Login PLayer Response" + jsonResponse["data"]);
            Debug.Log(jsonResponse);
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                Debug.Log("Name "+_allSeriesRes.data[0].name);
                foreach (NetworkConst.series tmpSeries in _allSeriesRes.data)
                {
                    createNewSeries(tmpSeries);
                }
                seriesViewPanel.gameObject.SetActive(true);
            }
            else
            {
            //message.text = jsonResponse["message"].ToString();
            Debug.Log("Message is " + _allSeriesRes.message);
            }

        });
    }
    public void deleteSeries(int id)
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.series data = new NetworkConst.series();
        data.id = id;
        Debug.Log("ID is " + id);

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIDeleteSeriesData(json, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonResponse = JsonMapper.ToObject(resData);
            if ((bool)jsonResponse["status"])
            {
                Debug.Log("Status is True");
                //SeriesManager.Instance.createNewSeries(data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                deleSeries(selectedSeries);
                UIPopupManager.instance.HideSelectedPopUp();

            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(jsonResponse["message"].ToString());
                //message.text = seriesJsonData["message"].ToString();
            }

        });
    }

    public void editSeries(int id)
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.series data = new NetworkConst.series();
        data.id = id;

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIEditSeriesData(json, (string resData) =>
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

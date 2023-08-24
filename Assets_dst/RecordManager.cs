using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : SingletonMonoBehaviour<RecordManager>
{
    [SerializeField]
    Record prefab;
    [SerializeField]
    GameObject parentContent;

    [SerializeField]
    GameObject viewPanel;

    public List<Record> objList = new List<Record>();
    Record selectedObj;


    public JsonData jsonResponse;

    public void Initialize(string data)
    {
        loadAllObj();
    }
    public void addBtnClicked()
    {
        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.AddRecord);
    }

    public void createNewObj(NetworkConst.record1 temp)
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


        Record tmpObj;
        tmpObj = Instantiate(prefab, parentContent.transform) as Record;
        tmpObj.gameObject.SetActive(true);
        tmpObj.initializeData(temp);
        addObj(tmpObj);
        UIPopupManager.Instance.HideSelectedPopUp();

    }
    public void addObj(Record obj)
    {
        objList.Add(obj);
    }
    public void deleObj(Record _obj)
    {
        //musiclist.Remove(_music);
        Destroy(_obj.gameObject);
    }
    public void deleteAllObj()
    {
        Debug.Log("Count is " + objList.Count);
        for (int i = 0; i < objList.Count; i++)
        {
            Debug.Log(i);
            deleObj(objList[i]);
        }
        objList.Clear();
        //gameObject.SetActive(false);
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
    }

    public void hideDeleteConfirmPopup()
    {
        UIPopupManager.Instance.HideSelectedPopUp();
    }
    public void setSelectedObj(Record _obj)
    {
        this.selectedObj = _obj;
        Debug.Log("Selected Record ID is " + _obj.ID);
    }
    public void callDeleteAPI(Record _obj)
    {
        setSelectedObj(_obj);
        delete(_obj.ID);
    }

    public void loadAllObj()
    {
        NetworkConst.allRecordRes _allRecordRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postRecord data = new NetworkConst.postRecord();
        data.limit = 10;
        data.page_no = 1;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAllRecordData(json, (string data) =>
        {
            _allRecordRes = JsonUtility.FromJson<NetworkConst.allRecordRes>(data);
            jsonResponse = JsonMapper.ToObject(data);
            Debug.Log("God Login PLayer Response" + jsonResponse["data"]);
            Debug.Log(jsonResponse);
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                Debug.Log("Name " + _allRecordRes.data[0].name);
                foreach (NetworkConst.record1 tmpObj in _allRecordRes.data)
                {
                    createNewObj(tmpObj);
                }
                viewPanel.gameObject.SetActive(true);
            }
            else
            {
                //message.text = jsonResponse["message"].ToString();
                Debug.Log("Message is " + _allRecordRes.message);
            }

        });
    }
    public void delete(int id)
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.record1 data = new NetworkConst.record1();
        data.id = id;
        Debug.Log("ID is " + id);

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIDeleteRecordData(json, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonResponse = JsonMapper.ToObject(resData);
            if ((bool)jsonResponse["status"])
            {
                Debug.Log("Status is True");
                objList.Remove(selectedObj);
                deleObj(selectedObj);
                UIPopupManager.Instance.HideSelectedPopUp();

            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(jsonResponse["message"].ToString());
            }

        });
    }
}

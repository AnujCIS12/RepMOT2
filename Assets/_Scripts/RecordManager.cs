using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordManager : SingletonMonoBehaviour<RecordManager>, IPanel
{
    [SerializeField]
    Record prefab;
    [SerializeField]
    GameObject parentContent;

    [SerializeField]
    GameObject viewPanel;

    public List<Record> objList = new List<Record>();
    Record selectedObj;


    public delegate void onSuccess(string Text);
    public Dictionary<string, NetworkConst.category> categoryDic = new Dictionary<string, NetworkConst.category>();
    public Dictionary<string, NetworkConst.subcategory> subcategoryDic = new Dictionary<string, NetworkConst.subcategory>();
    public Dictionary<string, NetworkConst.groupAccess> groupDic = new Dictionary<string, NetworkConst.groupAccess>();
    public Dictionary<int, NetworkConst.groupAccess> groupDic1 = new Dictionary<int, NetworkConst.groupAccess>();

    public JsonData jsonResponse;

    public void Initialize(string data, string namel, string authtasknamel)
    {
        loadAllObj();
    }
    public void addBtnClicked()
    {
        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.AddRecord);
    }

    public void createNewObj(NetworkConst.record1 temp)
    {
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
    public void BackToMainMenu()
    {
        deleteAllObj();
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
        data.recordID = id;
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

    public int getCategoryIDByName(string name)
    {
        Debug.Log(name);
        NetworkConst.category entry;
        if (string.IsNullOrEmpty(name)) return -1;
        Debug.Log("Before Try to get category " + name);
        if (categoryDic.TryGetValue(name, out entry))
        {
            Debug.Log("Entry" + entry);
            return entry.id;
        }
        else return -1;
    }
    public void loadAllCategory(onSuccess callback)
    {
        NetworkConst.allCategoryRes _allCategoryRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postCategory data = new NetworkConst.postCategory();
        data.moduleID = 3;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAllCategoryData(json, (string data) =>
        {
            _allCategoryRes = JsonUtility.FromJson<NetworkConst.allCategoryRes>(data);
            jsonResponse = JsonMapper.ToObject(data);
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                Debug.Log("---------Data -------------" + _allCategoryRes);
                categoryDic.Clear();
                foreach (NetworkConst.category tmpCategory in _allCategoryRes.data)
                {
                    Debug.Log(tmpCategory.name + "..................Name adding");
                    if (!categoryDic.ContainsKey(tmpCategory.name))
                        categoryDic.Add(tmpCategory.name, tmpCategory);
                }
                callback("Success");
            }
            else
            {
                //message.text = jsonResponse["message"].ToString();
                Debug.Log("Message is " + _allCategoryRes.message);
            }

        });
    }
    public void loadAllSubCategory(string categoryName, onSuccess callback)
    {
        NetworkConst.allSubCategoryRes _allSubCategoryRes;
        NetworkConst.postSubCategory data = new NetworkConst.postSubCategory();
        int _Cid = getCategoryIDByName(categoryName);
        if (_Cid < 0)
        {
            Debug.Log("Error" + _Cid);
            callback("Error");
            return;
        }
        data.categoryID = _Cid;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAllSubCategoryData(json, (string data) =>
        {
            _allSubCategoryRes = JsonUtility.FromJson<NetworkConst.allSubCategoryRes>(data);
            jsonResponse = JsonMapper.ToObject(data);
            subcategoryDic.Clear();
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                Debug.Log("---------data -------------" + _allSubCategoryRes);
                foreach (NetworkConst.subcategory tmpSubCategory in _allSubCategoryRes.data)
                {
                    if (!subcategoryDic.ContainsKey(tmpSubCategory.name))
                        subcategoryDic.Add(tmpSubCategory.name, tmpSubCategory);
                }
                callback("Success");
            }
            else
            {
                //message.text = jsonResponse["message"].ToString();
                Debug.Log("Message is " + _allSubCategoryRes.message);
                callback("Error");
            }

        }, (string data) => { callback("Error"); });
    }
}

using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class LevelManager : SingletonMonoBehaviour<LevelManager>, IPanel
{
    public static LevelManager Instance;
    [SerializeField]
    Level prefabLevel;

    [SerializeField]
    GameObject parentContent;
    [SerializeField]
    GameObject levelViewPanel;
    [SerializeField]
    TextMeshProUGUI levelTextl;
    private bool ascending_Name, ascending_Comment;
    List<Level> levellist = new List<Level>();
    Level selectedLevel;
    internal string autherpassname;
    public delegate void onSuccess(string Text);
    public GameObject levelManagerP;
    NetworkConst.allLevelRes _allLevelRes;//need to change this
    public JsonData jsonResponse;
    public GameObject SeriesPanel;
    internal string seriesVersion;

    public Dictionary<string, NetworkConst.category> categoryDic = new Dictionary<string, NetworkConst.category>();
    public Dictionary<string, NetworkConst.subcategory> subcategoryDic = new Dictionary<string, NetworkConst.subcategory>();
    private void Awake()
    {
        if (LevelManager.Instance==null)
        {
            LevelManager.Instance = this;
        }
        ascending_Name = ascending_Comment=false;
    }

    private void OnDisable()
    {
        RemoveChild();
        levellist.Clear();
    }

    public void OnClickHomeBtn()
    {
        if (SeriesPanel.activeInHierarchy)
        {
            SeriesPanel.SetActive(false);
        }
    }
    public void onClickLevelsbtn()
    {
        RemoveChild();
        if (levellist.Count > 0)
        {
            levellist.Clear();
        }
        levelManagerP.SetActive(false);
    }
    public void RemoveChild()
    {
        int childCount = parentContent.transform.childCount;
        for (int x = 0; x < childCount; x++)
        {
            Transform child = parentContent.transform.GetChild(x);
            if (child.gameObject.activeInHierarchy)
            {
                if (child.gameObject.tag == "level")
                {
                    Destroy(child.gameObject);
                }
            }

        }

    }
    public void SortSeriesbyComment()
    {
        RemoveChild();

        List<NetworkConst.level> temp_Level = new List<NetworkConst.level>();
        foreach (var x in levellist)
        {
            temp_Level.Add(x.getLevel());
        }

        if (ascending_Comment == false)
        {
            var Convert_List = from a in temp_Level orderby a.comment select a;
            ascending_Comment = true;
            foreach (var x in Convert_List)
            {
                Level tmpLevel;
                tmpLevel = Instantiate(prefabLevel, parentContent.transform);
                tmpLevel.gameObject.SetActive(true);
                tmpLevel.initializeData(x);
            }
            temp_Level.Clear();
        }
        else if (ascending_Comment == true)
        {
            var Convert_List = temp_Level.OrderByDescending(a => a.comment);
            ascending_Comment = false;
            foreach (var x in Convert_List)
            {
                Level tmpLevel;
                tmpLevel = Instantiate(prefabLevel, parentContent.transform);
                tmpLevel.gameObject.SetActive(true);
                tmpLevel.initializeData(x);
            }
            temp_Level.Clear();
        }


    }
    public void SortSeriesbyname()
    {
        RemoveChild();

        List<NetworkConst.level> temp_Level = new List<NetworkConst.level>();
        foreach (var x in levellist)
        {
            temp_Level.Add(x.getLevel());
        }

        if (ascending_Name == false)
        {
            var Convert_List = from a in temp_Level orderby a.name select a;
            ascending_Name = true;
            foreach (var x in Convert_List)
            {
               Level tmpLevel;
                tmpLevel = Instantiate(prefabLevel, parentContent.transform);
                tmpLevel.gameObject.SetActive(true);
                tmpLevel.initializeData(x);
            }
            temp_Level.Clear();
        }
        else if (ascending_Name == true)
        {
            var Convert_List = temp_Level.OrderByDescending(a => a.name);
            ascending_Name = false;
            foreach (var x in Convert_List)
            {
                Level tmpLevel;
                tmpLevel = Instantiate(prefabLevel, parentContent.transform);
                tmpLevel.gameObject.SetActive(true);
                tmpLevel.initializeData(x);
            }
            temp_Level.Clear();
        }


    }
    public void Initialize(string data,string namel="", string authtasknamel="")
    {
        if (levellist.Count > 0) deleteAllLevel();
        Debug.Log("<color=Red>Level Data "+data+"</color>");
        seriesVersion = data;
        loadAllLevel();
        autherpassname = namel;
        levelTextl.text = namel + " -> Levels";
    }
    public void addLevelBtnClicked()
    {
        
        AddLevelPopup.setSeriesVersion(seriesVersion);
        loadAllCategory((string message) => {
            UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.AddLevel);
        });
    }
    public void seriesBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.SeriesView);
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

    public void deleLevel(Level _level)
    {
        Destroy(_level.gameObject);
    }
    public void deleteAllLevel()
    {
        Debug.Log("Count is " + levellist.Count);
        for (int i = 0; i < levellist.Count; i++)
        {
            Debug.Log(i);
            deleLevel(levellist[i]);
        }
        levellist.Clear();
        //gameObject.SetActive(false);
        //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
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
    public void calldeleteLevelAPI(Level _level)
    {
        setSelectedLevel(_level);
        deleteLevel(_level.ID);
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

    public void loadAllLevel()
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postLevel data = new NetworkConst.postLevel();
        data.limit = 10;
        data.page_no = 1;
        data.seriesID = seriesVersion;

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APILoadAllLevelData(json, (string data) =>
        {
            this._allLevelRes = JsonUtility.FromJson<NetworkConst.allLevelRes>(data);
            Debug.Log("<color=Red> Inside API level data "+data+"</color>");
            jsonResponse = JsonMapper.ToObject(data);
            Debug.Log("God Login PLayer Response" + jsonResponse["data"]);
            Debug.Log(jsonResponse);
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                //Debug.Log("Name " + this._allLevelRes.data[0].name);
                foreach (NetworkConst.level tmpLevel in this._allLevelRes.data)
                {
                    createNewLevel(tmpLevel);
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
                levellist.Remove(selectedLevel);
                deleLevel(selectedLevel);
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

    public void loadAllCategory(onSuccess callback)
    {
        NetworkConst.allCategoryRes _allCategoryRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postCategory data = new NetworkConst.postCategory();
        data.moduleID = 7;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAllCategoryData(json, (string data) =>
        {
            _allCategoryRes = JsonUtility.FromJson<NetworkConst.allCategoryRes>(data);
            JsonData jsonResponse;
            jsonResponse = JsonMapper.ToObject(data);
            categoryDic.Clear();
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                Debug.Log("---------Data -------------" + _allCategoryRes);
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
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postSubCategory data = new NetworkConst.postSubCategory();
        int _Cid = getCategoryIDByName(categoryName);
        if (_Cid < 0)
        {
            callback("Error");
            return;
        }
        Debug.Log("Category Name " + categoryName + " Category ID " + _Cid);
        data.categoryID = _Cid;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAllSubCategoryData(json, (string data) =>
        {
            subcategoryDic.Clear();
            _allSubCategoryRes = JsonUtility.FromJson<NetworkConst.allSubCategoryRes>(data);
            Debug.Log("---------data -------------" + _allSubCategoryRes);
            foreach (NetworkConst.subcategory tmpSubCategory in _allSubCategoryRes.data)
            {
                if (!subcategoryDic.ContainsKey(tmpSubCategory.name))
                    subcategoryDic.Add(tmpSubCategory.name, tmpSubCategory);
            }
            callback("Success");

        }, (string data) => { callback("Error"); });
    }
}

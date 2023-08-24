using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : SingletonMonoBehaviour<MusicManager>, IPanel
{
    [SerializeField]
    Music prefabMusic;
    [SerializeField]
    GameObject parentContent;
    public GameObject errorGO;
    public GameObject downloadErrorMessage;
    [SerializeField]
    GameObject viewPanel;
    internal int currentMusicId;
    [SerializeField] GameObject addMusicBtn;
    public AudioSource audioSource;

    public delegate void onSuccess(string Text);
    public Dictionary<string, NetworkConst.category> categoryDic = new Dictionary<string, NetworkConst.category>();
    public Dictionary<string, NetworkConst.subcategory> subcategoryDic = new Dictionary<string, NetworkConst.subcategory>();
    public Dictionary<string, NetworkConst.groupAccess> groupDic = new Dictionary<string, NetworkConst.groupAccess>();
    public Dictionary<int, NetworkConst.groupAccess> groupDic1 = new Dictionary<int, NetworkConst.groupAccess>();

    public List<Music> musiclist = new List<Music>();
    Music selectedMusic;


    public JsonData jsonResponse;

    public void Initialize(string data,string namel, string authtasknamel)
    {
        loadAllMusic();
        setUpForRole();
    }
    
    public void setUpForRole()
    {

        bool canCreate = PermissionManager.instance.GetPermissionForRole1("Music", "create");
        //Debug.LogError(canCreate);
        addMusicBtn.SetActive(canCreate);
    }
    public void addMusicBtnClicked()
    {
        //UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.AddMusic);
        loadAllCategory((string message)=> {
            UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.AddMusic);
        });
    }

    public void createNewMusic(NetworkConst.music tempMusic)
    {

        Music tmpMusic;
        tmpMusic = Instantiate(prefabMusic, parentContent.transform) as Music;
        tmpMusic.gameObject.SetActive(true);
        tmpMusic.initializeData(tempMusic);
        addMusic(tmpMusic);
        UIPopupManager.Instance.HideSelectedPopUp();

    }
    public void addMusic(Music music)
    {
        musiclist.Add(music);
    }
    public void deleMusic(Music _music)
    {
        //musiclist.Remove(_music);
        Destroy(_music.gameObject);
    }
    public void deleteAllMusic()
    {
        Debug.Log("Count is " + musiclist.Count);
        for (int i = 0; i < musiclist.Count; i++)
        {
            Debug.Log(i);
            deleMusic(musiclist[i]);
        }
        musiclist.Clear();
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
        //gameObject.SetActive(false);
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
    }
    public void BackToMainMenu()
    {
        deleteAllMusic();
    }

    public void hideDeleteConfirmPopup()
    {
        UIPopupManager.Instance.HideSelectedPopUp();
    }
    public void setSelectedMusic(Music _music)
    {
        this.selectedMusic = _music;
        Debug.Log("Selected Music ID is " + _music.ID);
    }
    public void calldeleteMusicAPI(Music _music)
    {
        setSelectedMusic(_music);
        deleteMusic(_music.ID);
    }
    public int getCategoryIDByName(string name)
    {
        //Debug.Log(name);
        //if (string.IsNullOrEmpty(name)) return -1; 
        //return categoryDic[name].id;

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
        data.moduleID = 4;

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
                        categoryDic.Add(tmpCategory.name,tmpCategory);
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
    public void loadAllSubCategory(string categoryName,onSuccess callback)
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
        data.categoryID = _Cid;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAllSubCategoryData(json, (string data) =>
        {
            _allSubCategoryRes = JsonUtility.FromJson<NetworkConst.allSubCategoryRes>(data);
            jsonResponse = JsonMapper.ToObject(data);
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                Debug.Log("---------data -------------" + _allSubCategoryRes);
                subcategoryDic.Clear();
                foreach (NetworkConst.subcategory tmpSubCategory in _allSubCategoryRes.data)
                {
                    if (!subcategoryDic.ContainsKey(tmpSubCategory.name))
                        subcategoryDic.Add(tmpSubCategory.name,tmpSubCategory);
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

    public void loadAllMusic()
    {
        NetworkConst.allMusicRes _allMusicRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postMusic data = new NetworkConst.postMusic();
        data.limit = 10;
        data.page_no = 1;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAllMusicData(json, (string data) =>
        {
            _allMusicRes = JsonUtility.FromJson<NetworkConst.allMusicRes>(data);
            jsonResponse = JsonMapper.ToObject(data);
            Debug.Log("God Login PLayer Response" + jsonResponse["data"]);
            Debug.Log(jsonResponse);
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                Debug.Log("---------Music Path -------------" + _allMusicRes.base_url+" Count "+ _allMusicRes.data.Length);
                foreach (NetworkConst.music tmpMusic in _allMusicRes.data)
                {
                    createNewMusic(tmpMusic);
                }
                viewPanel.gameObject.SetActive(true);
            }
            else
            {
                //message.text = jsonResponse["message"].ToString();
                Debug.Log("Message is " + _allMusicRes.message);
            }

        });
    }
    public void deleteMusic(int id)
    {
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.music data = new NetworkConst.music();
        data.musicID = id;
        Debug.Log("ID is " + id);

        string json = JsonUtility.ToJson(data);
        NetworkingManager.Instance.APIDeleteMusicData(json, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonResponse = JsonMapper.ToObject(resData);
            if ((bool)jsonResponse["status"])
            {
                Debug.Log("Status is True");
                musiclist.Remove(selectedMusic);
                deleMusic(selectedMusic);
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

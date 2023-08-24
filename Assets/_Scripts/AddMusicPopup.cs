using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AddMusicPopup : MonoBehaviour
{
    [SerializeField]
    InputField titleNameIF, orquestraIF, singerIF, composerIF, lyricsFromIF, yearPublishedIF, commentIF, accessIF, tagIF;

    [SerializeField]
    Toggle enableToggle,licenceFreeToggle;

    [SerializeField]
    MultipleSelectionDropDown groupSelectionDD;

    [SerializeField] Button selectMusicFileBtn;

    [SerializeField]
    Dropdown categoryDD, subCategoryDD, availModeDD;

    [SerializeField]
    Text message;

    [SerializeField]
    Text musicNameText;

    JsonData jsonData;
    public static string musicVersion;
    public GameObject SoundActions,enableT,licenceT;
    public string musicPath;
    private int availType;
    public AudioSource audioSource;
  //  private Vector3 EndenableT,Endli
    void Start()
    {
        Debug.LogError("Enable Local "+enableT.transform.localPosition +" Enable Posiion "+enableT.transform.position);
        Debug.LogError("licence Local " + licenceT.transform.localPosition + " licence Posiion " + licenceT.transform.position);
        Debug.LogError("SoundActions Local " + SoundActions.transform.localPosition + " SoundActions Posiion " + SoundActions.transform.position);
        availModeDD.onValueChanged.AddListener(delegate {
            selectAvailOptionsDDValueChanged(availModeDD);
            //Code to handle logic of manage Avail DD Changes
        });
        categoryDD.onValueChanged.AddListener(delegate {
            //DropdownValueChanged(m_Dropdown);
            loadSubCategory();
        });
    }
    public static void setMusicVersion(string data)
    {
        Debug.Log("Music Version is " + data);
        musicVersion = data;
    }
    private void selectAvailOptionsDDValueChanged(Dropdown _dd)
    {
        availType = _dd.value + 1;
        Debug.Log("Value Changed " + _dd.value);
        switch (availType)
        {
            case 1:
                setForPublicAvail();
                break;
            case 2:
                setForCloseGroupAvail();
                break;
            case 3:
                setForKeyAccessAvail();
                break;
            case 4:
                setForInAppPurchaseAvail();
                break;
            case 5:
                setForPrivateAvail();
                break;
        }
    }
    void setForPublicAvail()
    {
        
        accessIF.gameObject.SetActive(false);
        groupSelectionDD.gameObject.SetActive(false);
    }
    void setForCloseGroupAvail()
    {
        groupSelectionDD.gameObject.SetActive(true);
        
        accessIF.gameObject.SetActive(false);
        loadAllGroup();
    }
    void setForKeyAccessAvail()
    {
        groupSelectionDD.gameObject.SetActive(false);
        
        accessIF.gameObject.SetActive(true);
    }
    void setForInAppPurchaseAvail()
    {
        //priceIF.gameObject.SetActive(true);
        if (!accessIF.gameObject.activeSelf) accessIF.gameObject.SetActive(true);
        groupSelectionDD.gameObject.SetActive(false);
    }
    void setForPrivateAvail()
    {
        //priceIF.gameObject.SetActive(false);
        accessIF.gameObject.SetActive(false);
        groupSelectionDD.gameObject.SetActive(false);
    }
    public void loadSubCategory()
    {
        MusicManager.Instance. loadAllSubCategory(categoryDD.options[categoryDD.value].text, (string status) => {
            if (status == "Error") return;
            updateSubCategoryDD();
        });
    }
    private void OnEnable()
    {
        updateCategoryDD();
        setUpForRole();
    }
    public void EnableDataForRole()
    {
        titleNameIF.interactable = false;
        orquestraIF.interactable = false;
        singerIF.interactable = false;
        composerIF.interactable = false;
        lyricsFromIF.interactable = false;
        yearPublishedIF.interactable = false;
        commentIF.interactable = false;
        accessIF.interactable = false;
        tagIF.interactable = false;
    }
    public void updateCategoryDD()
    {
        updateGroupAccessDD();
        categoryDD.ClearOptions();
        List<string> category = new List<string>();
        foreach(KeyValuePair<string, NetworkConst.category> _category in MusicManager.Instance.categoryDic)
        {
            category.Add(_category.Key);
            Debug.Log(_category.Key);
        }
        categoryDD.AddOptions(category);
        loadSubCategory();
    }
    public void updateSubCategoryDD()
    {
        subCategoryDD.ClearOptions();
        List<string> subCategory = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.subcategory> _subcategory in MusicManager.Instance.subcategoryDic)
        {
            subCategory.Add(_subcategory.Value.name);
        }
        subCategoryDD.AddOptions(subCategory);
    }
    public void updateGroupAccessDD()
    {
        availModeDD.ClearOptions();
        List<string> groupAccess = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.groupAccess> _groupAccess in GameManager.Instance.groupAccessDic)
        {
            groupAccess.Add(_groupAccess.Value.name);
        }
        availModeDD.AddOptions(groupAccess);
    }
    public void updateGroupDD()
    {
        //updateavailModeDD();
        groupSelectionDD.ClearOptions();
        List<string> groups = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.groupAccess> _group in MusicManager.Instance.groupDic)
        {
            groups.Add(_group.Key);
            Debug.Log(_group.Key);
        }
        groupSelectionDD.AddOptions(groups);
        //loadSubCategory();
    }
    //public string getIDFromList()
    //{
    //    StringBuilder s = new StringBuilder();
    //    foreach (string name in groupSelectionDD.selectedItemList)
    //    {

    //        Debug.Log(name);
    //        s.Append(MusicManager.Instance.groupDic[name].id.ToString() + ",");
    //    }
    //    Debug.Log(s);
    //    return s.ToString();
    //}

    public string getIDFromList()
    {
        Debug.Log("Selected Item List");
        StringBuilder s = new StringBuilder();
        foreach (string name in groupSelectionDD.selectedItemList)
        {
            Debug.Log(name);
            if (!string.IsNullOrEmpty(name)) s.Append(MusicManager.Instance.groupDic[name].id.ToString() + ",");
        }
        Debug.Log(s);
        string selectedGroupIds = s.ToString();
        if (s.Length > 1) selectedGroupIds = selectedGroupIds.Remove(selectedGroupIds.Length - 1, 1);
        Debug.Log(selectedGroupIds);
        return selectedGroupIds;
    }
    public void selectMusicBtnClicked()
    {
        FileBrowserEg1.Instance.GetMusicPath(onFileSelected);
    }
    public void onFileSelected(string[] paths)
    {
        Debug.Log("Paths is "+paths[0]);
        musicPath = paths[0];
        musicNameText.text = FileBrowserEg1.Instance.getName(musicPath);
        SoundActions.SetActive(true);
        byte[] audiobyte = System.IO.File.ReadAllBytes(musicPath);
    }
    public void OnClicMusicPlayBtn()
    {
        if(!audioSource.isPlaying)
        {
            StartCoroutine(LoadMusicCoroutine(musicNameText.text));
        }
    }
    public void OnClickMusicPauseBtn()
    {
        if(audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.Play();
        }
    }
    public void OnClickRewindBtn()
    {
        if(audioSource.clip!=null)
        {
            audioSource.Stop();
            audioSource.Play();
        }
        
    }
    private IEnumerator LoadMusicCoroutine(string filePath)
    {
        using (var audioLoader = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.UNKNOWN))
        {
            Debug.Log(filePath);
            yield return audioLoader.SendWebRequest();

            if (audioLoader.result == UnityWebRequest.Result.Success)
            {
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(audioLoader);

                if (audioClip != null)
                {
                    audioSource.clip = audioClip;
                    audioSource.Play();

                }
                else
                {
                    Debug.LogError("Failed to load audio clip.");
                }
            }
            else
            {
                Debug.LogError("Failed to load music: " + audioLoader.error);
            }
        }
    }

    public void DoneBtnClicked()
    {
        PostMusicData();
        //StartCoroutine(UploadDataOnServer());
        //StartCoroutine(Upload1());
    }
    public void closePopup()
    {
        setDefaultField();
        gameObject.SetActive(false);
        SoundActions.SetActive(false);
    }
    public void setDefaultField()
    {
        titleNameIF.text = "";
        orquestraIF.text = "";
        singerIF.text = "";
        composerIF.text = "";
        lyricsFromIF.text = "";
        yearPublishedIF.text = "";
        commentIF.text = "";
        accessIF.text = "";
        tagIF.text = "";
        //categoryDD.value = categoryDD.options.FindIndex(option => option.text == "Category");
        //subCategoryDD.value = subCategoryDD.options.FindIndex(option => option.text == "Sub Category");
        availModeDD.value = availModeDD.options.FindIndex(option => option.text == "Group Access");
        enableToggle.isOn = false;
        licenceFreeToggle.isOn = false;
    }

    public void setUpForRole()
    {


        //nameIF.interactable = false;
        //commentIF.interactable = false;
        //explanationIF.interactable = false;
        //tagIF.interactable = false;
        //dateCreateIF.interactable = false;
        //serieVersionIF.interactable = false;
        //settingsIF.interactable = false;
        //priceIF.interactable = false;
        //levelIF.interactable = false;
        //previewTextIF.interactable = false;

        bool canAddFiles = PermissionManager.Instance.GetPermissionForRole1("Music", "add files");
        selectMusicFileBtn.interactable = canAddFiles;
        //updateBtn.interactable = false;
        //viewLevelBtn.interactable = false;

        bool canEnable = PermissionManager.Instance.GetPermissionForRole1("Music", "enable");
        enableToggle.interactable = canEnable;

        //categoryDD.interactable = false;
        //subCategoryDD.interactable = false;
        bool canMakeAvail = PermissionManager.Instance.GetPermissionForRole1("Music", "availabilityMode");
        availModeDD.interactable = canMakeAvail;
    }
    public void loadAllGroup()
    {
        NetworkConst.allGroupAccessRes _allGroupRes;

        WWWForm wwwform = new WWWForm();
        wwwform.AddField("limit", 100);
        wwwform.AddField("page_no", 1);


        NetworkingManager.Instance.APILoadAllGroupListData(wwwform, (string data) =>
        {
            _allGroupRes = JsonUtility.FromJson<NetworkConst.allGroupAccessRes>(data);
            JsonData jsonResponse;
            jsonResponse = JsonMapper.ToObject(data);
            Debug.Log("God Login PLayer Response" + jsonResponse["data"]);
            Debug.Log(jsonResponse);
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                //Debug.Log("Name " + _allGroupRes.data[0].name);
                MusicManager.Instance.groupDic.Clear();
                foreach (NetworkConst.groupAccess tmpGroup in _allGroupRes.data)
                {
                    //createNewSeries(tmpSeries);
                    Debug.Log("Group Name " + tmpGroup.name);
                    if (!MusicManager.Instance.groupDic.ContainsKey(tmpGroup.name))
                        MusicManager.Instance.groupDic.Add(tmpGroup.name, tmpGroup);
                }
                updateGroupDD();
            }
            else
            {
                //message.text = jsonResponse["message"].ToString();
                Debug.Log("Message is " + _allGroupRes.message);
            }

        });
    }
    void PostMusicData()
    {

        NetworkConst.music data = new NetworkConst.music();
        //data.ID = "1";
        data.titleName = titleNameIF.text;
        data.orquestra = orquestraIF.text;
        data.lyricsFrom = lyricsFromIF.text;
        data.singer = singerIF.text;
        data.composer = composerIF.text;
        data.yearPublished = yearPublishedIF.text;
        data.licenceFree = false;
        data.comment = commentIF.text;
        data.tag = tagIF.text;
        data.category_music = categoryDD.options[categoryDD.value].text;
        data.subcategory_music = subCategoryDD.options[subCategoryDD.value].text;
        data.groupAccess = availModeDD.options[availModeDD.value].text;
        data.enabled = enableToggle.isOn;
        data.licenceFree = licenceFreeToggle.isOn;
        data.musicFileData = System.IO.File.ReadAllBytes(musicPath);
        data.musicFile = musicNameText.text;

        string json = JsonUtility.ToJson(data);

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("titleName", "nothing"));

        WWWForm wwwform = new WWWForm();        
        wwwform.AddField("titleName", titleNameIF.text);
        wwwform.AddField("orquestra", orquestraIF.text);
        wwwform.AddField("lyricsFrom", lyricsFromIF.text);
        wwwform.AddField("singer", singerIF.text);
        wwwform.AddField("composer", composerIF.text);
        wwwform.AddField("yearPublished", yearPublishedIF.text);
        wwwform.AddField("licenceFree", licenceFreeToggle.isOn.ToString());
        wwwform.AddField("comment", commentIF.text);
        wwwform.AddField("tag", tagIF.text);
        wwwform.AddField("category_music", categoryDD.options[categoryDD.value].text);
        wwwform.AddField("subcategory_music", subCategoryDD.options[subCategoryDD.value].text);
        wwwform.AddField("availabilityMode", availType);
        if (availType == 2) wwwform.AddField("group_id", getIDFromList());
        if (availType == 3) wwwform.AddField("accessKey", accessIF.text);
        if (availType == 4) wwwform.AddField("accessKey", accessIF.text);
        wwwform.AddField("public", "1");
        wwwform.AddField("enabled", enableToggle.isOn.ToString());
        wwwform.AddBinaryData("musicFileData", File.ReadAllBytes(musicPath), musicNameText.text, "audio/wav");


        NetworkingManager.Instance.APIAddMusicData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonData = JsonMapper.ToObject(resData);
            if ((bool)jsonData["status"])
            {
                Debug.Log("Status is True");
                NetworkConst.newMusicRes _newMusicRes;
                _newMusicRes = JsonUtility.FromJson<NetworkConst.newMusicRes>(resData);
                MusicManager.Instance.createNewMusic(_newMusicRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(jsonData["message"].ToString());
                //message.text = seriesJsonData["message"].ToString();
            }

        });
    }

    }

using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EditMusic : MonoBehaviour
{
    [SerializeField]
    InputField titleNameIF, orquestraIF, singerIF, composerIF, lyricsFromIF, yearPublishedIF, commentIF, accessIF, tagIF;

    [SerializeField]
    Toggle enableToggle, licenceFreeToggle;

    [SerializeField]
    Dropdown categoryDD, subCategoryDD, availModeDD;

    [SerializeField]
    MultipleSelectionDropDown groupSelectionDD;

    [SerializeField]
    Button selectMusicBtn, updateBtn;

    [SerializeField]
    Text message;

    [SerializeField]
    Text musicNameText;

    JsonData jsonData;
    Music music;
    NetworkConst.music data;

    public string musicPath;
    private int availType;

    private void Awake()
    {
        availModeDD.onValueChanged.AddListener(delegate
        {
            selectAvailOptionsDDValueChanged(availModeDD);
            //Code to handle logic of manage Avail DD Changes
        });
        
    }
    public void Start()
    {
        categoryDD.onValueChanged.AddListener(delegate
        {
            //DropdownValueChanged(m_Dropdown);
            loadSubCategory();
        });

    }

    private void OnEnable()
    {
        //updateCategoryDD();
    }
    public void loadSubCategory()
    {
        //MusicManager.Instance.loadAllSubCategory(this.music.getMusic().category_music, (string status) => {
        //    if (status == "Error") return;
        //    updateSubCategoryDD();
        //});

        subCategoryDD.ClearOptions();
        MusicManager.Instance.loadAllSubCategory(categoryDD.options[categoryDD.value].text, (string status) => {
            if (status == "Error")
            {
                Debug.LogError("NOt find subcategory");
                return;
            }
            updateSubCategoryDD();
        });

        //string selectedCategory = this.music.getMusic().category_music;
        //if (string.IsNullOrEmpty(selectedCategory) || MusicManager.Instance.getCategoryIDByName(selectedCategory) < 0) selectedCategory = categoryDD.options[categoryDD.value].text;
        //MusicManager.Instance.loadAllSubCategory(selectedCategory, (string status) => {
        //    if (status == "Error") return;
        //    updateSubCategoryDD();
        //});
    }

    public void updateCategoryDD()
    {
        //updateavailModeDD();
        categoryDD.ClearOptions();
        List<string> category = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.category> _category in MusicManager.Instance.categoryDic)
        {
            category.Add(_category.Key);
            Debug.Log(_category.Key);
        }
        categoryDD.AddOptions(category);
        categoryDD.value = categoryDD.options.FindIndex(option => option.text == this.music.getMusic().category_music);
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
        subCategoryDD.value = subCategoryDD.options.FindIndex(option => option.text == this.music.getMusic().subcategory_music);
    }
    public void updateavailModeDD()
    {
        availModeDD.ClearOptions();
        List<string> groupAccess = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.groupAccess> _groupAccess in GameManager.Instance.groupAccessDic)
        {
            groupAccess.Add(_groupAccess.Value.name);
        }
        availModeDD.AddOptions(groupAccess);
    }

    public void InitializeData(Music _music)
    {
        this.music = _music;
        NetworkConst.music tmpMusic;
        tmpMusic = _music.getMusic();

        titleNameIF.text = tmpMusic.titleName;
        musicNameText.text = tmpMusic.musicFile;
        orquestraIF.text = tmpMusic.orquestra;
        singerIF.text = tmpMusic.singer;
        composerIF.text = tmpMusic.composer;
        lyricsFromIF.text = tmpMusic.lyricsFrom;
        yearPublishedIF.text = tmpMusic.yearPublished;
        Debug.Log("_________________Year Published____________________" + tmpMusic.yearPublished);
        commentIF.text = tmpMusic.comment;
        if(!string.IsNullOrEmpty(tmpMusic.keyAccess)) accessIF.text = tmpMusic.keyAccess;
        tagIF.text = tmpMusic.tag;
        //categoryDD.value = categoryDD.options.FindIndex(option => option.text == tmpMusic.category);
        //subCategoryDD.value = subCategoryDD.options.FindIndex(option => option.text == tmpMusic.subcategory);
        //availModeDD.value = availModeDD.options.FindIndex(option => option.text == tmpMusic.groupAccess);
        enableToggle.isOn = tmpMusic.enabled;
        licenceFreeToggle.isOn = tmpMusic.licenceFree;
        int availMode;
        int.TryParse(tmpMusic.availabilityMode, out availMode);
        showPopup();
        //loadSubCategory();
        updateCategoryDD();
        Debug.LogError(availMode);
        if(availMode<1 || availMode > availModeDD.options.Count)
        {
            availModeDD.value = 0;
        }
        else
        {
            availModeDD.value = availMode - 1;
        }
        setUpForRole();


    }
    public void setData(Music _music)
    {
        NetworkConst.music tmpMusic;
        tmpMusic = _music.getMusic();

        titleNameIF.text = tmpMusic.titleName;
        musicNameText.text = tmpMusic.musicFile.ToString();
        orquestraIF.text = tmpMusic.orquestra;
        singerIF.text = tmpMusic.singer;
        composerIF.text = tmpMusic.composer;
        lyricsFromIF.text = tmpMusic.lyricsFrom;
        yearPublishedIF.text = tmpMusic.yearPublished;
        Debug.Log("_________________Year Published____________________" + tmpMusic.yearPublished);
        commentIF.text = tmpMusic.comment;
        accessIF.text = tmpMusic.keyAccess;
        tagIF.text = tmpMusic.tag;
        categoryDD.value = categoryDD.options.FindIndex(option => option.text == tmpMusic.category_music);
        subCategoryDD.value = subCategoryDD.options.FindIndex(option => option.text == tmpMusic.subcategory_music);
        int availMode;
        int.TryParse(tmpMusic.availabilityMode, out availMode);
        availModeDD.value = availMode-1;
        enableToggle.isOn = tmpMusic.enabled;
        licenceFreeToggle.isOn = tmpMusic.licenceFree;

        showPopup();
    }

    public void selectMusicBtnClicked()
    {
        FileBrowserEg1.Instance.GetMusicPath(onFileSelected);
    }

    public void onFileSelected(string[] paths)
    {
        Debug.Log("Paths is " + paths[0]);
        musicPath = paths[0];
        musicNameText.text = FileBrowserEg1.Instance.getName(musicPath);
        byte[] audiobyte = System.IO.File.ReadAllBytes(musicPath);
    }

    public void DoneBtnClicked()
    {
        PostMusicData();
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
        availModeDD.value = 0;
        enableToggle.isOn = false;
        licenceFreeToggle.isOn = false;
    }

    public void setUpForRole()
    {

        bool canEdit = PermissionManager.Instance.GetPermissionForRole1("Music", "edit");
        //Debug.LogError("Can Edit " + canEdit);

        if (!canEdit)
        {
            titleNameIF.interactable = false;
            commentIF.interactable = false;
            orquestraIF.interactable = false;
            tagIF.interactable = false;
            singerIF.interactable = false;
            composerIF.interactable = false;
            lyricsFromIF.interactable = false;
            yearPublishedIF.interactable = false;
            commentIF.interactable = false;
            tagIF.interactable = false;

            selectMusicBtn.interactable = false;
            updateBtn.interactable = false;
            //viewLevelBtn.interactable = false;

            enableToggle.interactable = false;
            licenceFreeToggle.interactable = false;

            categoryDD.interactable = false;
            subCategoryDD.interactable = false;

            availModeDD.interactable = false;
        }
        else
        {
            titleNameIF.interactable = true;
            commentIF.interactable = true;
            orquestraIF.interactable = true;
            tagIF.interactable = true;
            singerIF.interactable = true;
            composerIF.interactable = true;
            lyricsFromIF.interactable = true;
            yearPublishedIF.interactable = true;
            commentIF.interactable = true;
            tagIF.interactable = true;
            licenceFreeToggle.interactable = true;

            bool canAddFiles = PermissionManager.Instance.GetPermissionForRole1("Music", "add files");
            selectMusicBtn.interactable = canAddFiles;
            updateBtn.interactable = true;

            bool canEnable = PermissionManager.Instance.GetPermissionForRole1("Music", "publish/unpublish/enable");
            enableToggle.interactable = canEnable;

            categoryDD.interactable = true;
            subCategoryDD.interactable = true;

            availModeDD.interactable = PermissionManager.Instance.GetPermissionForRole1("Music", "publish/unpublish/enable");
        }
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

    //NetworkConst.music data;
    public void updateMusic(NetworkConst.music _music)
    {
        this.music.setMusic(_music);
    }
    public void updateGroupDD()
    {
        //updateGroupAccessDD();
        groupSelectionDD.ClearOptions();
        List<string> groups = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.groupAccess> _group in MusicManager.Instance.groupDic)
        {
            groups.Add(_group.Key);
            Debug.Log(_group.Key);
        }
        groupSelectionDD.AddOptions(groups);
        updateSelectedGroup();
    }
    //public void updateSelectedGroup()
    //{
    //    string[] ids = music.getMusic().groupIds.Split(',');
    //    List<string> items = new List<string>();
    //    for (int i = 0; i < ids.Length - 1; i++)
    //    {
    //        int id;
    //        NetworkConst.groupAccess _groupAccess;
    //        int.TryParse(ids[i], out id);
    //        MusicManager.Instance.groupDic1.TryGetValue(id, out _groupAccess);
    //        items.Add(_groupAccess.name);
    //    }
    //    groupSelectionDD.EnableSelectedItemList(items);
    //}
    //public string getIDFromList()
    //{
    //    StringBuilder s = new StringBuilder();
    //    foreach (string name in groupSelectionDD.selectedItemList)
    //    {
    //        //stringbuilder
    //        //string str = SeriesManager.Instance.groupDic()
    //        Debug.Log(name);
    //        s.Append(MusicManager.Instance.groupDic[name].id.ToString() + ",");
    //    }
    //    Debug.Log(s);
    //    return s.ToString();
    //}

    public void updateSelectedGroup()
    {
        string[] ids = music.getMusic().groupIds.Split(',');
        Debug.Log("Group iDS array Lenght : " + ids.Length);
        List<string> items = new List<string>();
        for (int i = 0; i < ids.Length; i++)
        {
            int id;
            NetworkConst.groupAccess _groupAccess;
            int.TryParse(ids[i], out id);
            MusicManager.Instance.groupDic1.TryGetValue(id, out _groupAccess);
            items.Add(_groupAccess.name);
        }
        groupSelectionDD.EnableSelectedItemList(items);
    }
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
    public void loadAllGroup()
    {
        NetworkConst.allGroupAccessRes _allGroupRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postSeries data = new NetworkConst.postSeries();
        data.limit = 10;
        data.page_no = 1;

        WWWForm wwwform = new WWWForm();
        wwwform.AddField("limit", 100);
        wwwform.AddField("page_no", 1);
        string json = JsonUtility.ToJson(data);


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
                MusicManager.Instance.groupDic1.Clear();
                foreach (NetworkConst.groupAccess tmpGroup in _allGroupRes.data)
                {
                    //createNewSeries(tmpSeries);
                    Debug.Log("Group Name " + tmpGroup.name);
                    if (!MusicManager.Instance.groupDic.ContainsKey(tmpGroup.name))
                    {
                        MusicManager.Instance.groupDic.Add(tmpGroup.name, tmpGroup);
                        MusicManager.Instance.groupDic1.Add(tmpGroup.id, tmpGroup);
                    }

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
    public void PostMusicData()
    {
        data = new NetworkConst.music();
        data.id = this.music.ID;
        data.titleName = titleNameIF.text;
        data.orquestra = orquestraIF.text;
        data.lyricsFrom = lyricsFromIF.text;
        data.singer = singerIF.text;
        data.composer = composerIF.text;
        data.yearPublished = yearPublishedIF.text;
        data.licenceFree = false;
        data.comment = commentIF.text;
        //data.price = priceIF.text;
        data.tag = tagIF.text;
        data.category_music = categoryDD.options[categoryDD.value].text;
        data.subcategory_music = subCategoryDD.options[subCategoryDD.value].text;
        data.groupAccess = availModeDD.options[availModeDD.value].text;
        data.enabled = enableToggle.isOn;
        data.licenceFree = licenceFreeToggle.isOn;
        Debug.Log(musicPath);
        if(!string.IsNullOrEmpty(musicPath)) data.musicFileData = System.IO.File.ReadAllBytes(musicPath);

        string json = JsonUtility.ToJson(data);

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("titleName", "nothing"));

        WWWForm wwwform = new WWWForm();
        Debug.Log("ID is " + this.music.ID);
        wwwform.AddField("musicID", this.music.ID);
        wwwform.AddField("titleName", titleNameIF.text);
        wwwform.AddField("orquestra", orquestraIF.text);
        wwwform.AddField("lyricsFrom", lyricsFromIF.text);
        wwwform.AddField("singer", singerIF.text);
        wwwform.AddField("composer", composerIF.text);
        wwwform.AddField("yearPublished", yearPublishedIF.text);
        wwwform.AddField("licenceFree", licenceFreeToggle.isOn.ToString());
        wwwform.AddField("comment", commentIF.text);
        //wwwform.AddField("price", priceIF.text);
        wwwform.AddField("availabilityMode", availType);
        if (availType == 2) wwwform.AddField("group_id", getIDFromList());
        if (availType == 3) wwwform.AddField("accessKey", accessIF.text);
        if (availType == 4) wwwform.AddField("accessKey", accessIF.text);
        wwwform.AddField("tag", tagIF.text);
        wwwform.AddField("category_music", categoryDD.options[categoryDD.value].text);
        wwwform.AddField("subcategory_music", subCategoryDD.options[subCategoryDD.value].text);
        //wwwform.AddField("groupAccess", availModeDD.options[availModeDD.value].text);
        wwwform.AddField("public", "1");
        wwwform.AddField("enabled", enableToggle.isOn.ToString());
        if(!string.IsNullOrEmpty(musicPath)) wwwform.AddBinaryData("musicFileData", File.ReadAllBytes(musicPath), musicNameText.text, "audio/wav");

        NetworkingManager.Instance.APIEditMusicData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonData = JsonMapper.ToObject(resData);
            if ((bool)jsonData["status"])
            {
                Debug.Log("Status is True");

                NetworkConst.newMusicRes _newMusicRes;
                _newMusicRes = JsonUtility.FromJson<NetworkConst.newMusicRes>(resData);
                updateMusic(_newMusicRes.data);
                closePopup();
            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(jsonData["message"].ToString());
                //message.text = levelJsonData["message"].ToString();
            }

            ////PlayerData.Instance.PlayerName = name;
            ////PlayerData.Instance.PlayerGUID = playerRowKey;

        });
    }
}

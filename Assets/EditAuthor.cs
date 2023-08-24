using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EditAuthor :  MonoBehaviour, IPanel
{
    [SerializeField]
    InputField uNameIF;
    [SerializeField]
    InputField emailIF;
    [SerializeField]
    InputField mobileIF;
    [SerializeField]
    InputField cNameIF;
    [SerializeField]
    InputField cityIF;
    [SerializeField]
    InputField countryIF, roleIF;
    //[SerializeField]
    //InputField passwordIF;

    [SerializeField]
    MultipleSelectionDropDown groupSelectionDD;

    [SerializeField]
    Dropdown languageDD;



    [SerializeField]
    Text message;

    public Dictionary<string, NetworkConst.groupAccess> groupDic = new Dictionary<string, NetworkConst.groupAccess>();
    public Dictionary<int, NetworkConst.groupAccess> groupDic1 = new Dictionary<int, NetworkConst.groupAccess>();
    NetworkConst.author _author;

    private void Start()
    {
        languageDD.onValueChanged.AddListener(delegate {
            LanguageChanged();
        });
        //Debug.LogError("Roles Name" + PermissionManager.Instance.GetRoleNames());

    }
    public void Initialize(string data,string namel="", string authtasknamel="")
    {
        this._author = JsonUtility.FromJson<NetworkConst.author>(data); ;
        uNameIF.text = _author.name;
        emailIF.text = _author.email;
        mobileIF.text = _author.mobile.ToString();
        cNameIF.text = _author.companyName;
        cityIF.text = _author.city;
        countryIF.text = _author.country;
        roleIF.text = PermissionManager.Instance.GetRoleNames();
        //roleDD.value = roleDD.options.FindIndex(option => option.text == _author.role);
        languageDD.value = languageDD.options.FindIndex(option => option.text == _author.language);


        Debug.Log("User Group" + this._author.userGroup);
        loadAllGroup();
    }
    public void LanguageChanged()
    {
        Debug.Log("Language Changed");
        //this._selectedLanguage = getLanguageByName(languageDD.options[languageDD.value].text);
    }
    public void onLoginButtonClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Login);
    }
    public void onHomeButtonClick()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
    }
    public void onUpdateButtonClick()
    {

        if (string.IsNullOrEmpty(uNameIF.text))
        {
            message.text = "Name field is  empty";
            Debug.Log("Password is null or empty");
            return;
        }

        if (!NetworkingManager.Instance.IsValidEmail(emailIF.text))
        {
            Debug.Log("Email is not valid");
            message.text = "Email is not valid";
            return;
        }
        //if (string.IsNullOrEmpty(passwordIF.text))
        //{
        //    message.text = "Password  field is empty";
        //    Debug.Log("Password is null or empty");
        //    return;
        //}
        Debug.Log("Register");
        PostUpdatedData();

    }

    public void updateGroupDD()
    {
        //updateGroupAccessDD();
        groupSelectionDD.ClearOptions();
        List<string> groups = new List<string>();
        foreach (KeyValuePair<string, NetworkConst.groupAccess> _group in groupDic)
        {
            groups.Add(_group.Key);
            Debug.Log(_group.Key);
        }
        groupSelectionDD.AddOptions(groups);
        updateSelectedGroup();
    }
    public void updateSelectedGroup()
    {
        string[] ids = this._author.userGroup.Split(',');
        Debug.Log("Group iDS array Lenght : " + ids.Length);
        List<string> items = new List<string>();
        for (int i = 0; i < ids.Length; i++)
        {
            int id;
            NetworkConst.groupAccess _groupAccess;
            int.TryParse(ids[i], out id);
            groupDic1.TryGetValue(id, out _groupAccess);
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
            if (!string.IsNullOrEmpty(name)) s.Append(groupDic[name].id.ToString() + ",");
        }
        Debug.Log(s);
        string selectedGroupIds = s.ToString();
        if(s.Length>1)selectedGroupIds = selectedGroupIds.Remove(selectedGroupIds.Length - 1, 1);
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
                groupDic.Clear();
                groupDic1.Clear();
                foreach (NetworkConst.groupAccess tmpGroup in _allGroupRes.data)
                {
                    //createNewSeries(tmpSeries);
                    Debug.Log("Group Name " + tmpGroup.name);
                    if (!groupDic.ContainsKey(tmpGroup.name))
                    {
                        groupDic.Add(tmpGroup.name, tmpGroup);
                        groupDic1.Add(tmpGroup.id, tmpGroup);
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

    public void PostUpdatedData()
    {
        string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.author data = new NetworkConst.author();
        data.email = emailIF.text;
        data.name = name;
        //data.password = passwordIF.text;
        //Int32.TryParse(mobileIF.text, out data.mobile);
        data.mobile = mobileIF.text;
        data.companyName = cNameIF.text;
        data.city = cityIF.text;
        data.country = countryIF.text;
        data.user_type = 3;//2 User 3 Author

        WWWForm wwwform = new WWWForm();
        wwwform.AddField("name", uNameIF.text);
        wwwform.AddField("email", emailIF.text);
        wwwform.AddField("mobile", mobileIF.text);
        wwwform.AddField("companyName", cNameIF.text);
        wwwform.AddField("city", cityIF.text);
        wwwform.AddField("country", countryIF.text);
        string lang = languageDD.options[languageDD.value].text;
        wwwform.AddField("language", lang);
        //wwwform.AddField("role", roleDD.options[roleDD.value].ToString());
        //wwwform.AddField("role", roleDD.value);
        wwwform.AddField("userGroup", getIDFromList());
        //wwwform.AddField("settings", emailIF.text);
        //wwwform.AddBinaryData("csv_file", File.ReadAllBytes(langFilePath), this._selectedLanguage.sort_name + ".csv", "text/csv");


        NetworkingManager.Instance.APIEditAuthorData(wwwform, (string data) =>
        {
            string token = CreatorData.Instance.Token;
            Debug.Log("God Login PLayer Response" + data);
            NetworkConst.loginRes _loginRes;
            _loginRes = JsonUtility.FromJson<NetworkConst.loginRes>(data);
            CreatorData.Instance.setCreatorData(_loginRes.data);
            CreatorData.Instance.Token = token;
            UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
        });
    }
}

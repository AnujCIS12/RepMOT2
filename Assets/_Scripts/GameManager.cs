using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

    public delegate void onSuccess(string Text);
    public JsonData jsonResponse;
    public Dictionary<string, NetworkConst.groupAccess> groupAccessDic = new Dictionary<string, NetworkConst.groupAccess>();
    public Dictionary<int, NetworkConst.role> rolesDic = new Dictionary<int, NetworkConst.role>();


    [HideInInspector]
    public Dictionary<int, string> groupIdsList = new();

    // Start is called before the first frame update
    void Start()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.Login);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void loadAllGroupAccess(onSuccess callback)
    {
        NetworkConst.allGroupAccessRes _allGroupAccessRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postCategory data = new NetworkConst.postCategory();
        data.moduleID = 4;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAllGroupAccessData(json, (string data) =>
        {
            _allGroupAccessRes = JsonUtility.FromJson<NetworkConst.allGroupAccessRes>(data);
            jsonResponse = JsonMapper.ToObject(data);
            if ((bool)jsonResponse["status"])
            {
                //CreatorData.Instance.setCreatorData(this._loginRes.data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                Debug.Log("---------Data -------------" + _allGroupAccessRes);
                foreach (NetworkConst.groupAccess tmpGroupAccess in _allGroupAccessRes.data)
                {
                    Debug.Log(tmpGroupAccess.name + "..................Name adding");
                    if (!groupAccessDic.ContainsKey(tmpGroupAccess.name))
                        groupAccessDic.Add(tmpGroupAccess.name, tmpGroupAccess);
                }
                if (callback != null) callback("Success");
            }
            else
            {
                //message.text = jsonResponse["message"].ToString();
                Debug.Log("Message is " + _allGroupAccessRes.message);
            }

        });
    }

    public void loadRolesData(onSuccess callback)
    {
        NetworkConst.allRolesRes _allRolesRes;
        //string playerRowKey = System.Guid.NewGuid().ToString();
        NetworkConst.postCategory data = new NetworkConst.postCategory();
        data.moduleID = 4;

        string json = JsonUtility.ToJson(data);

        NetworkingManager.Instance.APILoadAssignedRolesData(json, (string data) =>
        {
            _allRolesRes = JsonUtility.FromJson<NetworkConst.allRolesRes>(data);
            foreach (NetworkConst.role tmpRole in _allRolesRes.data)
            {
                Debug.Log(tmpRole.name + "..................Name adding");
                if (!rolesDic.ContainsKey(tmpRole.id))
                    rolesDic.Add(tmpRole.id, tmpRole);
            }
            if (callback != null) callback("Success");


        });
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
                foreach (NetworkConst.groupAccess tmpGroup in _allGroupRes.data)
                {
                    groupIdsList.Add(tmpGroup.id, tmpGroup.name);
                }
            }
            else
            {
                Debug.Log("Message is " + _allGroupRes.message);
            }
        });
    }
}

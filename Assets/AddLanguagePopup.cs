using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddLanguagePopup : MonoBehaviour
{
    [SerializeField]
    InputField titleNameIF, shortNameIF;

    [SerializeField]
    Text message;

    //[SerializeField]
    //Text languageNameText;

    JsonData jsonData;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void DoneBtnClicked()
    {
        PostLanguageData();
        //StartCoroutine(UploadDataOnServer());
        //StartCoroutine(Upload1());
    }
    public void closePopup()
    {
        setDefaultField();
        gameObject.SetActive(false);
    }
    public void setDefaultField()
    {
        titleNameIF.text = "";
        shortNameIF.text = "";
    }

    void PostLanguageData()
    {

        NetworkConst.language data = new NetworkConst.language();
        //data.ID = "1";
        data.name = titleNameIF.text;
        data.sort_name = shortNameIF.text;

        string json = JsonUtility.ToJson(data);

        WWWForm wwwform = new WWWForm();
        wwwform.AddField("name", titleNameIF.text);
        wwwform.AddField("sort_name", shortNameIF.text);


        NetworkingManager.Instance.APIAddNewLanguageData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            jsonData = JsonMapper.ToObject(resData);
            if ((bool)jsonData["status"])
            {
                Debug.Log("Status is True");
                NetworkConst.languageRes langRes;
                langRes = JsonUtility.FromJson<NetworkConst.languageRes>(resData);
                LanguageManager.Instance.AddNewLanguage(langRes.data);
                closePopup();
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

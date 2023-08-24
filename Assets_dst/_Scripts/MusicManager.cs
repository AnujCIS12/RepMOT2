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

    [SerializeField]
    GameObject viewPanel;

    public List<Music> musiclist = new List<Music>();
    Music selectedMusic;


    public JsonData jsonResponse;

    public void Initialize(string data)
    {
        loadAllMusic();
    }
    public void addMusicBtnClicked()
    {
        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.AddMusic);
    }

    public void createNewMusic(NetworkConst.music tempMusic)
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
        for (int i=0;i<musiclist.Count;i++)
        {
            Debug.Log(i);
            deleMusic(musiclist[i]);
        }
        musiclist.Clear();
        //gameObject.SetActive(false);
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
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
                Debug.Log("Name " + _allMusicRes.data[0].titleName);
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

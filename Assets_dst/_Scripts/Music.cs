using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    
    //List<Level> levels;

    [SerializeField]
    Text musicNameText;

    NetworkConst.music _music;

    [SerializeField]
    EditMusic editMusic;

    public void initializeData(NetworkConst.music music)
    {
        this._music = music;
        setMusicName(music.titleName);
        //musicNameText.text = music.titleName;
    }
    public void deleteMusic()
    {
        //DeleteConfirmPopup.Instance.initialize(confirmedDelete, "music");
    }
    
    public void musicBtnClicked()
    {
        UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MusicView, this._music.id.ToString());
    }
    public void setMusic(NetworkConst.music tmpMusic)
    {
        this._music = tmpMusic;
        setMusicName(tmpMusic.titleName);
    }
    public void setMusicName(string name)
    {
        musicNameText.text = name;
    }
    public int ID
    {
        get { return this._music.id; }
    }
    public NetworkConst.music getMusic()
    {
        return this._music;
    }
    public void showDeleteConfirmPopup()
    {
        //MusicManager.Instance.setSelectedMusic(this);
        UIPopupManager.Instance.showPopup(UIPopupManager.ePopup.ConirmPopup);

        DeleteConfirmPopup.Instance.initialize(confirmedDelete, "music");
    }
    public void showEditPopup()
    {
        editMusic.InitializeData(this);
    }

    public void confirmedDelete()
    {
        MusicManager.Instance.calldeleteMusicAPI(this);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.IO;

public class Music : MonoBehaviour
{
    
    //List<Level> levels;

    [SerializeField]
    TextMeshProUGUI musicNameText,orquestraText, composerText;

    NetworkConst.music _music;

    [SerializeField]
    EditMusic editMusic;
    
    [SerializeField] Button editBtn, deleteBtn;
    private string baseURL,audioURL;
    public AudioSource audioSource;
    public Sprite playSP,pauseSP;
    public Image playBtnImg;
    public Slider playbackSlider;
    public TextMeshProUGUI playbackTimeText;
    public TextMeshProUGUI musicDurationText;
    public bool play;

    public void initializeData(NetworkConst.music music)
    {
        this.baseURL = "https://rt.cisinlive.com/mot-vk/public/uploads/music/";
        setMusic(music);
        //setMusicName(music.titleName);
        setUpForRole();
    }

    private void Update()
    {
        if (this.audioSource.isPlaying && this.play == true)
        {
            this.playbackSlider.value = this.audioSource.time;
            this.playbackTimeText.text = this.audioSource.time.ToString("0:00");
        }
    }

    public void setUpForRole()
    {
        bool canView = PermissionManager.Instance.GetPermissionForRole1("Music", "view");
        editBtn.gameObject.SetActive(canView);

        bool canDelete = PermissionManager.Instance.GetPermissionForRole1("Music", "delete");
        deleteBtn.gameObject.SetActive(canDelete);


    }
    public void OnClickPlayBtn()
    {
        if (!this.audioSource.isPlaying)
        {
            foreach(Music m in MusicManager.Instance.musiclist)
            {
                m.PauseMusic();
            }
            string filename = this._music.musicFile;
            string fileURL = Path.Combine(Application.persistentDataPath, filename);
            Debug.Log("Music Name " + this._music.musicFile);
            if (File.Exists(fileURL))
            {
                Debug.Log("File URL " + fileURL);
                StartCoroutine(LoadMusicCoroutine(fileURL,this._music.id));
            }
            else
            {
                this.audioURL = this.baseURL + filename;
                Debug.Log("Audio URL " + this.audioURL);
                StartCoroutine(DownloadMusicCoroutine(filename, this._music.id));

            }
        }
       
    }
    public void OnClickMusicPauseBtn()
    {
        if (this._music.id == MusicManager.Instance.currentMusicId)
        {
            if (this.audioSource.isPlaying)
            {
                this.audioSource.Pause();
                this.play = false;
            }
            else
            {
                this.audioSource.Play();
                play = false;
                this.play = true;
            }
        }        
    }

    public void SeekAudio()
    {
        this.playbackSlider.onValueChanged.AddListener((value) =>
        {
            this.audioSource.time = value;
        });
    }

    private void PauseMusic()
    {
        this.audioSource.Pause();
    }

    public void OnClickRewind()
    {
        if(this._music.id == MusicManager.Instance.currentMusicId)
        { 
            this.audioSource.Stop();
            this.audioSource.Play();
            play = false;
            this.play = true;
        }
        
    }
    private IEnumerator LoadMusicCoroutine(string filePath,int currentMusicId)
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
                    this.playbackSlider.maxValue = audioClip.length;
                    this.playbackTimeText.text = "0:00";
                    this.musicDurationText.text = audioClip.length.ToString("0:00");
                    this.audioSource.clip = audioClip;
                    this.audioSource.Play();
                    play = false;
                    this.play = true;
                    MusicManager.Instance.currentMusicId = currentMusicId;
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
    private IEnumerator DownloadMusicCoroutine(string file, int currentMusicId)
    {
        this.audioURL = this.baseURL + file;
        using (UnityWebRequest www = UnityWebRequest.Get(audioURL))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string fileName = Path.GetFileName(audioURL); 
                string savePath = Path.Combine(Application.persistentDataPath, fileName); 

                File.WriteAllBytes(savePath, www.downloadHandler.data);

                Debug.Log("Music downloaded and saved: " + savePath);
                
                StartCoroutine(LoadMusicCoroutine(savePath,currentMusicId));
            }
            else
            {
                MusicManager.Instance.downloadErrorMessage.GetComponent<TextMeshProUGUI>().text = "Music not found "+www.error;
                Invoke(nameof(DeactivateMessage),3f);
                MusicManager.Instance.downloadErrorMessage.SetActive(true);
                MusicManager.Instance.errorGO.SetActive(true);
                Debug.LogError("Music not found " + www.error);
            }
        }
    }
    private void DeactivateMessage()
    {
        MusicManager.Instance.errorGO.SetActive(false);
        MusicManager.Instance.downloadErrorMessage.SetActive(false);
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
        Debug.Log("__________Set Music_____________" + tmpMusic.titleName + "   category    " + tmpMusic.category_music);
        this._music = tmpMusic;
        this.playbackSlider.value = 0;
        setMusicName(tmpMusic.titleName);
    }
    public void setMusicName(string name)
    {
        this.musicNameText.text = name+"-"+this._music.id;
      
        this.orquestraText.text = this._music.orquestra;
        this.composerText.text = this._music.composer;
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
        MusicManager.Instance.loadAllCategory((string message) => {
            editMusic.InitializeData(this);
        });
        
    }

    public void confirmedDelete()
    {
        MusicManager.Instance.calldeleteMusicAPI(this);
    }
}

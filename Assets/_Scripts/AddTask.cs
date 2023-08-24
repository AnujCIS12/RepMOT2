using JetBrains.Annotations;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Video;
using System.IO;

public class AddTask : MonoBehaviour
{

    [SerializeField]
    InputField nameIF, maxScoreIF, pointsACIF, threshholdIF, instructionsIF, explanationsIF;
    public RenderTexture renderTexture;
    [SerializeField]
    Dropdown selectRecOptionsDD, recordingsCDD;
    public VideoPlayer videoPlayer;
    public TMP_Dropdown fileDatDropDown;
    public GameObject soundActions,videoActions;
    private string[] dirEntries;
    private string dirPath = "Assets/_Recordings/";
    private string fileName;
    private string filePath;
    private string inputtracePath;
    private string settingsPath;
    private string selectedMusicPath;
    public string upmp;
    public Dictionary<string, NetworkConst.record1> recordDic = new Dictionary<string, NetworkConst.record1>();
    NetworkConst.record1 _selectedRecord;
    //Record selectedRecord;

    public int recordType;
    public AudioSource audioSource;
    public GameObject Screen;
    [SerializeField]
    Text message;

    [SerializeField]
    Text pictureNameText, musicNameText, videoNameText;

    JsonData jsonData;
    JsonData jsonResponse;
    public static string musicVersion;

    public string picturePath;
    public string musicPath;
    public string videoPath;
    private string selectedVideoPath;

    public void OnEnable()
    {
        OpenPopup();
        soundActions.SetActive(false);
        videoActions.SetActive(false);
        Screen.SetActive(false);
    }
    public void selectPictureBtnClicked()
    {
        FileBrowserEg1.Instance.GetImagePath((string[] paths) => {
            Debug.Log("Paths is " + paths[0]);
            picturePath = paths[0];
            string pCFilePath;
            pCFilePath = FileBrowserEg1.Instance.getName(picturePath);
            string[] tempPath = pCFilePath.Split("/");

            pictureNameText.text = tempPath[^1];
            //byte[] picturebyte = System.IO.File.ReadAllBytes(picturePath);
        });
    }
    public void LoadVideoFromURL(string url)
    {
        videoPlayer.url = url;
        videoPlayer.prepareCompleted += OnVideoPrepareCompleted;
        videoPlayer.Prepare();
    }

    private void OnVideoPrepareCompleted(VideoPlayer player)
    {
        player.prepareCompleted -= OnVideoPrepareCompleted;
        player.Play();
    }
    public void selectVideoBtnClicked()
    {
        FileBrowserEg1.Instance.GetVideoPath((string[] paths) => {
            Debug.Log("Paths is " + paths[0]);
            videoPath = paths[0];

            string pCFilePath;
            pCFilePath = FileBrowserEg1.Instance.getName(videoPath);
            selectedVideoPath =pCFilePath;
            string[] tempPath = pCFilePath.Split("/");

            videoNameText.text = tempPath[^1];
            if(!string.IsNullOrEmpty(videoNameText.text))
            {
                videoActions.SetActive(true);
          
            }
            //byte[] picturebyte = System.IO.File.ReadAllBytes(picturePath);
        });
    }
    public void OnVideoPlayBtnClick()
    {
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;
            
        }
        Screen.SetActive(true);
        Screen.GetComponentInChildren<RawImage>().texture = renderTexture;
        
        if(!videoPlayer.isPlaying)
        {
            LoadVideoFromURL(selectedVideoPath);
            //videoPlayer.Play();
        }

    }
    public void OnVideoPauseBtnClicked()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;

        }
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
        else
        {
            videoPlayer.Play();
        }
    }
    public void OnVideoRewind()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;

        }
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
        }
    }
    public void selectMusicBtnClicked()
    {
        FileBrowserEg1.Instance.GetMusicPath(onFileSelected);
    }
    
    public void OnclickPlayBtn()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            
           // videoPlayer.url = null;
            Screen.SetActive(false);
        }
        if (audioSource.isPlaying)
        {
            //audioSource.Stop();
            //StartCoroutine(LoadMusicCoroutine(selectedMusicPath));
        }
        else
        {
            StartCoroutine(LoadMusicCoroutine(selectedMusicPath));
        }
    }
    public void OnCloseScreen()
    {
        Screen.GetComponentInChildren<RawImage>().texture = null;
        videoPlayer.Stop();
        videoPlayer.url = null;
        
        Screen.SetActive(false);
       
    }
    public void OnClickPause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            videoPlayer.clip = null;
            // videoPlayer.url = null;
            Screen.SetActive(false);
        }
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }else
        {
            audioSource.Play();
        }
        
    }
    public void OnClickRewind()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            videoPlayer.clip = null;
            // videoPlayer.url = null;
            Screen.SetActive(false);
        }
        audioSource.Stop();
        audioSource.Play();
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

    public void onFileSelected(string[] paths)
    {
        Debug.Log("Paths is " + paths[0]);
        
        musicPath = paths[0];
        upmp = musicPath;
        Debug.Log("On File Selected 1"+musicPath);
        string pCFilePath;
        pCFilePath= FileBrowserEg1.Instance.getName(musicPath);
        selectedMusicPath = pCFilePath;
        string[] tempPath = pCFilePath.Split("/");
        musicNameText.text = tempPath[^1];
        if(!string.IsNullOrEmpty(musicNameText.text))
        {
            soundActions.SetActive(true);
        }
        byte[] audiobyte = System.IO.File.ReadAllBytes(musicPath);
    }

    private void selectRecOptionsDDValueChanged(Dropdown _dd)
    {
        recordType = _dd.value;
        Debug.Log("Value Changed " + _dd.value);
        switch (_dd.value)
        { 
            case 0:
                print("Select from Local");
                optSelectFromLocal();
                break;
            case 1:
                print("Select From Cloud");
                optLoadFromCloud();
                break;
        }
    }
    public void optSelectFromLocal()
    {
        if (recordingsCDD.gameObject.activeSelf) recordingsCDD.gameObject.SetActive(false);
        OpenPopup();
        fileDatDropDown.gameObject.SetActive(true);
    }
    public void optLoadFromCloud()
    {
        LoadRecordingFromCloud();
        if (fileDatDropDown.gameObject.activeSelf) fileDatDropDown.gameObject.SetActive(false);
        recordingsCDD.gameObject.SetActive(true);
    }
    public void UpdateLocalRecordPath()
    {
        //if (!CheckAllInputs())   // Disabled for testing have to enable
        //	return;

        fileName = fileDatDropDown.options[fileDatDropDown.value].text;
        filePath = dirPath + fileName;
        inputtracePath = dirPath + fileName + "/" + fileName + "_1.inputtrace";
        //musicPath = dirPath + fileName + "/" + fileName + "_2.wav";
        musicPath = upmp;
        Debug.Log("Update music path "+musicPath);
        settingsPath = dirPath + fileName + "/" + fileName + "_3.settings";
    }
    public void DoneBtnClicked()
    {
        PostTaskData();
        //StartCoroutine(UploadDataOnServer());
        //StartCoroutine(Upload1());
    }
    public void AVClosed()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            videoPlayer.clip = null;
            videoPlayer.url = null;
            Screen.SetActive(false);
        }
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
        if (videoActions.activeInHierarchy)
        {
            videoActions.SetActive(false);
        }
        if (soundActions.activeInHierarchy)
        {
            soundActions.SetActive(false);
        }
        selectedMusicPath = "";
        musicNameText.text = "Select Sound File";
        selectedVideoPath = "";
        videoNameText.text = "Select Video File";
        pictureNameText.text = "Select Picture";

    }
    public void closePopup()
    {
        AVClosed();
        setDefaultField();
        gameObject.SetActive(false);
    }
    public void setDefaultField()
    {
        nameIF.text = "";
        maxScoreIF.text = "";
        pointsACIF.text = "";
        threshholdIF.text = "";
        instructionsIF.text = "";
        explanationsIF.text = "";
    }

    public void OpenPopup()
    {
        OpenDirectory();
        //entryDateIF.text = System.DateTime.UtcNow.ToString("dd-MM-yyyy");
        //entryDateIF.interactable = false;
    }

    private void ProcessFile([CanBeNull] string _filename)
    {
        fileDatDropDown.options.Add(new TMP_Dropdown.OptionData() { text = Path.GetFileNameWithoutExtension(_filename) });
    }


    private void OpenDirectory()
    {
        // Option: append the '@' verbatim to the directory path string  
        // -> this gives the complete path like D://            dirPath = @"" + dirPath;
        dirEntries = Directory.GetFiles(dirPath);
        //fileDatDropDown.ClearOptions();
        if (dirEntries.Length >= 1)
        {
            foreach (var dirName in dirEntries)
            {
                ProcessFile(dirName);
            }
        }
        else
        {
            // former: ButtonFunctions.MessageToUser("No recording folder like this:   " + dirPath, 3);  
            // "Sorry, can not find this recording folder. Spelling right?"
            Debug.Log("Didn't find Recording");

            //var eventParam = new EventParam();

            //eventParam.message_text = "No recording folder like this:   " + dirPath;
            //eventParam.message_time = 3f;
            //EventManager.TriggerEvent("MessageToUser", eventParam: eventParam);
        }
    }

    public int getRecIDByName(string name)
    {
        Debug.Log(name);
        NetworkConst.record1 entry;
        if (string.IsNullOrEmpty(name)) return -1;
        Debug.Log("Before Try to get category " + name);
        if (recordDic.TryGetValue(name, out entry))
        {
            Debug.Log("Entry" + entry);
            return entry.id;
        }
        else return -1;
    }
    //public NetworkConst.record1 getRecByName(string name)
    //{
    //    Debug.Log(name);
    //    NetworkConst.record1 entry;
    //    if (string.IsNullOrEmpty(name)) return;
    //    Debug.Log("Before Try to get category " + name);
    //    if (recordDic.TryGetValue(name, out entry))
    //    {
    //        Debug.Log("Entry" + entry);
    //        return entry;
    //    }
    //    else return null;
    //}

    public void LoadRecordingFromCloud()
    {
        NetworkConst.allRecordRes _allRecordRes;
        TaskManager.Instance.loadAllRecords((string data) =>
        {
            _allRecordRes = JsonUtility.FromJson<NetworkConst.allRecordRes>(data);
            jsonResponse = JsonMapper.ToObject(data);

            foreach (NetworkConst.record1 tmpObj in _allRecordRes.data)
            {
                //createNewLanguage(tmpLanguage);
                if (!recordDic.ContainsKey(tmpObj.name))
                    recordDic.Add(tmpObj.name, tmpObj);
            }

            updateLanguageDD(_allRecordRes);
        });
    }

    List<string> recordTitle = new List<string>();
    public void updateLanguageDD(NetworkConst.allRecordRes resRecords)
    {
        recordingsCDD.ClearOptions();
        recordTitle.Clear();
        foreach (NetworkConst.record1 _record in resRecords.data)
        {
            recordTitle.Add(_record.name);

            Debug.Log(_record.name);
        }
        recordingsCDD.AddOptions(recordTitle);
        this._selectedRecord = resRecords.data[0];
        //languageDD.value = languageDD.options.FindIndex(option => option.text == this.music.getMusic().category_music);
    }
    public void cloudRecOptionsDDValueChanged(Dropdown _dd)
    {
        //this._selectedRecord = getRecIDByName(selectRecOptionsDD.options[selectRecOptionsDD.value].text);
        recordDic.TryGetValue(recordingsCDD.options[recordingsCDD.value].text, out this._selectedRecord);
    }

    void PostTaskData()
    {

        NetworkConst.task data = new NetworkConst.task();
        //data.ID = "1";
        data.name = nameIF.text;
        data.seriesID = TaskManager.Instance._level.seriesID;
        data.levelID = TaskManager.Instance._level.id.ToString();
        int.TryParse(maxScoreIF.text, out data.maxScorepointToGet);
        //data.maxScorepointToGet = data.maxScorepointToGet;
        int.TryParse(pointsACIF.text, out data.pointsAccomplished);
        //data.pointsAccomplished = pointsACIF.text;
        data.threshold = threshholdIF.text;
        data.instructions = instructionsIF.text;
        data.explanations = explanationsIF.text;

        Debug.Log("Series ID is : " + TaskManager.Instance._level.seriesID + "Series ID : " + TaskManager.Instance._level.id);

        

        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("titleName", "nothing"));

        WWWForm wwwform = new WWWForm();
        wwwform.AddField("name", nameIF.text);
        wwwform.AddField("seriesID", TaskManager.Instance._level.seriesID);
        wwwform.AddField("levelID", TaskManager.Instance._level.id);
        wwwform.AddField("maxScorepointToGet", maxScoreIF.text);
        wwwform.AddField("pointsAccomplished", pointsACIF.text);
        wwwform.AddField("threshold", threshholdIF.text);
        wwwform.AddField("instructions", instructionsIF.text);
        wwwform.AddField("explanations", explanationsIF.text);

        data.record = recordType.ToString();
        wwwform.AddField("record", recordType.ToString());
        if (recordType == 0)
        {
            UpdateLocalRecordPath();
            data.fileDat = filePath;
            data.fileInputTrace = File.ReadAllBytes(inputtracePath);
            data.fileMusic = File.ReadAllBytes(musicPath);
            data.fileSettings = File.ReadAllBytes(settingsPath);

            wwwform.AddField("fileDat", filePath);
            wwwform.AddBinaryData("fileInputTrace", File.ReadAllBytes(inputtracePath), fileName + "_1.inputtrace", "input/inputtrace");
            //  wwwform.AddBinaryData("fileMusic", music, string.Concat(fileName, "_2.wav"), "audio/wav");
            //wwwform.AddBinaryData("fileMusic", File.ReadAllBytes(musicPath), fileName + "_2.wav", "audio/wav");
            byte[] temp = File.ReadAllBytes(musicPath);
            wwwform.AddBinaryData("fileMusic", temp, fileName + "_2.wav", "audio/mpeg");
            wwwform.AddBinaryData("fileSettings", File.ReadAllBytes(settingsPath), fileName + "_1.inputtrace", "setting/settings");
        }
        if (recordType == 1)
        {
            data.recordID = this._selectedRecord.id;
            wwwform.AddField("recordID", this._selectedRecord.id);
        }
        if (!string.IsNullOrEmpty(picturePath)) {
            data.pictures = System.IO.File.ReadAllBytes(picturePath);
            wwwform.AddBinaryData("pictures[]", File.ReadAllBytes(picturePath), pictureNameText.text, "jpg/gif");
        }
        if (!string.IsNullOrEmpty(musicPath)) {
            Debug.LogError(musicPath);
            byte[] temp1 = File.ReadAllBytes(musicPath);
            data.sound = System.IO.File.ReadAllBytes(musicPath);
            wwwform.AddBinaryData("sound", temp1, musicNameText.text, "audio/wav");
        }
        if (!string.IsNullOrEmpty(videoPath))
        {
            data.video = System.IO.File.ReadAllBytes(videoPath);
            wwwform.AddBinaryData("video", File.ReadAllBytes(videoPath), videoNameText.text, "video/mp4");
        }

        string json = JsonUtility.ToJson(data);
        Debug.Log(json);
        NetworkingManager.Instance.APIAddTaskData(wwwform, (string resData) =>
        {
            
            Debug.Log("God Login PLayer Response" + resData);
            jsonData = JsonMapper.ToObject(resData);
            if ((bool)jsonData["status"])
            {
                Debug.Log("Status is True");
                NetworkConst.newTaskRes _newTask;
                _newTask = JsonUtility.FromJson<NetworkConst.newTaskRes>(resData);
                TaskManager.Instance.createNewTask(_newTask.data);
                closePopup();
                //RecordManager.Instance.createNewObj(data);
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

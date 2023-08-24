using JetBrains.Annotations;
using LitJson;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Video;
using System.IO;
using System.Text.RegularExpressions;
public class EditTask : MonoBehaviour
{

    [SerializeField]
    InputField nameIF, maxScoreIF, pointsACIF, threshholdIF, instructionsIF, explanationsIF;

    [SerializeField]
    Dropdown selectRecOptionsDD, recordingsCDD;
    [SerializeField]
    Text message;
    public GameObject soundActions, videoActions, Screen;
    [SerializeField]
    Text pictureNameText, musicNameText, videoNameText;

    public TMP_Dropdown fileDatDropDown;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    public Dictionary<string, NetworkConst.record1> recordDic = new Dictionary<string, NetworkConst.record1>();
    NetworkConst.record1 _selectedRecord;
    //Record selectedRecord;

    public int recordType,selectedCRecIndex, selectedLRecIndex;    

    JsonData jsonData;
    JsonData jsonResponse;

    public static string musicVersion;

    private string[] dirEntries;
    private string dirPath = "Assets/_Recordings/";
    private string fileName;
    private string filePath;
    private string inputtracePath;
    private string settingsPath;
    public string picturePath;
    public string musicPath;
    public string videoPath;
    private string selectedMusicPath;
    private string selectedVideoPath;
    private bool soundSelected = false;
    private string loadedMusicPath;
    JsonData levelJsonData;
    Task task;
    NetworkConst.task _selectedTask;


    public delegate void OnGotAPIResponse();
    OnGotAPIResponse onGotAPIResponse;

    public void OnEnable()
    {
        //OpenPopup();
    }
    public void OnVideoPlayBtnClick()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;

        }
        Screen.SetActive(true);

        if (!videoPlayer.isPlaying)
        {
            LoadVideoFromURL(selectedVideoPath);
            //videoPlayer.Play();
        }

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
    public void OnCloseScreen()
    {

        videoPlayer.Stop();
        videoPlayer.url = null;
       
        Screen.SetActive(false);

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
        if(audioSource.isPlaying==false)
        {
            if (soundSelected)
            {
                Debug.Log("Sound Selected " + selectedMusicPath);
                //if sound selected
                StartCoroutine(LoadMusicCoroutine(selectedMusicPath));
            }
            else if (soundSelected == false)
            {
                string existdownloadpath = Path.Combine(Application.persistentDataPath, musicNameText.text);
                if (File.Exists(existdownloadpath))
                {
                    Debug.Log("File found " + existdownloadpath);
                    StartCoroutine(LoadMusicCoroutine(existdownloadpath));
                }
                else
                {
                    Debug.Log("File Not found " + loadedMusicPath);
                    StartCoroutine(DownloadMusicCoroutine(loadedMusicPath));

                }
            }
        }
       
        
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
        }
        else
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
    private IEnumerator DownloadMusicCoroutine(string file)
    {
        Debug.Log("Download Music Path => " + file);

        using (UnityWebRequest www = UnityWebRequest.Get(file))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string fileName = Path.GetFileName(file);
                string savePath = Path.Combine(Application.persistentDataPath, musicNameText.text);

                File.WriteAllBytes(savePath, www.downloadHandler.data);

                Debug.Log("Music downloaded and saved: " + savePath);

                StartCoroutine(LoadMusicCoroutine(savePath));
            }
            else
            {
                //MusicManager.Instance.downloadErrorMessage.GetComponent<TextMeshProUGUI>().text = "Music not found " + www.error;
                //Invoke(nameof(DeactivateMessage), 3f);
                //MusicManager.Instance.downloadErrorMessage.SetActive(true);
                //MusicManager.Instance.errorGO.SetActive(true);
                Debug.LogError("Music not found " + www.error);
            }
        }
    }
    private IEnumerator LoadMusicCoroutine(string filePath)
    {
        Debug.Log("Load Music Path => " + filePath);
        using (var audioLoader = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.UNKNOWN))
        {

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
    public void setProfileDetails(int id)
    {
        Debug.Log("Task ID "+id);
        for(int i=0;i< TaskManager.Instance._listavTask.data.Length;i++)
        {
            if(id==TaskManager.Instance._listavTask.data[i].id)
            {
                if(!String.IsNullOrEmpty(TaskManager.Instance._listavTask.data[i].pictures))
                {
                    pictureNameText.text = TaskManager.Instance._listavTask.data[i].pictures;
                }
                if (!String.IsNullOrEmpty(TaskManager.Instance._listavTask.data[i].sound))
                {
                    string na= TaskManager.Instance._listavTask.data[i].sound;
                    na = Regex.Replace(na, @"\s", "");
                    musicNameText.text = na;
                   
                    soundActions.SetActive(true);
                    
                    loadedMusicPath = Path.Combine(TaskManager.Instance._listavTask.audios_path,musicNameText.text);
                    //selectedMusicPath = selectedMusicPath.Replace(" ","");
                    Debug.Log("Edit Sound path " + loadedMusicPath);
                }
                if (!String.IsNullOrEmpty(TaskManager.Instance._listavTask.data[i].video))
                {
                    videoNameText.text = TaskManager.Instance._listavTask.data[i].video;
                    videoActions.SetActive(true);

                    selectedVideoPath = TaskManager.Instance._listavTask.videos_path + "/" + videoNameText.text;
                    selectedVideoPath = selectedVideoPath.Replace(" ","");
                    Debug.Log("Edit video path "+selectedVideoPath);
                }
                
            }
        }
        
    }
    public void InitializeData(Task _task)
    {
        this.task = _task;
        setProfileDetails(_task.ID);
        _selectedTask = _task.getTask();
        nameIF.text = _selectedTask.name;
        maxScoreIF.text = _selectedTask.maxScorepointToGet.ToString();
        pointsACIF.text = _selectedTask.pointsAccomplished.ToString();
        threshholdIF.text = _selectedTask.threshold;
        instructionsIF.text = _selectedTask.instructions;
        explanationsIF.text = _selectedTask.explanations;
        string st= Encoding.UTF8.GetString(_selectedTask.fileMusic);
        string st2 = Encoding.UTF7.GetString(_selectedTask.sound);
        Debug.Log("File Music : "+st);
        Debug.Log("Sound : "+st2);
        setRecOptionValue();
        showPopup();
    }
    public void setRecOptionValue()
    {
        int recChoice;
        int.TryParse(_selectedTask.record, out recChoice);
        selectRecOptionsDD.value = recChoice;
        if (recChoice == 0)
        {
            OpenPopup();
            setLRecDDValue();
        }
        if (recChoice == 1)
        {
            onGotAPIResponse += setCRecDDValue;
            //setCRecDDValue();
        }
    }
    public void setCRecDDValue()
    {
        int tmpRecindex;
        //int.TryParse(_selectedTask.recordID, out tmpRecindex);
        recordingsCDD.value = getRecIndexByID(_selectedTask.recordID);
        //getRecIDByName();
    }
    public void setLRecDDValue()
    {
        string path = FileBrowserEg1.Instance.getName(_selectedTask.fileDat);
        string[] strArray = path.Split("/",10);
        string fileName = strArray[strArray.Length - 1];
        Debug.Log("_____________name_________________________" + name);
        Debug.Log(fileName);
        fileDatDropDown.value = fileDatDropDown.options.FindIndex(option => option.text == fileName);
    }

    public void DoneBtnClicked()
    {
        PostTaskData();
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
        soundSelected = false;

    }
    public void closePopup()
    {
        AVClosed();
        setDefaultField();
        gameObject.SetActive(false);
    }
    public void showPopup()
    {
        gameObject.SetActive(true);
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
    public void updateTask(NetworkConst.task _task)
    {
        this.task.setTask(_task);
    }
    public void setMusicName(string name)
    {
        musicNameText.text = name;
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

    public void selectVideoBtnClicked()
    {
        FileBrowserEg1.Instance.GetVideoPath((string[] paths) => {
            Debug.Log("Paths is " + paths[0]);
            videoPath = paths[0];

            string pCFilePath;
            pCFilePath = FileBrowserEg1.Instance.getName(videoPath);
            selectedVideoPath = pCFilePath;
            string[] tempPath = pCFilePath.Split("/");
            videoNameText.text = tempPath[^1];
            if (!string.IsNullOrEmpty(videoNameText.text))
            {
                videoActions.SetActive(true);
            }
            //byte[] picturebyte = System.IO.File.ReadAllBytes(picturePath);
        });
    }

    public void selectMusicBtnClicked()
    {
        FileBrowserEg1.Instance.GetMusicPath(onFileSelected);
    }
    public void onFileSelected(string[] paths)
    {
        Debug.Log("Paths is " + paths[0]);
        musicPath = paths[0];

        string pCFilePath;
        pCFilePath = FileBrowserEg1.Instance.getName(musicPath);
        selectedMusicPath = pCFilePath;
        string[] tempPath = pCFilePath.Split("/");
        soundSelected = true;

        musicNameText.text = tempPath[^1];
        if (!string.IsNullOrEmpty(musicNameText.text))
        {
            soundActions.SetActive(true);
        }
        byte[] audiobyte = System.IO.File.ReadAllBytes(musicPath);
    }



    public void selectRecOptionsDDValueChanged(Dropdown _dd)
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
        Debug.Log("On Load From Cloud");
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
        musicPath = dirPath + fileName + "/" + fileName + "_2.wav";
        settingsPath = dirPath + fileName + "/" + fileName + "_3.settings";
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
    public NetworkConst.record1 getRecByName(string name)
    {
        Debug.Log(name);
        NetworkConst.record1 entry;
        Debug.Log("Before Try to get category " + name);
        if (recordDic.TryGetValue(name, out entry))
        {
            Debug.Log("Entry" + entry);
            return entry;
        }
        else return entry;
    }
    public NetworkConst.record1 getRecByID(int id)
    {
        NetworkConst.record1 temp;
        temp = recordDic.First().Value;
        foreach (var (key,value) in recordDic)
        {
            if (value.id == id) return value;
        }
        temp.id = -1;
        return temp;
    }
    public int getRecIndexByID(int id)
    {
        int i = 0;
        foreach (var (key, value) in recordDic)
        {
            if (value.id == id) return i;
            i++;
        }
        return -1;
    }

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
            if(onGotAPIResponse!=null)onGotAPIResponse();
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
    NetworkConst.task data;
    void PostTaskData()
    {

        NetworkConst.task data = new NetworkConst.task();
        //data.ID = "1";
        data.id = this.task.getTask().id;
        data.taskID = this.task.getTask().id.ToString();
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
        wwwform.AddField("taskID", this.task.getTask().id.ToString());
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
            wwwform.AddBinaryData("fileMusic", File.ReadAllBytes(musicPath), fileName + "_2.wav", "audio/wav");
            wwwform.AddBinaryData("fileSettings", File.ReadAllBytes(settingsPath), fileName + "_3.settings", "setting/settings");
        }
        if (recordType == 1)
        {
            data.recordID = this._selectedRecord.id;
            wwwform.AddField("recordID", this._selectedRecord.id);
        }
        if (!string.IsNullOrEmpty(picturePath))
        {
            data.pictures = System.IO.File.ReadAllBytes(picturePath);
            wwwform.AddBinaryData("pictures[]", File.ReadAllBytes(picturePath), pictureNameText.text, "jpg/gif");
        }
        if (!string.IsNullOrEmpty(musicPath))
        {
            data.sound = System.IO.File.ReadAllBytes(musicPath);
            wwwform.AddBinaryData("sound", File.ReadAllBytes(musicPath), musicNameText.text, "audio/wav");
        }
        if (!string.IsNullOrEmpty(videoPath))
        {
            data.video = System.IO.File.ReadAllBytes(videoPath);
            wwwform.AddBinaryData("video", File.ReadAllBytes(videoPath), videoNameText.text, "video/mp4");
        }

        string json = JsonUtility.ToJson(data);
        Debug.Log(json);
        NetworkingManager.Instance.APIEditTaskData(wwwform, (string resData) =>
        {
            Debug.Log("God Login PLayer Response" + resData);
            levelJsonData = JsonMapper.ToObject(resData);
            if ((bool)levelJsonData["status"])
            {
                Debug.Log("Status is True");
                //LevelManager.Instance.createNewLevel(data);
                //UIPanelManager.Instance.changeMode(UIPanelManager.ePanel.MainMenu);
                NetworkConst.newTaskRes _newTask;
                _newTask = JsonUtility.FromJson<NetworkConst.newTaskRes>(resData);
                updateTask(_newTask.data);
                closePopup();
            }
            else
            {
                Debug.Log("Message ");
                Debug.Log(levelJsonData["message"].ToString());
                //message.text = levelJsonData["message"].ToString();
            }

        });
}
    }

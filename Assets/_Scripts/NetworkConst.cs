using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkConst : MonoBehaviour
{

    [Serializable]
    public struct author
    {
        public int id;
        public string name;
        public string email;
        public string password;
        public string mobile;
        public string companyName;
        public string city;
        public string country;
        public int user_group;
        //public string groupIds;
        public string userGroup;
        public string role;
        public string language;
        public string token;
        public int user_type;
        public byte[] avatar;
        public byte[] settings;
    }
    
    [Serializable]
    public struct role
    {
        public int id;
        public string name;
        public string guard_name;
        public int type;
        public string created_at;
        public string updated_at;
    }

    [Serializable]
    public struct role1
    {
        public int id;
        public string name;
        public permission[] permissions;
    }

    [Serializable]
    public struct permission
    {
        public int id;
        public string name;
        public string modal_name;
        public int role_id;
    }

    [Serializable]
    public struct allRolesRes1
    {
        public bool status;
        public role1[] data;
        public string message;
    }

    [Serializable]
    public struct allRolesRes
    {
        public bool status;
        public role[] data;
        public string message;
    } 

    [Serializable]
    public struct postLogin
    {
        public string email;
        public string password;
    }


    
    [Serializable]
    public struct loginRes
    {
        public bool status;
        public author data;
    }
    [Serializable]
    public struct allSeriesRes
    {
        public bool status;
        public series[] data;
        public string message;
    }

    [Serializable]
    public struct newSeriesRes
    {
        public bool status;
        public series data;
        public string message;
    }
    [Serializable]
    public struct postRevover
    {
        public string name;
        public string email;
    }
    [Serializable]
    public struct series
    {
        public int id;
        public int seriesID;
        public string name;
        public string serieVersion;
        public int creatorAccountID;
        public string price;
        public bool enabled;
        public string availabilityMode;
        public string groupAccess;
        public string groupIds;
        public string keyAccess;
        public string dateCreated;
        public string comment;
        public string settings;
        public string explanations;
        public int level_serie;
        public int level;
        public string category_series;
        public string subCategory_series;
        public string tag;
        public byte[] previewPicture;
        public string previewText;


    }
    [Serializable]
    public struct postSeries
    {
        public int limit;
        public int page_no;
    }

    [Serializable]
    public struct level
    {
        public int id;
        public int levelID;
        public string seriesID;
        public string name;
        public string threshold;
        public string levelVersion;
        public string version;
        public int creatorAccountID;
        public string dateCreated;
        public bool enabled;
        public string sinceDate;
        public string comment;
        public string settings;
        public string explanations;

        public string level_category;
        public string level_subcategory;
    }
    [Serializable]
    public struct newLevelRes
    {
        public bool status;
        public level data;
        public string message;
    }
    [Serializable]
    public struct allLevelRes
    {
        public bool status;
        public level[] data;
        public string message;
    }

    [Serializable]
    public struct postLevel
    {
        public int limit;
        public int page_no;
        public string seriesID;
    }


    //Task
    [Serializable]
    public struct task
    {
        public int id;
        public string name;
        public int recordID;
        public string seriesID;
        public string levelID;
        public string taskID;
        public int maxScorepointToGet;
        public int pointsAccomplished;
        public string threshold;
        public string instructions;
        public string explanations;
        public string record;
        public string fileDat;
        public byte[] fileInputTrace;
        public byte[] fileMusic;
        public byte[] fileSettings;
        public byte[] pictures;
        public byte[] sound;
        public byte[] video;
    }
    [Serializable]
    public struct innertask
    {
        public int id;
        public string name;
        public int recordID;
        public string seriesID;
        public string levelID;
        public string taskID;
        public int maxScorepointToGet;
        public int pointsAccomplished;
        public string threshold;
        public string instructions;
        public string explanations;
        public string record;
        public string fileDat;
        public byte[] fileInputTrace;
        public byte[] fileMusic;
        public byte[] fileSettings;
        public string  pictures;
        public string sound;
        public string video;
    }
    [Serializable]
    public struct selectedTaskDetail
    {
        public bool status;
        public innertask[] data;
        public string message;
        public string videos_path;
        public string audios_path;
    }
    [Serializable]
    public struct allTaskRes
    {
        public bool status;
        public task[] data;
        public string message;
    }
    [Serializable]
    public struct newTaskRes
    {
        public bool status;
        public task data;
        public string message;
    }

    [Serializable]
    public struct postTask
    {
        public int limit;
        public int page_no;
        public string seriesID;
        public string levelID;
    }



    //Music
    [Serializable]
    public struct music
    {
        public int id;
        public int musicID;
        public string titleName;
        
        public string orquestra;
        public string singer;
        public string lyricsFrom;
        public string composer;

        public string yearPublished;
        public bool licenceFree;
        public bool enabled;
        public string comment;
        public string keyAccess;
        public string price;
        public string currency;
        public string category_music;
        public string subcategory_music;
        public string tag;
        public string groupIds;
        public string availabilityMode;
        public string groupAccess;

        public byte[] musicFileData;
        public string musicFile;
    }
    [Serializable]
    public struct newMusicRes
    {
        public bool status;
        public music data;
        public string message;
    }
    [Serializable]
    public struct allMusicRes
    {
        public bool status;
        public music[] data;
        public string message;
        public string base_url;
    }

    [Serializable]
    public struct postMusic
    {
        public int limit;
        public int page_no;
    }


 
    //Record
    [Serializable]
    public struct record1
    {
        public int recordID;
        public int seriesID;
        public int levelID;
        public int id;
        public string accountsID;

        public string creator;
        public string name;
        public string role;
        //public string public;
        public string entryDate;
        public string needVersion;
        public string groupIds;
        public string availabilityMode;
        public string groupAccess;
        public string gameLevel;
        public string fileDat;
        public string fileInputTrace;
        public byte[] fileMusic;
        public string fileSettings;
        public string comment;
        public string accessKey;
        public string category_record;
        public string subCategory_record;
    }
    

    [Serializable]
    public struct newRecordRes
    {
        public bool status;
        public record1 data;
        public string message;
    }
    [Serializable]
    public struct allRecordRes
    {
        public bool status;
        public record1[] data;
        public string message;
        public string base_url;
    }

    [Serializable]
    public struct postRecord
    {
        public int limit;
        public int page_no;
    }


    //Language
    [Serializable]
    public struct language
    {
        public int languageID;
        public string creator;
        public string accountsID;
        public string entryDate;
        public string needVersion;
        public byte[] languageFileData;
        public string languageFile;

        
        public int id;
        public int appLangID;
        public string name;
        public string sort_name;
        public byte[] csv_file;
        public string file;
        public string created_at;
        public string updated_at;
        
    }


    [Serializable]
    public struct languageRes
    {
        public bool status;
        public language data;
        public string base_url;
        public string message;
    }

    [Serializable]
    public struct allLanguageRes
    {
        public bool status;
        public language[] data;
        public string base_url;
        public string message;
    }

    [Serializable]
    public struct postLanguage
    {
        public int limit;
        public int page_no;
    }


    ///////Category
    /// <summary>
    ///     [Serializable]
    public struct postCategory
    {
        public int moduleID;
    }
    /// </summary>

    [Serializable]
    public struct allCategoryRes
    {
        public bool status;
        public category[] data;
        public string message;
    }
    [Serializable]
    public struct category
    {
        public int id;
        public string name;
        public string category_module;
        public string created_at;
        public string updated_at;
        public string deleted_at;
    }
    //////Sub Category

    public struct postSubCategory
    {
        public int categoryID;
}
[Serializable]
    public struct allSubCategoryRes
    {
        public bool status;
        public subcategory[] data;
        public string message;
    }
    [Serializable]
    public struct subcategory
    {
        public int id;
        public string name;
        public int category_id;
        public string created_at;
        public string updated_at;
    }

    /////////////////Group Access //////////////////////////////
    [Serializable]
    public struct allGroupAccessRes
    {
        public bool status;
        public groupAccess[] data;
        public string message;
        public string total_page;
        public string base_url;
    }
    [Serializable]
    public struct groupAccess
    {
        public int id;
        public string name;
        public string creator;
        public string client;
        public string created_at;
        public string updated_at;
    }
}

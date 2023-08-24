using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkConst : MonoBehaviour
{
    [Serializable]
    public struct postLogin
    {
        public string email;
        public string password;
    }


    [Serializable]
    public struct postRegister
    {
        public string name;
        public string email;
        public string password;
        public int mobile;
        public string companyName;
        public string city;
        public string country;
        public int user_type;
        public string token;
    }
    [Serializable]
    public struct loginRes
    {
        public bool status;
        public postRegister data;
    }
    [Serializable]
    public struct allSeriesRes
    {
        public bool status;
        public series[] data;
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
        public string name;
        public string serieVersion;
        public int creatorAccountID;
        public float price;
        public bool enabled;
        public string availabilityMode;
        public DateTime dateCreated;
        public string comment;
        public string settings;
        public string explanations;
        public int level;
        public string category;
        public string subCategory;
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
        public int levelID;
        public string seriesID;
        public string name;
        public string threshold;
        public string levelVersion;
        public string version;
        public int creatorAccountID;
        public DateTime dateCreated;
        public bool enabled;
        public DateTime sinceDate;
        public string comment;
        public string settings;
        public string explanations;
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
    }


    //Task
    [Serializable]
    public struct task
    {
        public int id;
        public string name;
        public int recordID;
        public int maxScorepointToGet;
        public int pointsAccomplished;
        public string threshold;
        public string instructions;
        public string Explanations;
        public string Record;
        public byte[] Pictures;
        public byte[] Sound;
        public byte[] video;
    }

    [Serializable]
    public struct allTaskRes
    {
        public bool status;
        public task[] data;
        public string message;
    }

    [Serializable]
    public struct postTask
    {
        public int limit;
        public int page_no;
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
        public string price;
        public string category;
        public string subcategory;
        public string tag;
        public string groupAccess;

        public byte[] musicFile;
    }

    [Serializable]
    public struct allMusicRes
    {
        public bool status;
        public music[] data;
        public string message;
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
        public int id;
        public string accountsID;

        public string role;
        public string name;
        public string entryDate;
        public string needVersion;

        public string gameLevel;
        public string fileDat;
        public string fileInputTrace;
        public byte[] fileMusic;
        public string fileSettings;
        public string comment;
        public string category;
        public string subCategorie;
    }

    [Serializable]
    public struct allRecordRes
    {
        public bool status;
        public record1[] data;
        public string message;
    }

    [Serializable]
    public struct postRecord
    {
        public int limit;
        public int page_no;
    }


}

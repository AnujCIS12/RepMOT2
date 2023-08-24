using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreatorData : SingletonMonoBehaviour<CreatorData>
{
	public const int SAVE_VERSION = 1;

	public static readonly string GAME_KEY = "NPC_Prototype";

	public static readonly string PREFS_KEY = GAME_KEY + "_SAVEDATA_USERKEY";

	public static readonly string NOTIFICATION_KEY = GAME_KEY + "_NOTIFCATION_FLAG";
	public static readonly string NOTIFICATION_NEW_ITEM_KEY = GAME_KEY + "_NOTIFCATION_NEW_ITEM";

	public enum eNotiFlag
	{
		None = 0,
		Registed,
	}


	public enum eUrl
	{
		FreeAndPointDiscription,
		InGameDiscription,
		GoldTicketIOS,
		GoldTicketAnd,
		LotteryHelp,
		Situmon,
		Toiawase,
		Riyoukiyaku,
		PrivacyPolicy,
		AppReviewIOS,
		AppReviewAndroid,
		Max
	}


	[SerializeField]
	CreatorDataMain _creatorData;


	public string APP_VER = null;

	

    public CreatorDataMain creatorData
	{
		get { return this._creatorData; }
	}
	public bool IsNewAccount()
	{
		//if (string.IsNullOrEmpty(this._creatorData.creator_id) == true || string.IsNullOrEmpty(this._creatorData.token) == true)
		//{
		//	return true;
		//}
		return false;
	}
	public void setCreatorData(NetworkConst.postRegister _playerData)
    {
		this.PlayerName = _playerData.name;
		this.Token = "Bearer " + _playerData.token;
		Debug.Log("Token " + this.Token);
    }

	public string Token
	{
		set
		{
			this._creatorData.token = value;
			this.SaveData();
		}
		get { return this._creatorData.token; }
	}

	public int CreatorID
	{
		get { return this._creatorData.creator_id; }
	}

	public string PlayerName
    {
        set { this._creatorData.player_name = value;
			this.SaveData();
		}
		get { return this._creatorData.player_name; }
    }

	public string PlayerGUID
    {
        set { this._creatorData.player_guid = value;
			this.SaveData();
		}
        get { return this._creatorData.player_guid; }
    }

	public string LatestScoreKey
    {
        set { this._creatorData.latest_scoreKey=value;
			this.SaveData();
		}
        get { return this._creatorData.latest_scoreKey; }
    }

	public byte[] ReplayData
    {
        set { this._creatorData.replay_data = value;
			this.SaveData();
		}
		get { return this._creatorData.replay_data; }
    }

	public bool IsActiveGameData
    {
        set
        {
			this._creatorData.isActiveGame = value;
			this.SaveData();
        }
        get { return this._creatorData.isActiveGame; }
    }
	public byte[] ActiveGameSessionData
    {
        set { this._creatorData.activeGameSessionData = value;
			this.SaveData();
		}
        get { return this._creatorData.activeGameSessionData; }
    }





    public bool IsRegisted
	{
		get
		{
			//if (string.IsNullOrEmpty(this._creatorData.takeover_account_addr) == false)
			//{
			//	return true;
			//}
			return false;
		}
	}
	public eNotiFlag NotificationEable
	{
		get
		{
			if (this.HasSaveKey(NOTIFICATION_KEY) == true)
			{
				return eNotiFlag.Registed;
			}
			else
			{
				return eNotiFlag.Registed;
				//return eNotiFlag.None;
			}
			//return this._creatorData.notification_enable;
		}

		set
		{
			if (value == eNotiFlag.Registed)
			{
				this.SetSaveKeyInt(NOTIFICATION_KEY, (int)eNotiFlag.Registed);
			}
		}
	}

	public bool IsTodayFirst
	{
		get
		{
			long now_unix = UnixTime.FromDateTime(DateTime.Now.Date);
			if (_creatorData.last_login_unix_time == 0 || (this._creatorData.last_login_unix_time > now_unix))
			{
				return true;
			}
			return false;
		}
	}


	public void SetCareerHistory(int rank)
	{
#if OFF_LINE_MODE
		
		this._creatorData.career_history_array[rank]++;
		if (this._creatorData.career_history_array[rank] > 99999) this._creatorData.career_history_array[rank] = 99999;
#endif
		this.SaveData();
	}

	
	public int IsFirstGameMenu
	{
		get
		{
			return this._creatorData.first_login;
		}
		set
		{
			this._creatorData.first_login = value;
			this.SaveData();
		}
	}

    //--------------------------------------------------------------------------------------


	
	public void ResetScoreRecord()
	{
		//if (this.IsGetKuziPoint == true)
		//{
		//	this._creatorData.score_record_index = 0;
		//	for (int i = 0; i < this._creatorData.score_record_array.Length; i++)
		//	{
		//		this._creatorData.score_record_array[i] = 0;
		//	}
		//	this.SaveData();
		//}
		//this.IsGetKuziPoint = false;
	}

	//--------------------------------------------------------------------------------------
	bool FindGameData()
	{
		var r = PlayerPrefs.HasKey(PREFS_KEY);
		return r;
	}


	/// <summary>
	/// 
	/// </summary>
	public void SetInvitationedData()
	{
		this._creatorData.token = "";
		this._creatorData.creator_id = -1;
		this._creatorData.first_login = 0;
		this._creatorData.player_name = "";
		this._creatorData.player_guid = "";
		this._creatorData.replay_data = null;
		this._creatorData.last_login_unix_time = 0;
		this.SaveData();
	}

	public void CreateBlankData()
	{
		if (this._creatorData == null)
		{
			this._creatorData = new CreatorDataMain();
		}
		//this._creatorData.creator_id = "dummy_id_desu";
		this._creatorData.version = SAVE_VERSION;
		this._creatorData.last_login_unix_time = 0;
		this._creatorData.activeGameSessionData = null;
		this._creatorData.isActiveGame = false;
		this._creatorData.latest_scoreKey = null;

		this.SaveData();
	}

	public void LoadData()
	{
		if (this.FindGameData())
		{
            Debug.Log("Got Data from system");

			try
			{
				string json = Decode64(PlayerPrefs.GetString(PREFS_KEY));
				this._creatorData = JsonUtility.FromJson<CreatorDataMain>(json);
                //this._creatorData.creator_id = "62441";
                //this._creatorData.token = "achEfmUfsskt8Bvh4w3dwY";
                Debug.Log(json);
			}
			catch
			{
				this.CreateBlankData();
			}
			if (this._creatorData == null || this._creatorData.version != SAVE_VERSION)
			{
                Debug.Log("Player Data is null");
				this.CreateBlankData();
			}

		}
		else
		{
            Debug.Log("Create Blank Data");
			this._creatorData = new CreatorDataMain();
			this._creatorData.creator_id = -1;//SystemInfo.deviceUniqueIdentifier;

			this.CreateBlankData();
		}

		this.SaveData();
	}

	public void SaveData()
	{
		Debug.Log("Save Data");
		string json = Encorde64(JsonUtility.ToJson(this._creatorData));
		PlayerPrefs.SetString(PREFS_KEY, json);
		PlayerPrefs.Save();
	}
	public string Decode64(string src64)
	{
		src64 = src64.Replace("\"", "");
		byte[] decodedBytes = Convert.FromBase64String(src64);
		string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);
		return decodedText;
	}

	public string Encorde64(string src)
	{
		byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(src);
		string encodedText = Convert.ToBase64String(bytesToEncode);
		return encodedText;
	}


	public void UpdateLoginTime()
	{
		this._creatorData.last_login_unix_time = UnixTime.FromDateTime(DateTime.Now.Date);
		this.SaveData();
	}


	//--------------------------------------------------------------------------------------------------

	bool HasSaveKey(string key)
	{
		return PlayerPrefs.HasKey(key);
	}
	int GetSaveKeyInt(string key)
	{
		if (this.HasSaveKey(key) == true)
		{
			return PlayerPrefs.GetInt(key);
		}
		return 0;
	}
	void SetSaveKeyInt(string key, int val)
	{
		PlayerPrefs.SetInt(key, val);
		PlayerPrefs.Save();
	}
}

[System.Serializable]
public class CreatorDataMain
{
	public int version;

	public int creator_id;
	public string player_name;
	public string player_guid;
	public string latest_scoreKey;
	public long last_login_unix_time;
	public string token;
	public int first_login;
	public byte[] replay_data;
	public bool isActiveGame;
	public byte[] activeGameSessionData;
}


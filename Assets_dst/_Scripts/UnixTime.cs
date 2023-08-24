using System;


public static class UnixTime
{
    private static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    // 現在時刻からUnixTime(秒)を計算する.
    public static long Now()
    {
        return (FromDateTime(DateTime.UtcNow));
    }

    // 現在時刻からUnixTime(ミリ秒)を計算
    public static long NowMS()
    {
        return (FromDateTimeMS(DateTime.UtcNow));
    }

    // UnixTime(秒)からDateTimeに変換.
    public static DateTime FromUnixTime(long unixTime)
    {
        return UNIX_EPOCH.AddSeconds(unixTime).ToLocalTime();
    }


    // 時間をUnixTime(秒)に変換する.
    public static long FromDateTime(DateTime dateTime)
    {
        double nowTicks = (dateTime.ToUniversalTime() - UNIX_EPOCH).TotalSeconds;
        return (long)nowTicks;
    }



    // 時間をUnixTime(ミリ秒)に変換する.
    public static long FromDateTimeMS(DateTime dateTime)
    {
        double nowTicks = (dateTime.ToUniversalTime() - UNIX_EPOCH).TotalMilliseconds;
        return (long)nowTicks;
    }

    // UnixTimeから時分を取得
    public static string ConvHHMM(long unixTime)
    {
        System.DateTime date = UnixTime.FromUnixTime(unixTime);
        return String.Format("{0:00}", date.Hour) + ":" + String.Format("{0:00}", date.Minute);
    }

    // UnixTimeから時分秒を取得
    public static string ConvHHMMSS(long unixTime)
    {
        System.DateTime date = UnixTime.FromUnixTime(unixTime);
        return String.Format("{0:00}", date.Hour) + ":" + String.Format("{0:00}", date.Minute) + ":" + String.Format("{0:00}", date.Second);
    }

    // UnixTimeから年月日時分秒を取得
    public static string ConvYYYYMMDD_HHMMSS(long unixTime)
    {
        System.DateTime date = UnixTime.FromUnixTime(unixTime);
        return String.Format("{0:0000}", date.Year) + "-" + String.Format("{0:00}", date.Month) + "-" + String.Format("{0:00}", date.Day) + " " + String.Format("{0:00}", date.Hour) + ":" + String.Format("{0:00}", date.Minute) + ":" + String.Format("{0:00}", date.Second);
    }

	// UnixTimeから年月日時分秒を取得
	public static string ConvYYYYMMDD(long unixTime)
	{
		System.DateTime date = UnixTime.FromUnixTime(unixTime);
		return String.Format("{0:0000}", date.Year) + "年" + String.Format("{0:00}", date.Month) + "月" + String.Format("{0:00}", date.Day) + "日";
	}


}
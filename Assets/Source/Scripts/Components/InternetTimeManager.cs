using System;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Kuhpik;
using UnityEngine;

namespace InternetTime
{
    public static class InternetTimeManager
    {
        public static async Task<DateTime> DateTimeCheck()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://worldtimeapi.org/api/ip");
            string jsonString = await response.Content.ReadAsStringAsync();
            TimeData timeData = JsonUtility.FromJson<TimeData>(jsonString);

            DateTime currentTimeUtc = UnixTimeToDateTime(timeData.unixtime);
            DateTime currentTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(currentTimeUtc, DateTimeKind.Utc), TimeZoneInfo.Local);

            Debug.Log("Current time is: " + currentTimeLocal.ToString());
            return currentTimeUtc;
        }

        private static DateTime UnixTimeToDateTime(long unixTime)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
            return dateTimeOffset.UtcDateTime;
        }

        [Serializable]
        private class TimeData
        {
            public long unixtime;
        }
    }
}

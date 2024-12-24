using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InternWebApplication
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblPrayerTimes.Text = "Please enter the coordinates (latitude and longitude).";
            }
        }

        protected async void btnGetPrayerTimes_Click(object sender, EventArgs e)
        {
            string latitudeStr = txtLatitude.Value;
            string longitudeStr = txtLongitude.Value;

            double latitude, longitude;

            // Validate the coordinates entered by the user
            if (double.TryParse(latitudeStr, out latitude) && double.TryParse(longitudeStr, out longitude))
            {
                string apiUrl = $"https://mpt.i906.my/api/prayer/{latitude},{longitude}";
                var prayerData = await GetPrayerData(apiUrl);

                if (prayerData != null)
                {
                    DateTime today = DateTime.UtcNow.Date;
                    var todayPrayerTimes = prayerData.GetPrayerTimesForDate(today);

                    if (todayPrayerTimes != null)
                    {
                        lblPrayerTimes.Text = $"Fajr: {todayPrayerTimes[0]}<br>" +
                                              $"Sunrise: {todayPrayerTimes[1]}<br>" +
                                              $"Dhuhr: {todayPrayerTimes[2]}<br>" +
                                              $"Asr: {todayPrayerTimes[3]}<br>" +
                                              $"Maghrib: {todayPrayerTimes[4]}<br>" +
                                              $"Isha: {todayPrayerTimes[5]}";
                    }
                    else
                    {
                        lblPrayerTimes.Text = "No prayer times available for today.";
                    }
                }
                else
                {
                    lblPrayerTimes.Text = "Unable to fetch prayer times. Please test the coordinate first https://reqbin.com/. https://mpt.i906.my/api/prayer/{latitude},{longitude}";
                }
            }
            else
            {
                lblPrayerTimes.Text = "Invalid coordinates entered.Please test the coordinate first https://reqbin.com/. https://mpt.i906.my/api/prayer/{latitude},{longitude}";
            }
        }

        private async Task<PrayerApiResponse> GetPrayerData(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize JSON to a C# object
                    return JsonConvert.DeserializeObject<PrayerApiResponse>(responseBody);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        // Classes to map JSON response
        public class PrayerApiResponse
        {
            public PrayerData data { get; set; }

            public List<string> GetPrayerTimesForDate(DateTime date)
            {
                if (data == null || data.times == null || data.times.Count == 0) return null;

                // Get the index of the date (assuming times are in sequential order from the current month)
                DateTime startOfMonth = new DateTime(date.Year, date.Month, 1);
                int index = (date - startOfMonth).Days;

                if (index >= 0 && index < data.times.Count)
                {
                    var unixTimes = data.times[index];
                    List<string> prayerTimes = new List<string>();

                    foreach (var unixTime in unixTimes)
                    {
                        DateTime prayerTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime;
                        prayerTimes.Add(prayerTime.ToString("hh:mm tt", CultureInfo.InvariantCulture)); // 12-hour format
                    }

                    return prayerTimes;
                }

                return null;
            }
        }

        public class PrayerData
        {
            public List<List<long>> times { get; set; }
        }
    }
}

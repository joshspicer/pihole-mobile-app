using System;
using System.Net.Http;
using System.Threading.Tasks;
using PiholeDashboard.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PiholeDashboard.Functions
{
    public static class PiholeStateFunctions
    {
        // Generic disable Pihole Helper.
        public static async Task<bool> ModifyHelper(string operation, string duration, bool isBackupSelected, PiHoleConfig config)
        {
            try
            {
                var dest = isBackupSelected ? config.BackupUri : config.PrimaryUri;
                var auth = isBackupSelected ? config.BackupApiKey : config.PrimaryApiKey;
                var maybeDuration = duration != "" ? $"={duration}" : "";
                var uri = $"{dest}/admin/api.php?{operation}{maybeDuration}&auth={auth}";

                HttpClient _client = new HttpClient();
                _client.Timeout = TimeSpan.FromSeconds(5);
                var res = await _client.GetAsync(uri);

                // Even on auth failures, return code is 200...
                if (res.IsSuccessStatusCode)
                {
                    var content = await res.Content.ReadAsStringAsync();

                    // If return contains "status", we know we were successful.
                    if (content.Contains("status"))
                        return true;          
                }

                // If we get here, there's an error (maybe incorrect API key)
                string errStr = "Error toggling Pi-hole. Please check your WEBPASSWORD. (err=1)";
                Console.WriteLine($"Error with uri:{uri} ERR: {errStr}");
                return false;
            }
            catch (Exception err)
            {
                string errStr = "Could not connect to Pi-Hole service (err=2)";
                Console.WriteLine($"{errStr}: {err}");
                return false;
            }
        }
    }
}

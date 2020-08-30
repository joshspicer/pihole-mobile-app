using System;
using System.Text.Json;
using PiholeDashboard.Models;

namespace PiholeDashboard.Utils
{
    /// <summary>
    /// To keep values in App.Current.Properties they must be primitive values.
    /// Converts our config to a string to save its value.
    /// </summary>
    public static class PersistenceSerializer
    {
     
        public static void SerializeAndSaveConfig(PiHoleConfig configObj)
        {
            var configJson = JsonSerializer.Serialize<PiHoleConfig>(configObj);

            if (App.Current.Properties.ContainsKey("config"))
                App.Current.Properties["config"] = configJson;
            else
                App.Current.Properties.Add("config", configJson);
        }

        public static bool TryFetchConfig(out PiHoleConfig config)
        {
            config = null;

            if (App.Current.Properties.ContainsKey("config"))
            {
                var json = (string)App.Current.Properties["config"];
                config = JsonSerializer.Deserialize<PiHoleConfig>(json);

                if (config == null)
                    return false;

                return true;
            }

            return false;
        }
    }
}

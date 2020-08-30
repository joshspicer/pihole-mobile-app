namespace PiholeDashboard.Models
{
    public class PiHoleConfig
    {
        public string PrimaryUri { get; set; }
        public string PrimaryApiKey { get; set; }

        public string BackupUri { get; set; }
        public string BackupApiKey { get; set; }

        public PiHoleConfig()
        {
            PrimaryUri = "";
            PrimaryApiKey = "";
            BackupUri = "";
            BackupApiKey = "";
        }
    }
}
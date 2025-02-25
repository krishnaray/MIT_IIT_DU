namespace TicketAPI.Settings.DB
{
    public class DBSettingsBase
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseHost { get; set; } = string.Empty;
        public string DatabasePort { get; set; } = string.Empty;
        public string DatabaseUsername { get; set; } = string.Empty;
        public string DatabasePassword { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string UserTableName { get; set; } = string.Empty;
    }
}

namespace ManagerMoney.Models
{
    public class SecretsOptions
    {
        public string ConnectionString { get; set; }
        public string ConfigurationEmail_Email { get; set; }
        public string ConfigurationEmail_Password { get; set; }
        public string ConfigurationEmail_Host { get; set; }
        public int ConfigurationEmail_Port { get; set; }
    }
}

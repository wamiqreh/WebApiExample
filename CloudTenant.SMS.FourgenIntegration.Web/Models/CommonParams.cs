namespace SND.Models
{
    public class CommonParams
    {
        public string checksum { get; set; }
        public string device_id { get; set; }
        public string reg_id { get; set; }
        public string last_sync { get; set; } //mm/dd/yyyy

        public string brand_name { get; set; }
        public string model_name { get; set; }
        public string os_version { get; set; }
        public string resolution { get; set; }
    }
}
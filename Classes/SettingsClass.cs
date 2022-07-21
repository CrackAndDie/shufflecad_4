namespace shufflecad_4.Classes
{
    public class SettingsClass
    {
        // defaults
        public string PathToSrc { get; set; }
        public string MainFileName { get; set; }
        public string IpAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        // settings
        public bool DetailedOutput { get; set; }
        public bool ShowMinimap { get; set; }
        public bool AutoConnect { get; set; }
        public bool ShowPassword { get; set; }
    }
}

using System;
using System.IO;
using System.Xml.Serialization;

namespace DigitalProductionConfigEditor
{
    public class UserSettings
    {
        public bool ShowPcbGuide { get; set; } = true;
        public bool ShowConfigLoadedMessage { get; set; } = true;
        public string? BoxFolderPath { get; set; } // New property for Box path

        private static string SettingsPath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "DigitalProductionConfigEditor_Settings.xml");

        public static UserSettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var serializer = new XmlSerializer(typeof(UserSettings));
                    using (var stream = new FileStream(SettingsPath, FileMode.Open))
                    {
                        return (UserSettings)serializer.Deserialize(stream);
                    }
                }
            }
            catch { }
            return new UserSettings();
        }

        public void Save()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(UserSettings));
                using (var stream = new FileStream(SettingsPath, FileMode.Create))
                {
                    serializer.Serialize(stream, this);
                }
            }
            catch { }
        }
    }
}








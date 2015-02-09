using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server = Pluton.Server;

namespace Rustitute
{
    partial class Rustitute
    {
        private void LoadSettings()
        {
            botName = GetSettingOrDefault(ini, "Settings", "BotName", "Rust");
            Server.server_message_name = botName;

            GetSettingOrDefault(ini, "Settings", "motd", "");
            GetSettingOrDefault(ini, "Arena", "locationX", "");
            GetSettingOrDefault(ini, "Arena", "locationY", "");
            GetSettingOrDefault(ini, "Arena", "locationZ", "");
        }

        private string GetSettingOrDefault(IniParser i, string section, string name, string defaultValue)
        {
            string ret = defaultValue;

            if (i.ContainsSetting(section, name))
                ret = GetSetting(section, name);
            else
                i.AddSetting(section, name, defaultValue);

            return ret;
        }

        private string GetSetting(string section, string name)
        {
            return ini.GetSetting(section, name);
        }

        private bool GetSettingBool(string section, string name)
        {
            return ini.GetBoolSetting(section, name);
        }

        private int GetSettingInt(string section, string name)
        {
            var value = ini.GetSetting(section, name);
            if (value.Length == 0)
                return 0;
            return Int32.Parse(value);
        }

        private string SetSetting(string section, string name, string value)
        {
            if (!ini.ContainsSetting(section, name))
                ini.AddSetting(section, name, value);
            else
                ini.SetSetting(section, name, value);

            return value;
        }

        private bool SetSettingBool(string section, string name, bool value)
        {
            SetSetting(section, name, value ? "True" : "False");
            return value;
        }
    }
}

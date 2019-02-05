using Smod2;
using Smod2.API;
using Smod2.Attributes;
using Smod2.Config;
using System.IO;

namespace ReplacingDisconnecteds
{
	[PluginDetails(
		author = "InsetJesux",
		name = "Replacing Disconnecteds",
		description = "Spawn spectators replacing disconnected players",
		id = "insetjesux.replacing.disconnecteds",
		version = "1.0",
		SmodMajor = 3,
		SmodMinor = 2,
		SmodRevision = 2
		)]

	public class ReplacingDisconnecteds : Plugin
	{
        public bool ForceReplace { get; set; }
        public bool ReplaceValue { get; set; }
        public bool DefaultSettings { get; set; }
        public bool DropItems { get; set; }

        public override void OnDisable()
        {
            this.Info("ReplacingDisconnecteds hasn't loaded :'(");
        }

		public override void OnEnable()
		{
			this.Info("ReplacingDisconnecteds has loaded <3");
        }

        public override void Register()
		{
            //Events
            this.AddEventHandlers(new Events(this));
            //Configs
            AddConfig(new ConfigSetting("sod_force_replace", "false", SettingType.STRING, true, "Force setting replace for all users"));
            AddConfig(new ConfigSetting("sod_force_replace_value", "false", SettingType.STRING, true, "Force this value for all players, if sod_force_setting"));
            AddConfig(new ConfigSetting("sod_dropitems", "true", SettingType.STRING, true, "Drop items if there are no spectators available"));
            AddConfig(new ConfigSetting("sod_default_setting", "true", SettingType.STRING, true, "Default configuration for users who haven't changed it"));
        }

        public void RefreshConfig()
        {
            ForceReplace = GetConfigBool("sod_force_replace");
            ReplaceValue = GetConfigBool("sod_force_replace_value");
            DefaultSettings = GetConfigBool("sod_default_setting");
            DropItems = GetConfigBool("sod_dropitems");
        }
    }
}
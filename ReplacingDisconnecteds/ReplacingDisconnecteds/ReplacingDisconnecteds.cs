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
		SmodMinor = 4,
		SmodRevision = 0,
		configPrefix = "sod"
		)]

	public class ReplacingDisconnecteds : Plugin
	{
		[ConfigOption]
		public bool allowUserChoice = true; // While disabled force setting replace for all users

		[ConfigOption]
		public bool forceValue = false; // Force this value for all players, while sod_allow_user_choice is false

		[ConfigOption]
		public bool dropitems = true; // Drop items if there are no spectators available

		[ConfigOption]
		public bool defaultSetting = true; // Default configuration for users who haven't changed it
		
		[ConfigOption]
		public bool allowTutorialReplace = false; // Allow tutorial players to be replaced

		[ConfigOption]
		public bool broadcastPlayer = true; // If the player should get notified when he respawns

		[ConfigOption]
		public string broadcastmsg = "You replaced a disconnected player"; // Message to be broadcasted to the player when replacing a disconnected player

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
		}
	}
}
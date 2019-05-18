using Smod2;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReplacingDisconnecteds
{
	class Events : IEventHandlerDisconnect, IEventHandlerCallCommand, IEventHandlerWaitingForPlayers
	{
		public readonly string directory = "ReplacingDisconnecteds";

		private ReplacingDisconnecteds plugin;

		public Events(ReplacingDisconnecteds plugin)
		{
			this.plugin = plugin;
		}

		public void OnDisconnect(DisconnectEvent ev)
		{
			if (!plugin.AllowUserChoice && !plugin.DropItems && !plugin.ForceValue) return;

			List<Player> Players = PluginManager.Manager.Server.GetPlayers();
			Player SpectatorPlayer = SearchSpectator(Players);
			Player DisconnectedPlayer = SearchDisconnectedPlayer(Players);
			
			//Cannot find disconnected player
			if (DisconnectedPlayer == null) return;

			if (SpectatorPlayer == null)
			{
				//Drop items
				if (plugin.DropItems) DropItems(DisconnectedPlayer);
			}
			else
			{
				//Replace player
				ReplaceSpectator(SpectatorPlayer, DisconnectedPlayer);
			}
		}

		public void OnCallCommand(PlayerCallCommandEvent ev)
		{
			string command = ev.Command.ToLower();

			switch (command)
			{
				case "sod enable":
					ev.ReturnMessage = EnablePlayerReplace(ev.Player);
					break;
				case "sod disable":
					ev.ReturnMessage = DisablePlayerReplace(ev.Player);
					break;
			}
		}

		public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
		{
			plugin.RefreshConfig();
		}

		public void ReplaceSpectator(Player spectator, Player disconnected)
		{
			spectator.ChangeRole(disconnected.TeamRole.Role, false, false, false, false);
			spectator.SetHealth(disconnected.GetHealth(), DamageType.WALL);
			spectator.Teleport(disconnected.GetPosition(), true);
			foreach (Item item in disconnected.GetInventory())
			{
				spectator.GiveItem(item.ItemType);
			}
			spectator.SetAmmo(AmmoType.DROPPED_5, disconnected.GetAmmo(AmmoType.DROPPED_5));
			spectator.SetAmmo(AmmoType.DROPPED_7, disconnected.GetAmmo(AmmoType.DROPPED_7));
			spectator.SetAmmo(AmmoType.DROPPED_9, disconnected.GetAmmo(AmmoType.DROPPED_9));
		}

		public void DropItems(Player disconnected)
		{
			foreach (Item item in disconnected.GetInventory())
			{
				PluginManager.Manager.Server.Map.SpawnItem(item.ItemType, disconnected.GetPosition(), new Vector(0, 0, 0));
			}
		}

		public Player SearchDisconnectedPlayer(List<Player> PlayerList)
		{
			foreach (Player player in PlayerList)
			{
				if (player.IpAddress == "" || player.IpAddress == null)
				{
					if (player.TeamRole.Role == Role.SPECTATOR) continue;
					if (player.TeamRole.Role == Role.TUTORIAL && !plugin.AllowTutorialReplace) continue;
					return player;
				}
			}
			return null;
		}

		public Player SearchSpectator(List<Player> PlayerList)
		{
			Random rnd = new Random();

			foreach (Player player in PlayerList.OrderBy((item) => rnd.Next()))
			{
				if (player.TeamRole.Role == Role.SPECTATOR && !player.OverwatchMode)
				{
					if (!plugin.AllowUserChoice)
					{
						if (plugin.ForceValue == true) return player;
					}
					else
					{
						if (ReadUserConfig(player) == true) return player;
					}
				}
			}
			return null;
		}

		public bool ReadUserConfig(Player player)
		{
			//Default server config
			if (!File.Exists($"{directory}/{player.SteamId}.txt")) return plugin.DefaultSettings;

			//Specific user config
			return File.ReadAllText($"{directory}/{player.SteamId}.txt") == "true";
		}

		public string EnablePlayerReplace(Player player)
		{
			if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
			File.WriteAllText($"{directory}/{player.SteamId}.txt", "true");
			return "Replacing disconnected player on";
		}

		public string DisablePlayerReplace(Player player)
		{
			if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
			File.WriteAllText($"{directory}/{player.SteamId}.txt", "false");
			return "Replacing disconnected player off";
		}
	}
}
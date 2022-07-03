﻿using BepInEx;
using RoR2;
using UnityEngine.SceneManagement;
using DiscordRPC;
using DiscordRPC.Message;
using DiscordRPC.Unity;
using UnityEngine;
using System;
using R2API.Utils;
using System.Collections.Generic;
using BepInEx.Configuration;
using BepInEx.Logging;
using static DiscordRichPresence.DiscordRichPresencePlugin;

namespace DiscordRichPresence.Utils
{
    public static class PresenceUtils
    {
		public static void SetStagePresence(DiscordRpcClient client, RichPresence richPresence, SceneDef scene, Run run, bool isPaused, TeleporterStatus whatToShow = TeleporterStatus.None)
		{
			string sceneImageKey = scene.baseSceneName;
			if (sceneImageKey.StartsWith("it"))
            {
				sceneImageKey = sceneImageKey.Substring(2);
            }
			richPresence.Assets.LargeImageKey = sceneImageKey;
			richPresence.Assets.LargeImageText = "DiscordRichPresence v" + Instance.Info.Metadata.Version; //Language.GetString(scene.subtitleToken);

			richPresence.State = string.Format("Stage {0} - {1}", run.stageClearCount + 1, Language.GetString(scene.nameToken));
			if (CurrentSimulacrumWave > 0)
            {
				richPresence.State = string.Format("Wave {0} - {1}", CurrentSimulacrumWave, Language.GetString(scene.nameToken));
			}

			if (MoonDetonationController == null)
            {
				richPresence.Details = InfoTextUtils.GetDifficultyString(run.selectedDifficulty);
				if (whatToShow == TeleporterStatus.Boss && CurrentBoss != "None")
				{
					richPresence.Details = "Fighting " + CurrentBoss + " | " + InfoTextUtils.GetDifficultyString(run.selectedDifficulty);
				}
				else if (whatToShow == TeleporterStatus.Charge && CurrentChargeLevel > 0)
				{
					richPresence.Details = "Charging teleporter (" + CurrentChargeLevel * 100 + "%) | " + InfoTextUtils.GetDifficultyString(run.selectedDifficulty);
				}

				richPresence.Timestamps = new Timestamps(); // Clear timestamps
				if (scene.sceneType == SceneType.Stage && !isPaused)
				{
					richPresence.Timestamps.StartUnixMilliseconds = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds() - (ulong)run.GetRunStopwatch();
				}
			}
			else
            {
				richPresence.Details = "Escaping! | " + InfoTextUtils.GetDifficultyString(run.selectedDifficulty);
				richPresence.Timestamps.EndUnixMilliseconds = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds() + (ulong)MoonDetonationController.countdownDuration;
			}

			DiscordRichPresencePlugin.RichPresence = richPresence;
			client.SetPresence(richPresence);
		}

		public static void SetMainMenuPresence(DiscordRpcClient client, RichPresence richPresence, string details = "")
		{
            richPresence.Assets = new Assets
            {
                LargeImageKey = "riskofrain2", //lobby
                LargeImageText = "DiscordRichPresence v" + Instance.Info.Metadata.Version
            };

            richPresence.Details = PluginConfig.MainMenuIdleMessageEntry.Value;
			if (details != "")
            {
				richPresence.Details = details;
            }
			richPresence.Timestamps = new Timestamps(); // Clear timestamps

			richPresence.State = "In Lobby";
			if (!Facepunch.Steamworks.Client.Instance.Lobby.IsValid)
            {
				richPresence.State = "In Menu";
				richPresence.Secrets = new Secrets();
				richPresence.Party = new Party(); // Clear secrets and party
			}

			DiscordRichPresencePlugin.RichPresence = richPresence;
			client.SetPresence(richPresence);
		}

		public static void SetLobbyPresence(DiscordRpcClient client, RichPresence richPresence, Facepunch.Steamworks.Client faceClient)
		{
			richPresence.State = "In Lobby";
			richPresence.Details = "Preparing";
            richPresence.Assets = new Assets
            {
                LargeImageKey = "riskofrain2", //lobby
                LargeImageText = "DiscordRichPresence v" + Instance.Info.Metadata.Version
            };

            richPresence.Party.ID = faceClient.Username;
			richPresence.Party.Max = faceClient.Lobby.MaxMembers;
			richPresence.Party.Size = faceClient.Lobby.NumMembers;

			if (PluginConfig.AllowJoiningEntry.Value)
			{
				richPresence.Secrets.JoinSecret = faceClient.Lobby.CurrentLobby.ToString();
			}

			DiscordRichPresencePlugin.RichPresence = richPresence;
			client.SetPresence(richPresence);
		}
	}
}
﻿/*using RoR2;
using System;
using System.Collections;
using DiscordRichPresence.Utils;
using static DiscordRichPresence.DiscordRichPresencePlugin;

namespace DiscordRichPresence.Hooks
{
    public static class RoR2Hooks
    {
        public static void AddHooks()
        {
            CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
            CharacterBody.onBodyDestroyGlobal += CharacterBody_onBodyDestroyGlobal;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
            On.RoR2.TeleporterInteraction.FixedUpdate += TeleporterInteraction_FixedUpdate;
            On.RoR2.EscapeSequenceController.SetCountdownTime += EscapeSequenceController_SetCountdownTime;
            On.RoR2.InfiniteTowerRun.BeginNextWave += InfiniteTowerRun_BeginNextWave;
            On.RoR2.UI.MainMenu.BaseMainMenuScreen.OnEnter += BaseMainMenuScreen_OnEnter;
        }

        private static void Stage_onStageStartGlobal(Stage obj)
        {
            CharacterBody localBody = LocalUserManager.GetFirstLocalUser()?.cachedMasterController?.master?.GetBody(); // Don't know what exactly throws a null ref here so we'll just go all in on null checks
            if (localBody == null)
            {
                return;
            }

            LoggerEXT.LogInfo("LocalBodyBaseName: " + localBody.baseNameToken); //!!!USE THIS!!!
            RichPresence.Assets.SmallImageKey = InfoTextUtils.GetCharacterInternalName(localBody.GetDisplayName());
            RichPresence.Assets.SmallImageText = localBody.GetDisplayName();
            Client.SetPresence(RichPresence);
            PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, Run.instance);
        }

        public static void RemoveHooks()
        {
            CharacterBody.onBodyStartGlobal -= CharacterBody_onBodyStartGlobal;
            CharacterBody.onBodyDestroyGlobal -= CharacterBody_onBodyDestroyGlobal;
            Stage.onStageStartGlobal -= Stage_onStageStartGlobal;
            On.RoR2.TeleporterInteraction.FixedUpdate -= TeleporterInteraction_FixedUpdate;
            On.RoR2.EscapeSequenceController.SetCountdownTime -= EscapeSequenceController_SetCountdownTime;
            On.RoR2.InfiniteTowerRun.BeginNextWave -= InfiniteTowerRun_BeginNextWave;
            On.RoR2.UI.MainMenu.BaseMainMenuScreen.OnEnter -= BaseMainMenuScreen_OnEnter;
        }

        private static void CharacterBody_onBodyStartGlobal(CharacterBody obj)
        {
            if (obj.isChampion)
            {
                CurrentBoss = obj.GetDisplayName();
                PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, Run.instance);
            }
        }

        private static void CharacterBody_onBodyDestroyGlobal(CharacterBody obj)
        {
            if (obj.isChampion && Run.instance != null)
            {
                CurrentBoss = "";
                PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, Run.instance);
            }
        }

        // We use this method because it provides a robust update system that updates only when we need it to; that is, when the teleporter is active and charging
        // Additionally, comparing with CurrentChargeLevel prevents unnecessary presence updates (which would lead to ratelimiting)
        private static void TeleporterInteraction_FixedUpdate(On.RoR2.TeleporterInteraction.orig_FixedUpdate orig, TeleporterInteraction self)
        {
            if (Math.Round(self.chargeFraction, 2) != CurrentChargeLevel && PluginConfig.TeleporterStatusEntry.Value == PluginConfig.TeleporterStatus.Charge)
            {
                CurrentChargeLevel = (float)Math.Round(self.chargeFraction, 2);
                PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, Run.instance);
            }

            orig(self);
        }

        private static void EscapeSequenceController_SetCountdownTime(On.RoR2.EscapeSequenceController.orig_SetCountdownTime orig, EscapeSequenceController self, double secondsRemaining)
        {
            MoonCountdownTimer = (float)secondsRemaining + 1;
            PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, Run.instance);

            orig(self, secondsRemaining);
        }

        //Simulacrum
        private static void InfiniteTowerRun_BeginNextWave(On.RoR2.InfiniteTowerRun.orig_BeginNextWave orig, InfiniteTowerRun self)
        {
            PresenceUtils.SetStagePresence(Client, RichPresence, CurrentScene, self);

            orig(self);
        }

        private static void BaseMainMenuScreen_OnEnter(On.RoR2.UI.MainMenu.BaseMainMenuScreen.orig_OnEnter orig, RoR2.UI.MainMenu.BaseMainMenuScreen self, RoR2.UI.MainMenu.MainMenuController mainMenuController)
        {
            if (Facepunch.Steamworks.Client.Instance.Lobby.IsValid) // Messy if-else, but the goal is that when exiting a multiplayer game to the menu, it will display the lobby presence instead of the main menu presence
            {
                PresenceUtils.SetLobbyPresence(Client, RichPresence, Facepunch.Steamworks.Client.Instance);
            }
            else if (IsInEOSLobby)
            {
                PresenceUtils.SetLobbyPresence(Client, RichPresence, EOSLobbyManager.GetFromPlatformSystems());
            }
            else
            {
                PresenceUtils.SetMainMenuPresence(Client, RichPresence);
            }

            orig(self, mainMenuController);
        }
    }
}*/
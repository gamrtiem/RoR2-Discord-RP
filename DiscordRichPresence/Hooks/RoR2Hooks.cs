using RoR2;
using System;
using System.IO;
using DiscordRichPresence.Utils;
using UnityEngine;
using static DiscordRichPresence.DiscordRichPresencePlugin;
using System.Collections.Generic;
using System.Net.Http;

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
            On.RoR2.Run.OnClientGameOver += Run_OnClientGameOver;
        }

        private static void Stage_onStageStartGlobal(Stage obj)
        {
            CharacterBody localBody = LocalUserManager.GetFirstLocalUser()?.cachedMasterController?.master?.GetBody(); // Don't know what exactly throws a null ref here so we'll just go all in on null checks
            if (localBody == null)
            {
                return;
            }

            LoggerEXT.LogInfo("LocalBodyBaseName: " + localBody.baseNameToken); //!!!USE THIS!!!
            LoggerEXT.LogInfo("LocalBodyBaseName: " + InfoTextUtils.GetCharacterInternalName(localBody.GetDisplayName())); //for testing :3 
            
            var richPresence = RichPresence;
            richPresence.Assets.SmallImage = "https://raw.githubusercontent.com/mikhailmikhalchuk/RoR2-Discord-RP/refs/heads/master/Assets/Characters/" + InfoTextUtils.GetCharacterInternalName(localBody.GetDisplayName()) + ".png";
            richPresence.Assets.SmallText = localBody.GetDisplayName();
            
            var activityManager = Client.GetActivityManager();
            activityManager.UpdateActivity(richPresence, (result =>
            {
                LoggerEXT.LogInfo("activity updated, " + result);
            }));
            PresenceUtils.SetStagePresence(Client, richPresence, CurrentScene, Run.instance);

            var tex = localBody.GetComponent<CharacterBody>().portraitIcon;
            
            RenderTexture tmp = RenderTexture.GetTemporary( 
                tex.width,
                tex.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);
            
            Graphics.Blit(tex, tmp);
// Backup the currently set RenderTexture
            RenderTexture previous = RenderTexture.active;
// Set the current RenderTexture to the temporary one we created
            RenderTexture.active = tmp;
// Create a new readable Texture2D to copy the pixels to it
            Texture2D newTex = new Texture2D(tex.width, tex.height);
// Copy the pixels from the RenderTexture to the new Texture
            newTex.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
            newTex.Apply();
// Reset the active RenderTexture
            RenderTexture.active = previous;
// Release the temporary RenderTexture
            RenderTexture.ReleaseTemporary(tmp);

            byte[] bytes = newTex.EncodeToPNG();
            var dirPath = Application.dataPath + "/../";
            if(!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }
            File.WriteAllBytes(dirPath + "Image" + ".png", bytes);
            
            LoggerEXT.LogInfo(dirPath);

            UploadToCatbox(dirPath + "Image" + ".png");
            
            

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
                PresenceUtils.SetStagePresence(DiscordRichPresencePlugin.Client, RichPresence, CurrentScene, Run.instance);
            }
        }

        private static void CharacterBody_onBodyDestroyGlobal(CharacterBody obj)
        {
            if (obj.isChampion && Run.instance != null)
            {
                CurrentBoss = "";
                PresenceUtils.SetStagePresence(DiscordRichPresencePlugin.Client, RichPresence, CurrentScene, Run.instance);
            }
        }

        // We use this method because it provides a robust update system that updates only when we need it to; that is, when the teleporter is active and charging
        // Additionally, comparing with CurrentChargeLevel prevents unnecessary presence updates (which would lead to ratelimiting)
        private static void TeleporterInteraction_FixedUpdate(On.RoR2.TeleporterInteraction.orig_FixedUpdate orig, TeleporterInteraction self)
        {
            if (Math.Round(self.chargeFraction, 2) != CurrentChargeLevel && PluginConfig.TeleporterStatusEntry.Value == PluginConfig.TeleporterStatus.Charge)
            {
                CurrentChargeLevel = (float)Math.Round(self.chargeFraction, 2);
                PresenceUtils.SetStagePresence(DiscordRichPresencePlugin.Client, RichPresence, CurrentScene, Run.instance);
            }

            orig(self);
        }

        private static void EscapeSequenceController_SetCountdownTime(On.RoR2.EscapeSequenceController.orig_SetCountdownTime orig, EscapeSequenceController self, double secondsRemaining)
        {
            MoonCountdownTimer = (float)secondsRemaining + 1;
            PresenceUtils.SetStagePresence(DiscordRichPresencePlugin.Client, RichPresence, CurrentScene, Run.instance);

            orig(self, secondsRemaining);
        }

        //Simulacrum
        private static void InfiniteTowerRun_BeginNextWave(On.RoR2.InfiniteTowerRun.orig_BeginNextWave orig, InfiniteTowerRun self)
        {
            PresenceUtils.SetStagePresence(DiscordRichPresencePlugin.Client, RichPresence, CurrentScene, self);

            orig(self);
        }

        private static void BaseMainMenuScreen_OnEnter(On.RoR2.UI.MainMenu.BaseMainMenuScreen.orig_OnEnter orig, RoR2.UI.MainMenu.BaseMainMenuScreen self, RoR2.UI.MainMenu.MainMenuController mainMenuController)
        {
            if (Facepunch.Steamworks.Client.Instance.Lobby.IsValid) // Messy if-else, but the goal is that when exiting a multiplayer game to the menu, it will display the lobby presence instead of the main menu presence
            {
                PresenceUtils.SetLobbyPresence(DiscordRichPresencePlugin.Client, RichPresence, Facepunch.Steamworks.Client.Instance);
            }
            else if (IsInEOSLobby)
            {
                PresenceUtils.SetLobbyPresence(DiscordRichPresencePlugin.Client, RichPresence, EOSLobbyManager.GetFromPlatformSystems());
            }
            else
            {
                PresenceUtils.SetMainMenuPresence(DiscordRichPresencePlugin.Client, RichPresence);
            }

            orig(self, mainMenuController);
        }

        private static void Run_OnClientGameOver(On.RoR2.Run.orig_OnClientGameOver orig, Run self, RunReport runReport)
        {
            orig(self, runReport);
            if (Run.instance != null && CurrentScene != null)
            {
                PresenceUtils.SetStagePresence(DiscordRichPresencePlugin.Client, RichPresence, CurrentScene, Run.instance, true);
            }
            var richPresence = RichPresence;
            
            TimeSpan time = TimeSpan.FromSeconds((long)self.GetRunStopwatch());

            if ((long)self.GetRunStopwatch() > 60 * 60) // is it uhh longer then an hour 
            {
                richPresence.State = "Defeat! " +  time.ToString(@"hh\:mm\:ss") + " - " + richPresence.State;
            }
            else
            {
                richPresence.State = "Defeat! " +  time.ToString(@"mm\:ss") + " - " + richPresence.State;
            }
            var activityManager = DiscordRichPresencePlugin.Client.ActivityManagerInstance;
            activityManager.UpdateActivity(richPresence, (result =>
            {
                LoggerEXT.LogInfo("activity updated, " + result);
            }));
            
            
        }
    
        // thanks https://github.com/Metalloriff/csharp-catbox-upload !!
        private static async void UploadToCatbox(string filePath)
        {   
            using var http = new HttpClient();
            var file = File.OpenRead(filePath);
            
            // Create the HttpContent for the form to be posted.
            var requestContent = new FormUrlEncodedContent([
                new KeyValuePair<string, string>("reqtype", "fileupload"),
                new KeyValuePair<string, string>("fileToUpload", filePath)
                //new KeyValuePair<string, string>("reqtype", "fileupload")
            ]);

// Get the response.
            var response = await http.PostAsync("https://catbox.moe/user/api.php", requestContent);

// Get the response content.
            var responseContent = response.Content;

// Get the stream of the content.
using var reader = new StreamReader(await responseContent.ReadAsStreamAsync());
// Write the output.
LoggerEXT.LogInfo(await reader.ReadToEndAsync());
/*using var client = new HttpClient();
            client.BaseAddress = new Uri("https://catbox.moe/user/api.php");

            using var content = new MultipartFormDataContent();
			
            content.Add(new StringContent("fileupload"), "reqtype");
            content.Add(new StreamContent(File.OpenRead(filePath)), "fileToUpload", Path.GetFileName(filePath));

            LoggerEXT.LogInfo(content);
            var response = await client.PostAsync("https://catbox.moe/user/api.php", content);
            

            if (response.IsSuccessStatusCode)
            {
                var url = await response.Content.ReadAsStringAsync();
                LoggerEXT.LogInfo("upload to Catbox: " + url);

                var richPresence = RichPresence;
                richPresence.Assets.SmallImage = url;
                var activityManager = Client.ActivityManagerInstance; 
                activityManager.UpdateActivity(richPresence, (result =>
                {
                    LoggerEXT.LogInfo("activity updated, " + url);
                }));
            }
            else
            {
                // Handle your failed requests here
            }*/
        }
        
        
        
    }
}
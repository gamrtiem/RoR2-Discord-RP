using System;
using System.Collections.Generic;

namespace DiscordRichPresence.Utils
{
    public static class InfoTextUtils
    {
        public static List<string> CharactersWithAssets = new List<string>()
        {
            "anarbiter",
            "BANDIT2_BODY_NAME",
            "belmont",
            "bomber",
            "cadet",
            "CAPTAIN_BODY_NAME",
            "celestialwartank",
            "CHEF_BODY_NAME",
            "Chef",
            "chirr",
            "chronolegionnaire",
            "COMMANDO_BODY_NAME",
            "cosmicchampion",
            "CROCO_BODY_NAME",
            "custodian",
            "cyborg",
            "dancer",
            "deputy",
            "desolator",
            "driver",
            "enforcer",
            "ENGI_BODY_NAME",
            "executioner",
            "FALSESON_BODY_NAME",
            "han-d",
            "HERETIC_BODY_NAME",
            "HUNTRESS_BODY_NAME",
            "interrogator",
            "johnny",
            "LOADER_BODY_NAME",
            "MAGE_BODY_NAME",
            "matchmaker",
            "MERC_BODY_NAME",
            "miner",
            "MOFFEIN_ROCKET_BODY_NAME",
            "mortician",
            "nemesiscommando",
            "nemesisenforcer",
            "nemesismercenary",
            "nucleator",
            "paladin",
            "pathfinder",
            "pilot",
            "pyro",
            "RAILGUNNER_BODY_NAME",
            "ranger",
            "ravager",
            "redmist",
            "rifter",
            "robomando",
            "scout",
            "seamstress",
            "SEEKER_BODY_NAME",
            "sniper",
            "sonic",
            "sorceress",
            "spy",
            "submariner",
            "teslatrooper",
            "TOOLBOT_BODY_NAME",
            "TREEBOT_BODY_NAME",
            "unknown",
            "VOIDSURVIVOR_BODY_NAME",
            "vol-t",
            "wanderer",
        };
        
        public static List<string> StagesWithAssets = new List<string>()
        {
            //you can copy past this from a bash oneliner             for file in *.png; do echo "\"${file%.png}\","; done
            "agatevillage", //non nametoken ones should only be used as fallbacks
            "BulwarksHaunt_GhostWave",
            "catacombs_DS1_Catacombs",
            "drybasin",
            "FBLScene",
            "forgottenhaven",
            "riskofrain2",
            "slumberingsatellite",
            "sm64_bbf_SM64_BBF",
            "MAP_ANCIENTLOFT_TITLE", //vanilla maps 
            "MAP_ARENA_TITLE",
            "MAP_ARTIFACTWORLD_TITLE",
            "MAP_BAZAAR_TITLE",
            "MAP_BLACKBEACH_TITLE",
            "MAP_DAMPCAVE_TITLE",
            "MAP_FOGGYSWAMP_TITLE",
            "MAP_FROZENWALL_TITLE",
            "MAP_GOLDSHORES_TITLE",
            "MAP_GOLEMPLAINS_TITLE",
            "MAP_GOOLAKE_TITLE",
            "MAP_HABITATFALL_TITLE",
            "MAP_HABITAT_TITLE",
            "MAP_HELMINTHROOST_TITLE",
            "MAP_itancientloft_NAME",
            "MAP_itdampcave_NAME",
            "MAP_itfrozenwall_NAME",
            "MAP_itgolemplains_NAME",
            "MAP_itgoolake_NAME",
            "MAP_itmoon_NAME",
            "MAP_itskymeadow_NAME",
            "MAP_LAKESNIGHT_TITLE",
            "MAP_LAKES_TITLE",
            "MAP_LEMURIANTEMPLE_TITLE",
            "MAP_LIMBO_TITLE",
            "MAP_MERIDIAN_TITLE",
            "MAP_MOON_TITLE",
            "MAP_MYSTERYSPACE_TITLE",
            "MAP_ROOTJUNGLE_TITLE",
            "MAP_SHIPGRAVEYARD_TITLE",
            "MAP_SKYMEADOW_TITLE",
            "MAP_SNOWYFOREST_TITLE",
            "MAP_SULFURPOOLS_TITLE",
            "MAP_VILLAGENIGHT_TITLE",
            "MAP_VILLAGE_TITLE",
            "MAP_VOIDRAID_TITLE",
            "MAP_VOIDSTAGE_TITLE",
            "MAP_WISPGRAVEYARD_TITLE",
            "SNOWTIME_MAP_BLOODGULCH_0", // snowtime stages
            "SNOWTIME_MAP_CITY_0",
            "SNOWTIME_MAP_DEATHISLAND_0",
            "SNOWTIME_MAP_DHALO_0",
            "SNOWTIME_MAP_FLAT_0",
            "SNOWTIME_MAP_GPH_0",
            "SNOWTIME_MAP_HALO_0",
            "SNOWTIME_MAP_HC_0",
            "SNOWTIME_MAP_HIGHTOWER_0",
            "SNOWTIME_MAP_IF_0",
            "SNOWTIME_MAP_NMB_0",
            "SNOWTIME_MAP_SHRINE_0",
            "SNOWTIME_MAP_SW_0",
            "SNOWTIME_MsAP_GMC_0",
        };
        public enum StyleTag : byte
        {
            Damage = 1,
            Healing = 2,
            Utility = 3,
            Health = 4,
            Stack = 5,
            Mono = 6,
            Death = 7,
            UserSetting = 8,
            Artifact = 9,
            Sub = 10,
            Event = 11,
            WorldEvent = 12,
            KeywordName = 13,
            Shrine = 14
        }

        public static string GetCharacterInternalName(string name)
        {
            if (CharactersWithAssets.Contains(name))
            {
                return CharactersWithAssets.Find(c => c == name).ToLower().Replace(" ", "");
            }
            return "unknown";
        }

        public static string FormatTextStyleTag(string content, StyleTag styleTag)
        {
            string tagString;
            if ((byte)styleTag >= 1 && (byte)styleTag <= 4)
            {
                tagString = "cIs" + styleTag.ToString();
            }
            else
            {
                tagString = "c" + styleTag.ToString();
            }
            return $"<style={tagString}>{content}</style>";
        }
    }
}
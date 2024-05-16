using System.ComponentModel;
using Exiled.API.Interfaces;

namespace SCP096AntiLoneliness
{
    public class Config : IConfig
    {
        // Required
        public bool IsEnabled { get; set; } = true;
        
        [Description("How long SCP-096 should stay enraged (can be decimals)")]
        public bool Debug { get; set; } = false;

        // Other...
        [Description("How long SCP-096 should stay enraged (can be decimals)")]
        public float EnrageTime { get; set; } = 10f;
        
        [Description("How long the enragement is delayed when using the command")]
        public float RageCommandDelay { get; set; } = 2f;
        
        [Description("Enrage cooldown")] 
        public float RageCooldown { get; set; } = 30f;
        
        [Description("(Leave blank to disable) The broadcast message shown to the player as they spawn as SCP-096 to inform them of the command.")] 
        public string NoticeBroadcast { get; set; } = "<br><size=35><color=red><b>[ Anti-Loneliness ]</b></color><br><size=25>You can enrage yourself at any time for {RAGE_LENGTH} seconds<br>using the <color=orange>.enrage</color> (or <color=orange>.er</color>) command in your console.</size>";
    }
}
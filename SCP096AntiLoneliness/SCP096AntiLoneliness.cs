// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Globalization;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using Exiled.Loader;
using PlayerRoles;
using SCP096AntiLoneliness.Commands;
using Server = Exiled.Events.Handlers.Server;

namespace SCP096AntiLoneliness
{
    public class SCP096AntiLoneliness : Plugin<Config>
    {
        // Instance Tracking
        public static SCP096AntiLoneliness Instance = null!;

        // Plugin Configuration
        public override string Name => "SCP-096 Anti-Loneliness"; // The name of the plugin
        public override string Prefix => "scp096_anti_loneliness"; // As appears in configuration file
        public override string Author => "ASIGSEGV"; // Author name
        public override Version Version => new Version(1, 0, 0); // Version
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override Version RequiredExiledVersion { get; } = new Version(8, 8, 1); // Public Plugin Requirement

        // Sub-Instances Here
        public List<Player> CooldownTracker = new List<Player>();

        // Methods
        public override void OnEnabled()
        {
            Instance = this;
            RegisterEvents();
            Log.Info("Done registering events.");
            // Public Plugin Requirement
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            Log.Info("Done unregistering events.");
            // Public Plugin Requirement
            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            // Register Events Here
            Server.RestartingRound += Enrage.ResetEnvironmentData;
            Exiled.Events.Handlers.Player.ChangingRole += (e) =>
            {
                // notify player
                if(e.NewRole != RoleTypeId.Scp096) return;
                if (Config.NoticeBroadcast.IsEmpty()) return;
                e.Player.Broadcast(10, Config.NoticeBroadcast.Replace("{RAGE_LENGTH}",Config.EnrageTime.ToString(CultureInfo.CurrentCulture)));
            };
        }

        private void UnregisterEvents()
        {
            // UnRegister Events Here
            Server.RestartingRound -= Enrage.ResetEnvironmentData;
        }
    }
}
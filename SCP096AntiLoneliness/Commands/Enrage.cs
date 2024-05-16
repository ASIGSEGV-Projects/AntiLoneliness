using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp096;
using RemoteAdmin;
using UnityEngine.Rendering;
using Player = Exiled.API.Features.Player;
using Scp096Role = Exiled.API.Features.Roles.Scp096Role;

namespace SCP096AntiLoneliness.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Enrage : ICommand
    {
        public string Command { get; } = "enrage";
        public string[] Aliases { get; } = { "enrage","rage", "er" };
        public string Description { get; } = "Enrage as SCP-096 without someone looking at you.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            // Player-Only
            if (!(sender is PlayerCommandSender))
            {
                response = "Only a player can execute this command!";
                return false;
            }

            // Get Player
            Player plr = Player.Get(sender);
            if (plr.Role != RoleTypeId.Scp096)
            {
                response = "You're not SCP096!";
                return false;
            }
            // Get SCP-096 Component
            Scp096Role role = (Scp096Role) plr.Role;
            if (role.RageState != Scp096RageState.Docile)
            {
                response = "You're not docile!";
                return false;
            }

            if (SCP096AntiLoneliness.Instance.CooldownTracker.Contains(plr))
            {
                response =
                    $"You're still on cooldown. The cooldown lasts for {SCP096AntiLoneliness.Instance.Config.RageCooldown} seconds.";
                return false;
            }
            RoundCallDelayed(SCP096AntiLoneliness.Instance.Config.RageCommandDelay, () =>
            {
                if(plr.IsConnected==false||plr.Role!=RoleTypeId.Scp096) return;
                SCP096AntiLoneliness.Instance.CooldownTracker.Add(plr);
                Log.Debug($"{plr.Nickname} used enrage command to self-enrage.");
                role.Enrage(SCP096AntiLoneliness.Instance.Config.EnrageTime);
                // Remove Cooldown
                RoundCallDelayed(SCP096AntiLoneliness.Instance.Config.RageCooldown, ()=>RemoveCooldown(plr));
            });
            response = $"You will be enrage in {SCP096AntiLoneliness.Instance.Config.RageCommandDelay} seconds!";
            return true;
        }

        // Timing
        private static List<CoroutineHandle> handles = new List<CoroutineHandle>();

        private static void RoundCallDelayed(float time, Action action)
        {
            handles.Add(Timing.CallDelayed(time,action));
        }

        public static void ResetEnvironmentData()
        {
            Log.Debug($"Resetting round, environment data will be reset.");
            // Reset Cooldown tracker
            SCP096AntiLoneliness.Instance.CooldownTracker = new List<Player>();
            // Delete Coroutines
            Timing.KillCoroutines(handles.ToArray());
            handles = new List<CoroutineHandle>();
        }

        public static void RemoveCooldown(Player plr)
        {
            if(!SCP096AntiLoneliness.Instance.CooldownTracker.Contains(plr)) return;
            SCP096AntiLoneliness.Instance.CooldownTracker.Remove(plr);
        }
    }
}
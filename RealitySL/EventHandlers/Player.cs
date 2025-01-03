using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Features.Doors;
using Exiled.Events.Commands.Reload;
using Exiled.Events.EventArgs.Player;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Pickups;
using MEC;
using Mirror;
using RealitySL.Classes;
using RealitySL.Components;
using UnityEngine;

using static RealitySL.Variables.ServerManagers;

namespace RealitySL.EventHandlers
{
    public static class PlayerEvents
    {
        public static void OnVerified(VerifiedEventArgs ev)
        {
            if (!ev.Player.IsNPC)
            {
                PlayerStatuses.Add(ev.Player, new PlayerStatus
                {
                    IsSitDown = false,
                    IsChangingSitDownState = false
                });
            }
        }

        public static void OnLeft(LeftEventArgs ev)
        {
            if (!ev.Player.IsNPC)
            {
                if (PlayerStatuses.ContainsKey(ev.Player))
                    PlayerStatuses.Remove(ev.Player);
            }
        }

        public static void OnChaningRole(ChangingRoleEventArgs ev)
        {
            PlayerStatuses[ev.Player] = new PlayerStatus();

            ev.Player.Scale = new Vector3(1, 1, 1);
        }

        public static IEnumerator<float> OnTogglingNoClip(TogglingNoClipEventArgs ev)
        {
            if (!PlayerStatuses[ev.Player].IsChangingSitDownState && !ev.Player.IsJumping && !ev.Player.IsNoclipPermitted)
            {
                PlayerStatuses[ev.Player].IsChangingSitDownState = true;

                if (PlayerStatuses[ev.Player].IsSitDown)
                {
                    while (ev.Player.Scale.y >= 0.65f)
                    {
                        ev.Player.Scale = new Vector3(1, ev.Player.Scale.y - 0.01f, 1);

                        yield return Timing.WaitForOneFrame;
                    }

                    ev.Player.EnableEffect(EffectType.Slowness, 50);
                    ev.Player.EnableEffect(EffectType.SilentWalk, 5);

                    PlayerStatuses[ev.Player].IsSitDown = false;
                    PlayerStatuses[ev.Player].IsChangingSitDownState = false;
                }
                else
                {
                    while (ev.Player.Scale.y <= 1)
                    {
                        ev.Player.Scale = new Vector3(1, ev.Player.Scale.y + 0.01f, 1);

                        yield return Timing.WaitForOneFrame;
                    }

                    ev.Player.DisableEffect(EffectType.Slowness);
                    ev.Player.DisableEffect(EffectType.SilentWalk);

                    PlayerStatuses[ev.Player].IsSitDown = true;
                    PlayerStatuses[ev.Player].IsChangingSitDownState = false;
                }
            }
        }
    }
}

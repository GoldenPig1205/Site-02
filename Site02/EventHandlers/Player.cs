using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.Commands.Reload;
using Exiled.Events.EventArgs.Player;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Pickups;
using MEC;
using Mirror;
using PlayerRoles;
using Site02.Classes;
using Site02.Components;
using UnityEngine;

using static Site02.Variables.ServerManagers;

namespace Site02.EventHandlers
{
    public static class PlayerEvents
    {
        public static void OnVerified(VerifiedEventArgs ev)
        {
            if (Round.IsLobby)
            {
                IEnumerator<float> loop()
                {
                    while (Round.IsLobby)
                    {
                        ev.Player.ShowHint($"\n\n\n\n<align=left><size=20>Site-02 / Record Tag: #1205 ({Round.LobbyWaitingTime})</size></align>", 1.2f);

                        yield return Timing.WaitForSeconds(1);
                    }
                }

                Timing.RunCoroutine(loop());

                ev.Player.EnableEffect(EffectType.FogControl, 7);
            }
            else
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
        }

        public static void OnLeft(LeftEventArgs ev)
        {
            if (!ev.Player.IsNPC)
            {
                if (PlayerStatuses.ContainsKey(ev.Player))
                    PlayerStatuses.Remove(ev.Player);
            }
        }

        public static void OnSpawned(SpawnedEventArgs ev)
        {
            ev.Player.EnableEffect(EffectType.SoundtrackMute);
            ev.Player.EnableEffect(EffectType.FogControl, 7);
        }

        public static void OnChaningRole(ChangingRoleEventArgs ev)
        {
            PlayerStatuses[ev.Player] = new PlayerStatus();

            ev.Player.Scale = new Vector3(1, 1, 1);
        }

        public static IEnumerator<float> OnTogglingNoClip(TogglingNoClipEventArgs ev)
        {
            if (!PlayerStatuses[ev.Player].IsChangingSitDownState && !ev.Player.IsJumping && !ev.Player.IsNoclipPermitted && ev.Player.IsHuman)
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

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
using GGUtils;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Pickups;
using MEC;
using Mirror;
using MultiBroadcast.API;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers;
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
                        ev.Player.ShowHint($"\n\n\n\n<align=left><size=20>Site-02 / Record Tag: #1205 ({Server.PlayerCount}/{Server.MaxPlayerCount}) ({Round.LobbyWaitingTime})</size></align>", 1.2f);

                        yield return Timing.WaitForSeconds(1);
                    }
                }

                Timing.RunCoroutine(loop());

                ev.Player.EnableEffect(EffectType.FogControl, 5);
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
            if (ev.Player.IsAlive)
            {
                ev.Player.EnableEffect(EffectType.FogControl, 2);

                if (ev.Player.IsScp && ev.Reason == SpawnReason.ItemUsage)
                {
                    switch (ev.Player.Role.Type)
                    {
                        case RoleTypeId.Scp049:
                            IEnumerator<float> scp049()
                            {
                                AudioPlayer audioPlayer = AudioPlayer.CreateOrGet($"Player {ev.Player.Nickname}", onIntialCreation: (p) =>
                                {
                                    p.transform.parent = ev.Player.GameObject.transform;

                                    Speaker speaker = p.AddSpeaker("Main", isSpatial: true, minDistance: 5, maxDistance: 10);

                                    speaker.transform.parent = ev.Player.GameObject.transform;

                                    speaker.transform.localPosition = Vector3.zero;
                                });

                                while (ev.Player.Role.Type == RoleTypeId.Scp049)
                                {
                                    audioPlayer.AddClip($"scp049-{UnityEngine.Random.Range(1, 10)}", 2);

                                    yield return Timing.WaitForSeconds(UnityEngine.Random.Range(30, 180));
                                }

                                audioPlayer.Destroy();

                                yield break;
                            };

                            Timing.RunCoroutine(scp049());

                            break;

                        case RoleTypeId.Scp096:
                            IEnumerator<float> scp096()
                            {
                                AudioPlayer audioPlayer = AudioPlayer.CreateOrGet($"Player {ev.Player.Nickname}", onIntialCreation: (p) =>
                                {
                                    p.transform.parent = ev.Player.GameObject.transform;

                                    Speaker speaker = p.AddSpeaker("Main", isSpatial: true, minDistance: 5, maxDistance: 10);

                                    speaker.transform.parent = ev.Player.GameObject.transform;

                                    speaker.transform.localPosition = Vector3.zero;
                                });

                                audioPlayer.AddClip("Scp096crying", 2, loop: true);

                                while (ev.Player.Role.Type == RoleTypeId.Scp096)
                                    yield return Timing.WaitForSeconds(1);

                                audioPlayer.Destroy();

                                yield break;
                            };

                            Timing.RunCoroutine(scp096());

                            ev.Player.Teleport(RoomType.Hcz096, new Vector3(1.988281f, 0.9667969f, -0.02471924f));
                            break;

                        case RoleTypeId.Scp106:
                            ev.Player.Teleport(RoomType.Hcz106, new Vector3(-10.94531f, -1.622559f, -12.77344f));
                            break;
                    }
                }
            }
        }

        public static IEnumerator<float> OnChaningRole(ChangingRoleEventArgs ev)
        {
            if (ev.Reason == SpawnReason.RoundStart)
            {
                ev.IsAllowed = false;

                yield return Timing.WaitForSeconds(1);

                ev.Player.ShowHint($"\n\n<size=20><b>The Site-02 recording and playback device that had been stopped is now working again.</b></size>", 5);

                yield return Timing.WaitForSeconds(6);

                try
                {
                    ev.Player.ShowHint($"\n\n<size=20><b>In this recording, you will be writing to <color={ev.NewRole.GetColor().ToHex()}>{ev.NewRole.GetFullName()}</color>.</b></size>", 5);
                }
                catch (Exception ex)
                {
                    ev.Player.ShowHint($"오류가 발생했습니다. 개발자에게 문의해주세요.\n\n<size=20><b>{ex}</b></size>", 5);
                }

                yield return Timing.WaitForSeconds(6);

                ev.Player.Role.Set(ev.NewRole, SpawnReason.ItemUsage);
            }

            if (!ev.NewRole.IsDead())
            {
                if (!ev.NewRole.IsScp())
                {
                    PlayerStatuses[ev.Player] = new PlayerStatus();

                    ev.Player.Scale = new Vector3(1, 1, 1);
                }
            }
        }

        public static IEnumerator<float> OnTogglingNoClip(TogglingNoClipEventArgs ev)
        {
            if (!PlayerStatuses[ev.Player].IsChangingSitDownState && !ev.Player.IsJumping && !ev.Player.IsNoclipPermitted && ev.Player.IsHuman)
            {
                PlayerStatuses[ev.Player].IsChangingSitDownState = true;

                AudioPlayer audioPlayer = AudioPlayer.CreateOrGet($"Player {ev.Player.Nickname}", onIntialCreation: (p) =>
                {
                    Speaker speaker = p.AddSpeaker("Main", maxDistance: 3);

                    p.transform.parent = ev.Player.GameObject.transform;

                    speaker.transform.parent = ev.Player.GameObject.transform;

                    speaker.transform.localPosition = Vector3.zero;
                });

                if (PlayerStatuses[ev.Player].IsSitDown)
                {
                    audioPlayer.AddClip($"standing", volume: 2);

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
                    audioPlayer.AddClip($"sitting", volume: 2);

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

        public static void OnHurt(HurtEventArgs ev)
        {
            if (ev.Player.IsScp)
            {
                if (ev.Player.Health > ev.Player.MaxHealth)
                    ev.Player.EnableEffect(EffectType.Slowness, 30);
            }
        }

        public static void OnChangedEmotion(ChangedEmotionEventArgs ev)
        {
            EmotionCooldown.Add(ev.Player);

            EmotionPresetType type = ev.EmotionPresetType;

            if (type == EmotionPresetType.Neutral)
                return;

            string emotion()
            {
                if (type == EmotionPresetType.Happy)
                    return "행복한 표정을 짓고 있습니다";

                else if (type == EmotionPresetType.AwkwardSmile)
                    return "뒤틀린 미소를 짓고 있습니다";

                else if (type == EmotionPresetType.Scared)
                    return "두려운 표정을 짓고 있습니다";

                else if (type == EmotionPresetType.Angry)
                    return "화가난 표정을 짓고 있습니다";

                else if (type == EmotionPresetType.Chad)
                    return "꼭 채드처럼 보이는군요";

                else
                    return "꼭 오우거같이 보이는군요";
            }

            foreach (var player in Player.List.Where(x => x.IsDead || Vector3.Distance(x.Position, ev.Player.Position) < 11))
                player.AddBroadcast(5, $"<size=20><color={ev.Player.Role.Color.ToHex()}>{ev.Player.DisplayNickname}</color>(은)는 {emotion()}.</size>");
        }
    }
}

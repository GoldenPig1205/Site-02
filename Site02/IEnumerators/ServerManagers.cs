using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static Site02.Variables.ServerManagers;

namespace Site02.IEnumerators
{
    public static class ServerManagers
    {
        public static IEnumerator<float> HumanLoop()
        {
            while (!Round.IsEnded)
            {
                foreach (var player in Player.List)
                {
                    if (player.IsHuman)
                    {
                        if (!JumpScareCooldown.Contains(player))
                        {
                            if (Physics.Raycast(player.ReferenceHub.PlayerCameraReference.position + player.ReferenceHub.PlayerCameraReference.forward * 0.2f, player.ReferenceHub.PlayerCameraReference.forward, out RaycastHit hit, 25) &&
                                hit.collider.TryGetComponent<IDestructible>(out IDestructible destructible))
                            {
                                if (Player.TryGet(hit.collider.GetComponentInParent<ReferenceHub>(), out Player t) && player != t && t.IsScp)
                                {
                                    JumpScareCooldown.Add(player);

                                    Timing.CallDelayed(60, () =>
                                    {
                                        JumpScareCooldown.Remove(player);
                                    });

                                    AudioPlayer audioPlayer = AudioPlayer.CreateOrGet($"Player {player.Nickname}", condition: (hub) =>
                                    {
                                        return hub == player.ReferenceHub;
                                    }, onIntialCreation: (p) =>
                                    {
                                        Speaker speaker = p.AddSpeaker("Main", isSpatial: false, maxDistance: 12050);
                                    });

                                    audioPlayer.AddClip($"facingScp-{UnityEngine.Random.Range(1, 7)}", volume: 2);

                                    Timing.CallDelayed(3, () =>
                                    {
                                        audioPlayer.AddClip("chase", volume: 2);
                                    });
                                }
                            }
                        }
                    }
                }

                yield return Timing.WaitForOneFrame;
            }
        }

        public static IEnumerator<float> Scp079Broadcast()
        {
            while (!Round.IsEnded)
            {
                if (UnityEngine.Random.Range(1, 1001) == 1)
                    GlobalPlayer.AddClip($"scp079-{UnityEngine.Random.Range(1, 3)}", volume: 1.5f);

                int citizenCount = Player.List.Where(x => x.Role.Type == RoleTypeId.ClassD || x.Role.Type == RoleTypeId.Scientist).Count();

                if (citizenCount == 1 && !IsWarningAlone)
                {
                    IsWarningAlone = true;

                    GlobalPlayer.AddClip("scp079-4", volume: 1.5f);
                }
                if (citizenCount == 0 && !IsClearCitizen)
                {
                    IsClearCitizen = true;

                    GlobalPlayer.AddClip("scp079-3", volume: 1.5f);
                }

                yield return Timing.WaitForSeconds(1);
            }
        }

        public static IEnumerator<float> InputCooldown()
        {
            while (true)
            {
                ChatCooldown.Clear();
                EmotionCooldown.Clear();

                yield return Timing.WaitForSeconds(2f);
            }
        }
    }
}

using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Server;
using MapEditorReborn.API.Features.Objects;
using MEC;
using Mirror;
using PluginAPI.Events;
using Site02.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static Site02.Variables.ServerManagers;

using static Site02.IEnumerators.ServerManagers;
using PlayerRoles;
using GameCore;

namespace Site02.EventHandlers
{
    public class ServerEvents
    {
        public static IEnumerator<float> OnWaitingForPlayers()
        {
            yield return Timing.WaitForSeconds(1);

            GlobalPlayer = AudioPlayer.CreateOrGet($"Global AudioPlayer", onIntialCreation: (p) =>
            {
                Speaker speaker = p.AddSpeaker("Main", isSpatial: false, maxDistance: 5000);
            });

            Server.ExecuteCommand($"/mp load Record");

            GameObject.Find("StartRound").transform.localScale = Vector3.zero;

            GlobalPlayer.AddClip("The FFE_ost", loop: true);
        }

        public static IEnumerator<float> OnRoundStarted()
        {
            if (AudioPlayer.TryGet("Global AudioPlayer", out AudioPlayer ap))
                ap.RemoveAllClips();

            Round.IsLocked = true;
            Map.IsDecontaminationEnabled = false;

            GlobalPlayer.AddClip("TapeSound");

            foreach (var door in Door.List)
            {
                switch (door.Type)
                {
                    case DoorType.PrisonDoor:
                        door.Lock(DoorLockType.SpecialDoorFeature);
                        break;

                    case DoorType.Scp049Gate:
                        door.Lock(DoorLockType.SpecialDoorFeature);
                        break;

                    case DoorType.Scp173NewGate:
                        door.Lock(DoorLockType.SpecialDoorFeature);
                        break;

                    case DoorType.Scp096:
                        door.Lock(DoorLockType.SpecialDoorFeature);
                        break;
                }

                if (door.GameObject.TryGetComponent<DoorObject>(out DoorObject doorObject))
                {
                    if (doorObject.Base.DoorHealth == 939)
                        door.Lock(DoorLockType.SpecialDoorFeature);
                }
            }

            yield return Timing.WaitForSeconds(15);

            GlobalPlayer.AddClip("Intro", volume: 1.5f);

            foreach (var player in Player.List)
            {
                if (player.IsDead)
                {
                    player.Role.Set(RoleTypeId.ClassD);
                    player.ShowHint($"\n\n<b><size=20>영사기가 당신을 빨아들였습니다.</size></b>");
                }
            }

            yield return Timing.WaitForSeconds(35);

            Dictionary<Room, Color> roomColor = new Dictionary<Room, Color>();
                
            foreach (var room in Room.List)
                roomColor.Add(room, room.Color);

            IEnumerator<float> lightBlink()
            {
                for (int i = 0; i < UnityEngine.Random.Range(11, 31); i++)
                {
                    if (UnityEngine.Random.Range(1, 5) == 1)
                        Warhead.Shake();

                    foreach (var room in Room.List) room.Color = Color.red;

                    yield return Timing.WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.5f));

                    foreach (var room in Room.List) room.Color = roomColor[room];

                    yield return Timing.WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.5f));
                }
            }

            Timing.RunCoroutine(lightBlink());

            yield return Timing.WaitForSeconds(UnityEngine.Random.Range(5, 11));

            foreach (var door in Door.List.Where(x => x.IsLocked))
            {
                door.Unlock();
                door.IsOpen = true;
            }

            Round.IsLocked = false;
            Map.IsDecontaminationEnabled = true;

            Timing.RunCoroutine(Scp079Broadcast());
            Timing.RunCoroutine(HumanLoop());
        }

        public static IEnumerator<float> OnRoundEnded(RoundEndedEventArgs ev)
        {
            yield return Timing.WaitForSeconds(8);

            DummyUtils.DestroyAllDummies();

            yield return Timing.WaitForSeconds(1);

            Server.ExecuteCommand($"/sr");
        }
    }
}

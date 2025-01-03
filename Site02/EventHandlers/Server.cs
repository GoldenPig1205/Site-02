using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Map;
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

namespace Site02.EventHandlers
{
    public class ServerEvents
    {
        public static void OnWaitingForPlayers()
        {
            GameObject.Find("StartRound").transform.localScale = Vector3.zero;
            Room.List.ToList().ForEach(x => x.Color = new Color(0, 0, 0));

            AudioPlayer audioPlayer = AudioPlayer.CreateOrGet($"Global AudioPlayer", onIntialCreation: (p) =>
            {
                Speaker speaker = p.AddSpeaker("Main", isSpatial: false, maxDistance: 5000);
            });

            audioPlayer.AddClip("Main Theme", 0.3f, true);
        }

        public static void OnRoundStarted()
        {
            if (AudioPlayer.TryGet("Global AudioPlayer", out AudioPlayer ap))
                ap.RemoveAllClips();
        }
    }
}

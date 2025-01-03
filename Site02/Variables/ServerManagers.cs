using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Site02.Classes;
using UnityEngine;

namespace Site02.Variables
{
    public static class ServerManagers
    {
        public static Dictionary<Player, PlayerStatus> PlayerStatuses = new Dictionary<Player, PlayerStatus>();
        public static Dictionary<byte, float> DoorHealthes = new Dictionary<byte, float>();
        public static Dictionary<string, string> AudioClips = new Dictionary<string, string>() 
        {
            {"Remorseless", "Main Theme"}
        };

        public static List<Transform> Balls = new List<Transform>();
    }
}

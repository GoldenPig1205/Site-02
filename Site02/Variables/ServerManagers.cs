using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Exiled.API.Features;
using Site02.Classes;
using UnityEngine;

namespace Site02.Variables
{
    public static class ServerManagers
    {
        public static Dictionary<Player, PlayerStatus> PlayerStatuses = new Dictionary<Player, PlayerStatus>();
        public static Dictionary<byte, float> DoorHealthes = new Dictionary<byte, float>();

        public static List<Player> JumpScareCooldown = new List<Player>();
        public static List<Player> ChatCooldown = new List<Player>();
        public static List<Player> EmotionCooldown = new List<Player>();

        public static AudioPlayer GlobalPlayer;
        public static bool IsWarningAlone = false;
        public static bool IsClearCitizen = false;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Exiled.API.Features;
using RealitySL.Classes;
using UnityEngine;

namespace RealitySL.Variables
{
    public static class ServerManagers
    {
        public static Dictionary<Player, PlayerStatus> PlayerStatuses = new Dictionary<Player, PlayerStatus>();

        public static List<Transform> Balls = new List<Transform>();
    }
}

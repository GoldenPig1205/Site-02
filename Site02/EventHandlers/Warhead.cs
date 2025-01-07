using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Warhead;
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

namespace Site02.EventHandlers
{
    public static class WarheadEvents
    {
        public static IEnumerator<float> OnDetonating(DetonatingEventArgs ev)
        {
            yield return Timing.WaitForSeconds(180);

            GlobalPlayer.AddClip("SCP - Breach", volume: 0.5f);
        }
    }
}

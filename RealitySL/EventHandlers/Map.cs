using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using RealitySL.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static RealitySL.Variables.ServerManagers;

namespace RealitySL.EventHandlers
{
    public class MapEvents
    {
        public static void OnPickupAdded(PickupAddedEventArgs ev)
        {
            if (ev.Pickup.Weight >= 0.7f)
            {
                ev.Pickup.Transform.gameObject.AddComponent<BallComponent>();

                Balls.Add(ev.Pickup.Transform);
            }
        }

        public static void OnPickupDestroyed(PickupDestroyedEventArgs ev)
        {
            if (Balls.Contains(ev.Pickup.Transform))
                Balls.Remove(ev.Pickup.Transform);
        }
    }
}

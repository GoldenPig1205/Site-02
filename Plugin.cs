using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Exiled.API.Features;
using MEC;

using static RealitySL.EventHandlers.PlayerEvents;
using static RealitySL.EventHandlers.MapEvents;

using static RealitySL.IEnumerators.ServerManagers;

namespace RealitySL
{
    public class RealitySL : Plugin<Config>
    {
        public override string Name => "RealitySL";
        public override string Author => "GoldenPig1205";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(1, 2, 0, 5);

        public static RealitySL Instance;

        CoroutineHandle _ball = new CoroutineHandle();

        public override void OnEnabled()
        {
            Instance = this;
            base.OnEnabled();

            _ball = Timing.RunCoroutine(Ball());

            Exiled.Events.Handlers.Map.PickupAdded += OnPickupAdded;
            Exiled.Events.Handlers.Map.PickupDestroyed += OnPickupDestroyed;

            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Player.ChangingRole += OnChaningRole;
            Exiled.Events.Handlers.Player.TogglingNoClip += OnTogglingNoClip;
        }

        public override void OnDisabled()
        {
            Timing.KillCoroutines(_ball);

            Exiled.Events.Handlers.Map.PickupAdded -= OnPickupAdded;
            Exiled.Events.Handlers.Map.PickupDestroyed -= OnPickupDestroyed;

            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChaningRole;
            Exiled.Events.Handlers.Player.TogglingNoClip -= OnTogglingNoClip;

            base.OnDisabled();
            Instance = null;
        }
    }
}

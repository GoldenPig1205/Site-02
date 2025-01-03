using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Exiled.API.Features;
using MEC;

using static Site02.Variables.ServerManagers;

using static Site02.EventHandlers.PlayerEvents;
using static Site02.EventHandlers.MapEvents;
using static Site02.EventHandlers.ServerEvents;

using static Site02.IEnumerators.ServerManagers;

namespace Site02
{
    public class Site02 : Plugin<Config>
    {
        public override string Name => "Site02";
        public override string Author => "GoldenPig1205";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(1, 2, 0, 5);

        public static Site02 Instance;

        public override void OnEnabled()
        {
            Instance = this;
            base.OnEnabled();

            foreach (var _audioClip in AudioClips)
                AudioClipStorage.LoadClip(GGUtils.Gtool.ConventToAudioPath(_audioClip.Key), _audioClip.Value);

            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;

            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Player.ChangingRole += OnChaningRole;
            Exiled.Events.Handlers.Player.TogglingNoClip += OnTogglingNoClip;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;

            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChaningRole;
            Exiled.Events.Handlers.Player.TogglingNoClip -= OnTogglingNoClip;

            base.OnDisabled();
            Instance = null;
        }
    }
}

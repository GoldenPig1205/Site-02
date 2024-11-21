using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static RealitySL.Variables.ServerManagers;

namespace RealitySL.IEnumerators
{
    public class ServerManagers
    {
        public static IEnumerator<float> Ball()
        {
            while (true)
            {
                foreach (Player player in Player.List.Where(x => x.IsAlive))
                {
                    foreach (Transform Ball in Balls)
                    {
                        GameObject _ball = Ball.gameObject;
                        float _radius = Vector3.Distance(_ball.transform.position, player.Position);

                        if (0.75f < _radius && _radius < 1)
                        {
                            _ball.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rig);
                            rig.AddForce(player.GameObject.transform.forward + new Vector3(0, 0.003f, 0), ForceMode.Impulse);
                        }
                    }
                }

                yield return Timing.WaitForOneFrame;
            }
        }
    }
}

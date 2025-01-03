using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Site02.Components
{
    public class BallComponent : MonoBehaviour
    {
        private SphereCollider sphere;
        private Rigidbody rigid;
        internal static float SpeedBoost = 0.1f;
        private void Start()
        {
            sphere = gameObject.AddComponent<SphereCollider>();
            sphere.isTrigger = true;
            sphere.radius = 1.1f;

            rigid = gameObject.AddComponent<Rigidbody>();
            rigid.isKinematic = false;
            rigid.useGravity = true;
            rigid.mass = 0.1f;
            rigid.drag = 0.1f;
        }

        private void FixedUpdate()
        {
            transform.position += rigid.velocity * (Time.fixedDeltaTime * SpeedBoost);
        }
    }
}

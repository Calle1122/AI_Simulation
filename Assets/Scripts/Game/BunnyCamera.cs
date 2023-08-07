using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Camera))]
    public class BunnyCamera : MonoBehaviour
    {
        private Bunny   m_bunny;

        void Update()
        {
            // grab any bunny to follow
            if (m_bunny == null && 
                Grid.Instance != null)
            {
                m_bunny = Grid.Instance.transform.GetComponentInChildren<Bunny>();
            }

            // follow that bunny!
            if (m_bunny != null)
            {
                Vector3 vTarget = m_bunny.transform.position - transform.forward * 7.0f;
                transform.position += (vTarget - transform.position) * Time.deltaTime;
            }
        }
    }
}
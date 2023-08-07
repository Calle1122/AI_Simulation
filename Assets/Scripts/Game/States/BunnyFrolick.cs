using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class BunnyFrolic : BunnyBaseState
    {
        [SerializeField]
        public AnimationCurve   m_position;

        [SerializeField]
        public AnimationCurve   m_rotation;

        float       m_fTime = 0.0f;

        public override void OnStateStart()
        {
            base.OnStateStart();

            m_fTime = 0.0f;
        }

        public override void MachineTick()
        {
            base.MachineTick();

            m_fTime += Time.deltaTime;

            Bunny.MeshTransform.localPosition = new Vector3(0.0f, m_position.Evaluate(m_fTime), 0.0f);
            Bunny.MeshTransform.localRotation = Quaternion.Euler(m_rotation.Evaluate(m_fTime), 0.0f, 0.0f);
        }

        public override bool ShouldTransitionToState(out State nextState)
        {
            if (m_fTime > m_position.keys[m_position.length - 1].time)
            {
                Bunny.MeshTransform.localPosition = Vector3.zero;
                Bunny.MeshTransform.localRotation = Quaternion.identity;
                nextState = GetRandomConnectedState(new System.Type[] { typeof(BunnyIdle), typeof(BunnyRandomMove) });
                return true;
            }

            return base.ShouldTransitionToState(out nextState);
        }
    }
}
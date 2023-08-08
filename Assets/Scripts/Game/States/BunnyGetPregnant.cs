using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class BunnyGetPregnant : BunnyBaseState
    {
        [SerializeField]
        public AnimationCurve   m_size;

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

            Bunny.MeshTransform.localScale = new Vector3(m_size.Evaluate(m_fTime), m_size.Evaluate(m_fTime), m_size.Evaluate(m_fTime));
        }

        public override bool ShouldTransitionToState(out State nextState)
        {
            if (m_fTime > m_size.keys[m_size.length - 1].time)
            {
                //Spawn new bunny
                Grid.Instance.CreateItemAt(Grid.Instance.m_bunnyPrefab, Bunny.Tile);
                
                //Reset scale and move on
                Bunny.MeshTransform.localScale = new Vector3(1, 1, 1);

                Bunny.MatingCooldown = 0f;
                
                nextState = GetRandomConnectedState(new System.Type[] { typeof(BunnyIdle), typeof(BunnyRandomMove) });
                return true;
            }

            return base.ShouldTransitionToState(out nextState);
        }
    }
}
using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class WolfGetPregnant : WolfBaseState
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

            Wolf.MeshTransform.localScale = new Vector3(m_size.Evaluate(m_fTime), Wolf.MeshTransform.localScale.y, Wolf.MeshTransform.localScale.z);
        }

        public override bool ShouldTransitionToState(out State nextState)
        {
            if (m_fTime > m_size.keys[m_size.length - 1].time)
            {
                //Spawn new bunny
                Grid.Instance.CreateItemAt(Grid.Instance.m_wolfPrefab, Wolf.Tile);
                
                //Reset scale and move on
                Wolf.MeshTransform.localScale = new Vector3(0.0075f, 0.0075f, 0.0075f);

                Wolf.MatingCooldown = 0f;
                
                nextState = GetRandomConnectedState(new System.Type[] { typeof(WolfIdle), typeof(WolfRandomMove) });
                return true;
            }

            return base.ShouldTransitionToState(out nextState);
        }
    }
}
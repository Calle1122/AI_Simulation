using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class BunnyIdle : BunnyBaseState
    {
        float m_fWaitTime = 1.0f;

        public override void OnStateStart()
        {
            base.OnStateStart();

            m_fWaitTime = Random.Range(1.0f, 2.0f);
        }

        public override void MachineTick()
        {
            base.MachineTick();

            m_fWaitTime -= Time.deltaTime;
        }

        protected virtual State GetNextState()
        {
            // hungry?
            if (Bunny.IsHungry)
            {
                if (Bunny.Tile.HasItem<Carrot>())
                {
                    return GetConnectedState<BunnyEatCarrot>();
                }
                else
                {
                    return GetConnectedState<BunnySeekCarrot>();
                }
            }
            if (Bunny.MatingCooldown == 30)
            {
                return GetConnectedState<BunnySeekMate>();
            }

            return GetRandomConnectedState(new System.Type[] { typeof(BunnyFrolic), typeof(BunnyRandomMove) });
        }

        public override bool ShouldTransitionToState(out State nextState)
        {
            if (m_fWaitTime < 0.0f)
            {
                nextState = GetNextState();
                return true;
            }

            return base.ShouldTransitionToState(out nextState);
        }
    }
}
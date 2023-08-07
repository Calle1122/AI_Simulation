using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class WolfRandomMove : WolfBaseState
    {
        protected Grid.Tile     m_targetTile;
        protected Quaternion    m_qTargetDirection;

        #region Properties

        #endregion

        public override void OnStateStart()
        {
            base.OnStateStart();

            m_targetTile = GetTargetTile();

            // calculate target direction
            Vector3 vDir = m_targetTile.WorldPosition - Wolf.transform.position;
            vDir.y = 0.0f;
            m_qTargetDirection = Quaternion.LookRotation(vDir.normalized);
        }

        protected virtual Grid.Tile GetTargetTile()
        {
            // get random target tile
            List<Grid.Tile> neighborTiles = new List<Grid.Tile>(Wolf.Tile.Neighbors);
            return neighborTiles[Random.Range(0, neighborTiles.Count)];
        }

        public override void MachineTick()
        {
            base.MachineTick();

            if (Wolf.Tile != m_targetTile)
            {
                // move to and face target
                Vector3 vTarget = m_targetTile.WorldPosition;
                Wolf.transform.position = Vector3.MoveTowards(Wolf.transform.position, vTarget, 0.7f * Time.deltaTime);
                Wolf.DirectionTransform.rotation = Quaternion.Slerp(Wolf.DirectionTransform.rotation, m_qTargetDirection, Time.deltaTime * 3.0f);

                // reached target?
                if (Vector3.Distance(Wolf.transform.position, vTarget) < 0.01f)
                {
                    Wolf.Tile = m_targetTile;
                }
            }
        }

        public override bool ShouldTransitionToState(out State nextState)
        {
            if (Wolf.Tile == m_targetTile)
            {
                nextState = GetRandomConnectedState(new System.Type[] { typeof(WolfIdle) });
                return true;
            }

            return base.ShouldTransitionToState(out nextState);
        }
    }
}
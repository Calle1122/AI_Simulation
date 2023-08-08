using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class WolfSeekMate : WolfRandomMove
    {
        #region Properties

        #endregion

        private Wolf targetMate;
        
        public override void OnStateStart()
        {
            base.OnStateStart();

            Wolf.IsLookingForMate = true;
        }
        
        protected override Grid.Tile GetTargetTile()
        {
            targetMate = Grid.Instance.GetClosestItem<Wolf>(Wolf.transform.position, Wolf);
            Grid.Tile targetTile = null;

            if (targetMate != null)
            {
                List<Grid.Tile> neighborTiles = new List<Grid.Tile>(Wolf.Tile.Neighbors);

                float fBestDistance = float.MaxValue;
                foreach (Grid.Tile tile in neighborTiles)
                {
                    float fDistToMate = Vector3.Distance(tile.WorldPosition, targetMate.transform.position);
                    if (fDistToMate < fBestDistance)
                    {
                        targetTile = tile;
                        fBestDistance = fDistToMate;
                    }
                }
            }

            return targetTile != null ? targetTile : base.GetTargetTile();
        }

        public override bool ShouldTransitionToState(out State nextState)
        {
            if (Wolf.Tile == m_targetTile)
            {
                if (m_targetTile.HasItem<Wolf>() && m_targetTile.GetItem<Wolf>() == targetMate)
                {
                    Wolf.IsLookingForMate = false;
                    
                    nextState = GetConnectedState<WolfGetPregnant>();
                }
                else if (Wolf.IsLookingForMate)
                {
                    // restart self
                    nextState = null;
                    OnStateStart();
                    return false;
                }
                else
                {
                    nextState = GetConnectedState<WolfIdle>();
                }

                return true;
            }

            return base.ShouldTransitionToState(out nextState);
        }
    }
}
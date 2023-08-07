using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class BunnySeekMate : BunnyRandomMove
    {
        #region Properties

        #endregion

        private Bunny targetMate;
        
        public override void OnStateStart()
        {
            base.OnStateStart();

            Bunny.IsLookingForMate = true;
        }
        
        protected override Grid.Tile GetTargetTile()
        {
            targetMate = Grid.Instance.GetClosestItem<Bunny>(Bunny.transform.position, Bunny);
            Grid.Tile targetTile = null;

            if (targetMate != null)
            {
                List<Grid.Tile> neighborTiles = new List<Grid.Tile>(Bunny.Tile.Neighbors);

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
            if (Bunny.Tile == m_targetTile)
            {
                if (m_targetTile.HasItem<Bunny>() && m_targetTile.GetItem<Bunny>() == targetMate)
                {
                    Bunny.IsLookingForMate = false;
                    
                    nextState = GetConnectedState<BunnyGetPregnant>();
                }
                else if (Bunny.IsLookingForMate)
                {
                    // restart self
                    nextState = null;
                    OnStateStart();
                    return false;
                }
                else
                {
                    nextState = GetConnectedState<BunnyIdle>();
                }

                return true;
            }

            return base.ShouldTransitionToState(out nextState);
        }
    }
}
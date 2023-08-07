using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class WolfSeekBunny : WolfRandomMove
    {
        #region Properties

        #endregion

        protected override Grid.Tile GetTargetTile()
        {
            Bunny targetBunny = Grid.Instance.GetClosestItem<Bunny>(Wolf.transform.position);
            Grid.Tile targetTile = null;

            if (targetBunny != null)
            {
                List<Grid.Tile> neighborTiles = new List<Grid.Tile>(Wolf.Tile.Neighbors);

                float fBestDistance = float.MaxValue;
                foreach (Grid.Tile tile in neighborTiles)
                {
                    float fDistToBunny = Vector3.Distance(tile.WorldPosition, targetBunny.transform.position);
                    if (fDistToBunny < fBestDistance)
                    {
                        targetTile = tile;
                        fBestDistance = fDistToBunny;
                    }
                }
            }

            return targetTile != null ? targetTile : base.GetTargetTile();
        }

        public override bool ShouldTransitionToState(out State nextState)
        {
            if (Wolf.Tile == m_targetTile)
            {
                if (m_targetTile.HasItem<Bunny>())
                {
                    nextState = GetConnectedState<WolfEatBunny>();
                }
                else if (Wolf.IsHungry)
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
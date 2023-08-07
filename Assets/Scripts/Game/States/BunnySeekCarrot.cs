using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class BunnySeekCarrot : BunnyRandomMove
    {
        #region Properties

        #endregion

        protected override Grid.Tile GetTargetTile()
        {
            Carrot targetCarrot = Grid.Instance.GetClosestItem<Carrot>(Bunny.transform.position);
            Grid.Tile targetTile = null;

            if (targetCarrot != null)
            {
                List<Grid.Tile> neighborTiles = new List<Grid.Tile>(Bunny.Tile.Neighbors);

                float fBestDistance = float.MaxValue;
                foreach (Grid.Tile tile in neighborTiles)
                {
                    float fDistToCarrot = Vector3.Distance(tile.WorldPosition, targetCarrot.transform.position);
                    if (fDistToCarrot < fBestDistance)
                    {
                        targetTile = tile;
                        fBestDistance = fDistToCarrot;
                    }
                }
            }

            return targetTile != null ? targetTile : base.GetTargetTile();
        }

        public override bool ShouldTransitionToState(out State nextState)
        {
            if (Bunny.Tile == m_targetTile)
            {
                if (m_targetTile.HasItem<Carrot>())
                {
                    nextState = GetConnectedState<BunnyEatCarrot>();
                }
                else if (Bunny.IsHungry)
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
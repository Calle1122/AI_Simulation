using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class WolfEatBunny : WolfIdle
    {
        public override void OnStateStart()
        {
            base.OnStateStart();

            Bunny bunny = Wolf.Tile.GetItem<Bunny>();
            if (bunny != null)
            {
                // dig in
                Wolf.MeshTransform.localRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
                Wolf.MeshTransform.localPosition = Vector3.up * 0.15f;

                // destroy carrot
                Destroy(bunny.gameObject);

                // reduce hunger
                Wolf.Hunger -= 10;
            }
        }

        public override void OnStateStop()
        {
            base.OnStateStop();

            // done!
            Wolf.MeshTransform.localRotation = Quaternion.identity;
            Wolf.MeshTransform.localPosition = Vector3.zero;
        }

        protected override State GetNextState()
        {
            if (Wolf.IsHungry)
            {
                return GetConnectedState<WolfSeekBunny>();
            }

            return GetConnectedState<WolfIdle>();
        }
    }
}
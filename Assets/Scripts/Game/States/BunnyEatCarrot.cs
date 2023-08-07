using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class BunnyEatCarrot : BunnyIdle
    {
        public override void OnStateStart()
        {
            base.OnStateStart();

            Carrot carrot = Bunny.Tile.GetItem<Carrot>();
            if (carrot != null)
            {
                // dig in
                Bunny.MeshTransform.localRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
                Bunny.MeshTransform.localPosition = Vector3.up * 0.15f;

                // destroy carrot
                Destroy(carrot.gameObject);

                // reduce hunger
                Bunny.Hunger -= 10;
            }
        }

        public override void OnStateStop()
        {
            base.OnStateStop();

            // done!
            Bunny.MeshTransform.localRotation = Quaternion.identity;
            Bunny.MeshTransform.localPosition = Vector3.zero;
        }

        protected override State GetNextState()
        {
            if (Bunny.IsHungry)
            {
                return GetConnectedState<BunnySeekCarrot>();
            }

            return GetConnectedState<BunnyIdle>();
        }
    }
}
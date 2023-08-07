using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public abstract class BunnyBaseState : State
    {
        #region Properties

        public Bunny Bunny => Machine?.Target?.GetComponent<Bunny>();

        #endregion

        public override void OnStateStart()
        {
            base.OnStateStart();

            // increase hunger
            Bunny.Hunger++;
        }
    }
}
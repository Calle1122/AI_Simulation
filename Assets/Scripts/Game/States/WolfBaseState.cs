using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public abstract class WolfBaseState : State
    {
        #region Properties

        public Wolf Wolf => Machine?.Target?.GetComponent<Wolf>();

        #endregion

        public override void OnStateStart()
        {
            base.OnStateStart();

            // increase hunger
            Wolf.Hunger++;
        }
    }
}
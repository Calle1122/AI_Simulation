using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Carrot : Grid.Item
    {
        protected override void Start()
        {
            base.Start();

            StatManager.Instance.AliveCarrots++;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            StatManager.Instance.AliveCarrots--;
        }
    }
}
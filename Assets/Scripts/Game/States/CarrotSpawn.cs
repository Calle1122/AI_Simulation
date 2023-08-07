using FiniteStateMachines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.States
{
    public class CarrotSpawn : State
    {
        float m_fSpawnTime;

        public override void OnMachineStart(FiniteStateMachine machine)
        {
            base.OnMachineStart(machine);

            m_fSpawnTime = Random.Range(10.0f, 20.0f);
        }

        public override void MachineTick()
        {
            base.MachineTick();

            // wait for spawn
            m_fSpawnTime -= Time.deltaTime;
            if (m_fSpawnTime < 0.0f)
            {
                m_fSpawnTime = Random.Range(5.0f, 12.0f);

                // try to get neighbor tile
                Grid.Item item = Machine.Target.GetComponent<Grid.Item>();
                if (item != null &&
                    item.Tile != null)
                {
                    List<Grid.Tile> neighbors = new List<Grid.Tile>(item.Tile.Neighbors);                    
                    if (neighbors.Count > 0)
                    {
                        Grid.Tile tile = neighbors[Random.Range(0, neighbors.Count)];
                        if (tile != null && !tile.HasItem<Carrot>())
                        {
                            Grid.Instance.CreateItemAt(Grid.Instance.m_carrotPrefab, tile);
                            //Debug.Log("Carrot spawned at: " + tile.GridPosition);
                        }
                    }
                }                
            }
        }
    }
}
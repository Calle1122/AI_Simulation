using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game
{
    public class Bunny : Grid.Item
    {
        [SerializeField]
        private Transform   m_meshTransform;

        [SerializeField]
        private Transform   m_directionTransform;

        [SerializeField]
        private int         m_iHunger;
        
        public bool IsLookingForMate;

        private float m_lifeTime;
        
        public float m_matingCooldown = 20f;

        #region Properties

        public Transform MeshTransform => m_meshTransform;

        public Transform DirectionTransform => m_directionTransform;

        public int Hunger
        {
            get => m_iHunger;
            set
            {
                m_iHunger = Mathf.Max(value, 0);

                if (m_iHunger > 30)
                {
                    // oh no... death by starvation
                    Destroy(gameObject);
                }
            }            
        }

        public float LifeTime
        {
            get => m_lifeTime;
            set
            {
                m_lifeTime = Mathf.Max(value, 0);

                if (m_lifeTime > 180)
                {
                    // oh no... death by old
                    Destroy(gameObject);
                }
            }
        }

        public float MatingCooldown
        {
            get => m_matingCooldown;
            set
            {
                m_matingCooldown = Mathf.Min(value, 30);
            }
        }

        public bool IsHungry => m_iHunger > 10;

        #endregion

        protected override void Start()
        {
            base.Start();

            StatManager.Instance.AliveBunnies++;

            m_meshTransform = transform.Find("Direction/Mesh");
            m_directionTransform = transform.Find("Direction");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            StatManager.Instance.AliveBunnies--;
        }
    }
}
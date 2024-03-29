using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Wolf : Grid.Item
    {
        [SerializeField]
        private Transform   m_meshTransform;

        [SerializeField]
        private Transform   m_directionTransform;

        [SerializeField]
        private int         m_iHunger;

        public bool IsLookingForMate;
        
        public float m_matingCooldown = 45f;
        
        #region Properties

        public Transform MeshTransform => m_meshTransform;

        public Transform DirectionTransform => m_directionTransform;

        public int Hunger
        {
            get => m_iHunger;
            set
            {
                m_iHunger = Mathf.Max(value, 0);

                if (m_iHunger > 100)
                {
                    // oh no... death by starvation
                    Destroy(gameObject);
                }
            }            
        }

        public float MatingCooldown
        {
            get => m_matingCooldown;

            set
            {
                m_matingCooldown = Mathf.Min(value, 90);
            }
        }

        public bool IsHungry => m_iHunger > 6;

        #endregion

        protected override void Start()
        {
            base.Start();

            StatManager.Instance.AliveWolves++;
            
            m_meshTransform = transform.Find("Direction/Mesh");
            m_directionTransform = transform.Find("Direction");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            StatManager.Instance.AliveWolves--;
        }
    }
}
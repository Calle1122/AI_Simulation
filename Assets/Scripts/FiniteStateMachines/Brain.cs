using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FiniteStateMachines
{
    public class Brain : MonoBehaviour
    {
        [SerializeField]
        private FiniteStateMachine  m_machine;

        #region Properties

        public FiniteStateMachine Machine => m_machine;

        #endregion

        protected virtual void Start()
        {
            if (m_machine != null)
            {
                m_machine = m_machine.Clone();
                m_machine.StartMachine(gameObject);
            }
        }

        protected virtual void Update()
        {
            m_machine?.TickMachine();
        }
    }
}
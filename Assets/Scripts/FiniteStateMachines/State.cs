using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FiniteStateMachines
{
    public abstract class State : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private int                 m_ID;

        [SerializeField, HideInInspector]
        public Vector2              m_vPosition;

        [SerializeField]
        public List<State>          m_connectedStates = new List<State>();

        private FiniteStateMachine  m_machine;

        #region Properties

        public int ID => m_ID;

        public FiniteStateMachine Machine => m_machine;

        #endregion

        private void OnValidate()
        {
            if (m_ID == 0)
            {
                m_ID = System.Guid.NewGuid().GetHashCode();
            }
        }

        public virtual void OnMachineStart(FiniteStateMachine machine)
        {
            m_machine = machine;
        }

        public virtual void OnStateStart()
        {
        }

        public virtual void OnStateStop()
        {
        }

        public virtual void MachineTick()
        {
        }

        public virtual bool ShouldTransitionToState(out State nextState)
        {
            nextState = null;
            return false;
        }

        public virtual State Clone()
        {
            State clone = Instantiate(this);
            clone.name = name;
            return clone;
        }

        public T GetConnectedState<T>() where T : State
        {
            return m_connectedStates.Find(c => c is T) as T;
        }

        public State GetRandomConnectedState(System.Type[] types)
        {
            List<State> possibleStates = m_connectedStates.FindAll(s => System.Array.IndexOf(types, s.GetType()) >= 0);
            return possibleStates.Count > 0 ? possibleStates[Random.Range(0, possibleStates.Count)] : null;
        }
    }
}
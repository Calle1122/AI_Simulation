using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FiniteStateMachines
{
    [CreateAssetMenu(fileName = "FiniteStateMachine", menuName = "AICourse/FiniteStateMachine")]
    public class FiniteStateMachine : ScriptableObject
    {
        [SerializeField]
        public List<State>  m_states = new List<State>();

        private GameObject  m_target;
        private State       m_currentState;

        #region Properties

        public GameObject Target => m_target;

        public State CurrentState => m_currentState;

        #endregion

        public State CreateState(System.Type type)
        {
            #if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this, "State Created");
            #endif

            State state = ScriptableObject.CreateInstance(type) as State;
            state.name = type.Name;
            m_states.Add(state);

            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.AddObjectToAsset(state, this);
            UnityEditor.Undo.RegisterCreatedObjectUndo(state, "State Created");
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
            #endif

            return state;
        }

        public void DeleteState(State state)
        {
            if (state == null)
            {
                return;
            }

            #if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(this, "State Deleted");
            #endif

            m_states.Remove(state);

            #if UNITY_EDITOR
            UnityEditor.Undo.DestroyObjectImmediate(state);
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
            #endif
        }

        public void ConnectStates(State fromState, State toState)
        {
            if (toState != null && !fromState.m_connectedStates.Contains(toState))
            {
            #if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(fromState, "State Connected");
            #endif

                fromState.m_connectedStates.Add(toState);

            #if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(fromState);
            #endif
            }
        }

        public void RemoveConnection(State fromState, State toState)
        {
            if (toState != null && fromState.m_connectedStates.Contains(toState))
            {
            #if UNITY_EDITOR
                UnityEditor.Undo.RecordObject(fromState, "State Connection Removed");
            #endif

                fromState.m_connectedStates.Remove(toState);

            #if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(fromState);
            #endif
            }
        }

        public FiniteStateMachine Clone()
        {
            FiniteStateMachine clone = Instantiate(this);
            clone.name = clone.name.Replace("(Clone)", " (Runtime)");

            // clone states
            clone.m_states = m_states.ConvertAll(s => s.Clone());

            // clone connections
            clone.m_states.ForEach(from => from.m_connectedStates = from.m_connectedStates.ConvertAll(to => clone.m_states[m_states.IndexOf(to)]));

            return clone;
        }

        public void StartMachine(GameObject target)
        {
            m_target = target;
            m_states.ForEach(s => s.OnMachineStart(this));
            m_currentState = m_states.Count > 0 ? m_states[0] : null;
            m_currentState?.OnStateStart();
        }

        public void TickMachine()
        {
            if (m_currentState != null)
            {
                // tick current state
                m_currentState.MachineTick();

                // should transition to another state?
                State nextState;
                if (m_currentState.ShouldTransitionToState(out nextState))
                {
                    m_currentState.OnStateStop();
                    m_currentState = nextState;
                    m_currentState?.OnStateStart();
                }
            }
        }
    }
}
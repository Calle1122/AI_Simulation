using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;

namespace FiniteStateMachines
{
    public class StateNodeControl : UnityEditor.Experimental.GraphView.Node
    {
        private State   m_state;
        private Port    m_input;
        private Port    m_output;
        private Label   m_stateLabel;

        #region Properties

        public State State => m_state;

        public Port Input => m_input;

        public Port Output => m_output;

        #endregion

        public StateNodeControl(State state) : base("Assets/Editor/FiniteStateMachines/StateNodeControl.uxml")
        {
            m_state = state;
            title = state.name;
            viewDataKey = state.ID.ToString();

            style.left = state.m_vPosition.x;
            style.top = state.m_vPosition.y;

            CreatePorts();

            m_stateLabel = this.Q<Label>("State");
            m_stateLabel.text = "";
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Undo.RecordObject(State, "Moving State");
            m_state.m_vPosition = new Vector2(newPos.xMin, newPos.yMin);
            EditorUtility.SetDirty(State);
        }

        private void CreatePorts()
        {
            m_input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            m_input.portName = "";
            m_input.style.flexDirection = FlexDirection.Column;
            m_input.style.alignSelf = Align.FlexStart;
            inputContainer.Add(m_input);

            m_output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            m_output.portName = "";
            m_output.style.flexDirection = FlexDirection.Column;
            m_output.style.alignSelf = Align.FlexEnd;
            outputContainer.Add(m_output);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            Selection.activeObject = State;
        }

        public void UpdateRunningState()
        {
            if (Application.isPlaying && 
                m_stateLabel != null &&
                m_state != null && 
                m_state.Machine != null)
            {
                RemoveFromClassList("running");
                m_stateLabel.text = "";

                if (m_state.Machine.CurrentState == m_state)
                {
                    m_stateLabel.text = "Running";
                    AddToClassList("running");
                }
            }
        }
    }
}
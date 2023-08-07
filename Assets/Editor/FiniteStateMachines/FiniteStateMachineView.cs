using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using System.Linq;

namespace FiniteStateMachines
{
    public class FiniteStateMachineView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<FiniteStateMachineView, GraphView.UxmlTraits> { }

        private FiniteStateMachine      m_machine;

        #region Properties

        #endregion

        public FiniteStateMachineView()
        {
            Insert(0, new GridBackground());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/FiniteStateMachines/FiniteStateMachineEditor.uss");
            styleSheets.Add(styleSheet);

            Undo.undoRedoPerformed += OnUndoRedo;
        }

        private void OnUndoRedo()
        {
            PopulateView(m_machine);
            AssetDatabase.SaveAssets();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //base.BuildContextualMenu(evt);

            // create states
            var types = TypeCache.GetTypesDerivedFrom<State>();
            foreach (var type in types)
            {
                evt.menu.AppendAction("Create State/" + type.Name, (a) => CreateNode(type));
            }
        }

        private void CreateNode(System.Type type)
        {
            if (m_machine != null)
            {
                State state = m_machine.CreateState(type);
                CreateStateNode(state);
            }
        }

        internal void PopulateView(FiniteStateMachine fsm)
        {
            m_machine = fsm;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (m_machine != null)
            {
                // create states
                m_machine.m_states.ForEach(n => CreateStateNode(n));

                // create edges
                m_machine.m_states.ForEach(n =>
                {
                    StateNodeControl from = FindStateControl(n);
                    n.m_connectedStates.ForEach(c =>
                    {
                        StateNodeControl to = FindStateControl(c);
                        Edge edge = from.Output.ConnectTo(to.Input);
                        AddElement(edge);
                    });
                });
            }
        }

        void CreateStateNode(State state)
        {
            if (state != null)
            {
                StateNodeControl snc = new StateNodeControl(state);
                AddElement(snc);
            }
        }

        StateNodeControl FindStateControl(State state)
        {
            return GetNodeByGuid(state.ID.ToString()) as StateNodeControl;
        }

        public void UpdateNodeStates()
        {
            nodes.ForEach(n => 
            {
                if (n is StateNodeControl snc)
                {
                    snc.UpdateRunningState();
                }
            });
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            // remove elements
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(e =>
                {
                    if (e is StateNodeControl snc)
                    {
                        m_machine.DeleteState(snc.State);
                    }

                    if (e is Edge edge)
                    {
                        StateNodeControl from = edge.output.node as StateNodeControl;
                        StateNodeControl to = edge.input.node as StateNodeControl;
                        m_machine.RemoveConnection(from.State, to.State);
                    }
                });
            }

            // create edges
            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge => 
                {
                    StateNodeControl from = edge.output.node as StateNodeControl;
                    StateNodeControl to = edge.input.node as StateNodeControl;
                    m_machine.ConnectStates(from.State, to.State);
                });
            }

            return graphViewChange;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }
    }
}
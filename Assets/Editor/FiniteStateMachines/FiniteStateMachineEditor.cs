using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace FiniteStateMachines
{
    public class FiniteStateMachineEditor : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset         m_VisualTreeAsset = default;

        private FiniteStateMachineView  m_view;

        [MenuItem("AICourse/FiniteStateMachineEditor")]
        public static void ShowFSM()
        {
            FiniteStateMachineEditor wnd = GetWindow<FiniteStateMachineEditor>();
            wnd.titleContent = new GUIContent("FSM");
        }

        [OnOpenAsset]
        public static bool OpenAITree(int instanceID, int line)
        {
            FiniteStateMachine fsm = EditorUtility.InstanceIDToObject(instanceID) as FiniteStateMachine;
            State state = EditorUtility.InstanceIDToObject(instanceID) as State;

            if (fsm == null && state != null)
            {
                fsm = AssetDatabase.LoadAssetAtPath<FiniteStateMachine>(AssetDatabase.GetAssetPath(instanceID));
            }

            if (fsm != null)
            {
                FiniteStateMachineEditor treeWindow = GetWindow<FiniteStateMachineEditor>();
                Selection.activeObject = fsm;
                treeWindow.titleContent = new GUIContent(fsm.name);
                return true;
            }

            return false;
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += PlayModeChanged;
        }

        private void OnDestroy()
        {
            EditorApplication.playModeStateChanged -= PlayModeChanged;
        }

        private void PlayModeChanged(PlayModeStateChange obj)
        {
            CreateGUI();
            m_view?.PopulateView(null);
            titleContent = new GUIContent("FSM");
        }

        public void CreateGUI()
        {
            if (m_view == null)
            {
                VisualElement root = rootVisualElement;
                m_VisualTreeAsset.CloneTree(root);
                m_view = root.Query<FiniteStateMachineView>();
            }

            OnSelectionChange();
        }

        private void OnSelectionChange()
        {
            // selected a machine?
            FiniteStateMachine fsm = Selection.activeObject as FiniteStateMachine;

            // runtime brain?
            if (fsm == null &&
                Application.isPlaying &&
                Selection.activeGameObject != null)
            {
                Brain brain = Selection.activeGameObject.GetComponent<Brain>();
                fsm = brain?.Machine;
            }

            // got a machine?
            if (fsm != null)
            {
                m_view.PopulateView(fsm);
                titleContent = new GUIContent(fsm.name);
            }
        }

        private void OnInspectorUpdate()
        {
            m_view?.UpdateNodeStates();
        }
    }
}
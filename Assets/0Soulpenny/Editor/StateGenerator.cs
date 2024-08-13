using UnityEditor;
using UnityEngine;
using System.IO;

public class StateGenerator : EditorWindow
{
    private string stateName = "NewState";
    private int selectedStateTypeIndex = 0; // Track the selected index
    private string[] stateTypes = new string[] { "Movement", "Interaction", "Social" };
    private string folderPath = "Assets/Scripts/States/";

    [MenuItem("Tools/State Generator")]
    public static void ShowWindow()
    {
        GetWindow<StateGenerator>("State Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("State Generator", EditorStyles.boldLabel);

        stateName = EditorGUILayout.TextField("State Name", stateName);
        selectedStateTypeIndex = EditorGUILayout.Popup("State Type", selectedStateTypeIndex, stateTypes);
        folderPath = EditorGUILayout.TextField("Folder Path", folderPath);

        if (GUILayout.Button("Generate State"))
        {
            GenerateStateClass();
        }
    }

    private void GenerateStateClass()
    {
        string stateType = stateTypes[selectedStateTypeIndex];
        string className = GetStateClassName();
        string baseClass = GetBaseClassName(stateType);
        string fullClassName = $"{stateType}{className}";

        string classContent =
$@"using UnityEngine;
using ECM2;
using Animancer;

namespace Soul
{{
    public class {fullClassName} : {baseClass}
    {{
        private FPPlayerCharacter character;

        public {fullClassName}(StateMachine stateMachine, FPPlayerCharacter character, {GetAnimationSetType(stateType)} animSet) 
            : base(stateMachine, animSet)
        {{
            this.character = character;
        }}

        public override void Enter()
        {{
            Debug.Log(""{fullClassName} state entered."");
        }}

        public override void Exit()
        {{
            Debug.Log(""{fullClassName} state exited."");
        }}

        public override void Execute()
        {{
            Debug.Log(""{fullClassName} state executed."");
        }}

        public override IState GetNextState()
        {{
            // Define transition logic here
            return null;
        }}
    }}
}}";

        string filePath = Path.Combine(folderPath, $"{fullClassName}.cs");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        File.WriteAllText(filePath, classContent);
        AssetDatabase.Refresh();
    }

    private string GetStateClassName()
    {
        return stateName.EndsWith("State") ? stateName : $"{stateName}State";
    }

    private string GetBaseClassName(string stateType)
    {
        switch (stateType)
        {
            case "Movement":
                return "MovementStateBase";
            case "Interaction":
                return "InteractionStateBase";
            case "Social":
                return "SocialStateBase";
            default:
                return "BaseState";
        }
    }

    private string GetAnimationSetType(string stateType)
    {
        switch (stateType)
        {
            case "Movement":
                return "AnimationSetMovement";
            case "Interaction":
                return "AnimationSetInteraction";
            case "Social":
                return "AnimationSetSocial";
            default:
                return "AnimationSetBase";
        }
    }
}

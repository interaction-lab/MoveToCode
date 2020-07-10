using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ResourcePathConstants : MonoBehaviour {

        // Prefabs
        public static string PrefabFolder = "Prefabs/";
        public static string ExerciseJsonFolder = "ExerciseJsons/";
        public static string CodeBlockPrefabFolder = PrefabFolder + "CodeBlockPrefabs/";
        //public static string ExercisePrefabFolder = PrefabFolder + "ExercisePrefabs/";

        public static string VariableCodeBlockPrefab = CodeBlockPrefabFolder + "VariableCodeBlock";
        public static string CodeBlockTextPrefab = CodeBlockPrefabFolder + "CodeBlockText";
        public static string VariableCodeBlockCollectionPrefab = CodeBlockPrefabFolder + "VariableCodeBlockCollection";
        public static string PrintCodeBlockPrefab = CodeBlockPrefabFolder + "PrintCodeBlock";
        public static string ConditionBlockPrefab = CodeBlockPrefabFolder + "ConditionalCodeBlock";
        public static string IfCodeBlockPrefab = CodeBlockPrefabFolder + "IfCodeBlock";
        public static string IntCodeBlockPrefab = CodeBlockPrefabFolder + "IntCodeBlock";
        public static string MathCodeBlockPrefab = CodeBlockPrefabFolder + "MathCodeBlock";
        public static string SetVariableCodeBlockPrefab = CodeBlockPrefabFolder + "SetVariableCodeBlock";
        public static string StringCodeBlockPrefab = CodeBlockPrefabFolder + "StringCodeBlock";
        public static string WhileCodeBlockPrefab = CodeBlockPrefabFolder + "WhileCodeBlock";
        public static string CharCodeBlockPrefab = CodeBlockPrefabFolder + "CharCodeBlock";
        public static string ArrayCodeBlockPrefab = CodeBlockPrefabFolder + "ArrayCodeBlock";
        public static string ElementCodeBlockPrefab = CodeBlockPrefabFolder + "ElementCodeBlock";
        public static string ArrayIndexCodeBlockPrefab = CodeBlockPrefabFolder + "ArrayIndexCodeBlock";

        public static string ExercisePrefab = PrefabFolder + "Exercise";

        public static Dictionary<string, GameObject> codeBlockDictionary = new Dictionary<string, GameObject> {
            {"Print", Resources.Load<GameObject>(PrintCodeBlockPrefab)},
            {"Conditional", Resources.Load<GameObject>(ConditionBlockPrefab)},
            {"If", Resources.Load<GameObject>(IfCodeBlockPrefab)},
            {"Int", Resources.Load<GameObject>(IntCodeBlockPrefab)},
            {"Math", Resources.Load<GameObject>(MathCodeBlockPrefab)},
            {"SetVar", Resources.Load<GameObject>(SetVariableCodeBlockPrefab)},
            {"String", Resources.Load<GameObject>(StringCodeBlockPrefab)},
            {"While", Resources.Load<GameObject>(WhileCodeBlockPrefab)},
            {"Char", Resources.Load<GameObject>(CharCodeBlockPrefab)},
            {"Array", Resources.Load<GameObject>(ArrayCodeBlockPrefab)},
            {"ArrayIndex", Resources.Load<GameObject>(ArrayIndexCodeBlockPrefab)},
            {"Variable", Resources.Load<GameObject>(VariableCodeBlockPrefab)}
        };

        // Materials
        public static string MaterialFolder = "Materials/";
        public static string OutlineSnapColliderMaterial = MaterialFolder + "OutlineSnapCollider";
        public static string OutlineCodeBlockMaterial = MaterialFolder + "OutlineCodeBlock";
        public static string FakeButtonPressParticleMaterial = MaterialFolder + "FakeButtonPressParticle";

        // Audio
        public static string AudioFolder = "Audio/";
        public static string CorrectSound = AudioFolder + "correct";
        public static string IncorrectSound = AudioFolder + "incorrect";
        public static string PoofSound = AudioFolder + "poof";
        public static string PopSound = AudioFolder + "pop";
        public static string SnapSound = AudioFolder + "snap";
        public static string SpawnSound = AudioFolder + "spawn";
        public static string ComputerNoises = AudioFolder + "ComputerNoises";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ResourcePathConstants : MonoBehaviour {

        // Prefabs
        public static string PrefabFolder = "Prefabs/";
        public static string CodeBlockPrefabFolder = PrefabFolder + "CodeBlockPrefabs/";
        public static string ExercisePrefabFolder = PrefabFolder + "ExercisePrefabs/";

        public static string VariableCodeBlockPrefab = CodeBlockPrefabFolder + "VariableCodeBlock";
        public static string CodeBlockTextPrefab = CodeBlockPrefabFolder + "CodeBlockText";
        public static string VariableCodeBlockCollectionPrefab = CodeBlockPrefabFolder + "VariableCodeBlockCollection";

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
    }
}
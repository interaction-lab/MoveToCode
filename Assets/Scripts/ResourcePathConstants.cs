using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class ResourcePathConstants : MonoBehaviour {

        // Prefabs
        public static string PrefabFolder = "Prefabs/";
        public static string VariableCodeBlockPrefab = PrefabFolder + "VariableCodeBlock";
        public static string CodeBlockTextPrefab = PrefabFolder + "CodeBlockText";

        // Materials
        public static string MaterialFolder = "Materials/";
        public static string OutlineSnapColliderMaterial = MaterialFolder + "OutlineSnapCollider";
    }
}
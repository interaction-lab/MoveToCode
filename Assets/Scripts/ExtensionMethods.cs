using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public static class ExtensionMethods {

        // List Extensions
        public static bool Empty<T>(this List<T> l) {
            return l.Count == 0;
        }

        public static void Resize<T>(this List<T> l, int desiredSize) {
            while (l.Count < desiredSize) {
                l.Add(default(T));
            }
        }

        // Stack Extensions
        public static bool Empty<T>(this Stack<T> s) {
            return s.Count == 0;
        }

        // Hashset Extensions
        public static bool Empty<T>(this HashSet<T> h) {
            return h.Count == 0;
        }

        // Dictionary Extensions
        public static bool Empty<T, V>(this Dictionary<T, V> h) {
            return h.Count == 0;
        }


        // Transform Extensions
        public static void SnapToParent(this Transform t, Transform prospectiveParent) {
            t.rotation = prospectiveParent.rotation;
            t.SetParent(prospectiveParent);
        }
        public static void SnapToParent(this Transform t, Transform prospectiveParent, Vector3 newLocalPosition) {
            t.rotation = prospectiveParent.rotation;
            t.SetParent(prospectiveParent);
            t.localPosition = newLocalPosition;
        }
        public static void SnapToCodeBlockManager(this Transform t) {
            SnapToParent(t, CodeBlockManager.instance.transform);
            t.localScale = Vector3.one;
        }
        public static void ResetCodeBlockSize(this Transform t) {
            Transform origParent = t.parent;
            SnapToCodeBlockManager(t);
            SnapToParent(t, origParent);
        }

        // Gameobject Extensions
        public static string TryGetCodeBlockNameOfGameObject(this GameObject go) {
            // check object mesh
            CodeBlockObjectMesh cbom = go.GetComponentInParent<CodeBlockObjectMesh>();
            if (cbom != null) {
                return cbom.GetMyCodeBlock().name;
            }
            SnapCollider sc = go.GetComponent<SnapCollider>();
            if (sc != null) {
                return sc.GetMyCodeBlock().name;
            }
            return go.name;
        }

    }
}
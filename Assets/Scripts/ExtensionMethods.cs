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

        // Transform Extensions
        // TODO: look into this with weird object scaling
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
    }
}
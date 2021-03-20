﻿using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;
using TMPro;
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

        // String Extensions
        public static string ReplaceFirst(this string text, string search, string replace) {
            int pos = text.IndexOf(search);
            if (pos < 0) {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        // TextMesh Extensions
        public static void ForceTextUpdate(this TextMeshPro tmp) {
            tmp.enabled = false;
            tmp.enabled = true;
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

        public static void RotateTowardsUser(this Transform t) {
            t.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }

        // Gameobject Extensions
        public static string TryGetNiceNameOfObjectForLogging(this GameObject go) {
            CodeBlockObjectMesh cbom = go.GetComponentInParent<CodeBlockObjectMesh>();
            if (cbom != null) {
                return cbom.GetMyCodeBlock().name;
            }
            SnapCollider sc = go.GetComponent<SnapCollider>();
            if (sc != null) {
                return sc.MyCodeBlock.name;
            }
            PressableButtonHoloLens2 pbh2 = go.GetComponentInParent<PressableButtonHoloLens2>();
            if (pbh2 != null) {
                return pbh2.name;
            }

            return go.name;
        }

        public static CodeBlock GetDestructableCodeBlockObject(this GameObject go) {
            CodeBlock result = null;
            CodeBlockObjectMesh cbom = go.GetComponentInParent<CodeBlockObjectMesh>();
            if (cbom != null) {
                result = cbom.GetMyCodeBlock();
            }
            SnapCollider sc = go.GetComponent<SnapCollider>();
            if (sc != null) {
                result = sc.MyCodeBlock;
            }
            // StartCodeBlock is indestructible
            if (result == StartCodeBlock.instance) {
                result = null;
            }
            return result;
        }

        // ObjectManipulator
        public static void RemoveTwoHandedScaling(this ObjectManipulator om) {
            om.TwoHandedManipulationType = om.TwoHandedManipulationType &
                (Microsoft.MixedReality.Toolkit.Utilities.TransformFlags.Move | Microsoft.MixedReality.Toolkit.Utilities.TransformFlags.Rotate);
        }
    }
}
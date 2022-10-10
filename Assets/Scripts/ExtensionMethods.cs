using Microsoft.MixedReality.Toolkit.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.XR.ARFoundation;

namespace MoveToCode {
    public static class ExtensionMethods {
        // List Extensions
        public static bool Empty<T>(this List<T> l) {
            return l.Count == 0;
        }

        public static int MaxIndex<T>(this IEnumerable<T> source) {
            IComparer<T> comparer = Comparer<T>.Default;
            using (var iterator = source.GetEnumerator()) {
                if (!iterator.MoveNext()) {
                    throw new InvalidOperationException("Empty sequence");
                }
                int maxIndex = 0;
                T maxElement = iterator.Current;
                int index = 0;
                while (iterator.MoveNext()) {
                    index++;
                    T element = iterator.Current;
                    if (comparer.Compare(element, maxElement) > 0) {
                        maxElement = element;
                        maxIndex = index;
                    }
                }
                return maxIndex;
            }
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

        // Queue Extensions
        public static bool Empty<T>(this Queue<T> h) {
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

        public static string TrimLastCharacter(this string str) {
            if (string.IsNullOrEmpty(str)) {
                return str;
            }
            else {
                return str.TrimEnd(str[str.Length - 1]);
            }
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

        public static T GetComponentInChildrenOnlyDepthOne<T>(this Transform t) where T : Component {
            foreach (Transform tmp in t) {
                T cmp = tmp.GetComponent<T>();
                if (cmp != null) {
                    return cmp;
                }
            }
            return null;
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

        public static T GetComponentInChildrenOnlyDepthOne<T>(this GameObject go) where T : Component {
            return go.transform.GetComponentInChildrenOnlyDepthOne<T>();
        }

        // ObjectManipulator
        public static void RemoveTwoHandedScaling(this ObjectManipulator om) {
            om.TwoHandedManipulationType = om.TwoHandedManipulationType &
                (Microsoft.MixedReality.Toolkit.Utilities.TransformFlags.Move | Microsoft.MixedReality.Toolkit.Utilities.TransformFlags.Rotate);
        }

        // Float
        public static float TimeSince(this float f) {
            return Time.time - f;
        }

        // AR
        public static string ImgName(this ARTrackedImage img) {
            return img.referenceImage.name;
        }

        // Vector3
        public static Vector3 Norm180Minus180(this Vector3 v) {
            // normalize vector 3 angles between -180 and 180
            v.x = NormalizeAngle(v.x);
            v.y = NormalizeAngle(v.y);
            v.z = NormalizeAngle(v.z);
            return v;
        }

        public static float NormalizeAngle(float x) {
            // normalize angle between -180 and 180
            x = x % 360;
            if (x < -180) {
                x += 360;
            }
            else if (x > 180) {
                x -= 360;
            }
            return x;
        }

        public static float NormalizedAngleSubtract(float x, float y) {
            // subtract two angles and normalize the result between -180 and 180
            return NormalizeAngle(x - y);
        }
        // Animator
        public static bool IsAnimationInAnimator(this Animator animator, string animationName) {
            if (animator.runtimeAnimatorController.animationClips.Length > 0) {
                foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips) {
                    if (clip.name == animationName) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsThisAnimationPlaying(this Animator animator, string animationName) {
            try {
                return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == animationName;
            }
            catch (System.Exception) {
                return false;
            }
        }


        public static bool IsFullyIdle(this Animator animator) {
            return animator.enabled && (animator.IsThisAnimationPlaying("Idle") || animator.IsThisAnimationPlaying("neutral"));
        }
    }
}
using System.Collections.Generic;

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
    }
}
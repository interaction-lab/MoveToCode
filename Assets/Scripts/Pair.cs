using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class Pair<T1, T2> {
        public T1 First { get; set; }
        public T2 Second { get; set; }

        public Pair() {
            First = default(T1);
            Second = default(T2);
        }
        public Pair(T1 _first, T2 _second) {
            First = _first;
            Second = _second;
        }
        public override int GetHashCode() {
            if (First == null && Second == null) {
                return 0;
            }
            else if (First == null) {
                return Second.GetHashCode() + 1; // +1 to avoid this.First == other.Second
            }
            else if (Second == null) {
                return First.GetHashCode();
            }
            else {
                return First.GetHashCode() ^ Second.GetHashCode();
            }
        }

        public override bool Equals(object obj) {
            Pair<T1, T2> other = obj as Pair<T1, T2>;
            return other != null &&
                EqualityComparer<T1>.Default.Equals(First, other.First) &&
                EqualityComparer<T2>.Default.Equals(Second, other.Second);
        }
    }
}

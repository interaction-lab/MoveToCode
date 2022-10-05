using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TheKiwiCoder;
using System.Linq;

namespace MoveToCode {
    public class KuriBTEventRouter : Singleton<KuriBTEventRouter> {
        Dictionary<string, UnityEvent> events = new Dictionary<string, UnityEvent>();
        public string[] eventNames;

        public void AddEvent(string eventName, UnityEvent evt) {
            if (!events.ContainsKey(eventName)) {
                events.Add(eventName, evt);
            }
            else {
                events[eventName] = evt;
            }
            eventNames = events.Keys.ToArray();
        }

        public UnityEvent GetEvent(string eventName) {
            if (events.ContainsKey(eventName)) {
                Debug.Log(events[eventName]);
                return events[eventName];
            }
            return null;
        }
    }
}
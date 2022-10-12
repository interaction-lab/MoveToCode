using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoveToCode;

namespace TheKiwiCoder {

    // This is the blackboard container shared between all nodes.
    // Use this to store temporary data that multiple nodes need read and write access to.
    // Add other properties here that make sense for your specific use case.
    [System.Serializable]
    public class Blackboard {
        public Vector3 goalPosition;
        public Vector3 goalRotation;
        public Transform objToPointTo;
        public Transform objToLookAt;
        public Transform objToTurnTo;
        public Transform objToMoveTo;
        public float headSpeed = 2f;
        public float armSpeed = 1f;
        public float bodySpeed = 4f;
        public float timeToPoint = 5f;
        public float BodyAnimatorSemaphoreCount = 0;
        public float ArmAnimatorSemaphoreCount = 0;
        public KuriController.EMOTIONS emotion;
        public string pointToPaperName;
    }
}
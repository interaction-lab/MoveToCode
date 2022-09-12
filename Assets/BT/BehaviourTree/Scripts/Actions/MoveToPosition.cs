using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using MoveToCode;

public class MoveToPosition : ActionNode {
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;
    VirtualKuriAudio audioManager;
    public VirtualKuriAudio AudioManager {
        get {
            if (audioManager == null) {
                audioManager = TutorKuriTransformManager.instance.GetComponent<VirtualKuriAudio>();
            }
            return audioManager;
        }
    }

    TutorKuriTransformManager kuriTransformManager;

    protected override void OnStart() {
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.destination = blackboard.goalPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
        kuriTransformManager = TutorKuriTransformManager.instance;
        context.agent.isStopped = false;
        // AudioManager.Play(); // used to play kuri moving sound
    }

    protected override void OnStop() {
        context.agent.isStopped = true;
        // AudioManager.Stop();
    }

    protected override State OnUpdate() {
        context.agent.destination = blackboard.goalPosition;
        kuriTransformManager.Position = new Vector3(kuriTransformManager.Position.x, kuriTransformManager.GroundYCord, kuriTransformManager.Position.z);
        if (context.agent.pathPending) {
            return State.Running;
        }

        if (context.agent.remainingDistance < tolerance) {
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid) {
            return State.Failure;
        }

        return State.Running;
    }
}

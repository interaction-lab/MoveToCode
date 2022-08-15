using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using NRISVTE;

public class MoveToPosition : ActionNode {
    public float speed = 5;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public float acceleration = 40.0f;
    public float tolerance = 1.0f;
    KuriDriveAudioManager audioManager;
    public KuriDriveAudioManager AudioManager {
        get {
            if (audioManager == null) {
                audioManager = KuriDriveAudioManager.instance;
            }
            return audioManager;
        }
    }

    KuriTransformManager kuriTransformManager;

    protected override void OnStart() {
        context.agent.stoppingDistance = stoppingDistance;
        context.agent.speed = speed;
        context.agent.destination = blackboard.goalPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
        kuriTransformManager = KuriManager.instance.GetComponent<KuriTransformManager>();
        context.agent.isStopped = false;
        AudioManager.Play();
    }

    protected override void OnStop() {
        context.agent.isStopped = true;
        AudioManager.Stop();
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

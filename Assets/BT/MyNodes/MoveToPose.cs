using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class MoveToPose : ActionNode {
        public float speed = 1; // m/s
        public float slowDownDistance = 0.1f; // m
        public float stoppingDistance = 0.01f; // m
        public bool updateRotation = true; // if false, the agent will not rotate to face the goal
        private Vector3 goalPosition, goalRotation;
        float minSpeed = 0.1f;

        PlayerTransformManager playerTransformManager;
        KuriTransformManager kuriTransformManager;

        protected override void OnStart() {
            SetGoalFromBB();
            playerTransformManager = Camera.main.GetComponent<PlayerTransformManager>();
            kuriTransformManager = KuriManager.instance.GetComponent<KuriTransformManager>();
        }

        protected override void OnStop() {
        }

        private void SetGoalFromBB(){
            goalPosition = blackboard.goalPosition;
            goalRotation = blackboard.goalRotation;
        }

        protected override State OnUpdate() {
            // hacking in set position
            blackboard.goalPosition = playerTransformManager.Position;
            blackboard.goalPosition.y = kuriTransformManager.GroundYCord;
            blackboard.goalRotation = Quaternion.LookRotation(playerTransformManager.Position - kuriTransformManager.Position, Vector3.up).eulerAngles;
            
            SetGoalFromBB();
            
            if (updateRotation) {
                context.kuriTransformManager.Rotation = Quaternion.Euler(
                    context.kuriTransformManager.Rotation.eulerAngles.x,
                    goalRotation.y,
                    context.kuriTransformManager.Rotation.eulerAngles.z);
            }
            if (Vector3.Distance(context.kuriTransformManager.Position, goalPosition) < stoppingDistance) {
                context.kuriTransformManager.Position = goalPosition;
                return State.Success;
            }
            float distToGoal = Vector3.Distance(context.kuriTransformManager.Position, goalPosition);
            if (distToGoal < slowDownDistance) {
                float scaleFactor = Mathf.Max(distToGoal / slowDownDistance, minSpeed);
                context.kuriTransformManager.Position = Vector3.MoveTowards(context.kuriTransformManager.Position, goalPosition, speed * Time.deltaTime * scaleFactor);
            }
            else {
                context.kuriTransformManager.Position = Vector3.MoveTowards(context.kuriTransformManager.Position, goalPosition, speed * Time.deltaTime);
            }

            return State.Running;
        }
    }
}

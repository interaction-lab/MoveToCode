using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

namespace NRISVTE {
    public class TurnHeadToLookAtUser : ActionNode {
        public float speed = 5; // deg per second
        PlayerTransformManager _playerTransformManager;
        PlayerTransformManager playerTransformManager {
            get {
                if (_playerTransformManager == null) {
                    _playerTransformManager = Camera.main.GetComponent<PlayerTransformManager>();
                }
                return _playerTransformManager;
            }
        }
        KuriHeadPositionManager _kuriHeadPositionManager;
        KuriHeadPositionManager kuriHeadPositionManager {
            get {
                if (_kuriHeadPositionManager == null) {
                    _kuriHeadPositionManager = KuriHeadPositionManager.instance;
                }
                return _kuriHeadPositionManager;
            }
        }
        float elapsedTime = 0, timeItShouldTake = 0;
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            Vector3 dir = playerTransformManager.Position - kuriHeadPositionManager.HeadPosition;
            Quaternion rot = Quaternion.LookRotation(dir);
            kuriHeadPositionManager.HeadRotation = Quaternion.Slerp(kuriHeadPositionManager.HeadRotation, rot, speed * Time.deltaTime);
            // if rotation is close enough, stop
            if (Quaternion.Angle(kuriHeadPositionManager.HeadRotation, rot) < 0.1f) {
                kuriHeadPositionManager.HeadRotation = rot;
                return State.Success;
            }
            return State.Running;
        }
    }
}

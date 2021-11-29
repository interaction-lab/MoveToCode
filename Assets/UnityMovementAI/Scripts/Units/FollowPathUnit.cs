using UnityEngine;

namespace UnityMovementAI
{
    [RequireComponent(typeof(FollowPath))]
    public class FollowPathUnit : MonoBehaviour
    {
        public bool pathLoop = false;

        public bool reversePath = false;

        public LinePath path;

        SteeringBasics steeringBasics;
        FollowPath followPath;

        void Start()
        {
            path.CalcDistances();
            steeringBasics = GetComponent<SteeringBasics>();
            followPath = GetComponent<FollowPath>();
        }

        void FixedUpdate()
        {
            path.Draw();

            if (reversePath && followPath.IsAtEndOfPath(path))
            {
                path.ReversePath();
            }

            Vector3 accel = followPath.GetSteering(path, pathLoop);

            steeringBasics.Steer(accel);
            steeringBasics.LookWhereYoureGoing();
        }

        private void OnEnable() {
            path.CalcDistances();
        }
    }
}
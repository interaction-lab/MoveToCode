using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MoveToCode;
namespace TheKiwiCoder {

    // The context is a shared object every node has access to.
    // Commonly used components and subsytems should be stored here
    // It will be somewhat specfic to your game exactly what to add here.
    // Feel free to extend this class 
    public class Context {
        public GameObject gameObject;
        public Transform transform;
        public Animator mainAnimator, armAnimator;
        public Rigidbody physics;
        public NavMeshAgent agent;
        public SphereCollider sphereCollider;
        public BoxCollider boxCollider;
        public CapsuleCollider capsuleCollider;
        public TutorKuriTransformManager kuriTransformManager;
        public KuriController KController;
        public KuriBTEventRouter eventRouter;
        public KuriArms kuriArms;

        // Add other game specific systems here

        public static Context CreateFromGameObject(GameObject gameObject) {
            // Fetch all commonly used components
            Context context = new Context();
            context.gameObject = gameObject;
            context.transform = gameObject.transform;
            context.mainAnimator = gameObject.GetComponent<Animator>();
            context.armAnimator = KuriArms.instance.ArmAnimator;
            context.physics = gameObject.GetComponent<Rigidbody>();
            context.agent = gameObject.GetComponent<NavMeshAgent>();
            context.sphereCollider = gameObject.GetComponent<SphereCollider>();
            context.boxCollider = gameObject.GetComponent<BoxCollider>();
            context.capsuleCollider = gameObject.GetComponent<CapsuleCollider>();

            // M2C specific components
            context.kuriTransformManager = TutorKuriTransformManager.instance;
            context.KController = TutorKuriManager.instance.KController;
            context.eventRouter = KuriBTEventRouter.instance;
            context.kuriArms = KuriArms.instance;
            return context;
        }
    }
}
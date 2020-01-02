using UnityEngine;

// Empty Class to denote type of GameObject
// See CodeBlockSnap for more information
namespace MoveToCode {
    public class SnapColliders : MonoBehaviour {

        // TODO: should be one function
        public void DisableAllCollidersAndChildrenColliders() {
            foreach (SnapCollider sc in transform.GetComponentsInChildren<SnapCollider>()) {
                gameObject.SetActive(false);
            }
            CodeBlock myCodeBlock = transform.parent.GetComponent<CodeBlock>();
            foreach (CodeBlock c in myCodeBlock.GetAllAttachedCodeBlocks()) {
                c.GetSnapColliders()?.DisableAllCollidersAndChildrenColliders();
            }
        }

        public void EnableAllCollidersAndChildrenColliders() {
            foreach (SnapCollider sc in transform.GetComponentsInChildren<SnapCollider>()) {
                gameObject.SetActive(true);
            }
            CodeBlock myCodeBlock = transform.parent.GetComponent<CodeBlock>();
            foreach (CodeBlock c in myCodeBlock.GetAllAttachedCodeBlocks()) {
                c.GetSnapColliders()?.EnableAllCollidersAndChildrenColliders();
            }
        }
    }
}
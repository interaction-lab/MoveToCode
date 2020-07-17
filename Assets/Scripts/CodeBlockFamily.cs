using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace MoveToCode {
    public class CodeBlockFamily : MonoBehaviour {

        public GameObject[] blocksInFamily;

        private void Awake() {
            CodeBlockMenuManager.instance.SetFamily(this);
            HideFamily();
        }

        private void Start() {
            InstantiateBlocksInFamily();
        }

        public void ShowFamily() {
            gameObject.SetActive(true);
        }

        public void HideFamily() {
            gameObject.SetActive(false);
        }

        public void InstantiateBlocksInFamily() {
            for (int i = 0; i < blocksInFamily.Length; i++) {
                GameObject go = Instantiate(blocksInFamily[i]);
                go.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f - i / 5f, transform.position.z);
                go.AddComponent<CloneOnDrag>().SetCodeBlockType(blocksInFamily[i]);
                go.GetComponent<CodeBlock>().SetIsMenuBlock(true);
                go.transform.SnapToParent(transform);
                go.transform.localScale = Vector3.one;
            }
        }
    }
}
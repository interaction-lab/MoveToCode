using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace MoveToCode {
    public class CodeBlockFamily : MonoBehaviour {

        public GameObject[] blocksInFamily;
        MeshRenderer shelfMeshRenderer;

        private void Awake() {
            CodeBlockMenuManager.instance.SetFamily(this);
            shelfMeshRenderer = gameObject.transform.parent.parent.transform.GetChild(0).GetComponent<MeshRenderer>();
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
                //Vector3 currBlockSize = GetBlockSize(go);
                go.transform.position = new Vector3(shelfMeshRenderer.bounds.center.x, shelfMeshRenderer.bounds.extents.y - i * GetBlockHeight(go)/3, transform.position.z);
                
                //set codeblocks as blocks in menu
                go.AddComponent<CloneOnDrag>().SetCodeBlockType(blocksInFamily[i]);
                go.GetComponent<CodeBlock>().SetIsMenuBlock(true);
                go.transform.SnapToParent(transform);
                go.transform.localScale = Vector3.one;
            }
        }

        /*private Vector3 GetBlockSize(GameObject goIn) {
            Vector3 size = Vector3.zero;
            goIn.transform.GetChild(0);
            int debugCount = 0;
            MeshRenderer[] allObjectRenderers = goIn.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mRenderer in allObjectRenderers) {
                size += mRenderer.bounds.size;
                debugCount++;
            }
            Debug.Log(debugCount);
            return size;
        }*/

        private float GetBlockHeight(GameObject goIn) {
            float height = goIn.GetComponentInChildren<CodeBlockObjectMesh>().GetBlockVerticalSize();
            Debug.Log(height);
            return height;
        }
    }
}
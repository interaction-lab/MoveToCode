using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

namespace MoveToCode {
    public class CloneOnDrag : MonoBehaviour {
        ManipulationHandler manipulationHandler;
        Vector3 startingPosition;
        GameObject clone;
        GameObject codeBlockType;
        bool stillInContactWithOriginal;
        bool blockStillInMenu;

        private MeshRenderer renderer;
        private MeshRenderer cloneRenderer;

        private void Awake() {
            startingPosition = transform.position;
            blockStillInMenu = true;
            renderer = GetComponent<MeshRenderer>();
        }

        public void SetCodeBlockType(GameObject cb) {
            codeBlockType = cb;
        }

        public void OnTriggerEnter() {
            Debug.Log("qwerty");
            //if(cloneCol.GetComponent<CodeBlock>() == codeBlockType.GetComponent<CodeBlock>()) {
                stillInContactWithOriginal = true;
            //}
            if (cloneRenderer != null) {
                foreach (Material mat in cloneRenderer.materials) {
                    mat.SetFloat("_Outline", 0.15f);
                }
            }
        }

        public void OnTriggerExit(Collider cloneCol) {
            if (cloneCol.GetComponent<CodeBlock>() == codeBlockType.GetComponent<CodeBlock>()) {
                //Destroy(gameObject);
                stillInContactWithOriginal = false;
            }
            if (cloneRenderer != null) {
                foreach (Material mat in cloneRenderer.materials) {
                    mat.SetFloat("_Outline", 0f);
                }
            }
            if (renderer != null) {
                foreach (Material mat in renderer.materials) {
                    mat.SetFloat("_Outline", 0f);
                }
            }
        }

        public void OnEnable() {
            manipulationHandler = GetComponent<ManipulationHandler>();
            manipulationHandler.OnManipulationStarted.AddListener(StartedMotion);
            //manipulationHandler.OnManipulationEnded.AddListener(StoppedMotion);
            //Destroy(GetComponent<CodeBlockSnap>());
            //Destroy(GetComponent<CodeBlockArgumentList>());
        }

        /*private void StoppedMotion(ManipulationEventData arg0) {
            Debug.Log("asdf");
            if (stillInContactWithOriginal) {
                
                Destroy(gameObject);
            } else {
                
                transform.gameObject.AddComponent<CodeBlockSnap>();
                transform.SnapToCodeBlockManager();
                blockStillInMenu = false;
            }
            if (cloneRenderer != null) {
                foreach (Material mat in cloneRenderer.materials) {
                    mat.SetFloat("_Outline", 0f);
                }
            }
            if (renderer != null) {
                foreach (Material mat in renderer.materials) {
                    mat.SetFloat("_Outline", 0f);
                }
            }
        }*/

        private void StartedMotion(ManipulationEventData arg0) {
            if (blockStillInMenu) {
                transform.GetComponent<CodeBlock>().SetIsMenuBlock(false);
                clone = InstantiateBlock(codeBlockType, startingPosition);
                clone.GetComponent<CodeBlock>().SetIsMenuBlock(true);
                cloneRenderer = clone.GetComponent<MeshRenderer>();
                transform.SnapToCodeBlockManager();

                

                //menu block replaced by clone
                //Destroy(clone.GetComponent<CodeBlockSnap>());
                Destroy(transform.GetComponent<CloneOnDrag>());
                //transform.gameObject.AddComponent<CodeBlockArgumentList>();
                //transform.gameObject.AddComponent<CodeBlockSnap>();
            }
        }

        private GameObject InstantiateBlock(GameObject block, Vector3 spawnPos) {
            GameObject go = Instantiate(block, spawnPos, Quaternion.identity);
            go.AddComponent<CloneOnDrag>().SetCodeBlockType(codeBlockType);
            go.transform.SetParent(transform.parent);
            go.transform.localScale = Vector3.one;

            /*Collider[] allColliders = go.GetComponentsInChildren<Collider>();
            foreach (Collider col in allColliders) {
                col.isTrigger = true;
            }*/

            return go;
        }
    }
}
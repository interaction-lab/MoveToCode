using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MoveToCode {
    public class CreateVarInMemButton : MonoBehaviour {
        Interactable interactable;

        private void Awake() {
            interactable = GetComponent<Interactable>();
            interactable.OnClick.AddListener(SpawnVariableBlock);
        }

        private void SpawnVariableBlock() {
            FreePlayMenuManager.instance.InstantiateVariableBlockCollection();
        }
    }
}

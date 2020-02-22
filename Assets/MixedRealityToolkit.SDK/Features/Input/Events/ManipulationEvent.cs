// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine.Events;

namespace Microsoft.MixedReality.Toolkit.UI
{
    /// <summary>
    /// A UnityEvent callback containing a ManipulationEventData payload.
    /// </summary>
    [System.Serializable]
    public class ManipulationEvent : UnityEvent<ManipulationEventData> {
        public void AddListener(object v) {
            throw new NotImplementedException();
        }
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class SnapColliderArrayElement : SnapCollider {
        private HashSet<Type> _compatibleTypes = new HashSet<Type> { typeof(BasicDataType) };
        public HashSet<Type> compatibleTypes {
            get { return _compatibleTypes; }
            set { _compatibleTypes = value; }
        }
    }
}


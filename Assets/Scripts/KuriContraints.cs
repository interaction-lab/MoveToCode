using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
	public class KuriContraints {
		public static float HeadPanDeg = 45f;
		public static float HeadTiltDeg = 45f;
		public static float HeadPanRad = HeadPanDeg * Mathf.Deg2Rad;
		public static float HeadTiltRad = HeadTiltDeg * Mathf.Deg2Rad;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveToCode {
    public class MPTypeFactory {
        #region members
        public enum TYPE {
            WALL,
            BABYKURI,
            LEFTTURN,
            RIGHTTURN,
            LTURN,
            RESVERSELTURN,
            GOAL
        }
        #endregion

        #region unity
        #endregion

        #region public
        // North, South, East, West, Babykuri, Goal
        public MPType CreateMPType(TYPE type) {
            switch (type) {
                case TYPE.WALL:
                    return new MPType(true, true, false, false, false, false);
                case TYPE.BABYKURI:
                    return new MPType(true, true, true, true, true, false);
                case TYPE.LEFTTURN:
                    return new MPType(false, true, false, true, false, false);
                case TYPE.RIGHTTURN:
                    return new MPType(false, true, true, false, false, false);
                case TYPE.LTURN:
                    return new MPType(true, false, false, true, false, false);
                case TYPE.RESVERSELTURN:
                    return new MPType(true, false, true, false, false, false);
                case TYPE.GOAL:
                    return new MPType(true, true, true, true, false, true);
                default:
                    return new MPType(false, false, false, false, false, false); // null
            }
        }
        #endregion

        #region private
        #endregion
    }
}

using UnityEngine;
using RosSharp.RosBridgeClient.MessageTypes.MobileBaseDriver;
using Microsoft.MixedReality.Toolkit.Experimental.ColorPicker;

namespace RosSharp.RosBridgeClient {
    public class ChestLedPublisher : UnityPublisher<ChestLeds> {
        private ChestLeds message;

        protected override void Start() {
            base.Start();
            InitializeMessage();
        }

        private void InitializeMessage() {
            message = new ChestLeds();
            for (int i = 0; i < message.leds.Length; i++) {
                message.leds[i] = new Led();
            }
            SetColor(0, 0, 0);
        }

        public void SetColor(Color32 c) {
            SetColor(c.r, c.g, c.b);
        }

        public void SetColor(int red, int green, int blue) {
            //animPub.PublishAnim(AnimationPublisher.ANIMATION_CMD.smile);
            for (int i = 0; i < message.leds.Length; i++) {
                message.leds[i].red = (byte)red;
                message.leds[i].green = (byte)green;
                message.leds[i].blue = (byte)blue;
            }
            Publish(message);
        }

        public void SetBlue(int b) {
            for (int i = 0; i < message.leds.Length; i++) {
                message.leds[i].blue = (byte)b;
            }
            Publish(message);
        }

        public void SetRed(int r) {
            for (int i = 0; i < message.leds.Length; i++) {
                message.leds[i].red = (byte)r;
            }
            Publish(message);
        }

        public void SetGreen(int g) {
            for (int i = 0; i < message.leds.Length; i++) {
                message.leds[i].green = (byte)g;
            }
            Publish(message);
        }
    }
}
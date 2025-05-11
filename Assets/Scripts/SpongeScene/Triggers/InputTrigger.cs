using UnityEngine;
using UnityEngine.InputSystem;

namespace SpongeScene.Triggers
{
        public class InputTrigger : Trigger
        {
            [Header("Player Input Settings")] private Gamepad player1Gamepad; // Player 1's Gamepad
            private Gamepad player2Gamepad; // Player 2's Gamepad
            private bool player1PressedButton;
            private bool player2PressedButton;

            private void Start()
            {
                // Automatically assign gamepads (if they are connected)
                player1Gamepad = Gamepad.all.Count > 0 ? Gamepad.all[0] : null; // First Gamepad
                player2Gamepad = Gamepad.all.Count > 1 ? Gamepad.all[1] : null; // Second Gamepad
            }

            public override bool IsActivated()
            {
                return (player1PressedButton && player2PressedButton);
            }

            private void Update()
            {
                if (player1Gamepad.buttonNorth.wasPressedThisFrame)
                {
                    player1PressedButton = true;
                }

                if (player2Gamepad.buttonNorth.wasPressedThisFrame)
                {
                    player2PressedButton = true;
                }
            }
        }
    }

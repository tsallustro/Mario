using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Commands;

namespace Controllers
{
    public class GamepadController : IController
    {
        /* TODO - Handle up to 4 controllers */
        private readonly Dictionary<int, ICommand> buttonMapping;
        private GamePadState currentState;
        private GamePadState previousState;
        private GamePadState emptyState;

        public GamepadController()
        {
            emptyState = new GamePadState(); // Create empty state for comparison
            previousState = GamePad.GetState(PlayerIndex.One);
            buttonMapping = new Dictionary<int, ICommand>();
        }

        public void AddMapping(int key, ICommand command)
        {
            if (this.buttonMapping.ContainsKey(key))
            {
                this.buttonMapping.Remove(key);
                this.buttonMapping.Add(key, command);
            }
            else
            {
                this.buttonMapping.Add(key, command);
            }
        }

        // TODO: Consider moving these out of seperate methods and right into the update
        // execute on press
        private void HandleButtonPress(Buttons button)
        {
            buttonMapping[(int)button].Execute(1);
        }
        // execute on hold
        private void HandleButtonHold(Buttons button)
        {
            buttonMapping[(int)button].Execute(2);
        }
        // execute on release
        private void HandleButtonRelease(Buttons button)
        {
            buttonMapping[(int)button].Execute(3);
        }

        public void Update()
        {
            currentState = GamePad.GetState(PlayerIndex.One);

            if (currentState.IsConnected && currentState != emptyState)
            {
                var buttonsPressed = (Buttons[])Enum.GetValues(typeof(Buttons));

                foreach (var button in buttonsPressed)
                {
                    // Button Press press
                    if (currentState.IsButtonDown(button) && previousState.IsButtonUp(button) && buttonMapping.ContainsKey((int)button))
                    {
                        HandleButtonPress(button);
                    }
                    // Key Press hold down
                    if (currentState.IsButtonDown(button) && previousState.IsButtonDown(button) && buttonMapping.ContainsKey((int)button))
                    {
                        HandleButtonHold(button);
                    }
                    // Key Press release
                    if (currentState.IsButtonUp(button) && previousState.IsButtonDown(button) && buttonMapping.ContainsKey((int)button))
                    {
                        HandleButtonRelease(button);
                    }
                }
            }

            previousState = currentState;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Commands;
using OUpdater;
namespace Controllers
{
    public class GamepadController : IController
    {
        /* TODO - Handle up to 4 controllers */
        private readonly Dictionary<int, ICommand> buttonMapping;
        private GamePadState currentState;
        private GamePadState previousState;
        private GamePadState emptyState;
        private ObjectUpdater objectUpdater;

        // Default constructor
        public GamepadController(ObjectUpdater OU)
        {
            objectUpdater = OU;
        }

        public GamepadController()
        {
            emptyState = new GamePadState(); // Create empty state for comparison
            previousState = GamePad.GetState(PlayerIndex.One);
            buttonMapping = new Dictionary<int, ICommand>();
        }

        public void AddMapping(int key, ICommand command)
        {
            this.buttonMapping.Add(key, command);
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
                    if (currentState.IsButtonDown(button) &&  previousState.IsButtonUp(button) && buttonMapping.ContainsKey((int)button))
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

        // TEMPORARILY COMMENTED OUT... use this with Object updater and no commands
        /*public void Update()
        {
            previousState = currentState;
            currentState = GamePad.GetState(PlayerIndex.One);

            if (currentState.IsButtonDown(Buttons.Start) && previousState.IsButtonUp(Buttons.Start))
            {
                objectUpdater.quitGame = true;
            }
            if (currentState.IsButtonDown(Buttons.A) && previousState.IsButtonUp(Buttons.A))
            {
                objectUpdater.fixedSpriteVisibility = true;
            }

            if (currentState.IsButtonDown(Buttons.B) && previousState.IsButtonUp(Buttons.B))
            {
                objectUpdater.fixedAnimatedSpriteVisibility = true;
            }

            if (currentState.IsButtonDown(Buttons.Y) && previousState.IsButtonUp(Buttons.Y))
            {
                objectUpdater.movingSpriteVisibility = true;
            }

            if (currentState.IsButtonDown(Buttons.X) && previousState.IsButtonUp(Buttons.X))
            {
                objectUpdater.movingAnimatedSpriteVisibility = true;
            }
        }*/
    }
}

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
    public class KeyboardController : IController
    {
        private readonly Dictionary<int, ICommand> keyMapping;
        private KeyboardState currentState;
        private KeyboardState previousState;
        private ObjectUpdater objectUpdater;

        // Default constructor
        public KeyboardController(ObjectUpdater OU)
        {
            objectUpdater = OU;
        }

        public KeyboardController()
        {
            previousState = Keyboard.GetState();
            keyMapping = new Dictionary<int, ICommand>();
        }

        public void AddMapping(int key, ICommand command)
        {
            this.keyMapping.Add(key, command);
        }
        
        // execute on press
        private void HandleKeyPress(Keys key)
        {
            keyMapping[(int)key].Execute(1);
        }
        private void HandleKeyHold(Keys key)
        {
            keyMapping[(int)key].Execute(2);
        }
        private void HandleKeyRelease(Keys key)
        {
            keyMapping[(int)key].Execute(3);
        }

        public void Update()
        {
            currentState = Keyboard.GetState();
            Keys[] keysPressed = currentState.GetPressedKeys();

            foreach (Keys key in keysPressed)
            {
                // Key Press press
                if (currentState.IsKeyDown(key) && previousState.IsKeyUp(key) && keyMapping.ContainsKey((int)key))
                {
                    HandleKeyPress(key);
                }
                // Key Press hold down
                if (currentState.IsKeyDown(key) && previousState.IsKeyDown(key) && keyMapping.ContainsKey((int)key))
                {
                    HandleKeyHold(key);
                }
                // Key Press release
                if (currentState.IsKeyUp(key) && previousState.IsKeyDown(key) && keyMapping.ContainsKey((int)key))
                {
                    HandleKeyRelease(key);
                }
            }

            previousState = currentState;
        }

        // TEMPORARILY COMMENTED OUT... for use with ObjectUpdater without Commands
        /*public void Update()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();

            if (currentState.IsKeyDown(Keys.Q) && previousState.IsKeyUp(Keys.Q))
            {
                objectUpdater.quitGame = true;
            }
            if (currentState.IsKeyDown(Keys.W) && previousState.IsKeyUp(Keys.W))
            {
                objectUpdater.fixedSpriteVisibility = true;
            }

            if (currentState.IsKeyDown(Keys.E) && previousState.IsKeyUp(Keys.E))
            {
                objectUpdater.fixedAnimatedSpriteVisibility = true;
            }

            if (currentState.IsKeyDown(Keys.R) && previousState.IsKeyUp(Keys.R))
            {
                objectUpdater.movingSpriteVisibility = true;
            }

            if (currentState.IsKeyDown(Keys.T) && previousState.IsKeyUp(Keys.T))
            {
                objectUpdater.movingAnimatedSpriteVisibility = true;
            }

            if (currentState.IsKeyDown(Keys.Divide) && previousState.IsKeyUp(Keys.Divide))
            {
                objectUpdater.bumpQuestion();
            }

            if (currentState.IsKeyDown(Keys.B) && previousState.IsKeyUp(Keys.B))
            {
                objectUpdater.bumpBrick();
            }

            if (currentState.IsKeyDown(Keys.H) && previousState.IsKeyUp(Keys.H))
            {
                objectUpdater.bumpHidden();
            }
        }*/
    }
}

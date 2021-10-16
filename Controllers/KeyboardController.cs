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
    public class KeyboardController : IController
    {
        private readonly Dictionary<int, ICommand> keyMapping;
        private KeyboardState currentState;
        private KeyboardState previousState;

        public KeyboardController()
        {
            previousState = Keyboard.GetState();
            keyMapping = new Dictionary<int, ICommand>();
        }

        public void AddMapping(int key, ICommand command)
        {
            if (this.keyMapping.ContainsKey(key))
            {
                this.keyMapping.Remove(key);
                this.keyMapping.Add(key, command);
            } else
            {
                this.keyMapping.Add(key, command);
            }
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
    }
}

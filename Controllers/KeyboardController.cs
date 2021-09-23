using Microsoft.Xna.Framework.Input;

namespace Game1
{
    public class KeyboardController : IController
    {
        private KeyboardState state;
        private KeyboardState oldState;
        private ObjectUpdater objectUpdater;

        // Default constructor
        public KeyboardController(ObjectUpdater OU)
        {
            objectUpdater = OU;
        }
        public void Update()
        {
            oldState = state;
            state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Q) && oldState.IsKeyUp(Keys.Q))
            {
                objectUpdater.quitGame = true;
            }
            if (state.IsKeyDown(Keys.W) && oldState.IsKeyUp(Keys.W))
            {
                objectUpdater.fixedSpriteVisibility = true;
            }

            if (state.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
            {
                objectUpdater.fixedAnimatedSpriteVisibility = true;
            }

            if (state.IsKeyDown(Keys.R) && oldState.IsKeyUp(Keys.R))
            {
                objectUpdater.movingSpriteVisibility = true;
            }

            if (state.IsKeyDown(Keys.T) && oldState.IsKeyUp(Keys.T))
            {
                objectUpdater.movingAnimatedSpriteVisibility = true;
            }

            if (state.IsKeyDown(Keys.Divide) && oldState.IsKeyUp(Keys.Divide))
            {
                objectUpdater.bumpQuestion();
            }

            if (state.IsKeyDown(Keys.B) && oldState.IsKeyUp(Keys.B))
            {
                objectUpdater.bumpBrick();
            }

            if (state.IsKeyDown(Keys.H) && oldState.IsKeyUp(Keys.H))
            {
                objectUpdater.bumpHidden();
            }
        }
    }
}

// Maxwell Ortwig

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using Controllers;
using Commands;
using GameObjects;
using Factories;
using States;

namespace Game1
{
    public class MarioGame : Game
    {
        
        
        //Temporary for debugging. Needs to be deleted prior to submit.
        static private MarioGame _sprint;

        public static MarioGame Sprint
        {
            get
            {
                return _sprint;
            }
        }

        //Monogame Objects
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // controllers that detect input from gamepad and keyboard. These only change ObjectUpdater
        private IController keyboardController;
        private IController gamepadController;

        // Simple display of sprites for sprint1


        //Block States
        private IBlockState brickBlockState;
        private IBlockState questoinBlockState;
        private IBlockState hiddenBlockState;
        //Item States
        private ItemState coinState;
        private ItemState superMushroomState;
        private ItemState oneUpMushroomState;
        private ItemState fireFlowerState;
        private ItemState starState;
        //Game objects
        private Goomba goomba;
        private Mario mario;
        //Block sbjects
        private Block brickBlock;
        private Block questionBlock;
        private Block hiddenBlock;
        private Block block;
        //Item objects
        private Item item;
        private Item fireFlower;
        private Item coin;
        private Item superMushroom;
        private Item oneUpMushroom;
        private Item star;
        private ISprite koopaTroopa;

        //Sprite factories
        private MarioSpriteFactory marioSpriteFactory;
        private GoombaSpriteFactory goombaSpriteFactory;
        private BlockSpriteFactory blockSpriteFactory;
        private ItemSpriteFactory itemSpriteFactory;
        public MarioGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            keyboardController = new KeyboardController();
            gamepadController = new GamepadController();

            marioSpriteFactory = MarioSpriteFactory.Instance;
            goombaSpriteFactory = GoombaSpriteFactory.Instance;
            blockSpriteFactory = BlockSpriteFactory.Instance;
            itemSpriteFactory = ItemSpriteFactory.Instance;

            this.Window.Title = "Cornet Mario Game";
            _sprint = this;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
           
            marioSpriteFactory.LoadTextures(this);
            goombaSpriteFactory.LoadTextures(this);
            blockSpriteFactory.LoadTextures(this);
            itemSpriteFactory.LoadTextures(this);


            //Visuals for Sprint 1

            koopaTroopa = new KoopaTroopa(true, Sprint, new Vector2(350, 100));

            mario = new Mario(new Vector2(50, 225));
            goomba = new Goomba();
            brickBlock = new Block(new Vector2(100, 200));
            questionBlock = new Block(new Vector2(200, 200));
            hiddenBlock = new Block(new Vector2(300, 200));
            block = new Block(new Vector2(0,0));
            coin = new Item(new Vector2(100, 50));
            superMushroom = new Item(new Vector2(150, 50));
            oneUpMushroom = new Item(new Vector2(200, 50));
            fireFlower = new Item(new Vector2(50, 50));
            star = new Item(new Vector2(250, 50));

            //Set block states
            brickBlockState = new BrickBlockState(block);
            questoinBlockState = new QuestionBlockState(block);
            hiddenBlockState = new HiddenBlockState(block);
            brickBlock.SetBlockState(brickBlockState);
            questionBlock.SetBlockState(questoinBlockState);
            hiddenBlock.SetBlockState(hiddenBlockState);

            //Set item states
            coinState = new CoinState(item);
            superMushroomState = new SuperMushroomState(item);
            oneUpMushroomState = new OneUpMushroomState(item);
            fireFlowerState = new FireFlowerState(item);
            starState = new StarState(item);
            coin.SetItemState(coinState);
            superMushroom.SetItemState(superMushroomState);
            oneUpMushroom.SetItemState(oneUpMushroomState);
            fireFlower.SetItemState(fireFlowerState);
            star.SetItemState(starState);

            // Initialize commands that will be repeated
            ICommand moveLeft = new MoveLeftCommand(mario);
            ICommand moveRight = new MoveRightCommand(mario);
            ICommand jump = new JumpCommand(mario);
            ICommand crouch = new CrouchCommand(mario);

            // Initialize keyboard controller mappings
            keyboardController.AddMapping((int)Keys.Q, new QuitCommand(this));
            keyboardController.AddMapping((int)Keys.Left, moveLeft);
            keyboardController.AddMapping((int)Keys.A, moveLeft);
            keyboardController.AddMapping((int)Keys.Right, moveRight);
            keyboardController.AddMapping((int)Keys.D, moveRight);
            keyboardController.AddMapping((int)Keys.Up, jump);
            keyboardController.AddMapping((int)Keys.W, jump);
            keyboardController.AddMapping((int)Keys.Down, crouch);
            keyboardController.AddMapping((int)Keys.S, crouch);

            keyboardController.AddMapping((int)Keys.Y, new StandardMarioCommand(mario));
            keyboardController.AddMapping((int)Keys.U, new SuperMarioCommand(mario));
            keyboardController.AddMapping((int)Keys.I, new FireMarioCommand(mario));
            keyboardController.AddMapping((int)Keys.O, new DeadMarioCommand(mario));

            keyboardController.AddMapping((int)Keys.Z, new IdleGoombaCommand(goomba));
            keyboardController.AddMapping((int)Keys.X, new MovingGoombaCommand(goomba));
            keyboardController.AddMapping((int)Keys.C, new StompedGoombaCommand(goomba));

            keyboardController.AddMapping((int)Keys.OemBackslash, new BumpCommand(brickBlock));
            keyboardController.AddMapping((int)Keys.B, new BumpCommand(questionBlock));
            keyboardController.AddMapping((int)Keys.H, new BumpCommand(hiddenBlock));

            

            // Initialize gamepad controller mappings
            gamepadController.AddMapping((int)Buttons.DPadLeft, moveLeft);
            gamepadController.AddMapping((int)Buttons.DPadRight, moveRight);
            gamepadController.AddMapping((int)Buttons.A, jump);
            gamepadController.AddMapping((int)Buttons.DPadDown, crouch);
        }
        protected override void Update(GameTime gameTime)
        {
            // Update the controllers
            gamepadController.Update();
            keyboardController.Update();

            //Update the Game Objects
            mario.Update(gameTime, graphics);
            goomba.Update(); 
            questionBlock.Update();
            brickBlock.Update();
            hiddenBlock.Update();

            coin.Update(gameTime, graphics);
            superMushroom.Update(gameTime, graphics);
            oneUpMushroom.Update(gameTime, graphics);
            fireFlower.Update(gameTime, graphics);
            star.Update(gameTime, graphics);
            
            koopaTroopa.Update();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            // call draw methods from each sprite and pass in sprite batch
            mario.Draw(spriteBatch);
            goomba.Draw(spriteBatch);
            brickBlock.Draw(spriteBatch);
            questionBlock.Draw(spriteBatch);
            hiddenBlock.Draw(spriteBatch);
            coin.Draw(spriteBatch);
            superMushroom.Draw(spriteBatch);
            oneUpMushroom.Draw(spriteBatch);
            fireFlower.Draw(spriteBatch);
            star.Draw(spriteBatch);
            
            koopaTroopa.Draw(spriteBatch, true);

            // Draw Legend
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }


}

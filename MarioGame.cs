// Maxwell Ortwig

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Controllers;
using Commands;
using GameObjects;
using Factories;
using States;
using System.Collections.Generic;

namespace Game1
{
    public class MarioGame : Game
    {
        List<IGameObject> objects;
        
        //Monogame Objects
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        //Controllers
        private IController keyboardController;
        private IController gamepadController;

        //Game Objects
        //Enemy objects
        private IEnemy goomba;
        private IEnemy koopaTroopa;
        private IEnemy redKoopaTroopa;

        //Character ojbects
        private IAvatar mario;

        //Obstacle objects
        private IBlock questionBlock;
        private IBlock usedBlock; 
        private IBlock brickBlock;
        private IBlock floorBlock;
        private IBlock stairBlock;
        private IBlock hiddenBlock;

        //Item objects
        private IItem item;
        private IItem fireFlower;
        private IItem coin;
        private IItem superMushroom;
        private IItem oneUpMushroom;
        private IItem star;

        //Sprite factories
        private MarioSpriteFactory marioSpriteFactory;
        private GoombaSpriteFactory goombaSpriteFactory;
        private ItemSpriteFactory itemSpriteFactory;
        private KoopaTroopaSpriteFactory koopaTroopaSpriteFactory;
        private RedKoopaTroopaSpriteFactory redKoopaTroopaSpriteFactory;

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
            itemSpriteFactory = ItemSpriteFactory.Instance;
            koopaTroopaSpriteFactory = KoopaTroopaSpriteFactory.Instance;
            redKoopaTroopaSpriteFactory = RedKoopaTroopaSpriteFactory.Instance;

            this.Window.Title = "Cornet Mario Game";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
           
            marioSpriteFactory.LoadTextures(this);

            /*
             * Since we will have multiple enemies and items, we should make
             * these factories not be singletons.
             */
            goombaSpriteFactory.LoadTextures(this);
            itemSpriteFactory.LoadTextures(this);
            koopaTroopaSpriteFactory.LoadTextures(this);
            redKoopaTroopaSpriteFactory.LoadTextures(this);
            Texture2D blockSprites = Content.Load<Texture2D>("BlocksV3");

            // Visuals for Sprint 1
            mario = new Mario(new Vector2(10, graphics.PreferredBackBufferHeight - 30), graphics);
            goomba = new Goomba(new Vector2(300, 100));
            koopaTroopa = new KoopaTroopa(new Vector2(350, 100));
            redKoopaTroopa = new RedKoopaTroopa(new Vector2(400, 100));

            questionBlock = new Block(new Vector2(100, 200), blockSprites);
            usedBlock = new Block(new Vector2(150, 200), blockSprites);
            brickBlock = new Block(new Vector2(200, 200), blockSprites);
            floorBlock = new Block(new Vector2(250, 200), blockSprites);
            stairBlock = new Block(new Vector2(300, 200), blockSprites);
            hiddenBlock = new Block(new Vector2(350, 200), blockSprites);

            item = new Item(new Vector2(0, 0));
            coin = new Item(new Vector2(100, 50));
            superMushroom = new Item(new Vector2(150, 50));
            oneUpMushroom = new Item(new Vector2(200, 50));
            fireFlower = new Item(new Vector2(50, 50));
            star = new Item(new Vector2(250, 50));

            // Set obstacle states
            questionBlock.SetBlockState(new QuestionBlockState(questionBlock));
            usedBlock.SetBlockState(new UsedBlockState(usedBlock));
            brickBlock.SetBlockState(new BrickBlockState(brickBlock));
            floorBlock.SetBlockState(new FloorBlockState(floorBlock));
            stairBlock.SetBlockState(new StairBlockState(stairBlock));
            hiddenBlock.SetBlockState(new HiddenBlockState(hiddenBlock));

            // Set item states
            coin.SetItemState(new CoinState(item));
            superMushroom.SetItemState(new SuperMushroomState(item));
            oneUpMushroom.SetItemState(new OneUpMushroomState(item));
            fireFlower.SetItemState(new FireFlowerState(item));
            star.SetItemState(new StarState(item));

            // Initialize commands that will be repeated
            ICommand moveLeft = new MoveLeftCommand(mario);
            ICommand moveRight = new MoveRightCommand(mario);
            ICommand jump = new JumpCommand(mario);
            ICommand crouch = new CrouchCommand(mario);

            // Initialize keyboard controller mappings
            // Action commands
            keyboardController.AddMapping((int)Keys.Q, new QuitCommand(this));
            keyboardController.AddMapping((int)Keys.Left, moveLeft);
            keyboardController.AddMapping((int)Keys.A, moveLeft);
            keyboardController.AddMapping((int)Keys.Right, moveRight);
            keyboardController.AddMapping((int)Keys.D, moveRight);
            keyboardController.AddMapping((int)Keys.Up, jump);
            keyboardController.AddMapping((int)Keys.W, jump);
            keyboardController.AddMapping((int)Keys.Down, crouch);
            keyboardController.AddMapping((int)Keys.S, crouch);

            // Power-up commands
            keyboardController.AddMapping((int)Keys.Y, new StandardMarioCommand(mario));
            keyboardController.AddMapping((int)Keys.U, new SuperMarioCommand(mario));
            keyboardController.AddMapping((int)Keys.I, new FireMarioCommand(mario));
            keyboardController.AddMapping((int)Keys.O, new DeadMarioCommand(mario));

            // Enemy commands
            keyboardController.AddMapping((int)Keys.Z, new IdleGoombaCommand(goomba));
            keyboardController.AddMapping((int)Keys.X, new MovingGoombaCommand(goomba));
            keyboardController.AddMapping((int)Keys.C, new StompedGoombaCommand(goomba));
            keyboardController.AddMapping((int)Keys.V, new IdleKoopaTroopaCommand(koopaTroopa));
            keyboardController.AddMapping((int)Keys.N, new MovingKoopaTroopaCommand(koopaTroopa));
            keyboardController.AddMapping((int)Keys.M, new StompedKoopaTroopaCommand(koopaTroopa));
            keyboardController.AddMapping((int)Keys.J, new IdleRedKoopaTroopaCommand(redKoopaTroopa));
            keyboardController.AddMapping((int)Keys.K, new MovingRedKoopaTroopaCommand(redKoopaTroopa));
            keyboardController.AddMapping((int)Keys.L, new StompedRedKoopaTroopaCommand(redKoopaTroopa));

            // Brick commands
            keyboardController.AddMapping((int)Keys.OemQuestion, new BumpCommand(questionBlock, (Mario) mario));
            keyboardController.AddMapping((int)Keys.B, new BumpCommand(brickBlock, (Mario) mario));
            keyboardController.AddMapping((int)Keys.H, new BumpCommand(hiddenBlock, (Mario) mario));

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
            mario.Update(gameTime);
            goomba.Update(gameTime); 
            koopaTroopa.Update(gameTime);
            redKoopaTroopa.Update(gameTime);

            questionBlock.Update(gameTime);
            usedBlock.Update(gameTime);
            brickBlock.Update(gameTime);
            floorBlock.Update(gameTime);
            stairBlock.Update(gameTime);
            hiddenBlock.Update(gameTime);
            coin.Update(gameTime);
            superMushroom.Update(gameTime);
            oneUpMushroom.Update(gameTime);
            fireFlower.Update(gameTime);
            star.Update(gameTime);
            
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            // call draw methods from each sprite and pass in sprite batch
            mario.Draw(spriteBatch);
            goomba.Draw(spriteBatch);
            koopaTroopa.Draw(spriteBatch);
            redKoopaTroopa.Draw(spriteBatch);

            questionBlock.Draw(spriteBatch);
            usedBlock.Draw(spriteBatch);
            brickBlock.Draw(spriteBatch);
            floorBlock.Draw(spriteBatch);
            stairBlock.Draw(spriteBatch);
            hiddenBlock.Draw(spriteBatch);

            coin.Draw(spriteBatch);
            superMushroom.Draw(spriteBatch);
            oneUpMushroom.Draw(spriteBatch);
            fireFlower.Draw(spriteBatch);
            star.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }


}

// Maxwell Ortwig

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
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

        //Controllers
        private IController keyboardController;
        private IController gamepadController;

        //Game Objects
        //Block States
        private IBlockState questionBlockState;
        private IBlockState usedBlockState;
        private IBlockState brickBlockState;
        private IBlockState floorBlockState;
        private IBlockState stairBlockState;
        private IBlockState hiddenBlockState;

        //Item States
        private IItemState coinState;
        private IItemState superMushroomState;
        private IItemState oneUpMushroomState;
        private IItemState fireFlowerState;
        private IItemState starState;

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

        //Block objects
        private IBlock testBrickBlock;
        private IBlock testQuestionBlock;
        private IBlock testHiddenBlock;
        private IBlock block;

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
        private BlockSpriteFactory blockSpriteFactory;
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
            blockSpriteFactory = BlockSpriteFactory.Instance;
            itemSpriteFactory = ItemSpriteFactory.Instance;
            koopaTroopaSpriteFactory = KoopaTroopaSpriteFactory.Instance;
            redKoopaTroopaSpriteFactory = RedKoopaTroopaSpriteFactory.Instance;

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
            koopaTroopaSpriteFactory.LoadTextures(this);
            redKoopaTroopaSpriteFactory.LoadTextures(this);

            // Visuals for Sprint 1
            mario = new Mario(new Vector2(50, 225));
            goomba = new Goomba(new Vector2(300, 100));
            koopaTroopa = new KoopaTroopa(new Vector2(350, 100));
            redKoopaTroopa = new RedKoopaTroopa(new Vector2(400, 100));

            questionBlock = new Block(new Vector2(100, 200));
            usedBlock = new Block(new Vector2(150, 200));
            brickBlock = new Block(new Vector2(200, 200));
            floorBlock = new Block(new Vector2(350, 200));
            stairBlock = new Block(new Vector2(400, 200));
            hiddenBlock = new Block(new Vector2(450, 200));
            block = new Block(new Vector2(0,0));

            testBrickBlock = new Block(new Vector2(100, 400));
            testQuestionBlock = new Block(new Vector2(200, 400));
            testHiddenBlock = new Block(new Vector2(300, 400));

            item = new Item(new Vector2(0, 0));
            coin = new Item(new Vector2(100, 50));
            superMushroom = new Item(new Vector2(150, 50));
            oneUpMushroom = new Item(new Vector2(200, 50));
            fireFlower = new Item(new Vector2(50, 50));
            star = new Item(new Vector2(250, 50));

            // Set obstacle states
            questionBlockState = new QuestionBlockState(questionBlock);
            usedBlockState = new UsedBlockState(usedBlock);
            brickBlockState = new BrickBlockState(brickBlock);
            floorBlockState = new FloorBlockState(floorBlock);
            stairBlockState = new StairBlockState(stairBlock);
            hiddenBlockState = new HiddenBlockState(hiddenBlock);

            questionBlock.SetBlockState(questionBlockState);
            usedBlock.SetBlockState(usedBlockState);
            brickBlock.SetBlockState(brickBlockState);
            floorBlock.SetBlockState(floorBlockState);
            stairBlock.SetBlockState(stairBlockState);
            hiddenBlock.SetBlockState(hiddenBlockState);

            // Set changing block states 
            testBrickBlock.SetBlockState(brickBlockState);
            testQuestionBlock.SetBlockState(questionBlockState);
            testHiddenBlock.SetBlockState(hiddenBlockState);

            // Set item states
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
            keyboardController.AddMapping((int)Keys.OemBackslash, new BumpCommand(testBrickBlock));
            keyboardController.AddMapping((int)Keys.B, new BumpCommand(testQuestionBlock));
            keyboardController.AddMapping((int)Keys.H, new BumpCommand(testHiddenBlock));

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
            koopaTroopa.Update();
            redKoopaTroopa.Update();

            questionBlock.Update();
            usedBlock.Update();
            brickBlock.Update();
            floorBlock.Update();
            stairBlock.Update();
            hiddenBlock.Update();

            testBrickBlock.Update();
            testQuestionBlock.Update();
            testHiddenBlock.Update();

            coin.Update();
            superMushroom.Update();
            oneUpMushroom.Update();
            fireFlower.Update();
            star.Update();
            
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

            testBrickBlock.Draw(spriteBatch);
            testQuestionBlock.Draw(spriteBatch);
            testHiddenBlock.Draw(spriteBatch);

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

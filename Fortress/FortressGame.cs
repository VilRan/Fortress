using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Fortress
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class FortressGame : Game
    {
        public Random Random;
        public Session Session;
        public GameState State;
        public Effect DefaultShader;
        public SpriteFont DefaultFont;
        public Texture2D MatterTexture;
        public Texture2D TransparentTexture;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RasterizerState rasterizerState;
        DepthStencilState depthStencilState;

        public int Width { get { return graphics.PreferredBackBufferWidth; } }
        public int Height { get { return graphics.PreferredBackBufferHeight; } }

        public FortressGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Content.RootDirectory = "Content";
            Window.IsBorderless = true;
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Random = new Random();

            rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullClockwiseFace;

            depthStencilState = new DepthStencilState();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            DefaultShader = Content.Load<Effect>("DefaultShader");
            DefaultFont = Content.Load<SpriteFont>("DefaultFont");
            MatterTexture = Content.Load<Texture2D>("MatterRectangle");
            TransparentTexture = Content.Load<Texture2D>("TransparentRectangle");

            Session = new Session(this);
            State = new MapGameState(this);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            State.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(0.0f, 0.0f, 0.0f));

            GraphicsDevice.RasterizerState = rasterizerState;
            GraphicsDevice.DepthStencilState = depthStencilState;
            State.Draw();
            
            spriteBatch.Begin();
            spriteBatch.DrawString(DefaultFont, string.Format("FPS: {0:N}", 1.0 / gameTime.ElapsedGameTime.TotalSeconds), Vector2.Zero, Color.White);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HexagonWin
{
    using HexagonLibrary;
    using HexagonLibrary.Entity.GameObjects;
    using HexagonLibrary.Model;
    using System.Collections.Generic;

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class HexagonWinGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Core gameCore;
        
        public HexagonWinGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;

            this.gameCore = new Core(HexagonLibrary.Device.GameDeviceType.Mouse);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            this.gameCore.LoadContent(this.Content);
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

            // TODO: Add your update logic here
            this.gameCore.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();
            foreach(var item in this.gameCore.Map.Items)
            {
                this.DrawObject(item);
            }
            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawObject(MonoObject mObj)
        {
            if (mObj.Texture != null)
            {
                this.spriteBatch.Draw(mObj.Texture, mObj.Position, mObj.Color);
                SpriteFont sp = this.Content.Load<SpriteFont>("LifeFont");
                
                this.spriteBatch.DrawString(sp, mObj.Text, mObj.TextPositon, Color.Black);
            }
        }
    }
}

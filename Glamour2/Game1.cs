using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Glamour2
{

    public class Game1 : Game
    {
        public static string GAME_NAME = "Farmville Royale";
        public static int SCREEN_WIDTH = 1920;
        public static int SCREEN_HEIGHT = 1080;

        public static Texture2D WhiteTexture;
        public static MusicManager Music;

        public static SpriteFont font;
        public static SpriteFont arial;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        IGameState currentState;
        InputHandler ih;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

        public static Vector2 parseVector2(string v)
        {
            string[] vs = v.Split(',');
            return new Vector2(int.Parse(vs[0]), int.Parse(vs[1]));
        }
        public static Rectangle parseRectangle(string r)
        {
            string[] ap = r.Split(',');
            if (ap.Length != 4) return new Rectangle(0, 0, 0, 0);
            return new Rectangle(int.Parse(ap[0]), int.Parse(ap[1]), int.Parse(ap[2]), int.Parse(ap[3]));
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ih = new InputHandler();

            WhiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            WhiteTexture.SetData<Color>(new Color[1] { Color.White });

            Music = new MusicManager(Content);

            arial = Content.Load<SpriteFont>("SmallFont");
            font = Content.Load<SpriteFont>("BasicFont");


            transitionToMenu();
            //transitionToGame("bridge");
        }


        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {

            float dt = (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
            ih.update();

            if (ih.isKeyPressed(Keys.Escape)) Exit();

            if (currentState != null)
            {
                currentState.handleInput(ih, dt);
                currentState.update(dt);
            }


            base.Update(gameTime);
        }

        public void transitionToGame(string map)
        {
            currentState = new GameStateArena(this, Content, map);
        }
        public void transitionToMenu()
        {
            currentState = new GameStateMenu(this, Content);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (currentState != null)
            {

                spriteBatch.Begin();
                currentState.draw(spriteBatch);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}

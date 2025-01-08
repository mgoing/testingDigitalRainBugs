using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace testingDigitalRainBugs
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        private Random _random;
        private const int ScreenWidth = 800;
        private const int ScreenHeight = 600;

        private const int ColumnCount = 50;
        private const int FontSize = 16;

        private char[][] _columns;
        private float[] _speeds;
        private float[] _positions;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _random = new Random();
            _columns = new char[ColumnCount][];
            _speeds = new float[ColumnCount];
            _positions = new float[ColumnCount];

            for (int i = 0; i < ColumnCount; i++)
            {
                _columns[i] = new char[ScreenHeight / FontSize];
                _speeds[i] = (float)_random.NextDouble() * 100 + 50; // Random speeds
                _positions[i] = _random.Next(-ScreenHeight, 0); // Random start positions

                // Fill each column with random characters
                for (int j = 0; j < _columns[i].Length; j++)
                {
                    _columns[i][j] = GetRandomChar();
                }
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("ThisMatrixFont"); // Ensure "ThisMatrixFont.spritefont" is built
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < ColumnCount; i++)
            {
                _positions[i] += _speeds[i] * deltaTime;

                // Wrap text to the top once it goes off-screen
                if (_positions[i] > ScreenHeight)
                {
                    _positions[i] = -_columns[i].Length * FontSize;
                    _speeds[i] = (float)_random.NextDouble() * 100 + 50; // New random speed
                }

                // Randomize characters as the column falls
                for (int j = 0; j < _columns[i].Length; j++)
                {
                    if (_random.NextDouble() < 0.1) // 10% chance to change
                    {
                        _columns[i][j] = GetRandomChar();
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            for (int i = 0; i < ColumnCount; i++)
            {
                for (int j = 0; j < _columns[i].Length; j++)
                {
                    float yPos = _positions[i] + j * FontSize;

                    // Only draw visible characters
                    if (yPos > -FontSize && yPos < ScreenHeight)
                    {
                        _spriteBatch.DrawString(_font, _columns[i][j].ToString(), new Vector2(i * (ScreenWidth / ColumnCount), yPos), Color.Lime);
                    }
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private char GetRandomChar()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#$%^&*";
            return chars[_random.Next(chars.Length)];
        }
    }
}

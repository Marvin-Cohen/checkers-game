// Checkers Game created by Marvin Cohen under the supervision of Spencer Rosenfeld
// Checker piece assets and checker board assets are from https://opengameart.org/content/boardgame-tiles
// these assets were created by the artist Lanea Zimmerman


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System;

namespace Checkers
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const int TILE_WIDTH = 32;
        const int TILE_HEIGHT = 32;
        const int WindowWidth = 8 * TILE_WIDTH;
        const int WindowHeight = 8 * TILE_HEIGHT;
        const int NUMBER_OF_CHECKERS = 24;
        const int NUMBER_OF_TILES = 64;
        GraphicsDeviceManager graphics;     
        SpriteBatch spriteBatch;

        // Tiles 
        Texture2D[] tile = new Texture2D[2];
        Rectangle[] drawRectangle = new Rectangle[NUMBER_OF_TILES];

        // Kings
        Rectangle[] cap = new Rectangle[NUMBER_OF_CHECKERS]; // Caps for the kings 
        bool[] isKing = new bool[NUMBER_OF_CHECKERS];

        // Checker Pieces 
        // This data will eventaully need to be represented by a Checker Piece Object
        Texture2D[] checkerPiece = new Texture2D[2];
        Rectangle[] checker = new Rectangle[NUMBER_OF_CHECKERS]; // Drawing rectangles for checker pieces. 
        bool[] isOnBoard = new bool[NUMBER_OF_CHECKERS];  // true if piece at index "i" has not been captured
        string[] type = new string[NUMBER_OF_CHECKERS];

        // Mouse Cursor
        //Texture2D[] mouseCursor = new Texture2D[1];
        //Rectangle[] mc = new Rectangle[1];

        int selectedCheckerIndex = 0;
        int selectedCheckerX = 0;              //original, NOT current coordinates of any checker
        int selectedCheckerY = 0;
        bool checkerSelected = false;
        bool enterKeyHasBeenPressed = false;
        int toggle = 0;                        //by default, it is black's turn first (0 = black, 1 = red)
        bool newTurn = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            //Mouse cursor present
            IsMouseVisible = true;

            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
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
            // Load tile sprite images
            tile[1] = Content.Load<Texture2D>(@"C:\Users\marvi\source\repos\Checkers\Checkers\Content\sprite-graphics\darktile");
            tile[0] = Content.Load<Texture2D>(@"C:\Users\marvi\source\repos\Checkers\Checkers\Content\sprite-graphics\lighttile");
            //tile[2] = Content.Load<Texture2D>(@"C:\Users\marvi\source\repos\Checkers\Checkers\Content\sprite-graphics\mediumtile");
            checkerPiece[0] = Content.Load<Texture2D>(@"C:\Users\marvi\source\repos\Checkers\Checkers\Content\sprite-graphics\blackchecker(1)");
            checkerPiece[1] = Content.Load<Texture2D>(@"C:\Users\marvi\source\repos\Checkers\Checkers\Content\sprite-graphics\redchecker(1)");
            //checkerPiece[2] = Content.Load<Texture2D>(@"C:\Users\marvi\source\repos\Checkers\Checkers\Content\sprite-graphics\whitechecker");
            //mouseCursor[0] = Content.Load<Texture2D>(@"C:\Users\marvi\source\repos\Checkers\Checkers\Content\sprite-graphics\white-mouse-cursor-arrow-by-qubodup-11(1)");

            for (int i = 0; i < NUMBER_OF_CHECKERS; i++)
            {
                isKing[i] = false;
                isOnBoard[i] = true;
            }

            for (int i = 0; i < NUMBER_OF_CHECKERS / 2; i++)
            {
                type[i] = "Black";
                type[i + NUMBER_OF_CHECKERS / 2] = "Red";
            }

            for (int i = 0; i < NUMBER_OF_CHECKERS / 2; i++)
            {
                if (i >= 0 && i < 4)
                {
                    checker[i].Width = TILE_WIDTH;
                    checker[i].Height = TILE_HEIGHT;
                    checker[i].X = (((2 * i) + 1) % 8) * TILE_WIDTH;
                    checker[i].Y = 0;
                }
                else if (i >= 4 && i < 8)
                {
                    checker[i].Width = TILE_WIDTH;
                    checker[i].Height = TILE_HEIGHT;
                    checker[i].X = ((2 * i) % 8) * TILE_WIDTH;
                    checker[i].Y = TILE_HEIGHT;
                }
                else
                {
                    checker[i].Width = TILE_WIDTH;
                    checker[i].Height = TILE_HEIGHT;
                    checker[i].X = (((2 * i) + 1) % 8) * TILE_WIDTH;
                    checker[i].Y = 2 * TILE_HEIGHT;
                }
            }
            for (int i = NUMBER_OF_CHECKERS / 2; i < NUMBER_OF_CHECKERS; i++)
            {
                if (i >= 12 && i < 16)
                {
                    checker[i].Width = TILE_WIDTH;
                    checker[i].Height = TILE_HEIGHT;
                    checker[i].X = ((2 * i) % 8) * TILE_WIDTH;
                    checker[i].Y = 5 * TILE_HEIGHT;
                }
                else if (i >= 16 && i < 20)
                {
                    checker[i].Width = TILE_WIDTH;
                    checker[i].Height = TILE_HEIGHT;
                    checker[i].X = (((2 * i) + 1) % 8) * TILE_WIDTH;
                    checker[i].Y = 6 * TILE_HEIGHT;
                }
                else
                {
                    checker[i].Width = TILE_WIDTH;
                    checker[i].Height = TILE_HEIGHT;
                    checker[i].X = ((2 * i) % 8) * TILE_WIDTH;
                    checker[i].Y = 7 * TILE_HEIGHT;
                }
            }
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
            MouseState mouseState = Mouse.GetState();
            ButtonState buttonState = mouseState.LeftButton;
            //"Pressed" and "Released"
            // If the mouse button is pressed then I want the checker piece at the mouse (x,y)
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Enter))
            {
                enterKeyHasBeenPressed = true;
            }

            // The pressing and subsequent release of the Enter key triggers a new turn. 
            newTurn = enterKeyHasBeenPressed && keyState.IsKeyUp(Keys.Enter);
            if (newTurn == true)
            {
                if (toggle == 0)
                {
                    // Red's turn
                    toggle = 1;
                }
                else
                {
                    // Black's turn
                    toggle = 0;
                }
                newTurn = false;
                checkerSelected = false;
                enterKeyHasBeenPressed = false;
            }

            for (int i = 0; i < NUMBER_OF_CHECKERS / 2; i++)
            {
                // check if black checker piece is in 7th row
                if (checker[i].Y >= TILE_HEIGHT * 7 && mouseState.LeftButton.ToString() != "Pressed")
                {
                    isKing[i] = true;
                }
            }

            for (int i = NUMBER_OF_CHECKERS / 2; i < NUMBER_OF_CHECKERS; i++)
            {
                // check if red checker piece is in 0th row
                if (checker[i].Y < TILE_HEIGHT - 5 && mouseState.LeftButton.ToString() != "Pressed")
                {
                    // The subtraction of 5 from the TILE_HEIGHT is to avoid a bug where red checkers were 
                    // kinged even though they were only in the second row. 
                    isKing[i] = true;
                }
            }

            if (mouseState.LeftButton.ToString() == "Pressed")
            {
                if (checkerSelected == true)
                {
                    checker[selectedCheckerIndex].X = mouseState.X - TILE_WIDTH / 2;
                    checker[selectedCheckerIndex].Y = mouseState.Y - TILE_HEIGHT / 2;
                }
                else
                {
                    for (int i = 0; i < NUMBER_OF_CHECKERS; i++)
                    {
                        if (checker[i].X == (mouseState.X / TILE_WIDTH) * TILE_WIDTH && checker[i].Y == (mouseState.Y / TILE_HEIGHT) * TILE_HEIGHT)
                        {
                            if (toggle == 0 && i < NUMBER_OF_CHECKERS / 2)
                            {
                                //Move appropriate checker color piece one step right or left diagonally 
                                //in respective up/down direction
                                selectedCheckerIndex = i;
                                selectedCheckerX = mouseState.X;
                                selectedCheckerY = mouseState.Y;
                                checkerSelected = true;
                            }
                            else if (toggle == 1 && i >= NUMBER_OF_CHECKERS / 2)
                            {
                                selectedCheckerIndex = i;
                                selectedCheckerX = mouseState.X;
                                selectedCheckerY = mouseState.Y;
                                checkerSelected = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (checkerSelected == true)
                {
                    checkerSelected = false;
                    bool isValidMove;
                    if (isKing[selectedCheckerIndex] == false) {
                        isValidMove = IsMovingDiagonally(mouseState.X, mouseState.Y, selectedCheckerX, selectedCheckerY)
                        && IsMovingUpDownDirection(selectedCheckerX, selectedCheckerY, mouseState.X, mouseState.Y, type[selectedCheckerIndex])
                        && !IsMovingThreeOrMoreSpaces(selectedCheckerX, selectedCheckerY, mouseState.X, mouseState.Y)
                        && !IsTargetAtDestination(mouseState.X, mouseState.Y, checker)
                        && (MovedOnlyOneSpace(selectedCheckerX, selectedCheckerY, mouseState.X, mouseState.Y)
                        || MovementCapturedPiece(selectedCheckerX, selectedCheckerY, mouseState.X, mouseState.Y, checker, type[selectedCheckerIndex]) > -1);
                    }
                    else
                    {
                        isValidMove = IsMovingDiagonally(mouseState.X, mouseState.Y, selectedCheckerX, selectedCheckerY)
                        && !IsMovingThreeOrMoreSpaces(selectedCheckerX, selectedCheckerY, mouseState.X, mouseState.Y)
                        && !IsTargetAtDestination(mouseState.X, mouseState.Y, checker)
                        && (MovedOnlyOneSpace(selectedCheckerX, selectedCheckerY, mouseState.X, mouseState.Y)
                        || MovementCapturedPiece(selectedCheckerX, selectedCheckerY, mouseState.X, mouseState.Y, checker, type[selectedCheckerIndex]) > -1);
                    }

                    if (isValidMove == true)
                    {
                        int captureIndex = MovementCapturedPiece(selectedCheckerX, selectedCheckerY, mouseState.X, mouseState.Y, checker, type[selectedCheckerIndex]); 
                        if (captureIndex > -1)
                        {
                            isOnBoard[captureIndex] = false; //Stop drawing piece
                            checker[captureIndex].X = -5 * TILE_WIDTH; //Take piece off of board
                        }
                        checker[selectedCheckerIndex].X = (mouseState.X / TILE_WIDTH) * TILE_WIDTH;
                        checker[selectedCheckerIndex].Y = (mouseState.Y / TILE_HEIGHT) * TILE_HEIGHT;
                    }
                    else
                    {
                        checker[selectedCheckerIndex].X = (selectedCheckerX / TILE_WIDTH) * TILE_WIDTH;
                        checker[selectedCheckerIndex].Y = (selectedCheckerY / TILE_HEIGHT) * TILE_HEIGHT;
                    }
                }
            }

            //mc[0].X = mouseState.X;
            //mc[0].Y = mouseState.Y;
            //mc[0].Width = 11;
            //mc[0].Height = 20;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Set rectangle width and height
            for (int i = 0; i < NUMBER_OF_TILES; i++)
            {
                drawRectangle[i].Width = TILE_WIDTH;
                drawRectangle[i].Height = TILE_HEIGHT; 
                drawRectangle[i].X = (i % 8) * TILE_WIDTH;   //the rhs of these expressions will need to be changed
                drawRectangle[i].Y = i / 8 * TILE_HEIGHT;    //to equal the appropriate functions of "i"
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Drawing Checker Tiles
            spriteBatch.Begin();
            for (int i = 0; i < NUMBER_OF_TILES; i++)
            {
                spriteBatch.Draw(tile[(i + i/8) % 2], drawRectangle[i], Color.White);
            }
            
            // Drawing Black Pieces
            for (int i = 0; i < NUMBER_OF_CHECKERS / 2; i++)
            {
                if (isOnBoard[i])
                {
                    spriteBatch.Draw(checkerPiece[0], checker[i], Color.White);
                    if (isKing[i])
                    {
                        cap[i] = checker[i];
                        // Add another black piece on top and adjust height accordingly, to fit like a king
                        cap[i].Y = cap[i].Y - TILE_HEIGHT / 5;
                        spriteBatch.Draw(checkerPiece[0], cap[i], Color.White);
                    }
                }
            }

            // Drawing Red Pieces
            for (int i = NUMBER_OF_CHECKERS / 2; i < NUMBER_OF_CHECKERS; i++)
            {
                if (isOnBoard[i])
                {
                    spriteBatch.Draw(checkerPiece[1], checker[i], Color.White);
                    if (isKing[i])
                    {
                        cap[i] = checker[i];
                        // Add another red piece on top and adjust height accordingly, to fit like a king
                        cap[i].Y = cap[i].Y - TILE_HEIGHT / 5;
                        spriteBatch.Draw(checkerPiece[1], cap[i], Color.White);
                    }
                }
            }

            if (type[selectedCheckerIndex] == "Red")
            {
                spriteBatch.Draw(checkerPiece[1], checker[selectedCheckerIndex], Color.White);
                if (isKing[selectedCheckerIndex])
                {
                    cap[selectedCheckerIndex] = checker[selectedCheckerIndex];
                    cap[selectedCheckerIndex].Y = cap[selectedCheckerIndex].Y - TILE_HEIGHT / 5;
                    spriteBatch.Draw(checkerPiece[1], cap[selectedCheckerIndex], Color.White);
                }
            }
            else
            {
                spriteBatch.Draw(checkerPiece[0], checker[selectedCheckerIndex], Color.White);
                if (isKing[selectedCheckerIndex])
                {
                    cap[selectedCheckerIndex] = checker[selectedCheckerIndex];
                    cap[selectedCheckerIndex].Y = cap[selectedCheckerIndex].Y - TILE_HEIGHT / 5;
                    spriteBatch.Draw(checkerPiece[0], cap[selectedCheckerIndex], Color.White);
                }
            }

            //spriteBatch.Draw(mouseCursor[0], mc[0], Color.White);
            //spriteBatch.Draw(tile[1], drawRectangle[1], Color.White);
            //spriteBatch.Draw(tile[2], drawRectangle[2], Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static bool IsMovingThreeOrMoreSpaces(int x1, int y1, int x2, int y2)
        {
            int i1 = x1 / TILE_WIDTH;
            int i2 = x2 / TILE_WIDTH;
            return Math.Abs(i1 - i2) > 2;
        }

        public static bool MovedOnlyOneSpace(int x1, int y1, int x2, int y2)
        {
            x2 = (x2 / TILE_WIDTH) * TILE_WIDTH;
            y2 = (y2 / TILE_HEIGHT) * TILE_HEIGHT;
            x1 = (x1 / TILE_WIDTH) * TILE_WIDTH;
            y1 = (y1 / TILE_HEIGHT) * TILE_HEIGHT;
            return Math.Abs(x2 - x1) == TILE_WIDTH;
        }

        public static int MovementCapturedPiece(int x1, int y1, int x2, int y2, Rectangle [] checker, string type)
        {
            // Assumes a checker piece of type "type" has moved from x1 y1 to x2 y2.
            // If the movement is only by one space a capture is impossible. 
            // If the movement is by two spaces there may be a capture. 
            // If there no capture the function returns -1. 
            // If there is a capture the program returns the index of the captured piece. 
            // A piece can only capture another piece of a different type.
            x2 = (x2 / TILE_WIDTH) * TILE_WIDTH;
            y2 = (y2 / TILE_HEIGHT) * TILE_HEIGHT;
            x1 = (x1 / TILE_WIDTH) * TILE_WIDTH;
            y1 = (y1 / TILE_HEIGHT) * TILE_HEIGHT;
            int delta = x2 - x1;
            int returnIndex = -1;

            if (Math.Abs(delta) == 2 * TILE_WIDTH)
            {
                if (type == "Red")
                {
                    //black pieces are from checker[0] to checker[11]
                    for (int i = 0; i < NUMBER_OF_CHECKERS / 2; i++)
                    {
                        if (checker[i].X == x1 + (x2 - x1) / 2 && checker[i].Y == y1 + (y2 - y1) / 2)
                        {
                            returnIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    //red pieces are from checker[12] to checker[23]
                    for (int i = NUMBER_OF_CHECKERS / 2; i < NUMBER_OF_CHECKERS; i++)
                    {
                        if (checker[i].X == x1 + (x2 - x1) / 2 && checker[i].Y == y1 + (y2 - y1) / 2)
                        {
                            returnIndex = i;
                            break;
                        }
                    }
                }
            }            
            return returnIndex; 
        }

        public static bool IsTargetAtDestination(int x2, int y2, Rectangle [] checker)
        {
            // Determines if there is a checker piece already located at x2,y2.
            // return True if there is a piece at this location. 
            // return False if there is no piece at this location. 
            x2 = (x2 / TILE_WIDTH) * TILE_WIDTH;
            y2 = (y2 / TILE_HEIGHT) * TILE_HEIGHT;

            for (int i = 0; i < NUMBER_OF_CHECKERS; i++)
            {
                if (checker[i].X == x2 && checker[i].Y == y2)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsMovingUpDownDirection(int x1, int y1, int x2, int y2, string type)
        {
            // for the purpose of this function we assume IsDiagonallyConnected is already true 
            // we assume the starting position is (x1, y1) and the ending position is (x2, y2)
            if (type == "King")
            {
                return true;
            }
            else if (type == "Black")
            {
                // Black pieces go downward, left or right diagonally
                return y2 - y1 > 0;
            }
            else //if (type == "Red")
            {
                // Red pieces go upward, left or right diagonally
                return y2 - y1 < 0;
            }
        }

        public static bool IsMovingDiagonally(int x1, int y1, int x2, int y2)
        {
            int i1 = x1 / TILE_WIDTH;
            int i2 = x2 / TILE_WIDTH;
            int j1 = y1 / TILE_HEIGHT;
            int j2 = y2 / TILE_HEIGHT;
            return Math.Abs(i1 - i2) == Math.Abs(j1 - j2);
        }
    }
}

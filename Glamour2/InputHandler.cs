using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Glamour2
{
    // TODO make gamepad code less absolute garbage
    /*
    GAMEPAD REFERENCE GUIDE
     a, b, x, y = face buttons
     u, d, l, r = dpad
     s, t = LB/RB
     ,, z = LT/RT
     g = Start
     f = Select
     */


    public class InputHandler
    {
        Dictionary<Keys, bool> keys;
        int mleft;
        int mright;

        public static float DEADZONE = 0.1f;

        public Vector2 mousePosition;


        Dictionary<char, bool>[] gamepads;
        Vector2[] stickPositions;

        public List<Keys> pressedKeys
        {
            get { return keys.Keys.ToList<Keys>(); }
        }

        public InputHandler()
        {
            keys = new Dictionary<Keys, bool>();
            mleft = 0;
            mright = 0;
            mousePosition = Vector2.Zero;
            gamepads = new Dictionary<char, bool>[4];
            for (int x = 0; x < 4; x++) gamepads[x] = new Dictionary<char, bool>();
            stickPositions = new Vector2[8]
            {
                Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero,
                Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero
            };
        }

        public List<char> pressedButtons(int n)
        {
            return gamepads[n].Keys.ToList<char>();
        }


        // update keyboard
        public void update()
        {
            KeyboardState state = Keyboard.GetState();
            Keys[] pressed = state.GetPressedKeys();
            Keys[] handledKeys = keys.Keys.ToArray<Keys>();
            foreach (Keys k in handledKeys)
            {
                if (pressed.Contains(k) && keys[k])
                {
                    keys[k] = false;
                }
                else if (!pressed.Contains(k))
                {
                    keys.Remove(k);
                }
            }
            foreach (Keys k in pressed)
            {
                if (!keys.Keys.Contains(k))
                {
                    keys[k] = true;
                }
            }

            MouseState mouse = Microsoft.Xna.Framework.Input.Mouse.GetState(); // static reference to avoid confusion
            mousePosition = new Vector2(mouse.X, mouse.Y);
            if (mouse.LeftButton == ButtonState.Pressed) mleft++;
            else mleft = 0;
            if (mouse.RightButton == ButtonState.Pressed) mright++;
            else mright = 0;

            updateGamepad(0, GamePad.GetState(PlayerIndex.One));
            updateGamepad(1, GamePad.GetState(PlayerIndex.Two));
            updateGamepad(2, GamePad.GetState(PlayerIndex.Three));
            updateGamepad(3, GamePad.GetState(PlayerIndex.Four));
        }

        public bool isGamePadConnected(int n)
        {
            return GamePad.GetState(n).IsConnected;
        }

        // do not open unless you want to die of cancer
        public void updateGamepad(int n, GamePadState state)
        {
            Dictionary<char, bool> gamepad = gamepads[n];
            List<char> handledButtons = new List<char>(gamepad.Keys);

            if (!state.IsConnected) // no buttons pressed because controller isn't plugged in
            {
                foreach (char c in handledButtons)
                {
                    gamepad.Remove(c);
                }
                return;
            }

            // Analog sticks
            stickPositions[n] = state.ThumbSticks.Left;
            stickPositions[n + 1] = state.ThumbSticks.Right;

            // A
            if (state.Buttons.A == ButtonState.Pressed)
            {
                if (handledButtons.Contains('a') && gamepad['a'])
                {
                    gamepad['a'] = false;
                }
                else if (!handledButtons.Contains('a'))
                {
                    gamepad['a'] = true;
                }
            }
            else if (handledButtons.Contains('a'))
            {
                gamepad.Remove('a');
            }
            // B
            if (state.Buttons.B == ButtonState.Pressed)
            {
                if (handledButtons.Contains('b') && gamepad['b'])
                {
                    gamepad['b'] = false;
                }
                else if (!handledButtons.Contains('b'))
                {
                    gamepad['b'] = true;
                }
            }
            else if (handledButtons.Contains('b'))
            {
                gamepad.Remove('b');
            }
            // X
            if (state.Buttons.X == ButtonState.Pressed)
            {
                if (handledButtons.Contains('x') && gamepad['x'])
                {
                    gamepad['x'] = false;
                }
                else if (!handledButtons.Contains('x'))
                {
                    gamepad['x'] = true;
                }
            }
            else if (handledButtons.Contains('x'))
            {
                gamepad.Remove('x');
            }
            // Y
            if (state.Buttons.Y == ButtonState.Pressed)
            {
                if (handledButtons.Contains('y') && gamepad['y'])
                {
                    gamepad['y'] = false;
                }
                else if (!handledButtons.Contains('y'))
                {
                    gamepad['y'] = true;
                }
            }
            else if (handledButtons.Contains('y'))
            {
                gamepad.Remove('y');
            }

            // Up
            if (state.DPad.Up == ButtonState.Pressed)
            {
                if (handledButtons.Contains('u') && gamepad['u'])
                {
                    gamepad['u'] = false;
                }
                else if (!handledButtons.Contains('u'))
                {
                    gamepad['u'] = true;
                }
            }
            else if (handledButtons.Contains('u'))
            {
                gamepad.Remove('u');
            }
            // Down
            if (state.DPad.Down == ButtonState.Pressed)
            {
                if (handledButtons.Contains('d') && gamepad['d'])
                {
                    gamepad['d'] = false;
                }
                else if (!handledButtons.Contains('d'))
                {
                    gamepad['d'] = true;
                }
            }
            else if (handledButtons.Contains('d'))
            {
                gamepad.Remove('d');
            }
            // Left
            if (state.DPad.Left == ButtonState.Pressed)
            {
                if (handledButtons.Contains('l') && gamepad['l'])
                {
                    gamepad['l'] = false;
                }
                else if (!handledButtons.Contains('l'))
                {
                    gamepad['l'] = true;
                }
            }
            else if (handledButtons.Contains('l'))
            {
                gamepad.Remove('l');
            }
            // Right
            if (state.DPad.Right == ButtonState.Pressed)
            {
                if (handledButtons.Contains('r') && gamepad['r'])
                {
                    gamepad['r'] = false;
                }
                else if (!handledButtons.Contains('r'))
                {
                    gamepad['r'] = true;
                }
            }
            else if (handledButtons.Contains('r'))
            {
                gamepad.Remove('r');
            }
            // LB
            if (state.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                if (handledButtons.Contains('s') && gamepad['s'])
                {
                    gamepad['s'] = false;
                }
                else if (!handledButtons.Contains('s'))
                {
                    gamepad['s'] = true;
                }
            }
            else if (handledButtons.Contains('s'))
            {
                gamepad.Remove('s');
            }
            // RB
            if (state.Buttons.RightShoulder == ButtonState.Pressed)
            {
                if (handledButtons.Contains('t') && gamepad['t'])
                {
                    gamepad['t'] = false;
                }
                else if (!handledButtons.Contains('t'))
                {
                    gamepad['t'] = true;
                }
            }
            else if (handledButtons.Contains('t'))
            {
                gamepad.Remove('t');
            }
            // Start
            if (state.Buttons.Start == ButtonState.Pressed)
            {
                if (handledButtons.Contains('g') && gamepad['g'])
                {
                    gamepad['g'] = false;
                }
                else if (!handledButtons.Contains('g'))
                {
                    gamepad['g'] = true;
                }
            }
            else if (handledButtons.Contains('g'))
            {
                gamepad.Remove('g');
            }
            // LT
            if (state.Triggers.Left > 0.75f)
            {
                if (handledButtons.Contains(',') && gamepad[','])
                {
                    gamepad[','] = false;
                }
                else if (!handledButtons.Contains(','))
                {
                    gamepad[','] = true;
                }
            }
            else if (handledButtons.Contains(','))
            {
                gamepad.Remove(',');
            }
            // RT
            if (state.Triggers.Right > 0.75f)
            {
                if (handledButtons.Contains('z') && gamepad['z'])
                {
                    gamepad['z'] = false;
                }
                else if (!handledButtons.Contains('z'))
                {
                    gamepad['z'] = true;
                }
            }
            else if (handledButtons.Contains('z'))
            {
                gamepad.Remove('z');
            }
            // Select
            if (state.Buttons.Back == ButtonState.Pressed)
            {
                if (handledButtons.Contains('f') && gamepad['f'])
                {
                    gamepad['f'] = false;
                }
                else if (!handledButtons.Contains('f'))
                {
                    gamepad['f'] = true;
                }
            }
            else if (handledButtons.Contains('f'))
            {
                gamepad.Remove('f');
            }

            
        }

        public bool isKeyPressed(Keys k)
        {
            return (keys.Keys.Contains(k) && keys[k]);
        }
        public bool isKeyHeld(Keys k)
        {
            return keys.Keys.Contains(k);
        }

        public bool isMousePressed(char b)
        {
            if (b == 'l') return (mleft == 1);
            else if (b == 'r') return (mright == 1);
            else if (b == 'e') return (mright == 1 || mleft == 1);
            else return false;
        }
        public bool isMouseHeld(char b)
        {
            if (b == 'l') return (mleft > 0);
            else if (b == 'r') return (mright > 0);
            else if (b == 'e') return (mright > 0 || mleft > 0);
            else return false;
        }

        public bool isButtonPressed(int n, char b)
        {
            Dictionary<char, bool> gamepad = gamepads[n];
            return (gamepad.ContainsKey(b) && gamepad[b]);
        }
        public bool isButtonHeld(int n, char b)
        {
            Dictionary<char, bool> gamepad = gamepads[n];
            return (gamepad.ContainsKey(b));
        }
        public Vector2 stickPosition(int n, char s)
        {
            n *= 2;
            if (s == 'r') n++;
            return stickPositions[n];
        }

    }
}

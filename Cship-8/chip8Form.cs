using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Cship_8
{
    public partial class chip8Form : Form
    {
        Bitmap bm;
        Interpreter Ir;

        public chip8Form()
        {
            InitializeComponent();
            Ir = new Interpreter();
            bm = new Bitmap(64, 32);
            Canvas.Image = bm;
        }

        private void Draw(object sender, PaintEventArgs e)
        {
            switch (Ir.screenState)
            {
                // Clear the screen
                case Interpreter.ScreenState.CLEAR:
                    e.Graphics.Clear(Color.White);
                    Ir.screenState = Interpreter.ScreenState.IDLE;
                    break;
                // Draw from screen array
                case Interpreter.ScreenState.DRAW:
                    for (int y = 0; y < 32; y++)
                    {
                        for (int x = 0; x < 64; x++)
                        {
                            if (Ir.screen[x + y * 64] == 1)
                            {
                                bm.SetPixel(x, y, Color.Black);
                            }
                            else
                            {
                                bm.SetPixel(x, y, Color.White);
                            }
                        }
                    }
                    Ir.screenState = Interpreter.ScreenState.IDLE;
                    break;
                // Do nothing
                case Interpreter.ScreenState.IDLE:
                    break;
            }
        }

        private void Cycle(object sender, EventArgs e)
        {
            Ir.Cycle(this);
        }

        

        private void CheckKeyDown(object sender, KeyEventArgs e)
        {

        }
        private void CheckKeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}

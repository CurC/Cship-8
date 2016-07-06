using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
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
            bm = new Bitmap(640, 320);
            Canvas.Image = bm;
        }

        private void Draw(object sender, PaintEventArgs e)
        {
            switch (Ir.screenState)
            {
                // Clear the screen
                case Interpreter.ScreenState.CLEAR:
                    bm = new Bitmap(bm.Width, bm.Height);
                    Canvas.Image = bm;
                    Ir.screenState = Interpreter.ScreenState.IDLE;
                    break;
                // Draw from screen array
                case Interpreter.ScreenState.DRAW:
                    BitmapData bmData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    unsafe
                    {
                        byte* p = (byte*) bmData.Scan0;
                        for (int y = 0; y < ClientSize.Height; y++)
                        {
                            for (int x = 0; x < ClientSize.Width; x++)
                            {
                                p[0] = Ir.screen[x / 10, y / 10] == 1 ? (byte) 255 : (byte) 0;
                                p[1] = Ir.screen[x / 10, y / 10] == 1 ? (byte) 255 : (byte) 0;
                                p[2] = Ir.screen[x / 10, y / 10] == 1 ? (byte) 255 : (byte) 0;
                                p[3] = (byte) 255;
                                p += 4;
                            }
                        }
                    }

                    bm.UnlockBits(bmData);
                    Canvas.Image = bm;
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
            switch (e.KeyCode)
            {
                case Keys.D1:
                    Ir.keys[1] = 1;
                    break;
                case Keys.D2:
                    Ir.keys[2] = 1;
                    break;
                case Keys.D3:
                    Ir.keys[3] = 1;
                    break;
                case Keys.Q:
                    Ir.keys[4] = 1;
                    break;
                case Keys.W:
                    Ir.keys[5] = 1;
                    break;
                case Keys.E:
                    Ir.keys[6] = 1;
                    break;
                case Keys.A:
                    Ir.keys[7] = 1;
                    break;
                case Keys.S:
                    Ir.keys[8] = 1;
                    break;
                case Keys.D:
                    Ir.keys[9] = 1;
                    break;
                case Keys.Z:
                    Ir.keys[10] = 1;
                    break;
                case Keys.X:
                    Ir.keys[0] = 1;
                    break;
                case Keys.C:
                    Ir.keys[11] = 1;
                    break;
                case Keys.D4:
                    Ir.keys[12] = 1;
                    break;
                case Keys.R:
                    Ir.keys[13] = 1;
                    break;
                case Keys.F:
                    Ir.keys[14] = 1;
                    break;
                case Keys.V:
                    Ir.keys[15] = 1;
                    break;
            }
        }
        private void CheckKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                    Ir.keys[1] = 0;
                    break;
                case Keys.D2:
                    Ir.keys[2] = 0;
                    break;
                case Keys.D3:
                    Ir.keys[3] = 0;
                    break;
                case Keys.Q:
                    Ir.keys[4] = 0;
                    break;
                case Keys.W:
                    Ir.keys[5] = 0;
                    break;
                case Keys.E:
                    Ir.keys[6] = 0;
                    break;
                case Keys.A:
                    Ir.keys[7] = 0;
                    break;
                case Keys.S:
                    Ir.keys[8] = 0;
                    break;
                case Keys.D:
                    Ir.keys[9] = 0;
                    break;
                case Keys.Z:
                    Ir.keys[10] = 0;
                    break;
                case Keys.X:
                    Ir.keys[0] = 0;
                    break;
                case Keys.C:
                    Ir.keys[11] = 0;
                    break;
                case Keys.D4:
                    Ir.keys[12] = 0;
                    break;
                case Keys.R:
                    Ir.keys[13] = 0;
                    break;
                case Keys.F:
                    Ir.keys[14] = 0;
                    break;
                case Keys.V:
                    Ir.keys[15] = 0;
                    break;
            }
        }
    }
}

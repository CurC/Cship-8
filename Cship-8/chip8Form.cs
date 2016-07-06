using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Cship_8
{
    public partial class chip8Form : Form
    {
        Bitmap bm;
        Interpreter Ir;
        Dictionary<Keys, int> mapping;

        public chip8Form()
        {
            InitializeComponent();
            Ir = new Interpreter();
            bm = new Bitmap(640, 320);
            Canvas.Image = bm;
            SetMapping();
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

        public void SetMapping()
        {
            mapping = new Dictionary<Keys, int>
            {
                { Keys.D1, 0x1 },
                { Keys.D2, 0x2 },
                { Keys.D3, 0x3 },
                { Keys.Q, 0x4 },
                { Keys.W, 0x5 },
                { Keys.E, 0x6 },
                { Keys.A, 0x7 },
                { Keys.S, 0x8 },
                { Keys.D, 0x9 },
                { Keys.Z, 0xA },
                { Keys.X, 0x0 },
                { Keys.C, 0xB },
                { Keys.D4, 0xC },
                { Keys.R, 0xD },
                { Keys.F, 0xE },
                { Keys.V, 0xF },
            };
        }
        private void CheckKeyDown(object sender, KeyEventArgs e)
        {
            if (mapping.ContainsKey(e.KeyCode))
            {
                Ir.keys[mapping[e.KeyCode]] = 1;
            }
        }
        private void CheckKeyUp(object sender, KeyEventArgs e)
        {
            if (mapping.ContainsKey(e.KeyCode))
            {
                Ir.keys[mapping[e.KeyCode]] = 0;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cship_8
{
    public partial class MainForm : Form
    {
        SystemState cpustate = new SystemState();
        public MainForm()
        {
            InitializeComponent();
        }

        private void drawable_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Black, new Rectangle());
        }
    }
}

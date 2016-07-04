namespace Cship_8
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.drawable = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.drawable)).BeginInit();
            this.SuspendLayout();
            // 
            // drawable
            // 
            this.drawable.Location = new System.Drawing.Point(0, 0);
            this.drawable.Name = "drawable";
            this.drawable.Size = new System.Drawing.Size(640, 320);
            this.drawable.TabIndex = 0;
            this.drawable.TabStop = false;
            this.drawable.Paint += new System.Windows.Forms.PaintEventHandler(this.drawable_Paint);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 320);
            this.Controls.Add(this.drawable);
            this.Margin = new System.Windows.Forms.Padding(4, 20, 4, 20);
            this.Name = "MainForm";
            this.Text = "Cship-8";
            ((System.ComponentModel.ISupportInitialize)(this.drawable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox drawable;
    }
}


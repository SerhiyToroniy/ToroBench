
namespace Benchmark
{
    partial class ProgressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            this.guna2CircleProgressBar1 = new Guna.UI2.WinForms.Guna2CircleProgressBar();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.guna2GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2CircleProgressBar1
            // 
            this.guna2CircleProgressBar1.Animated = true;
            this.guna2CircleProgressBar1.AnimationSpeed = 0.4F;
            this.guna2CircleProgressBar1.FillColor = System.Drawing.Color.Gainsboro;
            this.guna2CircleProgressBar1.FillThickness = 5;
            this.guna2CircleProgressBar1.Font = new System.Drawing.Font("Segoe UI Semilight", 30F);
            this.guna2CircleProgressBar1.ForeColor = System.Drawing.Color.DimGray;
            this.guna2CircleProgressBar1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.guna2CircleProgressBar1.InnerColor = System.Drawing.SystemColors.Control;
            this.guna2CircleProgressBar1.Location = new System.Drawing.Point(112, 20);
            this.guna2CircleProgressBar1.Minimum = 0;
            this.guna2CircleProgressBar1.Name = "guna2CircleProgressBar1";
            this.guna2CircleProgressBar1.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.guna2CircleProgressBar1.ProgressStartCap = System.Drawing.Drawing2D.LineCap.Round;
            this.guna2CircleProgressBar1.ProgressThickness = 10;
            this.guna2CircleProgressBar1.ShadowDecoration.Color = System.Drawing.Color.LightGray;
            this.guna2CircleProgressBar1.ShadowDecoration.Enabled = true;
            this.guna2CircleProgressBar1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.guna2CircleProgressBar1.ShadowDecoration.Parent = this.guna2CircleProgressBar1;
            this.guna2CircleProgressBar1.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(3);
            this.guna2CircleProgressBar1.ShowPercentage = true;
            this.guna2CircleProgressBar1.Size = new System.Drawing.Size(150, 150);
            this.guna2CircleProgressBar1.TabIndex = 32;
            this.guna2CircleProgressBar1.Text = "guna2CircleProgressBar1";
            // 
            // guna2Button1
            // 
            this.guna2Button1.Animated = true;
            this.guna2Button1.AutoRoundedCorners = true;
            this.guna2Button1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Button1.BorderRadius = 15;
            this.guna2Button1.CheckedState.Parent = this.guna2Button1;
            this.guna2Button1.CustomImages.Parent = this.guna2Button1;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.DisabledState.Parent = this.guna2Button1;
            this.guna2Button1.FillColor = System.Drawing.Color.LightGray;
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.Black;
            this.guna2Button1.HoverState.Parent = this.guna2Button1;
            this.guna2Button1.Location = new System.Drawing.Point(239, 185);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.ShadowDecoration.BorderRadius = 20;
            this.guna2Button1.ShadowDecoration.Color = System.Drawing.Color.Gainsboro;
            this.guna2Button1.ShadowDecoration.Depth = 20;
            this.guna2Button1.ShadowDecoration.Enabled = true;
            this.guna2Button1.ShadowDecoration.Parent = this.guna2Button1;
            this.guna2Button1.Size = new System.Drawing.Size(106, 33);
            this.guna2Button1.TabIndex = 33;
            this.guna2Button1.Text = "Stop";
            this.guna2Button1.UseTransparentBackground = true;
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2GroupBox1.BorderColor = System.Drawing.SystemColors.Control;
            this.guna2GroupBox1.BorderRadius = 30;
            this.guna2GroupBox1.Controls.Add(this.guna2Button1);
            this.guna2GroupBox1.Controls.Add(this.guna2CircleProgressBar1);
            this.guna2GroupBox1.CustomBorderColor = System.Drawing.SystemColors.Control;
            this.guna2GroupBox1.FillColor = System.Drawing.SystemColors.Control;
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(218)))), ((int)(((byte)(223)))));
            this.guna2GroupBox1.Location = new System.Drawing.Point(12, 23);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.ShadowDecoration.BorderRadius = 40;
            this.guna2GroupBox1.ShadowDecoration.Enabled = true;
            this.guna2GroupBox1.ShadowDecoration.Parent = this.guna2GroupBox1;
            this.guna2GroupBox1.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(1, 0, 1, 5);
            this.guna2GroupBox1.Size = new System.Drawing.Size(370, 234);
            this.guna2GroupBox1.TabIndex = 33;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(395, 280);
            this.Controls.Add(this.guna2GroupBox1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProgressForm_FormClosed);
            this.Load += new System.EventHandler(this.ProgressForm_Load_1);
            this.Shown += new System.EventHandler(this.ProgressForm_Shown);
            this.guna2GroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        //private ComponentFactory.Krypton.Toolkit.KryptonPalette kryptonPalette1;
        private Guna.UI2.WinForms.Guna2CircleProgressBar guna2CircleProgressBar1;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
    }
}
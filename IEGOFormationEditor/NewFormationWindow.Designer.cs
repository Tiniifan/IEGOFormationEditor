
namespace FormationInazumaClass
{
    partial class NewFormationWindow
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
            this.battleButton = new System.Windows.Forms.Button();
            this.matchButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // battleButton
            // 
            this.battleButton.Location = new System.Drawing.Point(14, 55);
            this.battleButton.Name = "battleButton";
            this.battleButton.Size = new System.Drawing.Size(104, 23);
            this.battleButton.TabIndex = 0;
            this.battleButton.Text = "Battle";
            this.battleButton.UseVisualStyleBackColor = true;
            this.battleButton.Click += new System.EventHandler(this.BattleButton_Click);
            // 
            // matchButton
            // 
            this.matchButton.Location = new System.Drawing.Point(124, 55);
            this.matchButton.Name = "matchButton";
            this.matchButton.Size = new System.Drawing.Size(104, 23);
            this.matchButton.TabIndex = 1;
            this.matchButton.Text = "Match";
            this.matchButton.UseVisualStyleBackColor = true;
            this.matchButton.Click += new System.EventHandler(this.MatchButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select your formation type";
            // 
            // NewFormationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(255, 101);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.matchButton);
            this.Controls.Add(this.battleButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewFormationWindow";
            this.Text = "Insert";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button battleButton;
        private System.Windows.Forms.Button matchButton;
        private System.Windows.Forms.Label label1;
    }
}
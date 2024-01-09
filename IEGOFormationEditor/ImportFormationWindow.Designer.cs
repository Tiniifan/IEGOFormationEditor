
namespace FormationInazumaClass
{
    partial class ImportFormationWindow
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
            this.formationListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // formationListBox
            // 
            this.formationListBox.FormattingEnabled = true;
            this.formationListBox.Location = new System.Drawing.Point(12, 12);
            this.formationListBox.Name = "formationListBox";
            this.formationListBox.Size = new System.Drawing.Size(120, 290);
            this.formationListBox.TabIndex = 0;
            this.formationListBox.SelectedIndexChanged += new System.EventHandler(this.FormationListBox_SelectedIndexChanged);
            // 
            // ImportFormationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(141, 312);
            this.Controls.Add(this.formationListBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImportFormationWindow";
            this.Text = "Import";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox formationListBox;
    }
}
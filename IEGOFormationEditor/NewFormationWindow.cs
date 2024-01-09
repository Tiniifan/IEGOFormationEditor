using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormationInazumaClass
{
    public partial class NewFormationWindow : Form
    {
        public bool IsMatchFormation = false;

        public NewFormationWindow()
        {
            InitializeComponent();
        }

        private void BattleButton_Click(object sender, EventArgs e)
        {
            IsMatchFormation = false;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void MatchButton_Click(object sender, EventArgs e)
        {
            IsMatchFormation = true;
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}

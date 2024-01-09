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
    public partial class ImportFormationWindow : Form
    {
        public int SelectedIndex;

        public string SlectedItem;

        public ImportFormationWindow(string[] names)
        {
            InitializeComponent();
            formationListBox.Items.AddRange(names);
        }

        private void FormationListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (formationListBox.SelectedIndex == -1) return;

            SelectedIndex = formationListBox.SelectedIndex;
            SlectedItem = formationListBox.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonogameSamples.Engine.Editor.DialogForms
{
    public partial class GetNameForm : Form
    {
        public GetNameForm(string name)
        {
            InitializeComponent();
            this.Text = name;
        }

        public string Name;

        private void GetNameForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Name = NameBox.Text;
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

        }

        private void NameBox_TextChanged(object sender, EventArgs e)
        {
            if (NameBox.Text.Length > 0)
            {
                buttonOk.Enabled = true;
            } else
            {
                buttonOk.Enabled = false;
            }
        }
    }
}

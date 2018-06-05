using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace B18_Ex05.GUI
{
    public partial class SettingForm : Form
    {
        public int BoardSize { get; set; }

        public bool AgeinstComputer { get; set; }

        public string Player1Name { get; set; }

        public string Player2Name { get; set; }

        public SettingForm()
        {
            InitializeComponent();
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            AgeinstComputer = !checkBoxPlayer2.Checked;
            Player1Name = textBoxPlayer1Name.Text;
            Player2Name = textBoxPlayer2Name.Text;
            this.Close();
        }

        private void textBoxPlayer2Name_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked == true)
            {
                textBoxPlayer2Name.Text = string.Empty;
                textBoxPlayer2Name.ForeColor = Color.Black;
                textBoxPlayer2Name.Enabled = true;
            }
            else
            {
                textBoxPlayer2Name.Text = "Computer";
                textBoxPlayer2Name.Enabled = false;
            }
        }

        private void radioButton6x6_Click(object sender, EventArgs e)
        {
            BoardSize = 6;
        }

        private void radioButton8x8_Click(object sender, EventArgs e)
        {
            BoardSize = 8;
        }

        private void radioButton10x10_Click(object sender, EventArgs e)
        {
            BoardSize = 10;
        }

        private void textBoxPlayer1Name_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPlayer1Name.Text == string.Empty || textBoxPlayer2Name.Text == string.Empty)
            {
                buttonDone.Enabled = false;
            }
            else
            {
                buttonDone.Enabled = true;
            }
        }

        private void textBoxPlayer2Name_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPlayer1Name.Text == string.Empty || textBoxPlayer2Name.Text == string.Empty)
            {
                buttonDone.Enabled = false;
            }
            else
            {
                buttonDone.Enabled = true;
            }
        }
    }
}

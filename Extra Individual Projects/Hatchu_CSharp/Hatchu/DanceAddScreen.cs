using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hatchu
{
    public partial class DanceAddScreen : Form
    {
        public List<string> danceTypes = new List<string>();

        private readonly Form Hatchu;

        public DanceAddScreen(ComboBox.ObjectCollection collection, Form host)
        {
            InitializeComponent();

            Hatchu = host;

            foreach (string item in collection)
            {
                danceTypes.Add(item);
            }

            danceTypeTxt.Lines = danceTypes.ToArray();
        }

        private void danceTypeBtn_Click(object sender, EventArgs e)
        {
            danceTypes.Clear();
            foreach (string line in danceTypeTxt.Lines)
            {
                danceTypes.Add(line);
            }

            danceTypeBtn.BackColor = Color.FromArgb(128, 255, 128);
        }

        private void danceTypeTxt_TextChanged(object sender, EventArgs e)
        {
            danceTypeBtn.BackColor = Color.FromArgb(255, 128, 128);
        }
    }
}

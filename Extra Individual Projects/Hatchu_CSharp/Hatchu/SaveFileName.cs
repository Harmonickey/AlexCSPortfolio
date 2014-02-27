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
    public partial class SaveFileName : Form
    {
        System.Windows.Forms.Form Hatchu;

        public string saveFileName;

        public SaveFileName(System.Windows.Forms.Form host)
        {
            InitializeComponent();

            Hatchu = host;
        }

        public SaveFileName(System.Windows.Forms.Form host, string loadedFileName)
        {
            InitializeComponent();

            Hatchu = host;

            fileNameTxt.Text = loadedFileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileName = fileNameTxt.Text;

            this.Close();
        }

        private void SaveFileName_Load(object sender, EventArgs e)
        {

        }
    }
}

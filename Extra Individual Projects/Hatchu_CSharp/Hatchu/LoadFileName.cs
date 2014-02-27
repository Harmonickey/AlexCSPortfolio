using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Hatchu
{
    public partial class LoadFileName : Form
    {
        System.Windows.Forms.Form Hatchu;

        public string loadFileName;

        public LoadFileName(Form host)
        {
            InitializeComponent();

            Hatchu = host;

            if (!File.Exists("sessions.txt"))
            {
                File.Create("sessions.txt");
            }
            
            //look for session names in the session name xml file
            using (StreamReader sr = new StreamReader("sessions.txt"))
            {
                while (sr.Peek() > 0)
                {
                    comboBox1.Items.Add(sr.ReadLine());
                }
                if (comboBox1.Items.Count > 0)
                    comboBox1.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            loadFileName = comboBox1.SelectedItem.ToString();

            this.Close();
        }
    }
}

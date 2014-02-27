using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hatchu
{
    class DanceTypeBoxArray : System.Collections.CollectionBase
    {
        public int miniCount = 0;
        public int levelCount = 0;

        private readonly Form Hatchu;

        public static Dictionary<string, int> amountOfTypes = new Dictionary<string, int>();

        public DanceTypeBoxArray(Form host)
        {
           Hatchu = host;
           this.AddNewComboBox();
        }

        public ComboBox this[int Index]
        {
            get
            {
                return (ComboBox)this.List[Index];
            }
        }

        public ComboBox AddNewComboBox()
        {

            ComboBox newComboBox = new
                ComboBox();

            //add to the collection list
            this.List.Add(newComboBox);

            //add to the overall form controls
            Hatchu.Controls.Add(newComboBox);

            newComboBox.Top = 22 + levelCount * 170;
            newComboBox.Width = 95;
            newComboBox.Left = miniCount * (newComboBox.Width + 10) + 80;
            newComboBox.Tag = this.Count;
            newComboBox.Text = "";
            newComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            newComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            newComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(EnterHandler);

            AddDanceTypes(newComboBox);

            miniCount++;

            if (Count % 6 == 0)
            {
                miniCount = 0;
                levelCount++;
            }

            return newComboBox;
        }

        public void Remove()
        {
            // Check to be sure there is a button to remove.
            if (this.Count > 0)
            {
                // Remove the last button added to the array from the host form 
                // controls collection. Note the use of the indexer in accessing 
                // the array.
                Hatchu.Controls.Remove(this[this.Count - 1]);
                this.List.RemoveAt(this.Count - 1);
            }
        }

        private void AddDanceTypes(ComboBox thisBox)
        {
            thisBox.Items.Add("Salsa");
            thisBox.Items.Add("Swing");
            thisBox.Items.Add("Cha-Cha");
            thisBox.Items.Add("Waltz");
            thisBox.Items.Add("Foxtrot");
            thisBox.Items.Add("Tango");
            thisBox.Items.Add("Hustle");
            thisBox.Items.Add("Country");
            thisBox.Items.Add("Modern");
            thisBox.Items.Add("Jive");
            thisBox.Items.Add("Blues");
            thisBox.Items.Add("Samba");
            thisBox.Items.Add("Rumba");
            thisBox.Items.Add("Quickstep");
            thisBox.Items.Add("Polka");
            thisBox.Items.Add("Fusion");
        }

        public void EnterHandler(Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ComboBox currentComboBox = ((ComboBox)sender);

                currentComboBox.Text = currentComboBox.SelectedText.ToString();
            }
        }
    }
}

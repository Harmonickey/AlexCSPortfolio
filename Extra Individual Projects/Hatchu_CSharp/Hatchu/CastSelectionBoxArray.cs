using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hatchu
{

    class CastSelectionBoxArray : System.Collections.CollectionBase
    {
        public int miniCount = 0;
        public int levelCount = 0;

        private readonly Form Hatchu;

        private CastListBoxArray connectedListBoxes;

        public CastSelectionBoxArray(Form host, CastListBoxArray associatedListBox)
        {
           Hatchu = host;
           this.AddNewComboBox(associatedListBox);
        }

        public ComboBox this[int Index]
        {
            get
            {
                return (ComboBox)this.List[Index];
            }
        }

        public ComboBox AddNewComboBox(CastListBoxArray associatedListBoxes)
        {
            connectedListBoxes = associatedListBoxes;

            ComboBox newComboBox = new
                ComboBox();

            //add to the collection list
            this.List.Add(newComboBox);

            //add to the overall form controls
            Hatchu.Controls.Add(newComboBox);

            newComboBox.Top = 45 + levelCount * 170;
            newComboBox.Width = 95;
            newComboBox.Left = miniCount * (newComboBox.Width + 10) + 80;
            newComboBox.Tag = this.Count;
            newComboBox.Text = "";
            newComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            newComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;

            newComboBox.SelectedIndexChanged += new System.EventHandler(ClickHandler);
            newComboBox.KeyDown += new System.Windows.Forms.KeyEventHandler(EnterHandler);
            
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

        public void ClickHandler(Object sender, System.EventArgs e)
        {
            AddToList(sender, false);
        }

        public void EnterHandler(Object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                AddToList(sender, true);
        }

        private void AddToList(Object sender, bool enter)
        {
            ComboBox currentComboBox = ((ComboBox)sender);
            int index = this.List.IndexOf(currentComboBox);
            if (!enter)
            {
                if (currentComboBox.SelectedItem.ToString() != "")
                {
                    bool alreadySelected = connectedListBoxes.HasCastMember(currentComboBox.SelectedItem.ToString(), connectedListBoxes[index]);
                    if (currentComboBox.SelectedItem != null && !alreadySelected)
                        connectedListBoxes[index].Items.Add(currentComboBox.SelectedItem);

                    if (alreadySelected)
                        MessageBox.Show("You already have this cast member in the piece!", "Error!", MessageBoxButtons.OK);
                }
            }
        }


        public bool HasEmpty()
        {
            foreach (ComboBox list in this.List)
            {
                if (list.Items.Count == 0) return true;
            }

            return false;
        }
    }
}

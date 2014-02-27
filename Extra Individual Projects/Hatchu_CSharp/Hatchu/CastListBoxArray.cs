using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hatchu
{
    class CastListBoxArray : System.Collections.CollectionBase
    {
        public int miniCount = 0;
        public int levelCount = 0;

        private readonly Form Hatchu;

        public CastListBoxArray(Form host)
        {
           Hatchu = host;
           this.AddNewListBox();
        }

        public ListBox this[int Index]
        {
            get
            {
                return (ListBox)this.List[Index];
            }
        }

        public ListBox AddNewListBox()
        {
            ListBox  newListBox = new
                ListBox();

            //add to the collection list
            this.List.Add(newListBox);

            //add to the overall form controls
            Hatchu.Controls.Add(newListBox);

            newListBox.Top = 69 + levelCount * 170;
            newListBox.Width = 95;
            newListBox.Left = miniCount * (newListBox.Width + 10) + 80;
            newListBox.Tag = this.Count;
            newListBox.Text = "";

            miniCount++;

            if (Count % 6 == 0)
            {
                miniCount = 0;
                levelCount++;
            }

            newListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(KeyDownHandler);
            

            return newListBox;
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

        public void KeyDownHandler(Object sender, KeyEventArgs e)
        {
            ListBox currentListBox = ((System.Windows.Forms.ListBox)sender);

            if (e.KeyCode == Keys.Delete)
            {
                int index = currentListBox.SelectedIndex;
                if (index == currentListBox.Items.Count - 1)
                    index--;

                currentListBox.Items.Remove(currentListBox.SelectedItem);

                if (currentListBox.Items.Count != 0)
                    currentListBox.SelectedIndex = index;
            }
        }

        public bool HasCastMember(string castMember, ListBox whichList)
        {
            if (whichList.Items.Contains(castMember)) return true;
            else return false;
        }
    }
}

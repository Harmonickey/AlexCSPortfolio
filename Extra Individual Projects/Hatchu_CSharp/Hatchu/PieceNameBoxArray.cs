using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hatchu
{
    class PieceNameBoxArray : System.Collections.CollectionBase
    {
        public int miniCount = 0;
        public int levelCount = 0;

        private readonly System.Windows.Forms.Form Hatchu;

        public PieceNameBoxArray(System.Windows.Forms.Form host)
        {
           Hatchu = host;
           this.AddNewTextBox();
        }

        public System.Windows.Forms.TextBox this[int Index]
        {
            get
            {
                return (System.Windows.Forms.TextBox)this.List[Index];
            }
        }

        public System.Windows.Forms.TextBox AddNewTextBox()
        {
            System.Windows.Forms.TextBox newTextBox = new
                System.Windows.Forms.TextBox();

            //add to the collection list
            this.List.Add(newTextBox);

            //add to the overall form controls
            Hatchu.Controls.Add(newTextBox);

            newTextBox.Top = levelCount * 170;
            newTextBox.Width = 95;
            newTextBox.Left = miniCount * (newTextBox.Width + 10) + 80; 
            newTextBox.Tag = this.Count;
            newTextBox.Text = "";

            miniCount++;

            if (Count % 6 == 0)
            {
                miniCount = 0;
                levelCount++;
            }

            return newTextBox;
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
    }
}

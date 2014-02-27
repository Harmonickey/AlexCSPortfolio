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
using System.Xml.Serialization;
using System.Xml;

namespace Hatchu
{
    public partial class Hatchu : Form
    {
        PieceNameBoxArray textBoxArray;
        CastListBoxArray listBoxArray;
        CastSelectionBoxArray comboBoxArray;
        DanceTypeBoxArray danceTypeBoxArray;
        List<CastList> castListArray;

        ShowOrderPosition showOrderPosition;
        ShowOrder showOrder;

        List<ShowOrder> solutionList;    //our final solution list of possible show orders

        public int numberOfPieces, listThreshold, intermission;

        bool beingLoaded = false;

        public string loadedFileName;

        public Hatchu()
        {
            InitializeComponent();

            listBoxArray = new CastListBoxArray(this);
            comboBoxArray = new CastSelectionBoxArray(this, listBoxArray);
            danceTypeBoxArray = new DanceTypeBoxArray(this);
            textBoxArray = new PieceNameBoxArray(this);
            showOrder = new ShowOrder(this);
            showOrderPosition = new ShowOrderPosition(this);
            castListArray = new List<CastList>();
            solutionList = new List<ShowOrder>();

            listBoxArray[0].Parent = panelContainer;
            comboBoxArray[0].Parent = panelContainer;
            danceTypeBoxArray[0].Parent = panelContainer;
            textBoxArray[0].Parent = panelContainer;

            listBoxArray[0].Visible = false;
            comboBoxArray[0].Visible = false;
            danceTypeBoxArray[0].Visible = false;
            textBoxArray[0].Visible = false;

            pieceNameLbl.Parent = panelContainer;
            danceTypeLbl.Parent = panelContainer;
            castListLbl.Parent = panelContainer;
            castMembersLbl.Parent = panelContainer;
            

            solutionsContainer.Parent = this;
            solutionsContainer.Location = panelContainer.Location;
            loadingBox.Parent = this;
            loadingBox.Location = new Point(this.Width / 2, this.Height / 2);

            foreach (string item in danceTypeBoxArray[0].Items)
                DanceTypeBoxArray.amountOfTypes.Add(item, 0);

            SwitchVisibility("startup");

            if (!File.Exists("sessions.txt"))
            {
                loadSession.Enabled = false;
            }
        }

        private void castListTxt_TextChanged(object sender, EventArgs e)
        {
            if (!beingLoaded)
            {
                //iterate over each combo list
                foreach (ComboBox comboList in comboBoxArray)
                {
                    //empty out the list of items
                    comboList.Items.Clear();
                    //add in the new list that was just created
                    foreach (string line in castListTxt.Lines)
                    {
                        comboList.Items.Add(line);
                    }
                }
            }
        }

        public void AddNewPiece()
        {
            // Create a new instance of the Button class.
            ListBox listBox = listBoxArray.AddNewListBox();
            ComboBox comboBox = comboBoxArray.AddNewComboBox(listBoxArray);
            ComboBox comboBox2 = danceTypeBoxArray.AddNewComboBox();
            TextBox textBox = textBoxArray.AddNewTextBox();

            listBox.Parent = panelContainer;
            comboBox.Parent = panelContainer;
            comboBox2.Parent = panelContainer;
            textBox.Parent = panelContainer;

            listBoxArray[0].Visible = true;
            comboBoxArray[0].Visible = true;
            danceTypeBoxArray[0].Visible = true;
            textBoxArray[0].Visible = true;

        }

        private void createBtn_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < Convert.ToInt32(numPiecesTxt.Text) - 1; i++)
            {
                AddNewPiece();

                CastList aCastList = new CastList(this);
                castListArray.Add(aCastList);

                List<int> list = new List<int>();
                showOrderPosition.Add(list);

            }

            PopulateComboBoxes();

            numberOfPieces = castListArray.Count;

            firstHalfTxt.Text = (numberOfPieces / 2).ToString();
            limitTxt.Text = (numberOfPieces / 4).ToString();
            
            SwitchVisibility("created");
        }

        private void SwitchVisibility(string which)
        {
            switch (which)
            {
                case "startup":
                {
                    restartBtn.Enabled = false;
                    saveSession.Enabled = false;
                    danceTypeAdd.Enabled = false;

                    maxSolutionsLbl.Enabled = false;
                    thresholdTxt.Enabled = false;
                    break;
                }
                case "created":
                case "loaded":
                {
                    numPiecesLbl.Enabled = false;
                    numPiecesTxt.Enabled = false;
                    createBtn.Enabled = false;

                    firstHalfLbl.Enabled = true;
                    maxTypeLbl.Enabled = true;
                    limitTxt.Enabled = true;
                    maxSolutionsLbl.Enabled = true;
                    thresholdTxt.Enabled = true;
            
                    firstHalfTxt.Enabled = true;
                    orderingBtn.Enabled = true;

                    loadSession.Enabled = false;
                    saveSession.Enabled = true;
                    danceTypeAdd.Enabled = true;
                    break;
                }
                case "solutions":
                {
                    panelContainer.Visible = false;
                    solutionsContainer.Visible = true;

                    restartBtn.Enabled = true;

                    firstHalfLbl.Enabled = false;
                    maxTypeLbl.Enabled = false;
                    limitTxt.Enabled = false;
                    maxSolutionsLbl.Enabled = false;
                    thresholdTxt.Enabled = false;

                    firstHalfTxt.Enabled = false;
                    orderingBtn.Enabled = false;
                    break;
                }
                case "restart":
                {
                    panelContainer.Visible = true;
                    solutionsContainer.Visible = false;

                    restartBtn.Enabled = false;
                    firstHalfLbl.Enabled = true;
                    maxTypeLbl.Enabled = true;
                    limitTxt.Enabled = true;
                    maxSolutionsLbl.Enabled = true;
                    thresholdTxt.Enabled = true;

                    firstHalfTxt.Enabled = true;
                    orderingBtn.Enabled = true;
                    break;
                }
            }
        }

        private void PopulateComboBoxes()
        {
            if (castListTxt.Lines.Length > comboBoxArray[0].Items.Count ||
                comboBoxArray.HasEmpty())
            {
                foreach (ComboBox comboList in comboBoxArray)
                {
                    //empty out the list of items
                    comboList.Items.Clear();
                    //add in the new list that was just created
                    foreach (string line in castListTxt.Lines)
                    {
                        comboList.Items.Add(line);
                    }
                }
            }
        }

        public void WriteXML()
        {
            SaveFileName form_popup;
            if (loadedFileName != null)
                form_popup = new SaveFileName(this, loadedFileName);
            else
                form_popup = new SaveFileName(this);

            form_popup.ShowDialog();

            XmlSerializer stringSerializer = 
                new XmlSerializer(typeof(string));

            XmlSerializer integerSerializer =
                new XmlSerializer(typeof(int));

            string sessionPath = form_popup.saveFileName + "/" + form_popup.saveFileName;

            string filename = sessionPath + ".xml";

            if (!Directory.Exists(form_popup.saveFileName))
            {
                Directory.CreateDirectory(form_popup.saveFileName);
            }
            
            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fileStream))
            using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
            {
                foreach (ListBox listBox in listBoxArray)
                {
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlWriter.Indentation = 4;

                    xmlWriter.WriteStartElement("int");
                    xmlWriter.WriteValue(listBox.Items.Count);
                    xmlWriter.WriteEndElement();

                    foreach (string castMember in listBox.Items)
                    {
                        xmlWriter.WriteStartElement("string");
                        xmlWriter.WriteString(castMember);
                        xmlWriter.WriteEndElement();
                    }
                }
            }

            filename = sessionPath + "Titles.xml";

            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fileStream))
            using (XmlTextWriter xmlWriter = new XmlTextWriter(sw))
            {

                foreach (TextBox title in textBoxArray)
                {
                    xmlWriter.WriteStartElement("string");
                    xmlWriter.WriteString(title.Text);
                    xmlWriter.WriteEndElement();
                }
            }

            using (StreamWriter sw = new StreamWriter(sessionPath + "CastList.txt"))
            {
                foreach (string line in castListTxt.Lines)
                    sw.WriteLine(line);
            }

            using (StreamWriter sw = new StreamWriter(sessionPath + "DanceTypes.txt"))
            {
                foreach (ComboBox danceType in danceTypeBoxArray)
                    sw.WriteLine(danceType.SelectedItem);
            }

            
            using (StreamWriter sw = new StreamWriter(sessionPath + "DanceTypesExtra.txt"))
            {
                for (int i = 16; i < danceTypeBoxArray[0].Items.Count; i++)
                {
                    sw.WriteLine(danceTypeBoxArray[0].Items[i]);
                }

            }
            
            List<string> sessions = new List<string>();

            if (!File.Exists("sessions.txt"))
            {
                File.Create("sessions.txt");
            }

            using (StreamReader sr = new StreamReader("sessions.txt"))
            {
                while (sr.Peek() > 0)
                {
                    sessions.Add(sr.ReadLine());
                }
            }

            using (StreamWriter sw = new StreamWriter("sessions.txt"))
            {
                foreach (string session in sessions)
                {
                    sw.WriteLine(session);
                }

                if (!sessions.Contains(form_popup.saveFileName))
                    sw.WriteLine(form_popup.saveFileName);
            }

        }

        private void orderingBtn_Click(object sender, EventArgs e)
        {
            listThreshold = Convert.ToInt32(thresholdTxt.Text);

            intermission = Convert.ToInt32(firstHalfTxt.Text) + 1;

            if (limitTxt.Text == "")
            {
                limitTxt.Text = (intermission / 2).ToString();
            }

            OrderPieces();
        }

        private void OrderPieces()
        {
            loadingBox.Visible = true;
            panelContainer.Visible = false;

            //initialize the resources
            FillUpArrays();
            FillUpPossibles();

            //AI backtracking solution, modified to look for all solutions instead of just one
            BacktrackSearch();

            //report the solutions found
            DisplaySolutions();

            loadingBox.Visible = false;

        }

        private void FillUpArrays()
        {
            //for each cast list
            for (int i = 0; i < castListArray.Count; i++)
                if (listBoxArray[i].Items.Count > 0) //if the corresponding listbox has items
                    foreach (string castMember in listBoxArray[i].Items) //fill up the array with the cast members
                        castListArray[i].AddNewCastMember(castMember);
        }

        private void FillUpPossibles()
        {
            for (int i = 0; i < castListArray.Count; i++)
                for (int j = 0; j < castListArray.Count; j++)
                    showOrderPosition[i].Add(j);
        }

        //main backtrack function
        private bool BacktrackSearch()
        {
            if (solutionList.Count >= listThreshold) return true;

            if (solutionFull())
            {
                if (solutionAlreadyFound())
                    return true;

                ShowOrder temp = new ShowOrder(this);
                foreach (int item in showOrder)
                    temp.Add(item);
                addToSolutionList(temp);

                return true;
            }

            //insert the cast list that has the least amount of piece-possiblities but also 
            int nextPossible = 0;
            int insertedValue  = insertNextPossible(showOrder.Count, nextPossible);  

            while (insertedValue != -1)
            {
                
                //save the showOrderPosition into temp
                //save the showOrder into temp
                ShowOrderPosition tempShowOrderPosition = new ShowOrderPosition(this);
                ShowOrder tempShowOrder = new ShowOrder(this);
                CopyShowOrderPosition(showOrderPosition, tempShowOrderPosition);
                CopyShowOrder(showOrder, tempShowOrder);
                
                bool works = insertIntoShowOrder(insertedValue);

                if (works)
                    BacktrackSearch();

                //else return back to previous state
                CopyShowOrderPosition(tempShowOrderPosition, showOrderPosition);
                CopyShowOrder(tempShowOrder, showOrder);

                nextPossible++;
                insertedValue = insertNextPossible(showOrder.Count, nextPossible);

            }

            return false;
        }

        private void DisplaySolutions()
        {
            SwitchVisibility("solutions");

            int level = 0;
         
            foreach (ShowOrder list in solutionList)
            {
                TextBox aOrder = new TextBox();
                this.Controls.Add(aOrder);

                for (int i = 0; i < list.Count; i++)
                {
                    if (i != list.Count - 1)
                        aOrder.Text += textBoxArray[i].Text + ", ";
                    else
                        aOrder.Text += textBoxArray[i].Text;
                }

                aOrder.Parent = solutionsContainer;
                aOrder.Top = 25 * level;
                aOrder.Left = 0;

                Label tmpLabel = new Label();
                tmpLabel.Text = aOrder.Text;
                tmpLabel.AutoSize = true;
                aOrder.Size = new Size(tmpLabel.PreferredWidth, aOrder.Height);
                

                level++;
            }
        }

        private bool insertIntoShowOrder(int value)
        {
            
            showOrder.Add(value);

            int index = showOrder.Count;

            //make sure each half of the show doesn't have too many of the
            //same type of dance type (for now each half of the show may 
            //have no more than half being any type of dance
            if (CheckFirstHalf()) return false;
            if (CheckSecondHalf()) return false;

            //this is the forward checking for the backtracking algorithm
            for (int i = showOrder.Count; i < numberOfPieces; i++)
            {
                //if this insertion of a showOrder value results in only allowing one option when there are
                //at least 2 more show order values to be filled, then this is an incorrect option
                if (showOrderPosition[i].Count == 1 && showOrder.Count < numberOfPieces) return false;

                showOrderPosition[i].Remove(value);

            }

            //remove the respective possible values that each show order position can have, now that insertion is possible
            if (index != numberOfPieces)   //as long as this isn't the last show piece
                for (int i = 0; i < castListArray.Count; i++)
                    if ((i != index - 1) &&     //as long as we're not looking at the same show order position...
                        (castListArray[i].HasMatching(castListArray[index - 1])) &&  //as long as there is a matching cast member in the next show order position
                        (i != intermission))   //as long as we're not at intermission
                        showOrderPosition[index].Remove(i);  //then you can remove that possible value from the show order position

            return true;
        }

        private bool CheckFirstHalf()
        {
            int half = Convert.ToInt32(limitTxt.Text);
            
            //check first half of show
            for (int i = 0; i < (showOrder.Count < intermission ? showOrder.Count : intermission); i++)
            {
                if (isTooMany(i, half)) return true;
            }

            return false;
        }

        private bool CheckSecondHalf()
        {
            if (showOrder.Count > intermission)
            {
                int half = Convert.ToInt32(limitTxt.Text);

                for (int i = intermission; i < showOrder.Count; i++)
                {
                    if (isTooMany(i, half)) return true;
                }
            }

            return false;
        }

        private bool isTooMany(int index, int half)
        {
            foreach (string key in DanceTypeBoxArray.amountOfTypes.Keys.ToList())
            {
                if (key == danceTypeBoxArray[showOrder[index]].Text)
                {
                    DanceTypeBoxArray.amountOfTypes[key]++;
                    if (DanceTypeBoxArray.amountOfTypes[key] == half)
                        return true;
                }
            }

            return false;
        }

        private int insertNextPossible(int position, int index)
        {
            //insert the cast list that has the least amount of cast posibilities
            if (position < showOrderPosition.Count)
            {
                if (index < showOrderPosition[position].Count)
                    return showOrderPosition[position][index];
                else
                    return -1;
            }
            else
                return -1;
        }

        private bool solutionFull()
        {
            if (showOrder.Count == numberOfPieces) return true;
            else return false;
        }

        private void addToSolutionList(ShowOrder aShow)
        {
            solutionList.Add(aShow);
        }

        private bool solutionAlreadyFound()
        {
            if (solutionList.Contains(showOrder)) return true;
            else return false;
        }

        private void CopyShowOrderPosition(ShowOrderPosition src, ShowOrderPosition dest)
        {
            for (int i = 0; i < src.Count; i++)
            {
                if (dest.Count > i) dest[i].Clear();
                else
                {
                    List<int> temp = new List<int>();
                    dest.Add(temp);
                }
                foreach (int item in src[i])
                {
                    dest[i].Add(item);
                }
            }
        }

        private void CopyShowOrder(ShowOrder src, ShowOrder dest)
        {
            if (dest.Count > 0) dest.Clear();
            foreach (int item in src)
            {
                dest.Add(item);
            }
        }

        private void restartBtn_Click(object sender, EventArgs e)
        {
            SwitchVisibility("restart");
        }

        private void about_Click(object sender, EventArgs e)
        {
            About aboutScreen = new About();
            aboutScreen.Show();
        }

        private void howToUse_Click(object sender, EventArgs e)
        {
            HowToUse helpScreen = new HowToUse();
            helpScreen.Show();
        }

        private void ClearControls()
        {
            //clear text array
            foreach (TextBox text in textBoxArray)
                this.Controls.Remove(text);
            //clear dance array
            foreach (ComboBox combo in danceTypeBoxArray)
                this.Controls.Remove(combo);
            //clear cast selection array
            foreach (ComboBox combo in comboBoxArray)
                this.Controls.Remove(combo);
            //clear cast list array
            foreach (ListBox list in listBoxArray)
                this.Controls.Remove(list);
            
        }


        private void loadSession_Click(object sender, EventArgs e)
        {
            

            beingLoaded = true;

            LoadFileName form_popup = new LoadFileName(this);

            form_popup.ShowDialog();

            if (form_popup.loadFileName == "" ||
                form_popup.loadFileName == null)
            {
                return;
            }

            loadedFileName = form_popup.loadFileName;

            string sessionPath = loadedFileName + "/" + loadedFileName;

            SwitchVisibility("loaded");

            //read in the cast lists
            StreamReader file = new StreamReader(sessionPath + ".xml");

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            XmlReader reader = XmlReader.Create(file, settings);

            int howManyToInsert = 0;
            bool readHowMany = false;
            int index = -1;
            bool firstOne = true;

            castListTxt.Clear();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.Name == "int")
                        {
                            readHowMany = true;
                            index++;
                            if (!firstOne)
                            {
                                CastList aCastList = new CastList(this);
                                castListArray.Add(aCastList);
                                List<int> list = new List<int>();
                                showOrderPosition.Add(list);
                                AddNewPiece();
                                numberOfPieces++;
                            }
                            else
                            {
                                
                                CastList aCastList = new CastList(this);
                                castListArray.Add(aCastList);
                                List<int> list = new List<int>();
                                showOrderPosition.Add(list);
                                numberOfPieces++;
                                firstOne = false;
                            }
                        }
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        if (readHowMany)
                        {
                            howManyToInsert = Convert.ToInt32(reader.Value);
                            readHowMany = false;
                        }
                        else
                        {
                            listBoxArray[index].Items.Add(reader.Value);
                        }
                        break;
                }
            }

            file = new StreamReader(sessionPath + "Titles.xml");

            reader = XmlReader.Create(file, settings);

            index = 0;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    textBoxArray[index].Text = reader.Value;
                    index++;
                }
            }
            file.Close();

            using (StreamReader sr = new StreamReader(sessionPath + "DanceTypes.txt"))
                foreach (ComboBox danceType in danceTypeBoxArray)
                    danceType.SelectedItem = sr.ReadLine();

            if (File.Exists(sessionPath + "DanceTypesExtra.txt"))
            {
                using (StreamReader sr = new StreamReader(sessionPath + "DanceTypesExtra.txt"))
                {
                    int i = 16;
                    while (sr.Peek() > 0)
                    {
                        string extra = sr.ReadLine();
                        foreach (ComboBox danceType in danceTypeBoxArray)
                        {
                            danceType.Items[i] = extra;
                        }
                        i++;
                    }
                }
            }

            using (StreamReader sr = new StreamReader(sessionPath + "CastList.txt"))
                castListTxt.Text += sr.ReadToEnd();

            PopulateComboBoxes();

            beingLoaded = false;

            firstHalfTxt.Text = (numberOfPieces / 2).ToString();
            limitTxt.Text = (numberOfPieces / 4).ToString();

            if (castListTxt.Lines[castListTxt.Lines.Length - 1] == "")
            {
                List<string> aList = castListTxt.Lines.ToList();
                aList.RemoveAt(castListTxt.Lines.Length - 1);
                castListTxt.Lines = aList.ToArray();
            }
        }

        private void saveSession_Click(object sender, EventArgs e)
        {
            WriteXML();
        }

        private void loadCastFromFile_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                        {
                            
                            castListTxt.Clear();
                            List<string> names = new List<string>();
                            while (sr.Peek() > 0)
                            {
                                string name = sr.ReadLine();
                                if (name != "")
                                    names.Add(name);
                            }

                            for (int i = 0; i < names.Count - 2; i++)
                                castListTxt.Text += names[i] + Environment.NewLine;

                            castListTxt.Text += names[names.Count - 1];
                            
                            PopulateComboBoxes();
                        }

                        if (castListTxt.Lines[castListTxt.Lines.Length - 1] == "" || castListTxt.Lines[castListTxt.Lines.Length - 1] == "\n")
                        {
                            List<string> aList = castListTxt.Lines.ToList();
                            aList.RemoveAt(castListTxt.Lines.Length - 1);
                            castListTxt.Lines = aList.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void danceTypeAdd_Click(object sender, EventArgs e)
        {
            DanceAddScreen danceAddScreen = new DanceAddScreen(danceTypeBoxArray[0].Items, this);
            danceAddScreen.Show();

            danceAddScreen.FormClosing += new FormClosingEventHandler(addToDanceTypes);
        }

        private void addToDanceTypes(Object sender, FormClosingEventArgs e)
        {
            DanceAddScreen danceAddScreen = ((DanceAddScreen)sender);

            foreach (ComboBox danceArray in danceTypeBoxArray)
            {
                danceArray.Items.Clear();
                foreach (string item in danceAddScreen.danceTypes)
                    danceArray.Items.Add(item);
            }
            
        }

    }
}

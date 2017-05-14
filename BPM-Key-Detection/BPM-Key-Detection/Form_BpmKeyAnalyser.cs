using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPM_Key_Detection
{
    public partial class Form_BpmKeyAnalyser : Form
    {
        private List<MusicFile> Files = new List<MusicFile>();
        private bool BPMChecked = false;
        private bool KeyChecked = false;
        public Form_BpmKeyAnalyser()
        {
            InitializeComponent();
        }

        //Button "Select Files". Opens a filedialog with a filter for .mp3 and .wave.
        // Stores the filepath for all selected files in ArrayOfFiles.
        private void button_FileDialog(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Music file (*.mp3, *.wave) | *.mp3; *.wave";
            fd.Multiselect = true;
            fd.ShowDialog();
            string[] ArrayOfFiles = fd.FileNames;
            MakeListOfFiles(ArrayOfFiles);
        }
        
        //Makes instances of MusicFile with each filepath, and adds them to the list "Files".
        private void MakeListOfFiles(string[] FileArray)
        {
            var alreadyLoaded = new List<String>();
            var badFiles = new List<String>();
            bool badFilesInList = false;
            foreach (string item in FileArray)
            {
                bool alreadyHere = false;
                foreach (MusicFile foo in Files)
                {
                    if (item == foo.Filepath)
                    {
                        alreadyHere = true;
                        alreadyLoaded.Add(foo.FileName);
                        break;
                    }
                }
                if (!alreadyHere)
                {
                    Files.Add(new MusicFile(item));
                    if (Files.Last().BadFile == true)
                    {
                        badFiles.Add(Files.Last().FileName);
                        Files.RemoveAt(Files.Count - 1);
                        badFilesInList = true;
                    }
                }
            }

            if (alreadyLoaded.Count != 0)
            {
                var firstItems = alreadyLoaded.Take(25);
                int numberOfFiles = alreadyLoaded.Count - 25;
                if (alreadyLoaded.Count > 25)
                {
                    var messageAlreadyLoaded = string.Join(Environment.NewLine, firstItems);
                    MessageBox.Show("The following files were already loaded: \n\n" + messageAlreadyLoaded + "\n+ " + numberOfFiles + " other files.", "Loading Error");
                }
                else
                {
                    var messageAlreadyLoaded = string.Join(Environment.NewLine, alreadyLoaded);
                    MessageBox.Show("The following files were already loaded: \n\n" + messageAlreadyLoaded, "Loading Error");
                }
            }

            if (badFilesInList)
            {
                var firstItems = badFiles.Take(25);
                int numberOfFiles = badFiles.Count - 25;
                if (badFiles.Count > 25)
                {
                    var messageBadFiles = string.Join(Environment.NewLine, firstItems);
                    MessageBox.Show("The following files  could not be loaded: \n\n" + messageBadFiles + "\n+ " + numberOfFiles + " other files.", "Loading Error");
                }
                else
                {
                    var messageBadFiles = string.Join(Environment.NewLine, badFiles);
                    MessageBox.Show("The following files  could not be loaded: \n\n" + messageBadFiles, "Loading Error");
                }
            }

            MakeDataTable(Files);
        }

        //Makes a DataTable of MusicFile data, and binds the DataTable to the DataGridView.
        private void MakeDataTable(List<MusicFile> Files)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Filename", typeof(string)));
            dt.Columns.Add(new DataColumn("Title", typeof(string)));
            dt.Columns.Add(new DataColumn("Kunstner", typeof(string)));
            dt.Columns.Add(new DataColumn("Album", typeof(string)));
            dt.Columns.Add(new DataColumn("Comments", typeof(string)));
            dt.Columns.Add(new DataColumn("BPM", typeof(uint)));
            dt.Columns.Add(new DataColumn("Key", typeof(string)));
            dt.Columns.Add(new DataColumn("Filepatch", typeof(string)));

            foreach (var item in Files)
            {
                dt.Rows.Add(item.FileName,
                            item.Titel,
                            item.Artists,
                            item.Album,
                            item.Comment,
                            item.Bpm,
                            item.Key,
                            item.Filepath);
            }

            dataGridView.DataSource = dt;

            this.dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Columns[0].Width = 300;
            this.dataGridView.Columns[1].Width = 200;
            this.dataGridView.Columns[2].Width = 100;
            this.dataGridView.Columns[5].Width = 40;
            this.dataGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView.Columns[6].Width = 40;
            this.dataGridView.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void dataGridView_Files(object sender, DataGridViewCellEventArgs e)
        {

        }

        //Adds drap and drop effect, when holding files over the DataGridView.
        private void dataGridView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        //Enable drap and drop load of .mp3 and .wave.
        private void dataGridView_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string[] ArrayOfFiles = { };

            foreach (var item in files)
            {
                if (System.IO.Path.GetExtension(item).Equals(".mp3", StringComparison.InvariantCultureIgnoreCase) ||
                    System.IO.Path.GetExtension(item).Equals(".wav", StringComparison.InvariantCultureIgnoreCase))
                {
                    ArrayOfFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                }
                else
                {
                    MessageBox.Show("Not a MP3 or Wave file");
                }
            }
            MakeListOfFiles(ArrayOfFiles);
        }

        //Adds row number in DataGridView.
        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dg = (DataGridView)sender;
            // Current row record
            string rowNumber = (e.RowIndex + 1).ToString();

            // Format row based on number of records displayed by using leading zeros
            while (rowNumber.Length < dg.RowCount.ToString().Length) rowNumber = "0" + rowNumber;

            // Position text
            SizeF size = e.Graphics.MeasureString(rowNumber, this.Font);
            if (dg.RowHeadersWidth < (int)(size.Width + 20)) dg.RowHeadersWidth = (int)(size.Width + 20);

            // Use default system text brush
            Brush b = SystemBrushes.ControlText;

            // Draw row number
            e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
        }

        //Removes MusicFile object from the list "Files", when a user deletes a row in DataGridView.
        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var deletedItem = e.Row.Cells[0].Value.ToString();
            int index = Files.FindIndex(a => a.FileName == deletedItem);
            Files.RemoveAt(index);
        }

        private void Start_Click(object sender, EventArgs e)
        {
            int correctCounter = 0;
            if (Files.Any() == true)
            {
                if (BPMChecked || KeyChecked)
                {
                    foreach (var MusicFile in Files)
                    {
                        if (BPMChecked)
                        {
                            
                        }
                        if (KeyChecked)
                        {
                            KeyEstimation keyEstimation = new KeyEstimation(MusicFile);
                            if (MusicFile.Key.Contains(keyEstimation.MusicFileKey))
                            {
                                correctCounter++;
                            }
                        }
                    }
                    MessageBox.Show(correctCounter.ToString());
                }
                else
                {
                    MessageBox.Show("BPM or Key not checked!", "Check Error!");
                }
            }
            else
            {
                MessageBox.Show("No files selected!", "Start Error!");
            }
        }

        private void GetBPM_CheckedChanged(object sender, EventArgs e)
        {
            BPMChecked = !BPMChecked;
        }

        private void GetKey_CheckedChanged(object sender, EventArgs e)
        {
            KeyChecked = !KeyChecked;
        }
    }
}

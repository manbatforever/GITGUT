using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace BPM_Key_Detection
{
    //A windows form application which is responsible for UI and handling user input.
    partial class Form_BpmKeyAnalyser : Form
    {
        private List<MusicFile> ListOfFiles = new List<MusicFile>();
        private bool BPMChecked = false;
        private bool KeyChecked = false;
        private bool writeBPMToMetadata = false;
        private bool writeKeyToMetadata = false;
        private bool processRunning = false;
        public Form_BpmKeyAnalyser()
        {
            InitializeComponent();
        }

        //Button "Select Files". Opens a filedialog with a filter for .mp3 and .wave.
        // Stores the filepath for all selected files in ArrayOfFiles.
        private void button_FileDialog(object sender, EventArgs e)
        {
            if (processRunning)
            {
                MessageBox.Show("A process is already running.");
                return;
            }
            OpenFileDialog fd = new OpenFileDialog()
            {
                Filter = "Music file (*.mp3, *.wav, *.wave) | *.mp3; *.wav; *.wave",
                Multiselect = true
            };
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
            foreach (string filePath in FileArray)
            {
                bool alreadyHere = false;
                foreach (MusicFile musicFile in ListOfFiles)
                {
                    if (filePath == musicFile.Filepath)
                    {
                        alreadyHere = true;
                        alreadyLoaded.Add(musicFile.FileName);
                        break;
                    }
                }
                if (!alreadyHere)
                {
                    ListOfFiles.Add(new MusicFile(filePath));
                    if (ListOfFiles.Last().BadFile == true)
                    {
                        badFiles.Add(ListOfFiles.Last().FileName);
                        ListOfFiles.RemoveAt(ListOfFiles.Count - 1);
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
                    string messageAlreadyLoaded = string.Join(Environment.NewLine, firstItems);
                    MessageBox.Show("The following files were already loaded: \n\n" + messageAlreadyLoaded + "\n+ " + numberOfFiles + " other files.", "Loading Error");
                }
                else
                {
                    string messageAlreadyLoaded = string.Join(Environment.NewLine, alreadyLoaded);
                    MessageBox.Show("The following files were already loaded: \n\n" + messageAlreadyLoaded, "Loading Error");
                }
            }

            if (badFilesInList)
            {
                var firstItems = badFiles.Take(25);
                int numberOfFiles = badFiles.Count - 25;
                if (badFiles.Count > 25)
                {
                    string messageBadFiles = string.Join(Environment.NewLine, firstItems);
                    MessageBox.Show("The following files  could not be loaded: \n\n" + messageBadFiles + "\n+ " + numberOfFiles + " other files.", "Loading Error");
                }
                else
                {
                    string messageBadFiles = string.Join(Environment.NewLine, badFiles);
                    MessageBox.Show("The following files  could not be loaded: \n\n" + messageBadFiles, "Loading Error");
                }
            }

            MakeDataTable(ListOfFiles);
        }

        //Makes a DataTable of MusicFile data, and binds the DataTable to the DataGridView.
        private void MakeDataTable(List<MusicFile> Files)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Filename", typeof(string)));
            dt.Columns.Add(new DataColumn("Title", typeof(string)));
            dt.Columns.Add(new DataColumn("Artist", typeof(string)));
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
            int index = ListOfFiles.FindIndex(a => a.FileName == deletedItem);
            ListOfFiles.RemoveAt(index);
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (processRunning)
            {
                MessageBox.Show("A process is already running.");
                return;
            }
            Thread backgroundThread = new Thread( new ThreadStart(() =>
            {
                System.Diagnostics.Stopwatch totaltime = System.Diagnostics.Stopwatch.StartNew();
                List<string> badFiles = new List<string>();
                processRunning = true;
                int i = 0;
                int correctCounter = 0;
                if (ListOfFiles.Any() == true)
                {
                    if (BPMChecked || KeyChecked)
                    {
                        int test = 0;
                        foreach (MusicFile musicFile in ListOfFiles)
                        {
                            label1.BeginInvoke(new Action(() => { label1.Text = "Currently analysing: " + musicFile.FileName; }));
                            MusicFileSamples musicFileSamples = null;
                            try
                            {
                                musicFileSamples = musicFile.GetRawSamples();
                            }
                            catch (InvalidOperationException)
                            {
                                musicFile.BadFile = true;
                                badFiles.Add(musicFile.FileName);
                            }
                            if (!musicFile.BadFile && musicFileSamples != null)
                            {
                                if (BPMChecked)
                                {
                                    try
                                    {
                                        musicFile.EstimateBPM(musicFileSamples);
                                    }
                                    catch (SongNotLongEnoughException)
                                    {
                                        MessageBox.Show($"{musicFile.FileName} could not be analysed. The music file was to short!","BPM detection error!");
                                    }
                                    catch (SongIsNotStereoException)
                                    {
                                        MessageBox.Show($"{musicFile.FileName} could not be analysed. The music file was not stereo!", "BPM detection error!");
                                    }
                                    catch (Exception)
                                    {
                                        MessageBox.Show($"{musicFile.FileName} could not be analysed!", "BPM detection error!");
                                    }
                                    MessageBox.Show(musicFile.EstimatedBPM.ToString());
                                    if (musicFile.Bpm == musicFile.EstimatedBPM)
                                    {
                                        test++;
                                    }
                                }
                                if (KeyChecked)
                                {
                                    musicFile.EstimateKey(musicFileSamples);
                                    //MessageBox.Show(musicFile.CamelotNotation);
                                    if (musicFile.CamelotNotation != null)
                                    {
                                        if (musicFile.Key.Contains(musicFile.CamelotNotation))
                                        {
                                            correctCounter++;
                                        }
                                    }
                                }
                                if (writeKeyToMetadata || writeBPMToMetadata)
                                {
                                    musicFile.WriteMetadata(writeBPMToMetadata, writeKeyToMetadata);
                                }
                            }
                            progressBar1.BeginInvoke( new Action(() => { progressBar1.Value = i * 100 / ListOfFiles.Count(); }));
                            i++;
                        }
                        if (badFiles.Any())
                        {
                            string messageBadFiles = string.Join(Environment.NewLine, badFiles);
                            MessageBox.Show("The following files could not be analysed: \n\n" + messageBadFiles, "Analysing Error");
                        }
                        label1.BeginInvoke(new Action(() => { label1.Text = ""; }));
                        MessageBox.Show(correctCounter.ToString());
                        MessageBox.Show(test.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Get BPM or Get Key not checked!", "Check Error!");
                    }
                }
                else
                {
                    MessageBox.Show("No files selected!", "Start Error!");
                }
                MessageBox.Show("Analysis complete!","Status message");
                progressBar1.BeginInvoke(
                        new Action(() =>
                        {
                            progressBar1.Value = 0;
                        }
                ));
                processRunning = false;
            }
            ));
            backgroundThread.Start();
        }

        private void GetBPM_CheckedChanged(object sender, EventArgs e)
        {
            BPMChecked = !BPMChecked;
        }

        private void GetKey_CheckedChanged(object sender, EventArgs e)
        {
            KeyChecked = !KeyChecked;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void writeBPM_CheckedChanged(object sender, EventArgs e)
        {
            writeBPMToMetadata = !writeBPMToMetadata;
        }

        private void writeKey_CheckedChanged(object sender, EventArgs e)
        {
            writeKeyToMetadata = !writeKeyToMetadata;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
    }
}

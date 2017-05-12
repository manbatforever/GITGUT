namespace BPM_Key_Detection
{
    partial class Form_BpmKeyAnalyser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FileDialog = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.Start = new System.Windows.Forms.Button();
            this.GetBPM = new System.Windows.Forms.CheckBox();
            this.GetKey = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // FileDialog
            // 
            this.FileDialog.Location = new System.Drawing.Point(12, 12);
            this.FileDialog.Name = "FileDialog";
            this.FileDialog.Size = new System.Drawing.Size(112, 58);
            this.FileDialog.TabIndex = 0;
            this.FileDialog.Text = "Select Files";
            this.FileDialog.UseVisualStyleBackColor = true;
            this.FileDialog.Click += new System.EventHandler(this.button_FileDialog);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowDrop = true;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.Location = new System.Drawing.Point(12, 76);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(1010, 473);
            this.dataGridView.TabIndex = 1;
            this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_Files);
            this.dataGridView.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            this.dataGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dataGridView_UserDeletingRow);
            this.dataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragDrop);
            this.dataGridView.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragEnter);
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(292, 30);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(75, 23);
            this.Start.TabIndex = 2;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // GetBPM
            // 
            this.GetBPM.AutoSize = true;
            this.GetBPM.Location = new System.Drawing.Point(568, 30);
            this.GetBPM.Name = "GetBPM";
            this.GetBPM.Size = new System.Drawing.Size(69, 17);
            this.GetBPM.TabIndex = 3;
            this.GetBPM.Text = "Get BPM";
            this.GetBPM.UseVisualStyleBackColor = true;
            this.GetBPM.CheckedChanged += new System.EventHandler(this.GetBPM_CheckedChanged);
            // 
            // GetKey
            // 
            this.GetKey.AutoSize = true;
            this.GetKey.Location = new System.Drawing.Point(687, 30);
            this.GetKey.Name = "GetKey";
            this.GetKey.Size = new System.Drawing.Size(64, 17);
            this.GetKey.TabIndex = 4;
            this.GetKey.Text = "Get Key";
            this.GetKey.UseVisualStyleBackColor = true;
            this.GetKey.CheckedChanged += new System.EventHandler(this.GetKey_CheckedChanged);
            // 
            // Form_BpmKeyAnalyser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 561);
            this.Controls.Add(this.GetKey);
            this.Controls.Add(this.GetBPM);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.FileDialog);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "Form_BpmKeyAnalyser";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button FileDialog;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.CheckBox GetBPM;
        private System.Windows.Forms.CheckBox GetKey;
    }
}


namespace FastFileSearcher
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.comboBoxDrives = new System.Windows.Forms.ComboBox();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button(); // Новая кнопка отмены
            this.listBoxResults = new System.Windows.Forms.ListBox();
            this.labelDrive = new System.Windows.Forms.Label();
            this.labelFileName = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBoxDrives
            // 
            this.comboBoxDrives.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDrives.FormattingEnabled = true;
            this.comboBoxDrives.Location = new System.Drawing.Point(12, 29);
            this.comboBoxDrives.Name = "comboBoxDrives";
            this.comboBoxDrives.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDrives.TabIndex = 0;
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Location = new System.Drawing.Point(12, 69);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(260, 20);
            this.textBoxFileName.TabIndex = 1;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(12, 105);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 2;
            this.buttonSearch.Text = "Поиск";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(100, 105); // Позиция кнопки отмены
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // listBoxResults
            // 
            this.listBoxResults.FormattingEnabled = true;
            this.listBoxResults.Location = new System.Drawing.Point(12, 145);
            this.listBoxResults.Name = "listBoxResults";
            this.listBoxResults.Size = new System.Drawing.Size(460, 160);
            this.listBoxResults.TabIndex = 4;
            this.listBoxResults.DoubleClick += new System.EventHandler(this.listBoxResults_DoubleClick);
            // 
            // labelDrive
            // 
            this.labelDrive.AutoSize = true;
            this.labelDrive.Location = new System.Drawing.Point(12, 13);
            this.labelDrive.Name = "labelDrive";
            this.labelDrive.Size = new System.Drawing.Size(30, 13);
            this.labelDrive.TabIndex = 5;
            this.labelDrive.Text = "Диск";
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(12, 53);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(82, 13);
            this.labelFileName.TabIndex = 6;
            this.labelFileName.Text = "Имя файла";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(350, 110);  // Перемещаем в правый верхний угол
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(79, 13);
            this.labelStatus.TabIndex = 7;
            this.labelStatus.Text = "Ожидание";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.labelDrive);
            this.Controls.Add(this.listBoxResults);
            this.Controls.Add(this.buttonCancel); // Добавляем кнопку отмены
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.comboBoxDrives);
            this.Name = "MainForm";
            this.Text = "Fast File Searcher";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxDrives;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Button buttonCancel; // Объявление кнопки отмены
        private System.Windows.Forms.ListBox listBoxResults;
        private System.Windows.Forms.Label labelDrive;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Label labelStatus;
    }
}

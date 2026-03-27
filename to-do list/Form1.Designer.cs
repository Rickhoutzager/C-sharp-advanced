namespace to_do_list
{
    partial class Form1
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
            listBoxIncomplete = new ListBox();
            listBoxComplete = new ListBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            textBoxNewItem = new TextBox();
            btnToggleComplete = new Button();
            btnSaveFile = new Button();
            btnLoadFile = new Button();
            label4 = new Label();
            label5 = new Label();
            comboBoxPriority = new ComboBox();
            label6 = new Label();
            dateTimePickerDueDate = new DateTimePicker();
            label7 = new Label();
            comboBoxCategory = new ComboBox();
            btnAddDecorated = new Button();
            groupBoxEdit = new GroupBox();
            btnSaveChanges = new Button();
            btnEditSelected = new Button();
            dateTimePickerEditDueDate = new DateTimePicker();
            comboBoxEditCategory = new ComboBox();
            comboBoxEditPriority = new ComboBox();
            textBoxEditTitle = new TextBox();
            labelEditDueDate = new Label();
            labelEditCategory = new Label();
            labelEditPriority = new Label();
            labelEditTitle = new Label();
            labelEditHeader = new Label();
            btnUndo = new Button();
            btnRedo = new Button();
            labelUndo = new Label();
            labelRedo = new Label();
            labelUndoCount = new Label();
            labelRedoCount = new Label();
            btnCreateProject = new Button();
            btnAddToProject = new Button();
            btnCompleteProject = new Button();
            comboBoxProjects = new ComboBox();
            labelProjects = new Label();
            groupBoxProjects = new GroupBox();
            btnShowProjectContents = new Button();
            groupBox1 = new GroupBox();
            groupBoxEdit.SuspendLayout();
            groupBoxProjects.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // listBoxIncomplete
            // 
            listBoxIncomplete.FormattingEnabled = true;
            listBoxIncomplete.Location = new Point(25, 262);
            listBoxIncomplete.Margin = new Padding(3, 4, 3, 4);
            listBoxIncomplete.Name = "listBoxIncomplete";
            listBoxIncomplete.Size = new Size(578, 388);
            listBoxIncomplete.TabIndex = 0;
            listBoxIncomplete.SelectedIndexChanged += listBoxIncomplete_SelectedIndexChanged;
            // 
            // listBoxComplete
            // 
            listBoxComplete.FormattingEnabled = true;
            listBoxComplete.Location = new Point(614, 262);
            listBoxComplete.Margin = new Padding(3, 4, 3, 4);
            listBoxComplete.Name = "listBoxComplete";
            listBoxComplete.Size = new Size(578, 388);
            listBoxComplete.TabIndex = 1;
            listBoxComplete.SelectedIndexChanged += listBoxComplete_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(108, 29);
            label1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(25, 226);
            label2.Name = "label2";
            label2.Size = new Size(195, 32);
            label2.TabIndex = 2;
            label2.Text = "Incomplete Tasks";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(608, 226);
            label3.Name = "label3";
            label3.Size = new Size(193, 32);
            label3.TabIndex = 3;
            label3.Text = "Completed Tasks";
            label3.Click += label3_Click;
            // 
            // textBoxNewItem
            // 
            textBoxNewItem.Location = new Point(150, 107);
            textBoxNewItem.Margin = new Padding(3, 4, 3, 4);
            textBoxNewItem.Name = "textBoxNewItem";
            textBoxNewItem.Size = new Size(195, 39);
            textBoxNewItem.TabIndex = 5;
            // 
            // btnToggleComplete
            // 
            btnToggleComplete.Location = new Point(872, 13);
            btnToggleComplete.Margin = new Padding(3, 4, 3, 4);
            btnToggleComplete.Name = "btnToggleComplete";
            btnToggleComplete.Size = new Size(242, 67);
            btnToggleComplete.TabIndex = 6;
            btnToggleComplete.Text = "Toggle Complete";
            btnToggleComplete.UseVisualStyleBackColor = true;
            btnToggleComplete.Click += btnToggleComplete_Click_1;
            // 
            // btnSaveFile
            // 
            btnSaveFile.Location = new Point(12, 13);
            btnSaveFile.Margin = new Padding(3, 4, 3, 4);
            btnSaveFile.Name = "btnSaveFile";
            btnSaveFile.RightToLeft = RightToLeft.No;
            btnSaveFile.Size = new Size(208, 67);
            btnSaveFile.TabIndex = 7;
            btnSaveFile.Text = "Save";
            btnSaveFile.UseVisualStyleBackColor = true;
            btnSaveFile.Click += btnSaveFile_Click_1;
            // 
            // btnLoadFile
            // 
            btnLoadFile.Location = new Point(226, 13);
            btnLoadFile.Margin = new Padding(3, 4, 3, 4);
            btnLoadFile.Name = "btnLoadFile";
            btnLoadFile.Size = new Size(208, 67);
            btnLoadFile.TabIndex = 8;
            btnLoadFile.Text = "Load";
            btnLoadFile.UseVisualStyleBackColor = true;
            btnLoadFile.Click += btnLoadFile_Click_1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(31, 114);
            label4.Name = "label4";
            label4.Size = new Size(65, 32);
            label4.TabIndex = 9;
            label4.Text = "Title:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(27, 190);
            label5.Name = "label5";
            label5.Size = new Size(94, 32);
            label5.TabIndex = 10;
            label5.Text = "Priority:";
            // 
            // comboBoxPriority
            // 
            comboBoxPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPriority.FormattingEnabled = true;
            comboBoxPriority.Items.AddRange(new object[] { "1 - Low", "2 - Medium", "3 - High", "4 - Very High", "5 - Urgent" });
            comboBoxPriority.Location = new Point(150, 187);
            comboBoxPriority.Margin = new Padding(3, 4, 3, 4);
            comboBoxPriority.Name = "comboBoxPriority";
            comboBoxPriority.Size = new Size(195, 40);
            comboBoxPriority.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(27, 345);
            label6.Name = "label6";
            label6.Size = new Size(120, 32);
            label6.TabIndex = 12;
            label6.Text = "Due Date:";
            // 
            // dateTimePickerDueDate
            // 
            dateTimePickerDueDate.Location = new Point(150, 337);
            dateTimePickerDueDate.Margin = new Padding(3, 4, 3, 4);
            dateTimePickerDueDate.Name = "dateTimePickerDueDate";
            dateTimePickerDueDate.Size = new Size(195, 39);
            dateTimePickerDueDate.TabIndex = 13;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(27, 264);
            label7.Name = "label7";
            label7.Size = new Size(115, 32);
            label7.TabIndex = 14;
            label7.Text = "Category:";
            // 
            // comboBoxCategory
            // 
            comboBoxCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCategory.FormattingEnabled = true;
            comboBoxCategory.Items.AddRange(new object[] { "General", "Work", "Personal", "Shopping", "Health" });
            comboBoxCategory.Location = new Point(150, 267);
            comboBoxCategory.Margin = new Padding(3, 4, 3, 4);
            comboBoxCategory.Name = "comboBoxCategory";
            comboBoxCategory.Size = new Size(195, 40);
            comboBoxCategory.TabIndex = 15;
            // 
            // btnAddDecorated
            // 
            btnAddDecorated.Location = new Point(77, 428);
            btnAddDecorated.Margin = new Padding(3, 4, 3, 4);
            btnAddDecorated.Name = "btnAddDecorated";
            btnAddDecorated.Size = new Size(162, 42);
            btnAddDecorated.TabIndex = 16;
            btnAddDecorated.Text = "Add Decorated";
            btnAddDecorated.UseVisualStyleBackColor = true;
            btnAddDecorated.Click += btnAddDecorated_Click;
            // 
            // groupBoxEdit
            // 
            groupBoxEdit.Controls.Add(btnSaveChanges);
            groupBoxEdit.Controls.Add(btnEditSelected);
            groupBoxEdit.Controls.Add(dateTimePickerEditDueDate);
            groupBoxEdit.Controls.Add(comboBoxEditCategory);
            groupBoxEdit.Controls.Add(comboBoxEditPriority);
            groupBoxEdit.Controls.Add(textBoxEditTitle);
            groupBoxEdit.Controls.Add(labelEditDueDate);
            groupBoxEdit.Controls.Add(labelEditCategory);
            groupBoxEdit.Controls.Add(labelEditPriority);
            groupBoxEdit.Controls.Add(labelEditTitle);
            groupBoxEdit.Controls.Add(labelEditHeader);
            groupBoxEdit.Location = new Point(840, 713);
            groupBoxEdit.Margin = new Padding(3, 4, 3, 4);
            groupBoxEdit.Name = "groupBoxEdit";
            groupBoxEdit.Padding = new Padding(3, 4, 3, 4);
            groupBoxEdit.Size = new Size(368, 490);
            groupBoxEdit.TabIndex = 17;
            groupBoxEdit.TabStop = false;
            groupBoxEdit.Text = "Edit Selected Item";
            // 
            // btnSaveChanges
            // 
            btnSaveChanges.Location = new Point(195, 427);
            btnSaveChanges.Margin = new Padding(3, 4, 3, 4);
            btnSaveChanges.Name = "btnSaveChanges";
            btnSaveChanges.Size = new Size(141, 45);
            btnSaveChanges.TabIndex = 21;
            btnSaveChanges.Text = "Save Changes";
            btnSaveChanges.UseVisualStyleBackColor = true;
            btnSaveChanges.Click += btnSaveChanges_Click;
            // 
            // btnEditSelected
            // 
            btnEditSelected.Location = new Point(32, 427);
            btnEditSelected.Margin = new Padding(3, 4, 3, 4);
            btnEditSelected.Name = "btnEditSelected";
            btnEditSelected.Size = new Size(141, 45);
            btnEditSelected.TabIndex = 20;
            btnEditSelected.Text = "Edit Selected";
            btnEditSelected.UseVisualStyleBackColor = true;
            btnEditSelected.Click += btnEditSelected_Click;
            // 
            // dateTimePickerEditDueDate
            // 
            dateTimePickerEditDueDate.Location = new Point(141, 338);
            dateTimePickerEditDueDate.Margin = new Padding(3, 4, 3, 4);
            dateTimePickerEditDueDate.Name = "dateTimePickerEditDueDate";
            dateTimePickerEditDueDate.Size = new Size(195, 39);
            dateTimePickerEditDueDate.TabIndex = 19;
            // 
            // comboBoxEditCategory
            // 
            comboBoxEditCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEditCategory.FormattingEnabled = true;
            comboBoxEditCategory.Items.AddRange(new object[] { "General", "Work", "Personal", "Shopping", "Health" });
            comboBoxEditCategory.Location = new Point(141, 261);
            comboBoxEditCategory.Margin = new Padding(3, 4, 3, 4);
            comboBoxEditCategory.Name = "comboBoxEditCategory";
            comboBoxEditCategory.Size = new Size(195, 40);
            comboBoxEditCategory.TabIndex = 18;
            // 
            // comboBoxEditPriority
            // 
            comboBoxEditPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEditPriority.FormattingEnabled = true;
            comboBoxEditPriority.Items.AddRange(new object[] { "1 - Low", "2 - Medium", "3 - High", "4 - Very High", "5 - Urgent" });
            comboBoxEditPriority.Location = new Point(141, 184);
            comboBoxEditPriority.Margin = new Padding(3, 4, 3, 4);
            comboBoxEditPriority.Name = "comboBoxEditPriority";
            comboBoxEditPriority.Size = new Size(195, 40);
            comboBoxEditPriority.TabIndex = 17;
            // 
            // textBoxEditTitle
            // 
            textBoxEditTitle.Location = new Point(141, 107);
            textBoxEditTitle.Margin = new Padding(3, 4, 3, 4);
            textBoxEditTitle.Name = "textBoxEditTitle";
            textBoxEditTitle.Size = new Size(195, 39);
            textBoxEditTitle.TabIndex = 16;
            // 
            // labelEditDueDate
            // 
            labelEditDueDate.AutoSize = true;
            labelEditDueDate.Location = new Point(27, 344);
            labelEditDueDate.Name = "labelEditDueDate";
            labelEditDueDate.Size = new Size(120, 32);
            labelEditDueDate.TabIndex = 15;
            labelEditDueDate.Text = "Due Date:";
            // 
            // labelEditCategory
            // 
            labelEditCategory.AutoSize = true;
            labelEditCategory.Location = new Point(27, 267);
            labelEditCategory.Name = "labelEditCategory";
            labelEditCategory.Size = new Size(115, 32);
            labelEditCategory.TabIndex = 14;
            labelEditCategory.Text = "Category:";
            // 
            // labelEditPriority
            // 
            labelEditPriority.AutoSize = true;
            labelEditPriority.Location = new Point(27, 190);
            labelEditPriority.Name = "labelEditPriority";
            labelEditPriority.Size = new Size(94, 32);
            labelEditPriority.TabIndex = 13;
            labelEditPriority.Text = "Priority:";
            // 
            // labelEditTitle
            // 
            labelEditTitle.AutoSize = true;
            labelEditTitle.Location = new Point(27, 114);
            labelEditTitle.Name = "labelEditTitle";
            labelEditTitle.Size = new Size(65, 32);
            labelEditTitle.TabIndex = 12;
            labelEditTitle.Text = "Title:";
            // 
            // labelEditHeader
            // 
            labelEditHeader.AutoSize = true;
            labelEditHeader.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelEditHeader.Location = new Point(27, 64);
            labelEditHeader.Name = "labelEditHeader";
            labelEditHeader.Size = new Size(272, 31);
            labelEditHeader.TabIndex = 11;
            labelEditHeader.Text = "Select item to edit it";
            // 
            // btnUndo
            // 
            btnUndo.Location = new Point(440, 13);
            btnUndo.Margin = new Padding(3, 4, 3, 4);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(208, 67);
            btnUndo.TabIndex = 18;
            btnUndo.Text = "Undo";
            btnUndo.UseVisualStyleBackColor = true;
            // 
            // btnRedo
            // 
            btnRedo.Location = new Point(654, 13);
            btnRedo.Margin = new Padding(3, 4, 3, 4);
            btnRedo.Name = "btnRedo";
            btnRedo.Size = new Size(208, 67);
            btnRedo.TabIndex = 19;
            btnRedo.Text = "Redo";
            btnRedo.UseVisualStyleBackColor = true;
            // 
            // labelUndo
            // 
            labelUndo.AutoSize = true;
            labelUndo.Location = new Point(501, 84);
            labelUndo.Name = "labelUndo";
            labelUndo.Size = new Size(77, 32);
            labelUndo.TabIndex = 20;
            labelUndo.Text = "Undo:";
            // 
            // labelRedo
            // 
            labelRedo.AutoSize = true;
            labelRedo.Location = new Point(721, 84);
            labelRedo.Name = "labelRedo";
            labelRedo.Size = new Size(73, 32);
            labelRedo.TabIndex = 21;
            labelRedo.Text = "Redo:";
            // 
            // labelUndoCount
            // 
            labelUndoCount.AutoSize = true;
            labelUndoCount.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelUndoCount.Location = new Point(572, 84);
            labelUndoCount.Name = "labelUndoCount";
            labelUndoCount.Size = new Size(30, 31);
            labelUndoCount.TabIndex = 22;
            labelUndoCount.Text = "0";
            // 
            // labelRedoCount
            // 
            labelRedoCount.AutoSize = true;
            labelRedoCount.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelRedoCount.Location = new Point(797, 84);
            labelRedoCount.Name = "labelRedoCount";
            labelRedoCount.Size = new Size(30, 31);
            labelRedoCount.TabIndex = 23;
            labelRedoCount.Text = "0";
            // 
            // btnCreateProject
            // 
            btnCreateProject.Location = new Point(25, 128);
            btnCreateProject.Margin = new Padding(3, 4, 3, 4);
            btnCreateProject.Name = "btnCreateProject";
            btnCreateProject.Size = new Size(162, 45);
            btnCreateProject.TabIndex = 24;
            btnCreateProject.Text = "Create Project";
            btnCreateProject.UseVisualStyleBackColor = true;
            btnCreateProject.Click += btnCreateProject_Click;
            // 
            // btnAddToProject
            // 
            btnAddToProject.Location = new Point(198, 128);
            btnAddToProject.Margin = new Padding(3, 4, 3, 4);
            btnAddToProject.Name = "btnAddToProject";
            btnAddToProject.Size = new Size(162, 45);
            btnAddToProject.TabIndex = 25;
            btnAddToProject.Text = "Add to Project";
            btnAddToProject.UseVisualStyleBackColor = true;
            btnAddToProject.Click += btnAddToProject_Click;
            // 
            // btnCompleteProject
            // 
            btnCompleteProject.Location = new Point(25, 186);
            btnCompleteProject.Margin = new Padding(3, 4, 3, 4);
            btnCompleteProject.Name = "btnCompleteProject";
            btnCompleteProject.Size = new Size(336, 45);
            btnCompleteProject.TabIndex = 26;
            btnCompleteProject.Text = "Complete Project";
            btnCompleteProject.UseVisualStyleBackColor = true;
            btnCompleteProject.Click += btnCompleteProject_Click;
            // 
            // comboBoxProjects
            // 
            comboBoxProjects.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxProjects.FormattingEnabled = true;
            comboBoxProjects.Location = new Point(25, 77);
            comboBoxProjects.Margin = new Padding(3, 4, 3, 4);
            comboBoxProjects.Name = "comboBoxProjects";
            comboBoxProjects.Size = new Size(336, 40);
            comboBoxProjects.TabIndex = 27;
            // 
            // labelProjects
            // 
            labelProjects.AutoSize = true;
            labelProjects.Location = new Point(19, 45);
            labelProjects.Name = "labelProjects";
            labelProjects.Size = new Size(102, 32);
            labelProjects.TabIndex = 28;
            labelProjects.Text = "Projects:";
            // 
            // groupBoxProjects
            // 
            groupBoxProjects.Controls.Add(labelProjects);
            groupBoxProjects.Controls.Add(comboBoxProjects);
            groupBoxProjects.Controls.Add(btnCompleteProject);
            groupBoxProjects.Controls.Add(btnAddToProject);
            groupBoxProjects.Controls.Add(btnCreateProject);
            groupBoxProjects.Controls.Add(btnShowProjectContents);
            groupBoxProjects.Location = new Point(427, 713);
            groupBoxProjects.Margin = new Padding(3, 4, 3, 4);
            groupBoxProjects.Name = "groupBoxProjects";
            groupBoxProjects.Padding = new Padding(3, 4, 3, 4);
            groupBoxProjects.Size = new Size(400, 325);
            groupBoxProjects.TabIndex = 29;
            groupBoxProjects.TabStop = false;
            groupBoxProjects.Text = "Project Management";
            // 
            // btnShowProjectContents
            // 
            btnShowProjectContents.Location = new Point(25, 243);
            btnShowProjectContents.Margin = new Padding(3, 4, 3, 4);
            btnShowProjectContents.Name = "btnShowProjectContents";
            btnShowProjectContents.Size = new Size(336, 45);
            btnShowProjectContents.TabIndex = 30;
            btnShowProjectContents.Text = "Show Project Contents";
            btnShowProjectContents.UseVisualStyleBackColor = true;
            btnShowProjectContents.Click += btnShowProjectContents_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBoxNewItem);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(comboBoxPriority);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(comboBoxCategory);
            groupBox1.Controls.Add(dateTimePickerDueDate);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(btnAddDecorated);
            groupBox1.Location = new Point(43, 713);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(368, 490);
            groupBox1.TabIndex = 30;
            groupBox1.TabStop = false;
            groupBox1.Text = "Add New Item";
            // 
            // groupBoxTesting
            // 
            groupBoxTesting = new GroupBox();
            groupBoxTesting.Location = new Point(1220, 713);
            groupBoxTesting.Name = "groupBoxTesting";
            groupBoxTesting.Size = new Size(368, 490);
            groupBoxTesting.TabIndex = 31;
            groupBoxTesting.TabStop = false;
            groupBoxTesting.Text = "Concurrency Testing";
            
            // 
            // labelTestStatus
            // 
            labelTestStatus = new Label();
            labelTestStatus.AutoSize = true;
            labelTestStatus.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelTestStatus.Location = new Point(27, 427);
            labelTestStatus.Name = "labelTestStatus";
            labelTestStatus.Size = new Size(164, 31);
            labelTestStatus.TabIndex = 0;
            labelTestStatus.Text = "Test Status: Ready";
            
            // 
            // btnTestAsyncAwait
            // 
            btnTestAsyncAwait = new Button();
            btnTestAsyncAwait.Location = new Point(25, 32);
            btnTestAsyncAwait.Name = "btnTestAsyncAwait";
            btnTestAsyncAwait.Size = new Size(320, 45);
            btnTestAsyncAwait.TabIndex = 1;
            btnTestAsyncAwait.Text = "Test Async/Await Pattern";
            btnTestAsyncAwait.UseVisualStyleBackColor = true;
            btnTestAsyncAwait.Click += btnTestAsyncAwait_Click;
            
            // 
            // btnTestProducerConsumer
            // 
            btnTestProducerConsumer = new Button();
            btnTestProducerConsumer.Location = new Point(25, 83);
            btnTestProducerConsumer.Name = "btnTestProducerConsumer";
            btnTestProducerConsumer.Size = new Size(320, 45);
            btnTestProducerConsumer.TabIndex = 2;
            btnTestProducerConsumer.Text = "Test Producer-Consumer Pattern";
            btnTestProducerConsumer.UseVisualStyleBackColor = true;
            btnTestProducerConsumer.Click += btnTestProducerConsumer_Click;
            
            // 
            // btnTestReaderWriterLock
            // 
            btnTestReaderWriterLock = new Button();
            btnTestReaderWriterLock.Location = new Point(25, 134);
            btnTestReaderWriterLock.Name = "btnTestReaderWriterLock";
            btnTestReaderWriterLock.Size = new Size(320, 45);
            btnTestReaderWriterLock.TabIndex = 3;
            btnTestReaderWriterLock.Text = "Test Reader-Writer Lock Pattern";
            btnTestReaderWriterLock.UseVisualStyleBackColor = true;
            btnTestReaderWriterLock.Click += btnTestReaderWriterLock_Click;
            
            // 
            // btnTestBackgroundWorker
            // 
            btnTestBackgroundWorker = new Button();
            btnTestBackgroundWorker.Location = new Point(25, 185);
            btnTestBackgroundWorker.Name = "btnTestBackgroundWorker";
            btnTestBackgroundWorker.Size = new Size(320, 45);
            btnTestBackgroundWorker.TabIndex = 4;
            btnTestBackgroundWorker.Text = "Test Background Worker Pattern";
            btnTestBackgroundWorker.UseVisualStyleBackColor = true;
            btnTestBackgroundWorker.Click += btnTestBackgroundWorker_Click;
            
            // 
            // btnStressTest
            // 
            btnStressTest = new Button();
            btnStressTest.Location = new Point(25, 236);
            btnStressTest.Name = "btnStressTest";
            btnStressTest.Size = new Size(320, 45);
            btnStressTest.TabIndex = 5;
            btnStressTest.Text = "Comprehensive Stress Test";
            btnStressTest.UseVisualStyleBackColor = true;
            btnStressTest.Click += btnStressTest_Click;
            
            // 
            // btnBenchmark
            // 
            btnBenchmark = new Button();
            btnBenchmark.Location = new Point(25, 287);
            btnBenchmark.Name = "btnBenchmark";
            btnBenchmark.Size = new Size(320, 45);
            btnBenchmark.TabIndex = 6;
            btnBenchmark.Text = "Performance Benchmark";
            btnBenchmark.UseVisualStyleBackColor = true;
            btnBenchmark.Click += btnBenchmark_Click;
            
            // 
            // btnCleanup
            // 
            btnCleanup = new Button();
            btnCleanup.Location = new Point(25, 338);
            btnCleanup.Name = "btnCleanup";
            btnCleanup.Size = new Size(320, 45);
            btnCleanup.TabIndex = 7;
            btnCleanup.Text = "Cleanup Resources";
            btnCleanup.UseVisualStyleBackColor = true;
            btnCleanup.Click += btnCleanup_Click;
            
            // Add controls to group box
            groupBoxTesting.Controls.Add(labelTestStatus);
            groupBoxTesting.Controls.Add(btnCleanup);
            groupBoxTesting.Controls.Add(btnBenchmark);
            groupBoxTesting.Controls.Add(btnStressTest);
            groupBoxTesting.Controls.Add(btnTestBackgroundWorker);
            groupBoxTesting.Controls.Add(btnTestReaderWriterLock);
            groupBoxTesting.Controls.Add(btnTestProducerConsumer);
            groupBoxTesting.Controls.Add(btnTestAsyncAwait);
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1889, 1392);
            Controls.Add(groupBoxTesting);
            Controls.Add(groupBox1);
            Controls.Add(groupBoxProjects);
            Controls.Add(labelRedoCount);
            Controls.Add(labelUndoCount);
            Controls.Add(labelRedo);
            Controls.Add(labelUndo);
            Controls.Add(btnRedo);
            Controls.Add(btnUndo);
            Controls.Add(groupBoxEdit);
            Controls.Add(btnLoadFile);
            Controls.Add(btnSaveFile);
            Controls.Add(btnToggleComplete);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listBoxComplete);
            Controls.Add(listBoxIncomplete);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Todo List with Composite Pattern";
            groupBoxEdit.ResumeLayout(false);
            groupBoxEdit.PerformLayout();
            groupBoxProjects.ResumeLayout(false);
            groupBoxProjects.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBoxTesting.ResumeLayout(false);
            groupBoxTesting.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox listBoxIncomplete;
        private System.Windows.Forms.ListBox listBoxComplete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxNewItem;
        private System.Windows.Forms.Button btnToggleComplete;
        private System.Windows.Forms.Button btnSaveFile;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxPriority;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateTimePickerDueDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxCategory;
        private System.Windows.Forms.Button btnAddDecorated;
        private System.Windows.Forms.GroupBox groupBoxEdit;
        private System.Windows.Forms.Button btnSaveChanges;
        private System.Windows.Forms.Button btnEditSelected;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnRedo;
        private System.Windows.Forms.Label labelUndo;
        private System.Windows.Forms.Label labelRedo;
        private System.Windows.Forms.Label labelUndoCount;
        private System.Windows.Forms.Label labelRedoCount;
        private System.Windows.Forms.DateTimePicker dateTimePickerEditDueDate;
        private System.Windows.Forms.ComboBox comboBoxEditCategory;
        private System.Windows.Forms.ComboBox comboBoxEditPriority;
        private System.Windows.Forms.TextBox textBoxEditTitle;
        private System.Windows.Forms.Label labelEditDueDate;
        private System.Windows.Forms.Label labelEditCategory;
        private System.Windows.Forms.Label labelEditPriority;
        private System.Windows.Forms.Label labelEditTitle;
        private System.Windows.Forms.Label labelEditHeader;
        private System.Windows.Forms.Button btnCreateProject;
        private System.Windows.Forms.Button btnAddToProject;
        private System.Windows.Forms.Button btnCompleteProject;
        private System.Windows.Forms.Button btnShowProjectContents;
        private System.Windows.Forms.ComboBox comboBoxProjects;
        private System.Windows.Forms.Label labelProjects;
        private System.Windows.Forms.GroupBox groupBoxProjects;
        private GroupBox groupBox1;
        
        // Concurrency Testing Controls
        private System.Windows.Forms.Button btnTestAsyncAwait;
        private System.Windows.Forms.Button btnTestProducerConsumer;
        private System.Windows.Forms.Button btnTestReaderWriterLock;
        private System.Windows.Forms.Button btnTestBackgroundWorker;
        private System.Windows.Forms.Button btnStressTest;
        private System.Windows.Forms.Button btnBenchmark;
        private System.Windows.Forms.Button btnCleanup;
        private System.Windows.Forms.Label labelTestStatus;
        private GroupBox groupBoxTesting;
    }
}

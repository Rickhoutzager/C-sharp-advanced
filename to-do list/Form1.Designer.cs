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
            label8 = new Label();
            groupBoxEdit.SuspendLayout();
            SuspendLayout();
            // 
            // listBoxIncomplete
            // 
            listBoxIncomplete.FormattingEnabled = true;
            listBoxIncomplete.Location = new Point(36, 163);
            listBoxIncomplete.Margin = new Padding(3, 4, 3, 4);
            listBoxIncomplete.Name = "listBoxIncomplete";
            listBoxIncomplete.Size = new Size(550, 388);
            listBoxIncomplete.TabIndex = 0;
            listBoxIncomplete.SelectedIndexChanged += listBoxIncomplete_SelectedIndexChanged;
            // 
            // listBoxComplete
            // 
            listBoxComplete.FormattingEnabled = true;
            listBoxComplete.Location = new Point(626, 163);
            listBoxComplete.Margin = new Padding(3, 4, 3, 4);
            listBoxComplete.Name = "listBoxComplete";
            listBoxComplete.Size = new Size(550, 388);
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
            label2.Location = new Point(36, 127);
            label2.Name = "label2";
            label2.Size = new Size(195, 32);
            label2.TabIndex = 2;
            label2.Text = "Incomplete Tasks";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(620, 127);
            label3.Name = "label3";
            label3.Size = new Size(193, 32);
            label3.TabIndex = 3;
            label3.Text = "Completed Tasks";
            label3.Click += label3_Click;
            // 
            // textBoxNewItem
            // 
            textBoxNewItem.Location = new Point(154, 633);
            textBoxNewItem.Margin = new Padding(3, 4, 3, 4);
            textBoxNewItem.Name = "textBoxNewItem";
            textBoxNewItem.Size = new Size(267, 39);
            textBoxNewItem.TabIndex = 5;
            // 
            // btnToggleComplete
            // 
            btnToggleComplete.Location = new Point(479, 633);
            btnToggleComplete.Margin = new Padding(3, 4, 3, 4);
            btnToggleComplete.Name = "btnToggleComplete";
            btnToggleComplete.Size = new Size(269, 51);
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
            btnSaveFile.Size = new Size(154, 64);
            btnSaveFile.TabIndex = 7;
            btnSaveFile.Text = "Export Data";
            btnSaveFile.UseVisualStyleBackColor = true;
            btnSaveFile.Click += btnSaveFile_Click_1;
            // 
            // btnLoadFile
            // 
            btnLoadFile.Location = new Point(174, 13);
            btnLoadFile.Margin = new Padding(3, 4, 3, 4);
            btnLoadFile.Name = "btnLoadFile";
            btnLoadFile.Size = new Size(154, 61);
            btnLoadFile.TabIndex = 8;
            btnLoadFile.Text = "Import Data";
            btnLoadFile.UseVisualStyleBackColor = true;
            btnLoadFile.Click += btnLoadFile_Click_1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(36, 591);
            label4.Name = "label4";
            label4.Size = new Size(111, 32);
            label4.TabIndex = 9;
            label4.Text = "Add task:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(54, 680);
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
            comboBoxPriority.Location = new Point(154, 680);
            comboBoxPriority.Margin = new Padding(3, 4, 3, 4);
            comboBoxPriority.Name = "comboBoxPriority";
            comboBoxPriority.Size = new Size(267, 40);
            comboBoxPriority.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(27, 777);
            label6.Name = "label6";
            label6.Size = new Size(120, 32);
            label6.TabIndex = 12;
            label6.Text = "Due Date:";
            // 
            // dateTimePickerDueDate
            // 
            dateTimePickerDueDate.Location = new Point(154, 780);
            dateTimePickerDueDate.Margin = new Padding(3, 4, 3, 4);
            dateTimePickerDueDate.Name = "dateTimePickerDueDate";
            dateTimePickerDueDate.Size = new Size(270, 39);
            dateTimePickerDueDate.TabIndex = 13;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(32, 731);
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
            comboBoxCategory.Location = new Point(154, 728);
            comboBoxCategory.Margin = new Padding(3, 4, 3, 4);
            comboBoxCategory.Name = "comboBoxCategory";
            comboBoxCategory.Size = new Size(267, 40);
            comboBoxCategory.TabIndex = 15;
            // 
            // btnAddDecorated
            // 
            btnAddDecorated.Location = new Point(262, 842);
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
            groupBoxEdit.Location = new Point(819, 591);
            groupBoxEdit.Margin = new Padding(3, 4, 3, 4);
            groupBoxEdit.Name = "groupBoxEdit";
            groupBoxEdit.Padding = new Padding(3, 4, 3, 4);
            groupBoxEdit.Size = new Size(389, 511);
            groupBoxEdit.TabIndex = 17;
            groupBoxEdit.TabStop = false;
            groupBoxEdit.Text = "Edit Selected Item";
            // 
            // btnSaveChanges
            // 
            btnSaveChanges.Location = new Point(195, 441);
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
            btnEditSelected.Location = new Point(32, 441);
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
            dateTimePickerEditDueDate.Location = new Point(162, 352);
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
            comboBoxEditCategory.Location = new Point(162, 275);
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
            comboBoxEditPriority.Location = new Point(162, 198);
            comboBoxEditPriority.Margin = new Padding(3, 4, 3, 4);
            comboBoxEditPriority.Name = "comboBoxEditPriority";
            comboBoxEditPriority.Size = new Size(195, 40);
            comboBoxEditPriority.TabIndex = 17;
            // 
            // textBoxEditTitle
            // 
            textBoxEditTitle.Location = new Point(162, 121);
            textBoxEditTitle.Margin = new Padding(3, 4, 3, 4);
            textBoxEditTitle.Name = "textBoxEditTitle";
            textBoxEditTitle.Size = new Size(195, 39);
            textBoxEditTitle.TabIndex = 16;
            // 
            // labelEditDueDate
            // 
            labelEditDueDate.AutoSize = true;
            labelEditDueDate.Location = new Point(27, 358);
            labelEditDueDate.Name = "labelEditDueDate";
            labelEditDueDate.Size = new Size(120, 32);
            labelEditDueDate.TabIndex = 15;
            labelEditDueDate.Text = "Due Date:";
            // 
            // labelEditCategory
            // 
            labelEditCategory.AutoSize = true;
            labelEditCategory.Location = new Point(27, 281);
            labelEditCategory.Name = "labelEditCategory";
            labelEditCategory.Size = new Size(115, 32);
            labelEditCategory.TabIndex = 14;
            labelEditCategory.Text = "Category:";
            // 
            // labelEditPriority
            // 
            labelEditPriority.AutoSize = true;
            labelEditPriority.Location = new Point(27, 204);
            labelEditPriority.Name = "labelEditPriority";
            labelEditPriority.Size = new Size(94, 32);
            labelEditPriority.TabIndex = 13;
            labelEditPriority.Text = "Priority:";
            // 
            // labelEditTitle
            // 
            labelEditTitle.AutoSize = true;
            labelEditTitle.Location = new Point(27, 128);
            labelEditTitle.Name = "labelEditTitle";
            labelEditTitle.Size = new Size(65, 32);
            labelEditTitle.TabIndex = 12;
            labelEditTitle.Text = "Name:";
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
            btnUndo.Location = new Point(337, 13);
            btnUndo.Margin = new Padding(3, 4, 3, 4);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(154, 61);
            btnUndo.TabIndex = 18;
            btnUndo.Text = "Undo";
            btnUndo.UseVisualStyleBackColor = true;
            btnUndo.Click += btnUndo_Click;
            // 
            // btnRedo
            // 
            btnRedo.Location = new Point(497, 13);
            btnRedo.Margin = new Padding(3, 4, 3, 4);
            btnRedo.Name = "btnRedo";
            btnRedo.Size = new Size(154, 61);
            btnRedo.TabIndex = 19;
            btnRedo.Text = "Redo";
            btnRedo.UseVisualStyleBackColor = true;
            btnRedo.Click += btnRedo_Click;
            // 
            // labelUndo
            // 
            labelUndo.AutoSize = true;
            labelUndo.Location = new Point(373, 78);
            labelUndo.Name = "labelUndo";
            labelUndo.Size = new Size(77, 32);
            labelUndo.TabIndex = 20;
            labelUndo.Text = "Undo:";
            // 
            // labelRedo
            // 
            labelRedo.AutoSize = true;
            labelRedo.Location = new Point(523, 78);
            labelRedo.Name = "labelRedo";
            labelRedo.Size = new Size(73, 32);
            labelRedo.TabIndex = 21;
            labelRedo.Text = "Redo:";
            // 
            // labelUndoCount
            // 
            labelUndoCount.AutoSize = true;
            labelUndoCount.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelUndoCount.Location = new Point(444, 78);
            labelUndoCount.Name = "labelUndoCount";
            labelUndoCount.Size = new Size(30, 31);
            labelUndoCount.TabIndex = 22;
            labelUndoCount.Text = "0";
            // 
            // labelRedoCount
            // 
            labelRedoCount.AutoSize = true;
            labelRedoCount.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelRedoCount.Location = new Point(599, 78);
            labelRedoCount.Name = "labelRedoCount";
            labelRedoCount.Size = new Size(30, 31);
            labelRedoCount.TabIndex = 23;
            labelRedoCount.Text = "0";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(65, 636);
            label8.Name = "label8";
            label8.Size = new Size(83, 32);
            label8.TabIndex = 24;
            label8.Text = "Name:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1220, 1115);
            Controls.Add(label8);
            Controls.Add(groupBoxEdit);
            Controls.Add(btnAddDecorated);
            Controls.Add(comboBoxCategory);
            Controls.Add(label7);
            Controls.Add(dateTimePickerDueDate);
            Controls.Add(label6);
            Controls.Add(comboBoxPriority);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(btnLoadFile);
            Controls.Add(btnSaveFile);
            Controls.Add(btnToggleComplete);
            Controls.Add(textBoxNewItem);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(listBoxComplete);
            Controls.Add(listBoxIncomplete);
            Controls.Add(btnUndo);
            Controls.Add(btnRedo);
            Controls.Add(labelUndo);
            Controls.Add(labelRedo);
            Controls.Add(labelUndoCount);
            Controls.Add(labelRedoCount);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Todo List with Decorator Pattern";
            groupBoxEdit.ResumeLayout(false);
            groupBoxEdit.PerformLayout();
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
        private Label label8;
    }
}


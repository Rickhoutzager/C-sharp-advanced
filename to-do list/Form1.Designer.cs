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
            this.listBoxIncomplete = new System.Windows.Forms.ListBox();
            this.listBoxComplete = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxNewItem = new System.Windows.Forms.TextBox();
            this.btnToggleComplete = new System.Windows.Forms.Button();
            this.btnSaveFile = new System.Windows.Forms.Button();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxPriority = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dateTimePickerDueDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this.btnAddDecorated = new System.Windows.Forms.Button();
            this.groupBoxEdit = new System.Windows.Forms.GroupBox();
            this.btnSaveChanges = new System.Windows.Forms.Button();
            this.btnEditSelected = new System.Windows.Forms.Button();
            this.dateTimePickerEditDueDate = new System.Windows.Forms.DateTimePicker();
            this.comboBoxEditCategory = new System.Windows.Forms.ComboBox();
            this.comboBoxEditPriority = new System.Windows.Forms.ComboBox();
            this.textBoxEditTitle = new System.Windows.Forms.TextBox();
            this.labelEditDueDate = new System.Windows.Forms.Label();
            this.labelEditCategory = new System.Windows.Forms.Label();
            this.labelEditPriority = new System.Windows.Forms.Label();
            this.labelEditTitle = new System.Windows.Forms.Label();
            this.labelEditHeader = new System.Windows.Forms.Label();
            this.groupBoxEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxIncomplete
            // 
            this.listBoxIncomplete.FormattingEnabled = true;
            this.listBoxIncomplete.ItemHeight = 25;
            this.listBoxIncomplete.Location = new System.Drawing.Point(40, 200);
            this.listBoxIncomplete.Name = "listBoxIncomplete";
            this.listBoxIncomplete.Size = new System.Drawing.Size(319, 304);
            this.listBoxIncomplete.TabIndex = 0;
            this.listBoxIncomplete.SelectedIndexChanged += new System.EventHandler(this.listBoxIncomplete_SelectedIndexChanged);
            // 
            // listBoxComplete
            // 
            this.listBoxComplete.FormattingEnabled = true;
            this.listBoxComplete.ItemHeight = 25;
            this.listBoxComplete.Location = new System.Drawing.Point(416, 200);
            this.listBoxComplete.Name = "listBoxComplete";
            this.listBoxComplete.Size = new System.Drawing.Size(319, 304);
            this.listBoxComplete.TabIndex = 1;
            this.listBoxComplete.SelectedIndexChanged += new System.EventHandler(this.listBoxComplete_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Incomplete Tasks";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(411, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "Completed Tasks";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // textBoxNewItem
            // 
            this.textBoxNewItem.Location = new System.Drawing.Point(45, 621);
            this.textBoxNewItem.Name = "textBoxNewItem";
            this.textBoxNewItem.Size = new System.Drawing.Size(247, 31);
            this.textBoxNewItem.TabIndex = 5;
            // 
            // btnToggleComplete
            // 
            this.btnToggleComplete.Location = new System.Drawing.Point(512, 555);
            this.btnToggleComplete.Name = "btnToggleComplete";
            this.btnToggleComplete.Size = new System.Drawing.Size(223, 97);
            this.btnToggleComplete.TabIndex = 6;
            this.btnToggleComplete.Text = "Toggle Complete";
            this.btnToggleComplete.UseVisualStyleBackColor = true;
            this.btnToggleComplete.Click += new System.EventHandler(this.btnToggleComplete_Click_1);
            // 
            // btnSaveFile
            // 
            this.btnSaveFile.Location = new System.Drawing.Point(68, 38);
            this.btnSaveFile.Name = "btnSaveFile";
            this.btnSaveFile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSaveFile.Size = new System.Drawing.Size(192, 52);
            this.btnSaveFile.TabIndex = 7;
            this.btnSaveFile.Text = "Save";
            this.btnSaveFile.UseVisualStyleBackColor = true;
            this.btnSaveFile.Click += new System.EventHandler(this.btnSaveFile_Click_1);
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(304, 41);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(197, 48);
            this.btnLoadFile.TabIndex = 8;
            this.btnLoadFile.Text = "Load";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 584);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 25);
            this.label4.TabIndex = 9;
            this.label4.Text = "Add task:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 669);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 25);
            this.label5.TabIndex = 10;
            this.label5.Text = "Priority:";
            // 
            // comboBoxPriority
            // 
            this.comboBoxPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPriority.FormattingEnabled = true;
            this.comboBoxPriority.Items.AddRange(new object[] {
            "1 - Low",
            "2 - Medium",
            "3 - High",
            "4 - Very High",
            "5 - Urgent"});
            this.comboBoxPriority.Location = new System.Drawing.Point(130, 666);
            this.comboBoxPriority.Name = "comboBoxPriority";
            this.comboBoxPriority.Size = new System.Drawing.Size(162, 33);
            this.comboBoxPriority.TabIndex = 11;
            this.comboBoxPriority.SelectedIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(320, 669);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 25);
            this.label6.TabIndex = 12;
            this.label6.Text = "Due Date:";
            // 
            // dateTimePickerDueDate
            // 
            this.dateTimePickerDueDate.Location = new System.Drawing.Point(422, 666);
            this.dateTimePickerDueDate.Name = "dateTimePickerDueDate";
            this.dateTimePickerDueDate.Size = new System.Drawing.Size(250, 31);
            this.dateTimePickerDueDate.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(40, 717);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 25);
            this.label7.TabIndex = 14;
            this.label7.Text = "Category:";
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Items.AddRange(new object[] {
            "General",
            "Work",
            "Personal",
            "Shopping",
            "Health"});
            this.comboBoxCategory.Location = new System.Drawing.Point(142, 714);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(150, 33);
            this.comboBoxCategory.TabIndex = 15;
            this.comboBoxCategory.SelectedIndex = 0;
            // 
            // btnAddDecorated
            // 
            this.btnAddDecorated.Location = new System.Drawing.Point(304, 714);
            this.btnAddDecorated.Name = "btnAddDecorated";
            this.btnAddDecorated.Size = new System.Drawing.Size(150, 33);
            this.btnAddDecorated.TabIndex = 16;
            this.btnAddDecorated.Text = "Add Decorated";
            this.btnAddDecorated.UseVisualStyleBackColor = true;
            this.btnAddDecorated.Click += new System.EventHandler(this.btnAddDecorated_Click);
            // 
            // groupBoxEdit
            // 
            this.groupBoxEdit.Controls.Add(this.btnSaveChanges);
            this.groupBoxEdit.Controls.Add(this.btnEditSelected);
            this.groupBoxEdit.Controls.Add(this.dateTimePickerEditDueDate);
            this.groupBoxEdit.Controls.Add(this.comboBoxEditCategory);
            this.groupBoxEdit.Controls.Add(this.comboBoxEditPriority);
            this.groupBoxEdit.Controls.Add(this.textBoxEditTitle);
            this.groupBoxEdit.Controls.Add(this.labelEditDueDate);
            this.groupBoxEdit.Controls.Add(this.labelEditCategory);
            this.groupBoxEdit.Controls.Add(this.labelEditPriority);
            this.groupBoxEdit.Controls.Add(this.labelEditTitle);
            this.groupBoxEdit.Controls.Add(this.labelEditHeader);
            this.groupBoxEdit.Location = new System.Drawing.Point(760, 200);
            this.groupBoxEdit.Name = "groupBoxEdit";
            this.groupBoxEdit.Size = new System.Drawing.Size(340, 450);
            this.groupBoxEdit.TabIndex = 17;
            this.groupBoxEdit.TabStop = false;
            this.groupBoxEdit.Text = "Edit Selected Item";
            // 
            // btnSaveChanges
            // 
            this.btnSaveChanges.Location = new System.Drawing.Point(180, 390);
            this.btnSaveChanges.Name = "btnSaveChanges";
            this.btnSaveChanges.Size = new System.Drawing.Size(130, 35);
            this.btnSaveChanges.TabIndex = 21;
            this.btnSaveChanges.Text = "Save Changes";
            this.btnSaveChanges.UseVisualStyleBackColor = true;
            this.btnSaveChanges.Click += new System.EventHandler(this.btnSaveChanges_Click);
            // 
            // btnEditSelected
            // 
            this.btnEditSelected.Location = new System.Drawing.Point(30, 390);
            this.btnEditSelected.Name = "btnEditSelected";
            this.btnEditSelected.Size = new System.Drawing.Size(130, 35);
            this.btnEditSelected.TabIndex = 20;
            this.btnEditSelected.Text = "Edit Selected";
            this.btnEditSelected.UseVisualStyleBackColor = true;
            this.btnEditSelected.Click += new System.EventHandler(this.btnEditSelected_Click);
            // 
            // dateTimePickerEditDueDate
            // 
            this.dateTimePickerEditDueDate.Location = new System.Drawing.Point(130, 320);
            this.dateTimePickerEditDueDate.Name = "dateTimePickerEditDueDate";
            this.dateTimePickerEditDueDate.Size = new System.Drawing.Size(180, 31);
            this.dateTimePickerEditDueDate.TabIndex = 19;
            // 
            // comboBoxEditCategory
            // 
            this.comboBoxEditCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEditCategory.FormattingEnabled = true;
            this.comboBoxEditCategory.Items.AddRange(new object[] {
            "General",
            "Work",
            "Personal",
            "Shopping",
            "Health"});
            this.comboBoxEditCategory.Location = new System.Drawing.Point(130, 260);
            this.comboBoxEditCategory.Name = "comboBoxEditCategory";
            this.comboBoxEditCategory.Size = new System.Drawing.Size(180, 33);
            this.comboBoxEditCategory.TabIndex = 18;
            // 
            // comboBoxEditPriority
            // 
            this.comboBoxEditPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEditPriority.FormattingEnabled = true;
            this.comboBoxEditPriority.Items.AddRange(new object[] {
            "1 - Low",
            "2 - Medium",
            "3 - High",
            "4 - Very High",
            "5 - Urgent"});
            this.comboBoxEditPriority.Location = new System.Drawing.Point(130, 200);
            this.comboBoxEditPriority.Name = "comboBoxEditPriority";
            this.comboBoxEditPriority.Size = new System.Drawing.Size(180, 33);
            this.comboBoxEditPriority.TabIndex = 17;
            // 
            // textBoxEditTitle
            // 
            this.textBoxEditTitle.Location = new System.Drawing.Point(130, 140);
            this.textBoxEditTitle.Name = "textBoxEditTitle";
            this.textBoxEditTitle.Size = new System.Drawing.Size(180, 31);
            this.textBoxEditTitle.TabIndex = 16;
            // 
            // labelEditDueDate
            // 
            this.labelEditDueDate.AutoSize = true;
            this.labelEditDueDate.Location = new System.Drawing.Point(25, 325);
            this.labelEditDueDate.Name = "labelEditDueDate";
            this.labelEditDueDate.Size = new System.Drawing.Size(96, 25);
            this.labelEditDueDate.TabIndex = 15;
            this.labelEditDueDate.Text = "Due Date:";
            // 
            // labelEditCategory
            // 
            this.labelEditCategory.AutoSize = true;
            this.labelEditCategory.Location = new System.Drawing.Point(25, 265);
            this.labelEditCategory.Name = "labelEditCategory";
            this.labelEditCategory.Size = new System.Drawing.Size(96, 25);
            this.labelEditCategory.TabIndex = 14;
            this.labelEditCategory.Text = "Category:";
            // 
            // labelEditPriority
            // 
            this.labelEditPriority.AutoSize = true;
            this.labelEditPriority.Location = new System.Drawing.Point(25, 205);
            this.labelEditPriority.Name = "labelEditPriority";
            this.labelEditPriority.Size = new System.Drawing.Size(84, 25);
            this.labelEditPriority.TabIndex = 13;
            this.labelEditPriority.Text = "Priority:";
            // 
            // labelEditTitle
            // 
            this.labelEditTitle.AutoSize = true;
            this.labelEditTitle.Location = new System.Drawing.Point(25, 145);
            this.labelEditTitle.Name = "labelEditTitle";
            this.labelEditTitle.Size = new System.Drawing.Size(53, 25);
            this.labelEditTitle.TabIndex = 12;
            this.labelEditTitle.Text = "Title:";
            // 
            // labelEditHeader
            // 
            this.labelEditHeader.AutoSize = true;
            this.labelEditHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEditHeader.Location = new System.Drawing.Point(25, 50);
            this.labelEditHeader.Name = "labelEditHeader";
            this.labelEditHeader.Size = new System.Drawing.Size(280, 31);
            this.labelEditHeader.TabIndex = 11;
            this.labelEditHeader.Text = "Select item to edit it";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 871);
            this.Controls.Add(this.groupBoxEdit);
            this.Controls.Add(this.btnAddDecorated);
            this.Controls.Add(this.comboBoxCategory);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dateTimePickerDueDate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxPriority);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnLoadFile);
            this.Controls.Add(this.btnSaveFile);
            this.Controls.Add(this.btnToggleComplete);
            this.Controls.Add(this.textBoxNewItem);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxComplete);
            this.Controls.Add(this.listBoxIncomplete);
            this.Name = "Form1";
            this.Text = "Todo List with Decorator Pattern";
            this.groupBoxEdit.ResumeLayout(false);
            this.groupBoxEdit.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.DateTimePicker dateTimePickerEditDueDate;
        private System.Windows.Forms.ComboBox comboBoxEditCategory;
        private System.Windows.Forms.ComboBox comboBoxEditPriority;
        private System.Windows.Forms.TextBox textBoxEditTitle;
        private System.Windows.Forms.Label labelEditDueDate;
        private System.Windows.Forms.Label labelEditCategory;
        private System.Windows.Forms.Label labelEditPriority;
        private System.Windows.Forms.Label labelEditTitle;
        private System.Windows.Forms.Label labelEditHeader;
    }
}


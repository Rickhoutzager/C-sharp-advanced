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
            this.btnAdd = new System.Windows.Forms.Button();
            this.textBoxNewItem = new System.Windows.Forms.TextBox();
            this.btnToggleComplete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxIncomplete
            // 
            this.listBoxIncomplete.FormattingEnabled = true;
            this.listBoxIncomplete.ItemHeight = 25;
            this.listBoxIncomplete.Location = new System.Drawing.Point(725, 118);
            this.listBoxIncomplete.Name = "listBoxIncomplete";
            this.listBoxIncomplete.Size = new System.Drawing.Size(319, 304);
            this.listBoxIncomplete.TabIndex = 0;
            this.listBoxIncomplete.SelectedIndexChanged += new System.EventHandler(this.listBoxIncomplete_SelectedIndexChanged);
            // 
            // listBoxComplete
            // 
            this.listBoxComplete.FormattingEnabled = true;
            this.listBoxComplete.ItemHeight = 25;
            this.listBoxComplete.Location = new System.Drawing.Point(725, 522);
            this.listBoxComplete.Name = "listBoxComplete";
            this.listBoxComplete.Size = new System.Drawing.Size(319, 279);
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
            this.label2.Location = new System.Drawing.Point(725, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Incomplete Tasks";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(725, 459);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(179, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "Completed Tasks";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(149, 404);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(200, 80);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click_1);
            // 
            // textBoxNewItem
            // 
            this.textBoxNewItem.Location = new System.Drawing.Point(205, 224);
            this.textBoxNewItem.Name = "textBoxNewItem";
            this.textBoxNewItem.Size = new System.Drawing.Size(297, 31);
            this.textBoxNewItem.TabIndex = 5;
            // 
            // btnToggleComplete
            // 
            this.btnToggleComplete.Location = new System.Drawing.Point(260, 541);
            this.btnToggleComplete.Name = "btnToggleComplete";
            this.btnToggleComplete.Size = new System.Drawing.Size(223, 97);
            this.btnToggleComplete.TabIndex = 6;
            this.btnToggleComplete.Text = "Toggle Complete";
            this.btnToggleComplete.UseVisualStyleBackColor = true;
            this.btnToggleComplete.Click += new System.EventHandler(this.btnToggleComplete_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 871);
            this.Controls.Add(this.btnToggleComplete);
            this.Controls.Add(this.textBoxNewItem);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxComplete);
            this.Controls.Add(this.listBoxIncomplete);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxIncomplete;
        private System.Windows.Forms.ListBox listBoxComplete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox textBoxNewItem;
        private System.Windows.Forms.Button btnToggleComplete;
    }
}


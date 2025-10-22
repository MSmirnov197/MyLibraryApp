namespace WinFormsApp1
{
    partial class test
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
            listBoxBooks = new ListBox();
            txtId = new TextBox();
            txtTitle = new TextBox();
            txtAuthor = new TextBox();
            cmbGenre = new ComboBox();
            txtYear = new TextBox();
            btnCreate = new Button();
            btnDelete = new Button();
            btnRead = new Button();
            btnUpdate = new Button();
            btnGroup = new Button();
            btnClear = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            txtQuantity = new TextBox();
            SuspendLayout();
            // 
            // listBoxBooks
            // 
            listBoxBooks.FormattingEnabled = true;
            listBoxBooks.ItemHeight = 15;
            listBoxBooks.Location = new Point(14, 14);
            listBoxBooks.Margin = new Padding(4, 3, 4, 3);
            listBoxBooks.Name = "listBoxBooks";
            listBoxBooks.Size = new Size(419, 259);
            listBoxBooks.TabIndex = 0;
            // 
            // txtId
            // 
            txtId.Location = new Point(14, 288);
            txtId.Margin = new Padding(4, 3, 4, 3);
            txtId.Name = "txtId";
            txtId.Size = new Size(116, 23);
            txtId.TabIndex = 1;
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(14, 323);
            txtTitle.Margin = new Padding(4, 3, 4, 3);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(233, 23);
            txtTitle.TabIndex = 2;
            // 
            // txtAuthor
            // 
            txtAuthor.Location = new Point(14, 358);
            txtAuthor.Margin = new Padding(4, 3, 4, 3);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.Size = new Size(233, 23);
            txtAuthor.TabIndex = 3;
            // 
            // cmbGenre
            // 
            cmbGenre.FormattingEnabled = true;
            cmbGenre.Location = new Point(14, 392);
            cmbGenre.Margin = new Padding(4, 3, 4, 3);
            cmbGenre.Name = "cmbGenre";
            cmbGenre.Size = new Size(233, 23);
            cmbGenre.TabIndex = 4;
            // 
            // txtYear
            // 
            txtYear.Location = new Point(14, 427);
            txtYear.Margin = new Padding(4, 3, 4, 3);
            txtYear.Name = "txtYear";
            txtYear.Size = new Size(116, 23);
            txtYear.TabIndex = 5;
            // 
            // btnCreate
            // 
            btnCreate.Location = new Point(257, 288);
            btnCreate.Margin = new Padding(4, 3, 4, 3);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(88, 27);
            btnCreate.TabIndex = 7;
            btnCreate.Text = "Добавить";
            btnCreate.UseVisualStyleBackColor = true;
            btnCreate.Click += btnCreate_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(350, 288);
            btnDelete.Margin = new Padding(4, 3, 4, 3);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(88, 27);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Удалить";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRead
            // 
            btnRead.Location = new Point(257, 323);
            btnRead.Margin = new Padding(4, 3, 4, 3);
            btnRead.Name = "btnRead";
            btnRead.Size = new Size(88, 27);
            btnRead.TabIndex = 9;
            btnRead.Text = "Просмотреть";
            btnRead.UseVisualStyleBackColor = true;
            btnRead.Click += btnRead_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(350, 323);
            btnUpdate.Margin = new Padding(4, 3, 4, 3);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(88, 27);
            btnUpdate.TabIndex = 10;
            btnUpdate.Text = "Обновить";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnGroup
            // 
            btnGroup.Location = new Point(257, 358);
            btnGroup.Margin = new Padding(4, 3, 4, 3);
            btnGroup.Name = "btnGroup";
            btnGroup.Size = new Size(181, 27);
            btnGroup.TabIndex = 11;
            btnGroup.Text = "Группировка по жанрам";
            btnGroup.UseVisualStyleBackColor = true;
            btnGroup.Click += btnGroup_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(257, 392);
            btnClear.Margin = new Padding(4, 3, 4, 3);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(181, 27);
            btnClear.TabIndex = 12;
            btnClear.Text = "Очистить";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 270);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(21, 15);
            label1.TabIndex = 13;
            label1.Text = "ID:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 305);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(62, 15);
            label2.TabIndex = 14;
            label2.Text = "Название:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 339);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(43, 15);
            label3.TabIndex = 15;
            label3.Text = "Автор:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 374);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(41, 15);
            label4.TabIndex = 16;
            label4.Text = "Жанр:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(14, 408);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(29, 15);
            label5.TabIndex = 17;
            label5.Text = "Год:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 443);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(75, 15);
            label6.TabIndex = 18;
            label6.Text = "Количество:";
            // 
            // txtQuantity
            // 
            txtQuantity.Location = new Point(14, 462);
            txtQuantity.Margin = new Padding(4, 3, 4, 3);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Size = new Size(116, 23);
            txtQuantity.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(448, 520);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnClear);
            Controls.Add(btnGroup);
            Controls.Add(btnUpdate);
            Controls.Add(btnRead);
            Controls.Add(btnDelete);
            Controls.Add(btnCreate);
            Controls.Add(txtQuantity);
            Controls.Add(txtYear);
            Controls.Add(cmbGenre);
            Controls.Add(txtAuthor);
            Controls.Add(txtTitle);
            Controls.Add(txtId);
            Controls.Add(listBoxBooks);
            Margin = new Padding(4, 3, 4, 3);
            Name = "Form1";
            Text = "Библиотека";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListBox listBoxBooks;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtAuthor;
        private System.Windows.Forms.ComboBox cmbGenre;
        private System.Windows.Forms.TextBox txtYear;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnGroup;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

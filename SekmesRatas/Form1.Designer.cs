namespace SekmesRatas
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.Btn_Add = new System.Windows.Forms.Button();
            this.Btn_Delete = new System.Windows.Forms.Button();
            this.Btn_Save = new System.Windows.Forms.Button();
            this.Btn_Clear = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.btn_spin = new System.Windows.Forms.Button();
            this.Btn_External = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Btn_Help = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.timerRepaint = new System.Windows.Forms.Timer(this.components);
            this.Btn_Close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Elementai:";
            // 
            // Btn_Add
            // 
            this.Btn_Add.Location = new System.Drawing.Point(278, 129);
            this.Btn_Add.Name = "Btn_Add";
            this.Btn_Add.Size = new System.Drawing.Size(127, 53);
            this.Btn_Add.TabIndex = 2;
            this.Btn_Add.Text = "Prideti";
            this.Btn_Add.UseVisualStyleBackColor = true;
            this.Btn_Add.Click += new System.EventHandler(this.Btn_Add_Click);
            // 
            // Btn_Delete
            // 
            this.Btn_Delete.Location = new System.Drawing.Point(278, 188);
            this.Btn_Delete.Name = "Btn_Delete";
            this.Btn_Delete.Size = new System.Drawing.Size(127, 53);
            this.Btn_Delete.TabIndex = 3;
            this.Btn_Delete.Text = "Istrinti";
            this.Btn_Delete.UseVisualStyleBackColor = true;
            this.Btn_Delete.Click += new System.EventHandler(this.Btn_Delete_Click);
            // 
            // Btn_Save
            // 
            this.Btn_Save.Location = new System.Drawing.Point(145, 393);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(127, 53);
            this.Btn_Save.TabIndex = 5;
            this.Btn_Save.Text = "Issaugoti";
            this.Btn_Save.UseVisualStyleBackColor = true;
            this.Btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // Btn_Clear
            // 
            this.Btn_Clear.Location = new System.Drawing.Point(278, 247);
            this.Btn_Clear.Name = "Btn_Clear";
            this.Btn_Clear.Size = new System.Drawing.Size(127, 53);
            this.Btn_Clear.TabIndex = 4;
            this.Btn_Clear.Text = "Istrinti Viska";
            this.Btn_Clear.UseVisualStyleBackColor = true;
            this.Btn_Clear.Click += new System.EventHandler(this.Btn_Clear_Click);
            // 
            // listView1
            // 
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(15, 25);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(257, 362);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // btn_spin
            // 
            this.btn_spin.Location = new System.Drawing.Point(278, 315);
            this.btn_spin.Name = "btn_spin";
            this.btn_spin.Size = new System.Drawing.Size(127, 53);
            this.btn_spin.TabIndex = 7;
            this.btn_spin.Text = "SUKTI";
            this.btn_spin.UseVisualStyleBackColor = true;
            this.btn_spin.Click += new System.EventHandler(this.Btn_Spin_Click);
            // 
            // Btn_External
            // 
            this.Btn_External.Location = new System.Drawing.Point(12, 393);
            this.Btn_External.Name = "Btn_External";
            this.Btn_External.Size = new System.Drawing.Size(127, 53);
            this.Btn_External.TabIndex = 8;
            this.Btn_External.Text = "Ikelti";
            this.Btn_External.UseVisualStyleBackColor = true;
            this.Btn_External.Click += new System.EventHandler(this.Btn_External_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Btn_Help
            // 
            this.Btn_Help.Location = new System.Drawing.Point(278, 25);
            this.Btn_Help.Name = "Btn_Help";
            this.Btn_Help.Size = new System.Drawing.Size(127, 53);
            this.Btn_Help.TabIndex = 9;
            this.Btn_Help.Text = "Pagalba";
            this.Btn_Help.UseVisualStyleBackColor = true;
            this.Btn_Help.Click += new System.EventHandler(this.Btn_Help_Click);
            // 
            // panel
            // 
            this.panel.Location = new System.Drawing.Point(592, 25);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(749, 582);
            this.panel.TabIndex = 10;
            // 
            // Btn_Close
            // 
            this.Btn_Close.Location = new System.Drawing.Point(278, 393);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(127, 53);
            this.Btn_Close.TabIndex = 11;
            this.Btn_Close.Text = "Iseiti";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 619);
            this.Controls.Add(this.Btn_Close);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.Btn_Help);
            this.Controls.Add(this.Btn_External);
            this.Controls.Add(this.btn_spin);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.Btn_Save);
            this.Controls.Add(this.Btn_Clear);
            this.Controls.Add(this.Btn_Delete);
            this.Controls.Add(this.Btn_Add);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Sekmes Ratas";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Btn_Add;
        private System.Windows.Forms.Button Btn_Delete;
        private System.Windows.Forms.Button Btn_Save;
        private System.Windows.Forms.Button Btn_Clear;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btn_spin;
        private System.Windows.Forms.Button Btn_External;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button Btn_Help;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Timer timerRepaint;
        private System.Windows.Forms.Button Btn_Close;
    }
}


namespace RefactorMuch
{
  partial class MainForm
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
      this.directoryBrowse1 = new RefactorMuch.DirectoryBrowse();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.button1 = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // directoryBrowse1
      // 
      this.directoryBrowse1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.directoryBrowse1.Location = new System.Drawing.Point(17, 13);
      this.directoryBrowse1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
      this.directoryBrowse1.Name = "directoryBrowse1";
      this.directoryBrowse1.OnFinishedParsing = null;
      this.directoryBrowse1.Size = new System.Drawing.Size(831, 767);
      this.directoryBrowse1.TabIndex = 0;
      this.directoryBrowse1.Load += new System.EventHandler(this.directoryBrowse1_Load);
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(919, 125);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(378, 22);
      this.textBox1.TabIndex = 1;
      // 
      // textBox2
      // 
      this.textBox2.Location = new System.Drawing.Point(919, 153);
      this.textBox2.Multiline = true;
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(378, 115);
      this.textBox2.TabIndex = 2;
      // 
      // textBox3
      // 
      this.textBox3.Location = new System.Drawing.Point(919, 274);
      this.textBox3.Multiline = true;
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new System.Drawing.Size(378, 115);
      this.textBox3.TabIndex = 3;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(1318, 125);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 4;
      this.button1.Text = "Test Regex";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1459, 793);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.textBox3);
      this.Controls.Add(this.textBox2);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.directoryBrowse1);
      this.DoubleBuffered = true;
      this.Font = new System.Drawing.Font("Verdana", 9F);
      this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.Name = "MainForm";
      this.Text = "Refactor Much";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private DirectoryBrowse directoryBrowse1;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.Button button1;
  }
}


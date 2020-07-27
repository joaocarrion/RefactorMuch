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
      this.SuspendLayout();
      // 
      // directoryBrowse1
      // 
      this.directoryBrowse1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.directoryBrowse1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.directoryBrowse1.Location = new System.Drawing.Point(17, 13);
      this.directoryBrowse1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
      this.directoryBrowse1.Name = "directoryBrowse1";
      this.directoryBrowse1.OnFinishedParsing = null;
      this.directoryBrowse1.Size = new System.Drawing.Size(1004, 767);
      this.directoryBrowse1.TabIndex = 0;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1035, 793);
      this.Controls.Add(this.directoryBrowse1);
      this.DoubleBuffered = true;
      this.Font = new System.Drawing.Font("Verdana", 9F);
      this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Refactor Much";
      this.ResumeLayout(false);

    }

    #endregion

    private DirectoryBrowse directoryBrowse1;
  }
}


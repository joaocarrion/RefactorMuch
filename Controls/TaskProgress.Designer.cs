namespace RefactorMuch.Controls
{
  partial class TaskProgress
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.progressBar1 = new System.Windows.Forms.ProgressBar();
      this.lbTaskInfo = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // progressBar1
      // 
      this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.progressBar1.Location = new System.Drawing.Point(0, 0);
      this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new System.Drawing.Size(945, 90);
      this.progressBar1.TabIndex = 0;
      // 
      // lbTaskInfo
      // 
      this.lbTaskInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lbTaskInfo.Location = new System.Drawing.Point(953, 0);
      this.lbTaskInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.lbTaskInfo.Name = "lbTaskInfo";
      this.lbTaskInfo.Size = new System.Drawing.Size(241, 90);
      this.lbTaskInfo.TabIndex = 1;
      this.lbTaskInfo.Text = "No task";
      this.lbTaskInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // TaskProgress
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.lbTaskInfo);
      this.Controls.Add(this.progressBar1);
      this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.Name = "TaskProgress";
      this.Size = new System.Drawing.Size(1199, 90);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ProgressBar progressBar1;
    private System.Windows.Forms.Label lbTaskInfo;
  }
}

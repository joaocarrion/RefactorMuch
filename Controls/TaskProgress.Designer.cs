﻿namespace RefactorMuch.Controls
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
      this.lbInformation = new System.Windows.Forms.Label();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // progressBar1
      // 
      this.progressBar1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
      this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.progressBar1.Location = new System.Drawing.Point(3, 3);
      this.progressBar1.MarqueeAnimationSpeed = 85;
      this.progressBar1.Name = "progressBar1";
      this.tableLayoutPanel1.SetRowSpan(this.progressBar1, 2);
      this.progressBar1.Size = new System.Drawing.Size(668, 78);
      this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
      this.progressBar1.TabIndex = 0;
      // 
      // lbTaskInfo
      // 
      this.lbTaskInfo.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lbTaskInfo.Location = new System.Drawing.Point(677, 0);
      this.lbTaskInfo.Name = "lbTaskInfo";
      this.lbTaskInfo.Size = new System.Drawing.Size(219, 42);
      this.lbTaskInfo.TabIndex = 1;
      this.lbTaskInfo.Text = "No task";
      this.lbTaskInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lbInformation
      // 
      this.lbInformation.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lbInformation.Location = new System.Drawing.Point(677, 42);
      this.lbInformation.Name = "lbInformation";
      this.lbInformation.Size = new System.Drawing.Size(219, 42);
      this.lbInformation.TabIndex = 2;
      this.lbInformation.Text = "No task";
      this.lbInformation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
      this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.lbInformation, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.lbTaskInfo, 1, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel1.Size = new System.Drawing.Size(899, 84);
      this.tableLayoutPanel1.TabIndex = 3;
      // 
      // TaskProgress
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tableLayoutPanel1);
      this.Name = "TaskProgress";
      this.Size = new System.Drawing.Size(899, 84);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ProgressBar progressBar1;
    private System.Windows.Forms.Label lbTaskInfo;
    private System.Windows.Forms.Label lbInformation;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
  }
}

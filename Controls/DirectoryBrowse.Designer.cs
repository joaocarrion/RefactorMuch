﻿using RefactorMuch.Controls.FormFix;
using System.Drawing;
using System.Windows.Forms;

namespace RefactorMuch
{
  partial class DirectoryBrowse
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectoryBrowse));
      this.label1 = new System.Windows.Forms.Label();
      this.cbLeftDirectory = new System.Windows.Forms.ComboBox();
      this.btBrowseLeft = new System.Windows.Forms.Button();
      this.btStartRefresh = new System.Windows.Forms.Button();
      this.btBrowseRight = new System.Windows.Forms.Button();
      this.cbRightDirectory = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.btSettings = new System.Windows.Forms.Button();
      this.updateProgress = new System.Windows.Forms.Timer(this.components);
      this.treeView1 = new RefactorMuch.Controls.FormFix.BufferedTreeView();
      this.imageList1 = new System.Windows.Forms.ImageList(this.components);
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.filePreview = new RefactorMuch.Controls.FormFix.BufferedRichText();
      this.taskProgress1 = new RefactorMuch.Controls.TaskProgress();
      this.previewOutside = new System.Windows.Forms.Panel();
      this.previewInside = new System.Windows.Forms.Panel();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 11);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(70, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Left Directory";
      // 
      // cbLeftDirectory
      // 
      this.cbLeftDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cbLeftDirectory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.cbLeftDirectory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllSystemSources;
      this.cbLeftDirectory.Location = new System.Drawing.Point(86, 8);
      this.cbLeftDirectory.Name = "cbLeftDirectory";
      this.cbLeftDirectory.Size = new System.Drawing.Size(827, 21);
      this.cbLeftDirectory.TabIndex = 0;
      // 
      // btBrowseLeft
      // 
      this.btBrowseLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btBrowseLeft.Location = new System.Drawing.Point(919, 8);
      this.btBrowseLeft.Name = "btBrowseLeft";
      this.btBrowseLeft.Size = new System.Drawing.Size(75, 23);
      this.btBrowseLeft.TabIndex = 4;
      this.btBrowseLeft.Text = "Browse";
      this.btBrowseLeft.UseVisualStyleBackColor = true;
      this.btBrowseLeft.Click += new System.EventHandler(this.btBrowseLeft_Click);
      // 
      // btStartRefresh
      // 
      this.btStartRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btStartRefresh.Location = new System.Drawing.Point(1000, 40);
      this.btStartRefresh.Name = "btStartRefresh";
      this.btStartRefresh.Size = new System.Drawing.Size(108, 23);
      this.btStartRefresh.TabIndex = 2;
      this.btStartRefresh.Text = "Start / Refresh";
      this.btStartRefresh.UseVisualStyleBackColor = true;
      this.btStartRefresh.Click += new System.EventHandler(this.ButtonStart);
      // 
      // btBrowseRight
      // 
      this.btBrowseRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btBrowseRight.Location = new System.Drawing.Point(919, 40);
      this.btBrowseRight.Name = "btBrowseRight";
      this.btBrowseRight.Size = new System.Drawing.Size(75, 23);
      this.btBrowseRight.TabIndex = 5;
      this.btBrowseRight.Text = "Browse";
      this.btBrowseRight.UseVisualStyleBackColor = true;
      // 
      // cbRightDirectory
      // 
      this.cbRightDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cbRightDirectory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.cbRightDirectory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllSystemSources;
      this.cbRightDirectory.Location = new System.Drawing.Point(86, 42);
      this.cbRightDirectory.Name = "cbRightDirectory";
      this.cbRightDirectory.Size = new System.Drawing.Size(827, 21);
      this.cbRightDirectory.TabIndex = 1;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(3, 47);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(77, 13);
      this.label3.TabIndex = 8;
      this.label3.Text = "Right Directory";
      // 
      // btSettings
      // 
      this.btSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btSettings.Location = new System.Drawing.Point(1000, 8);
      this.btSettings.Name = "btSettings";
      this.btSettings.Size = new System.Drawing.Size(108, 23);
      this.btSettings.TabIndex = 3;
      this.btSettings.Text = "Settings";
      this.btSettings.UseVisualStyleBackColor = true;
      // 
      // updateProgress
      // 
      this.updateProgress.Enabled = true;
      this.updateProgress.Interval = 1000;
      this.updateProgress.Tick += new System.EventHandler(this.UpdateProgress);
      // 
      // treeView1
      // 
      this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeView1.ImageIndex = 0;
      this.treeView1.ImageList = this.imageList1;
      this.treeView1.Location = new System.Drawing.Point(0, 0);
      this.treeView1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
      this.treeView1.Name = "treeView1";
      this.treeView1.SelectedImageIndex = 0;
      this.treeView1.ShowNodeToolTips = true;
      this.treeView1.ShowRootLines = false;
      this.treeView1.Size = new System.Drawing.Size(350, 556);
      this.treeView1.TabIndex = 14;
      this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
      this.treeView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MouseClickTreeView);
      // 
      // imageList1
      // 
      this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
      this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "diff.png");
      this.imageList1.Images.SetKeyName(1, "duplicate.png");
      this.imageList1.Images.SetKeyName(2, "moved.png");
      this.imageList1.Images.SetKeyName(3, "changed.png");
      this.imageList1.Images.SetKeyName(4, "similar.png");
      // 
      // splitContainer1
      // 
      this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.splitContainer1.Location = new System.Drawing.Point(6, 69);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.treeView1);
      this.splitContainer1.Panel1MinSize = 350;
      // preview outside
      this.previewOutside.Size = new System.Drawing.Size(128, 128);
      this.previewOutside.Controls.Add(this.previewInside);
      this.previewInside.Location = new System.Drawing.Point(8, 8);
      this.previewInside.Size = new System.Drawing.Size(112, 112);
      this.previewInside.BackColor = Color.White;
      this.previewInside.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.previewOutside);
      this.splitContainer1.Size = new System.Drawing.Size(1102, 556);
      this.splitContainer1.SplitterDistance = 350;
      this.splitContainer1.TabIndex = 15;
      //
      this.previewOutside.Dock = DockStyle.Fill;
      this.previewOutside.BorderStyle = BorderStyle.Fixed3D;
      this.previewInside.BorderStyle = BorderStyle.None;
      this.previewInside.Controls.Add(this.filePreview);
      // 
      // filePreview
      // 
      this.filePreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.filePreview.Dock = System.Windows.Forms.DockStyle.Fill;
      this.filePreview.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.filePreview.Location = new System.Drawing.Point(0, 0);
      this.filePreview.Name = "filePreview";
      this.filePreview.Size = new System.Drawing.Size(748, 556);
      this.filePreview.TabIndex = 0;
      this.filePreview.Text = "";
      this.filePreview.WordWrap = false;
      // 
      // taskProgress1
      // 
      this.taskProgress1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.taskProgress1.CurrentTask = "Idle";
      this.taskProgress1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
      this.taskProgress1.Information = "";
      this.taskProgress1.Location = new System.Drawing.Point(6, 629);
      this.taskProgress1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
      this.taskProgress1.Name = "taskProgress1";
      this.taskProgress1.PercentDone = 0;
      this.taskProgress1.Size = new System.Drawing.Size(1102, 34);
      this.taskProgress1.Style = System.Windows.Forms.ProgressBarStyle.Blocks;
      this.taskProgress1.TabIndex = 13;
      // 
      // DirectoryBrowse
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.taskProgress1);
      this.Controls.Add(this.btSettings);
      this.Controls.Add(this.btBrowseRight);
      this.Controls.Add(this.cbRightDirectory);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.btStartRefresh);
      this.Controls.Add(this.btBrowseLeft);
      this.Controls.Add(this.cbLeftDirectory);
      this.Controls.Add(this.label1);
      this.Name = "DirectoryBrowse";
      this.Size = new System.Drawing.Size(1111, 664);
      this.Load += new System.EventHandler(this.DirectoryBrowse_Load);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cbLeftDirectory;
    private System.Windows.Forms.Button btBrowseLeft;
    private System.Windows.Forms.Button btStartRefresh;
    private System.Windows.Forms.Button btBrowseRight;
    private System.Windows.Forms.ComboBox cbRightDirectory;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button btSettings;
    private Controls.TaskProgress taskProgress1;
    private System.Windows.Forms.Timer updateProgress;
    private System.Windows.Forms.ImageList imageList1;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private BufferedTreeView treeView1;
    private BufferedRichText filePreview;
    private Panel previewOutside;
    private Panel previewInside;
  }
}

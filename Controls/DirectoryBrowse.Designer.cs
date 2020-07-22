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
      this.label1 = new System.Windows.Forms.Label();
      this.cbLeftDirectory = new System.Windows.Forms.ComboBox();
      this.btBrowseLeft = new System.Windows.Forms.Button();
      this.btStartRefresh = new System.Windows.Forms.Button();
      this.btBrowseRight = new System.Windows.Forms.Button();
      this.cbRightDirectory = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.btSettings = new System.Windows.Forms.Button();
      this.updateProgess = new System.Windows.Forms.Timer(this.components);
      this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.taskProgress1 = new RefactorMuch.Controls.TaskProgress();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(4, 3);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(91, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Left Directory";
      // 
      // cbLeftDirectory
      // 
      this.cbLeftDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cbLeftDirectory.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.cbLeftDirectory.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllSystemSources;
      this.cbLeftDirectory.FormattingEnabled = true;
      this.cbLeftDirectory.Location = new System.Drawing.Point(115, 0);
      this.cbLeftDirectory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.cbLeftDirectory.Name = "cbLeftDirectory";
      this.cbLeftDirectory.Size = new System.Drawing.Size(388, 22);
      this.cbLeftDirectory.TabIndex = 0;
      // 
      // btBrowseLeft
      // 
      this.btBrowseLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btBrowseLeft.Location = new System.Drawing.Point(512, 0);
      this.btBrowseLeft.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.btBrowseLeft.Name = "btBrowseLeft";
      this.btBrowseLeft.Size = new System.Drawing.Size(100, 25);
      this.btBrowseLeft.TabIndex = 4;
      this.btBrowseLeft.Text = "Browse";
      this.btBrowseLeft.UseVisualStyleBackColor = true;
      this.btBrowseLeft.Click += new System.EventHandler(this.btBrowseLeft_Click);
      // 
      // btStartRefresh
      // 
      this.btStartRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btStartRefresh.Location = new System.Drawing.Point(620, 34);
      this.btStartRefresh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.btStartRefresh.Name = "btStartRefresh";
      this.btStartRefresh.Size = new System.Drawing.Size(144, 25);
      this.btStartRefresh.TabIndex = 2;
      this.btStartRefresh.Text = "Start / Refresh";
      this.btStartRefresh.UseVisualStyleBackColor = true;
      this.btStartRefresh.Click += new System.EventHandler(this.btStartRefresh_Click);
      // 
      // btBrowseRight
      // 
      this.btBrowseRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btBrowseRight.Location = new System.Drawing.Point(512, 34);
      this.btBrowseRight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.btBrowseRight.Name = "btBrowseRight";
      this.btBrowseRight.Size = new System.Drawing.Size(100, 25);
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
      this.cbRightDirectory.FormattingEnabled = true;
      this.cbRightDirectory.Location = new System.Drawing.Point(115, 37);
      this.cbRightDirectory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.cbRightDirectory.Name = "cbRightDirectory";
      this.cbRightDirectory.Size = new System.Drawing.Size(388, 22);
      this.cbRightDirectory.TabIndex = 1;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(4, 42);
      this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(99, 14);
      this.label3.TabIndex = 8;
      this.label3.Text = "Right Directory";
      // 
      // btSettings
      // 
      this.btSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btSettings.Location = new System.Drawing.Point(620, 0);
      this.btSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.btSettings.Name = "btSettings";
      this.btSettings.Size = new System.Drawing.Size(144, 25);
      this.btSettings.TabIndex = 3;
      this.btSettings.Text = "Settings";
      this.btSettings.UseVisualStyleBackColor = true;
      // 
      // updateProgess
      // 
      this.updateProgess.Enabled = true;
      this.updateProgess.Interval = 1000;
      this.updateProgess.Tick += new System.EventHandler(this.updateProgess_Tick);
      // 
      // flowPanel
      // 
      this.flowPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.flowPanel.AutoScroll = true;
      this.flowPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.flowPanel.Location = new System.Drawing.Point(7, 65);
      this.flowPanel.Name = "flowPanel";
      this.flowPanel.Size = new System.Drawing.Size(757, 608);
      this.flowPanel.TabIndex = 4;
      // 
      // taskProgress1
      // 
      this.taskProgress1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.taskProgress1.CurrentTask = "No task";
      this.taskProgress1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.taskProgress1.Location = new System.Drawing.Point(4, 679);
      this.taskProgress1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.taskProgress1.Name = "taskProgress1";
      this.taskProgress1.PercentDone = 0;
      this.taskProgress1.Size = new System.Drawing.Size(760, 36);
      this.taskProgress1.Style = System.Windows.Forms.ProgressBarStyle.Blocks;
      this.taskProgress1.TabIndex = 13;
      // 
      // DirectoryBrowse
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.flowPanel);
      this.Controls.Add(this.taskProgress1);
      this.Controls.Add(this.btSettings);
      this.Controls.Add(this.btBrowseRight);
      this.Controls.Add(this.cbRightDirectory);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.btStartRefresh);
      this.Controls.Add(this.btBrowseLeft);
      this.Controls.Add(this.cbLeftDirectory);
      this.Controls.Add(this.label1);
      this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
      this.Name = "DirectoryBrowse";
      this.Size = new System.Drawing.Size(768, 715);
      this.Load += new System.EventHandler(this.DirectoryBrowse_Load);
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
    private System.Windows.Forms.Timer updateProgess;
    private System.Windows.Forms.FlowLayoutPanel flowPanel;
  }
}

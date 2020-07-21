namespace InteractiveMerge.Controls
{
  partial class MovedPath
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
      this.lbLeftPath = new System.Windows.Forms.Label();
      this.btDiff = new System.Windows.Forms.Button();
      this.lbRightPath = new System.Windows.Forms.Label();
      this.lbFilename = new System.Windows.Forms.Label();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.tablePanel = new System.Windows.Forms.TableLayoutPanel();
      this.panel1 = new System.Windows.Forms.Panel();
      this.tablePanel.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // lbLeftPath
      // 
      this.lbLeftPath.AutoEllipsis = true;
      this.lbLeftPath.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lbLeftPath.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.lbLeftPath.Location = new System.Drawing.Point(7, 36);
      this.lbLeftPath.Name = "lbLeftPath";
      this.lbLeftPath.Size = new System.Drawing.Size(348, 32);
      this.lbLeftPath.TabIndex = 0;
      this.lbLeftPath.Text = "File path A => File path B\r\n";
      this.lbLeftPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // btDiff
      // 
      this.btDiff.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btDiff.Location = new System.Drawing.Point(595, 0);
      this.btDiff.Name = "btDiff";
      this.btDiff.Size = new System.Drawing.Size(107, 26);
      this.btDiff.TabIndex = 1;
      this.btDiff.Text = "Diff...";
      this.btDiff.UseVisualStyleBackColor = true;
      this.btDiff.Click += new System.EventHandler(this.btDiff_Click);
      // 
      // lbRightPath
      // 
      this.lbRightPath.AutoEllipsis = true;
      this.lbRightPath.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lbRightPath.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.lbRightPath.Location = new System.Drawing.Point(361, 36);
      this.lbRightPath.Name = "lbRightPath";
      this.lbRightPath.Size = new System.Drawing.Size(348, 32);
      this.lbRightPath.TabIndex = 2;
      this.lbRightPath.Text = "File path A => File path B\r\n";
      this.lbRightPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lbFilename
      // 
      this.lbFilename.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lbFilename.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbFilename.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.lbFilename.Location = new System.Drawing.Point(0, 0);
      this.lbFilename.Name = "lbFilename";
      this.lbFilename.Size = new System.Drawing.Size(589, 26);
      this.lbFilename.TabIndex = 3;
      this.lbFilename.Text = "FileName A";
      this.lbFilename.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tablePanel
      // 
      this.tablePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tablePanel.ColumnCount = 2;
      this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tablePanel.Controls.Add(this.lbRightPath, 1, 1);
      this.tablePanel.Controls.Add(this.lbLeftPath, 0, 1);
      this.tablePanel.Controls.Add(this.panel1, 0, 0);
      this.tablePanel.Location = new System.Drawing.Point(0, 0);
      this.tablePanel.Name = "tablePanel";
      this.tablePanel.Padding = new System.Windows.Forms.Padding(4);
      this.tablePanel.RowCount = 2;
      this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tablePanel.Size = new System.Drawing.Size(716, 72);
      this.tablePanel.TabIndex = 0;
      // 
      // panel1
      // 
      this.tablePanel.SetColumnSpan(this.panel1, 2);
      this.panel1.Controls.Add(this.lbFilename);
      this.panel1.Controls.Add(this.btDiff);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(7, 7);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(702, 26);
      this.panel1.TabIndex = 4;
      // 
      // MovedPath
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.Controls.Add(this.tablePanel);
      this.MinimumSize = new System.Drawing.Size(400, 70);
      this.Name = "MovedPath";
      this.Size = new System.Drawing.Size(716, 72);
      this.tablePanel.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lbLeftPath;
    private System.Windows.Forms.Button btDiff;
    private System.Windows.Forms.Label lbRightPath;
    private System.Windows.Forms.Label lbFilename;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.TableLayoutPanel tablePanel;
    private System.Windows.Forms.Panel panel1;
  }
}

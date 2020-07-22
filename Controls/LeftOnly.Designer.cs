namespace RefactorMuch.Controls
{
  partial class LeftOnly
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
      this.lbFilename = new System.Windows.Forms.Label();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.tablePanel = new System.Windows.Forms.TableLayoutPanel();
      this.panel1 = new System.Windows.Forms.Panel();
      this.btCopy = new System.Windows.Forms.Button();
      this.lblIsLeft = new System.Windows.Forms.Label();
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
      this.lbLeftPath.Size = new System.Drawing.Size(316, 32);
      this.lbLeftPath.TabIndex = 0;
      this.lbLeftPath.Text = "File path A => File path B\r\n";
      this.lbLeftPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // btDiff
      // 
      this.btDiff.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btDiff.Location = new System.Drawing.Point(527, 0);
      this.btDiff.Name = "btDiff";
      this.btDiff.Size = new System.Drawing.Size(107, 26);
      this.btDiff.TabIndex = 1;
      this.btDiff.Text = "View...";
      this.btDiff.UseVisualStyleBackColor = true;
      this.btDiff.Click += new System.EventHandler(this.btDiff_Click);
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
      this.lbFilename.Size = new System.Drawing.Size(408, 26);
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
      this.tablePanel.Controls.Add(this.lbLeftPath, 0, 1);
      this.tablePanel.Controls.Add(this.panel1, 0, 0);
      this.tablePanel.Location = new System.Drawing.Point(64, 0);
      this.tablePanel.Name = "tablePanel";
      this.tablePanel.Padding = new System.Windows.Forms.Padding(4);
      this.tablePanel.RowCount = 2;
      this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tablePanel.Size = new System.Drawing.Size(652, 72);
      this.tablePanel.TabIndex = 1;
      // 
      // panel1
      // 
      this.tablePanel.SetColumnSpan(this.panel1, 2);
      this.panel1.Controls.Add(this.btCopy);
      this.panel1.Controls.Add(this.lbFilename);
      this.panel1.Controls.Add(this.btDiff);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(7, 7);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(638, 26);
      this.panel1.TabIndex = 4;
      // 
      // btCopy
      // 
      this.btCopy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.btCopy.Location = new System.Drawing.Point(414, 0);
      this.btCopy.Name = "btCopy";
      this.btCopy.Size = new System.Drawing.Size(107, 26);
      this.btCopy.TabIndex = 4;
      this.btCopy.Text = "Copy";
      this.btCopy.UseVisualStyleBackColor = true;
      this.btCopy.Click += new System.EventHandler(this.btCopy_Click);
      // 
      // lblIsLeft
      // 
      this.lblIsLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.lblIsLeft.Font = new System.Drawing.Font("Symbol", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
      this.lblIsLeft.Location = new System.Drawing.Point(4, 4);
      this.lblIsLeft.Name = "lblIsLeft";
      this.lblIsLeft.Size = new System.Drawing.Size(56, 52);
      this.lblIsLeft.TabIndex = 2;
      this.lblIsLeft.Text = "<";
      this.lblIsLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.toolTip1.SetToolTip(this.lblIsLeft, "Left only");
      // 
      // LeftOnly
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.Controls.Add(this.lblIsLeft);
      this.Controls.Add(this.tablePanel);
      this.Name = "LeftOnly";
      this.Size = new System.Drawing.Size(714, 70);
      this.tablePanel.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lbLeftPath;
    private System.Windows.Forms.Button btDiff;
    private System.Windows.Forms.Label lbFilename;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.TableLayoutPanel tablePanel;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button btCopy;
    private System.Windows.Forms.Label lblIsLeft;
  }
}

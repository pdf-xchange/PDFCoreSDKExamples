namespace CoreAPIDemo
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.sampleTree = new System.Windows.Forms.TreeView();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.runSample = new System.Windows.Forms.ToolStripButton();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.prevPage = new System.Windows.Forms.ToolStripButton();
			this.currentPage = new System.Windows.Forms.ToolStripTextBox();
			this.pagesCount = new System.Windows.Forms.ToolStripLabel();
			this.nextPage = new System.Windows.Forms.ToolStripButton();
			this.previewImage = new System.Windows.Forms.PictureBox();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.codeSource = new System.Windows.Forms.TextBox();
			this.toolStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.previewImage)).BeginInit();
			this.SuspendLayout();
			// 
			// sampleTree
			// 
			this.sampleTree.Dock = System.Windows.Forms.DockStyle.Left;
			this.sampleTree.Location = new System.Drawing.Point(0, 0);
			this.sampleTree.Name = "sampleTree";
			this.sampleTree.Size = new System.Drawing.Size(802, 924);
			this.sampleTree.TabIndex = 0;
			this.sampleTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.sampleTree_NodeMouseClick);
			this.sampleTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.sampleTree_NodeMouseDoubleClick);
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(802, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(10, 924);
			this.splitter1.TabIndex = 0;
			this.splitter1.TabStop = false;
			this.splitter1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter1_SplitterMoved);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runSample});
			this.toolStrip1.Location = new System.Drawing.Point(812, 477);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(740, 45);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// runSample
			// 
			this.runSample.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.runSample.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.runSample.Image = ((System.Drawing.Image)(resources.GetObject("runSample.Image")));
			this.runSample.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.runSample.Name = "runSample";
			this.runSample.Size = new System.Drawing.Size(169, 42);
			this.runSample.Text = "Run Sample";
			this.runSample.Click += new System.EventHandler(this.runSample_Click);
			// 
			// toolStrip2
			// 
			this.toolStrip2.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.prevPage,
            this.currentPage,
            this.pagesCount,
            this.nextPage});
			this.toolStrip2.Location = new System.Drawing.Point(812, 0);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(740, 31);
			this.toolStrip2.TabIndex = 0;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// prevPage
			// 
			this.prevPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.prevPage.Image = ((System.Drawing.Image)(resources.GetObject("prevPage.Image")));
			this.prevPage.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.prevPage.Name = "prevPage";
			this.prevPage.Size = new System.Drawing.Size(28, 28);
			this.prevPage.Text = "Previuos Page";
			this.prevPage.Click += new System.EventHandler(this.prevPage_Click);
			// 
			// currentPage
			// 
			this.currentPage.Name = "currentPage";
			this.currentPage.Size = new System.Drawing.Size(50, 31);
			this.currentPage.Text = "1";
			this.currentPage.TextChanged += new System.EventHandler(this.currentPage_TextChanged);
			// 
			// pagesCount
			// 
			this.pagesCount.Name = "pagesCount";
			this.pagesCount.Size = new System.Drawing.Size(29, 28);
			this.pagesCount.Text = "/0";
			// 
			// nextPage
			// 
			this.nextPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.nextPage.Image = ((System.Drawing.Image)(resources.GetObject("nextPage.Image")));
			this.nextPage.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.nextPage.Name = "nextPage";
			this.nextPage.Size = new System.Drawing.Size(28, 28);
			this.nextPage.Text = "Next Page";
			this.nextPage.Click += new System.EventHandler(this.nextPage_Click);
			// 
			// previewImage
			// 
			this.previewImage.BackColor = System.Drawing.SystemColors.Control;
			this.previewImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.previewImage.Location = new System.Drawing.Point(812, 0);
			this.previewImage.Name = "previewImage";
			this.previewImage.Size = new System.Drawing.Size(740, 532);
			this.previewImage.TabIndex = 0;
			this.previewImage.TabStop = false;
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter2.Location = new System.Drawing.Point(812, 522);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(740, 10);
			this.splitter2.TabIndex = 0;
			this.splitter2.TabStop = false;
			this.splitter2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter2_SplitterMoved);
			// 
			// codeSource
			// 
			this.codeSource.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.codeSource.Location = new System.Drawing.Point(812, 532);
			this.codeSource.Multiline = true;
			this.codeSource.Name = "codeSource";
			this.codeSource.Size = new System.Drawing.Size(740, 392);
			this.codeSource.TabIndex = 2;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1552, 924);
			this.Controls.Add(this.toolStrip2);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.splitter2);
			this.Controls.Add(this.previewImage);
			this.Controls.Add(this.codeSource);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.sampleTree);
			this.Name = "Form1";
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResizeEnd += new System.EventHandler(this.Form1_ResizeEnd);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.previewImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView sampleTree;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton runSample;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripButton prevPage;
		private System.Windows.Forms.ToolStripTextBox currentPage;
		private System.Windows.Forms.ToolStripButton nextPage;
		private System.Windows.Forms.PictureBox previewImage;
		private System.Windows.Forms.ToolStripLabel pagesCount;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.TextBox codeSource;
	}
}


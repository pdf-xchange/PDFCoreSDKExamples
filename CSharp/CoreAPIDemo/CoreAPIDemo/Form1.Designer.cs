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
			System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Create New Document");
			System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Open Document From String Path");
			System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Open Document From IStream");
			System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Open Password Protected Document From IAFS_Name");
			System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Save Document To File");
			System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Document", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11});
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.sampleTree = new System.Windows.Forms.TreeView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.runSample = new System.Windows.Forms.ToolStripButton();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.prevPage = new System.Windows.Forms.ToolStripButton();
			this.currentPage = new System.Windows.Forms.ToolStripTextBox();
			this.pagesCount = new System.Windows.Forms.ToolStripLabel();
			this.nextPage = new System.Windows.Forms.ToolStripButton();
			this.previewImage = new System.Windows.Forms.PictureBox();
			this.toolStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.previewImage)).BeginInit();
			this.SuspendLayout();
			// 
			// sampleTree
			// 
			this.sampleTree.Dock = System.Windows.Forms.DockStyle.Left;
			this.sampleTree.Location = new System.Drawing.Point(0, 32);
			this.sampleTree.Name = "sampleTree";
			treeNode7.Name = "createNewDoc";
			treeNode7.Text = "Create New Document";
			treeNode8.Name = "openDocFromStringPath";
			treeNode8.Text = "Open Document From String Path";
			treeNode9.Name = "openDocumentFromStream";
			treeNode9.Text = "Open Document From IStream";
			treeNode10.Name = "openPasswordProtectedDocument";
			treeNode10.Text = "Open Password Protected Document From IAFS_Name";
			treeNode11.Name = "saveDocumentToFile";
			treeNode11.Text = "Save Document To File";
			treeNode12.Name = "Document";
			treeNode12.Text = "Document";
			this.sampleTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode12});
			this.sampleTree.Size = new System.Drawing.Size(802, 892);
			this.sampleTree.TabIndex = 0;
			this.sampleTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.sampleTree_NodeMouseDoubleClick);
			// 
			// panel1
			// 
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1552, 924);
			this.panel1.TabIndex = 1;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(802, 32);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(10, 892);
			this.splitter1.TabIndex = 0;
			this.splitter1.TabStop = false;
			// 
			// toolStrip1
			// 
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runSample});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(1552, 32);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// runSample
			// 
			this.runSample.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.runSample.Image = ((System.Drawing.Image)(resources.GetObject("runSample.Image")));
			this.runSample.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.runSample.Name = "runSample";
			this.runSample.Size = new System.Drawing.Size(111, 29);
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
			this.toolStrip2.Location = new System.Drawing.Point(812, 32);
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
			this.pagesCount.Size = new System.Drawing.Size(29, 29);
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
			this.previewImage.Location = new System.Drawing.Point(812, 63);
			this.previewImage.Name = "previewImage";
			this.previewImage.Size = new System.Drawing.Size(740, 861);
			this.previewImage.TabIndex = 0;
			this.previewImage.TabStop = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1552, 924);
			this.Controls.Add(this.previewImage);
			this.Controls.Add(this.toolStrip2);
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.sampleTree);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.panel1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
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
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton runSample;
		private System.Windows.Forms.ToolStrip toolStrip2;
		private System.Windows.Forms.ToolStripButton prevPage;
		private System.Windows.Forms.ToolStripTextBox currentPage;
		private System.Windows.Forms.ToolStripButton nextPage;
		private System.Windows.Forms.PictureBox previewImage;
		private System.Windows.Forms.ToolStripLabel pagesCount;
	}
}


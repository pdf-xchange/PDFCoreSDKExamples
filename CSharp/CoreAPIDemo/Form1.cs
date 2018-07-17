using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using PDFXCoreAPI;

namespace CoreAPIDemo
{

	public partial class Form1 : Form
	{
		public IPXC_Inst		m_pxcInst = null;
		public IPXC_Document	m_CurDoc = null;
#if DEBUG
		public static string		m_sDirPath = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\CSharp\\CoreAPIDemo\\";
#else
		public static string		m_sDirPath = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).FullName + "\\CSharp\\CoreAPIDemo\\";
#endif
		public uint CurrentPage
		{
			get { return uint.Parse(currentPage.Text) - 1; }
		}


		public Form1()
		{
			m_pxcInst = new PXC_Inst();
			m_pxcInst.Init("");
			InitializeComponent();
		}
		[DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
		private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);

		private void Form1_Load(object sender, EventArgs e)
		{
			SetWindowTheme(sampleTree.Handle, "explorer", null);
			ImageList il = new ImageList();
			try
			{
				string sImgFolder = System.Environment.CurrentDirectory + "\\Images\\";
				Bitmap img = new Bitmap(sImgFolder + "folder_24.png");
				il.Images.Add(img);
				img = new Bitmap(sImgFolder + "run_24.png");
				il.Images.Add(img);
				img = new Bitmap(sImgFolder + "runGreyed_24.png");
				il.Images.Add(img);
				sampleTree.ImageList = il;
				sampleTree.TreeViewNodeSorter = new NodeSorter();
				ImageList ilfb = new ImageList();
				img = new Bitmap(System.Environment.CurrentDirectory + "\\Images\\bookmark_24.png");
				ilfb.Images.Add(img);
				bookmarksTree.ImageList = ilfb;
			}
			catch (Exception)
			{
			}
			RefillTree();
		}

		public class NodeSorter : IComparer
		{
			// Compare the length of the strings, or the strings
			// themselves, if they are the same length.
			public int Compare(object x, object y)
			{
				TreeNode tx = x as TreeNode;
				TreeNode ty = y as TreeNode;

				int index = tx.Text.IndexOf(' ');
				string sXNum = tx.Text.Substring(0, index);
				index = ty.Text.IndexOf(' ');
				string sYNum = ty.Text.Substring(0, index);

				string[] aXNums = sXNum.Split('.');
				string[] aYNums = sYNum.Split('.');
				for (int i = 0; i < aXNums.Length; i++)
				{
					if (i >= aYNums.Length)
						return 1; //x is greater
					int nX = int.Parse(aXNums[i]);
					int nY = int.Parse(aYNums[i]);
					if (nX == nY)
						continue;
					return (nX > nY) ? 1 : -1;
				}
				return 0;
			}
		}

		private void RefillTree()
		{
			sampleTree.BeginUpdate();
			sampleTree.Nodes.Clear();
			Type[] typeList = Assembly.GetExecutingAssembly().GetTypes();
			foreach (Type t in typeList)
			{
				AddClassToTree(t);
			}

			sampleTree.Sort();
			sampleTree.EndUpdate();
		}
		private void AddClassToTree(Type classType)
		{
			DescriptionAttribute attr = classType.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
			if (attr == null)
				return;

			string[] aFilters = filterEdit.Text.Split(' ');
			TreeNode root = sampleTree.Nodes.Insert(-1, attr.Description);
			root.ImageIndex = 0;
			root.SelectedImageIndex = 0;
			foreach (MethodInfo mi in classType.GetMethods())
			{
				attr = mi.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
				if (attr == null)
					continue;
				bool bSuccess = true;
				for (int i = 0; i < aFilters.Length; i++)
				{
					if ((aFilters[i].Length == 0) && (aFilters.Length > 1))
						continue;
					if (attr.Description.IndexOf(aFilters[i], StringComparison.CurrentCultureIgnoreCase) < 0)
					{
						bSuccess = false;
						break;
					}
				}
				if (bSuccess)
				{
					TreeNode child = root.Nodes.Insert(-1, classType.Name + "_" + mi.Name, attr.Description);
					if (child != null)
					{
						child.ImageIndex = 2;
						child.SelectedImageIndex = 1;
					}
				}
			}
			root.Expand();
			if (root.Nodes.Count == 0)
				sampleTree.Nodes.Remove(root);
		}

		class BookmarkNode : TreeNode
		{
			public BookmarkNode(IPXC_Bookmark book)
			{
				m_Bookmark = book;
			}
			~BookmarkNode()
			{
				m_Bookmark = null;
			}
		public IPXC_Bookmark m_Bookmark = null;
		}


		public void AddBookmarkToTree(TreeNode node, IPXC_Bookmark root)
		{
			IPXC_Bookmark child = root.FirstChild;

			for (int i = 0; i < root.ChildrenCount; i++)
			{
				IPXC_ActionsList aList = child.Actions;
				BookmarkNode childNode = new BookmarkNode(child);
				childNode.Name = ((node != null) ? (node.Name + ".") : "Bookmark") + (i + 1);
				childNode.ImageIndex = 0;
				childNode.SelectedImageIndex = 0;
				childNode.Text = child.Title;
				if (child.ChildrenCount > 0)
					AddBookmarkToTree(childNode, child);
				if (node != null)
					node.Nodes.Add(childNode);
				else
					bookmarksTree.Nodes.Add(childNode);
				child = child.Next;
			}
		}

		private MethodInfo GetCurrentMethod(TreeNode curNode)
		{
			do
			{
				TreeNode node = curNode;
				if (node == null)
					node = sampleTree.SelectedNode;
				if (node == null)
					break;
				int nIndex = node.Name.IndexOf('_');
				if (nIndex < 0)
					break;
				string sClass = node.Name.Substring(0, nIndex);
				string sMethod = node.Name.Substring(nIndex + 1);

				Type thisType = Type.GetType("CoreAPIDemo." + sClass + ", CoreAPIDemo");
				if (thisType == null)
					break;

				MethodInfo theMethod = thisType.GetMethod(sMethod);
				if (theMethod == null)
					break;
				return theMethod;
			} while (false);
			return null;
		}

		private int GetMethodLine(string className, string methodName)
		{
			string sPath = m_sDirPath + className + ".cs";
			string sData = System.IO.File.ReadAllText(sPath);
			int nIndex = sData.IndexOf(methodName);

			string[] aDataLen = sData.Substring(0, nIndex).Split('\n');
			return aDataLen.Length;
		}

		private string GetMethodCode(string className, string methodName)
		{
			try
			{
				string sPath = m_sDirPath + className + ".cs";
				string sData = System.IO.File.ReadAllText(sPath);
				int nIndex = sData.IndexOf(methodName);
				while (nIndex >= 0)
				{
					if (sData[nIndex] == '\n')
					{
						nIndex++;
						break;
					}
					nIndex--;
				}

				if (nIndex < 0)
					return "";

				string sWS = "";
				for (int i = nIndex; i < sData.Length; i++)
				{
					if (!Char.IsWhiteSpace(sData[i]))
					{
						sWS = sData.Substring(nIndex, i - nIndex);
						break;
					}
				}
				int nEnd = sData.IndexOf("\n" + sWS + "}", nIndex);
				if (nEnd < 0)
					return "";
				nEnd += sWS.Length + 2;
				string sRes = sData.Substring(nIndex, nEnd - nIndex);
				string[] aData = sRes.Split('\n');
				sRes = "";
				for (int i = 0; i < aData.Length; i++)
				{
					string sTmp = aData[i];
					int nPos = sTmp.IndexOf(sWS);
					if (nPos < 0)
						sRes += sTmp;
					else
						sRes += sTmp.Substring(sWS.Length);
				}
				return sRes;
			}
			catch (Exception)
			{

			}
			return "";
		}
		private void UpdateCodeSample(TreeNode curNode)
		{
			codeSource.Text = "";
			MethodInfo theMethod = GetCurrentMethod(curNode);
			if (theMethod == null)
				return;
			byte[] bytes = theMethod.GetMethodBody().GetILAsByteArray();
			codeSource.Text = GetMethodCode(theMethod.DeclaringType.Name, theMethod.Name);
		}

		private void InvokeMethod(TreeNode curNode)
		{
			MethodInfo theMethod = GetCurrentMethod(curNode);
			if (theMethod == null)
			{
				if ((curNode == null) || (curNode.Name.IndexOf('_') >= 0))
					MessageBox.Show("Please select a sample from the needed category in sample tree and click Run Sample to execute it.");
				return;
			}

			theMethod.Invoke(this, new Object[] { this });

			UpdateControlsFromDocument();
			UpdatePreviewFromCurrentDocument();
		}

		public void UpdatePreviewFromCurrentDocument()
		{
			if (m_CurDoc == null)
			{
				previewImage.Image = null;
				return;
			}
			int nPage = int.Parse(currentPage.Text) - 1;
			if (nPage >= m_CurDoc.Pages.Count)
			{
				nPage = (int)m_CurDoc.Pages.Count - 1;
				currentPage.Text = (nPage + 1).ToString();
			}

			IPXC_Page srcPage = m_CurDoc.Pages[(uint)nPage];
			IAUX_Inst auxInst = (IAUX_Inst)m_pxcInst.GetExtension("AUX");
			IIXC_Inst ixcInst = (IIXC_Inst)m_pxcInst.GetExtension("IXC");
			//Getting source page matrix
			PXC_Matrix srcPageMatrix = srcPage.GetMatrix(PXC_BoxType.PBox_PageBox);
			//Getting source page Page Box without rotation
			PXC_Rect srcRect = srcPage.get_Box(PXC_BoxType.PBox_PageBox);
			//Getting visual source Page Box by transforming it through matrix
			auxInst.MathHelper.Rect_Transform(srcPageMatrix, ref srcRect);
			//We'll insert the visual src page into visual dest page indented rectangle including page rotations and clipping

			PXC_Rect destRect;
			destRect.left = 0;
			destRect.right = previewImage.Width;
			destRect.top = 0;
			destRect.bottom = previewImage.Height;

			//Fit rect to rect
			{
				PXC_Rect rcRes, rcR, rcS;
				rcS = destRect;
				rcS.top = rcS.bottom;
				rcS.bottom = 0;
				rcR = srcRect;
				{
					var k1 = (rcR.right - rcR.left) / Math.Abs(rcR.top - rcR.bottom);
					var k2 = (rcS.right - rcS.left) / Math.Abs(rcS.top - rcS.bottom);

					if (k1 >= k2)
					{
						var h = (rcS.right - rcS.left) / k1;
						rcRes = rcS;
						rcRes.bottom += (Math.Abs(rcRes.top - rcRes.bottom) - h) / 2;
						rcRes.top = rcRes.bottom + h;
					}
					else
					{
						var w = Math.Abs(rcS.top - rcS.bottom) * k1;
						rcRes = rcS;
						rcRes.left += ((rcRes.right - rcRes.left) - w) / 2;
						rcRes.right = rcRes.left + w;
					}
				}

				destRect.left = 0;
				destRect.right = rcRes.right - rcRes.left;
				destRect.top = 0;
				destRect.bottom = rcRes.top - rcRes.bottom;
			}

			if (((int)(destRect.right - destRect.left) < 1) || ((int)(destRect.bottom - destRect.top) < 1))
				return;
			Bitmap image = new Bitmap((int)(destRect.right - destRect.left), (int)(destRect.bottom - destRect.top));


			PXC_Matrix pageToRectMatrix = auxInst.MathHelper.Matrix_RectToRect(srcRect, destRect);
			tagRECT rcTmp;
			rcTmp.left = (int)destRect.left;
			rcTmp.right = (int)destRect.right;
			rcTmp.top = (int)destRect.top;
			rcTmp.bottom = (int)destRect.bottom;
			pageToRectMatrix = auxInst.MathHelper.Matrix_Multiply(srcPageMatrix, pageToRectMatrix);

			Graphics g = Graphics.FromImage(image);
			g.Clear(Color.White);
			IntPtr hdc = g.GetHdc();
			srcPage.DrawToDevice((uint)hdc, ref rcTmp, ref pageToRectMatrix, 0/*(uint)PDFXEdit.PXC_DrawToDeviceFlags.DDF_AsVector*/);
			g.ReleaseHdc(hdc);
			previewImage.Image = image;
			g.Dispose();
			g = null;
			srcPage = null;
		}

		public void UpdateControlsFromDocument()
		{
			pagesCount.Text = "/0";
			bookmarksTree.Nodes.Clear();
			GC.Collect();
			GC.WaitForPendingFinalizers();
			if (m_CurDoc == null)
				return;
			pagesCount.Text = "/" + m_CurDoc.Pages.Count.ToString();
			int nPage = int.Parse(currentPage.Text) - 1;
			if (nPage >= m_CurDoc.Pages.Count)
			{
				nPage = (int)m_CurDoc.Pages.Count - 1;
				currentPage.Text = (nPage + 1).ToString();
			}
			//Updating bookmarks
			IPXC_Bookmark root = m_CurDoc.BookmarkRoot;
			if ((root != null) && (root.ChildrenCount != 0))
				AddBookmarkToTree(null, root);
		}

		public void CloseDocument()
		{
			if (m_CurDoc != null)
			{
				m_CurDoc.Close();
				m_CurDoc = null;
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
		}

		private void runSample_Click(object sender, EventArgs e)
		{
			InvokeMethod(null);
		}
		private void Form1_ResizeEnd(object sender, EventArgs e)
		{
			UpdatePreviewFromCurrentDocument();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (previewImage.Image != null)
				previewImage.Image.Dispose();
			CloseDocument();
			//GC.Collect();
			//GC.WaitForPendingFinalizers();
			m_pxcInst.Finalize();
			m_pxcInst = null;
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		private void currentPage_TextChanged(object sender, EventArgs e)
		{
			UpdatePreviewFromCurrentDocument();
		}

		private void prevPage_Click(object sender, EventArgs e)
		{
			if (m_CurDoc == null)
				return;
			int nPage = int.Parse(currentPage.Text) - 1;
			if (nPage > 0)
				currentPage.Text = (nPage).ToString();
		}

		private void nextPage_Click(object sender, EventArgs e)
		{
			if (m_CurDoc == null)
				return;
			int nPage = int.Parse(currentPage.Text) - 1;
			if (nPage < (m_CurDoc.Pages.Count - 1))
			{
				currentPage.Text = (nPage + 2).ToString();
			}
		}
		private void sampleTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			InvokeMethod(e.Node);
		}

		private void bookmarksTree_NodeMouseClick(object sender, EventArgs e)
		{
			IPXS_Inst pxcInst = m_pxcInst.GetExtension("PXS");
			uint typeGoTo = pxcInst.StrToAtom("GoTo");

			BookmarkNode curNode = (BookmarkNode)bookmarksTree.SelectedNode;
			IPXC_ActionsList aList = curNode.m_Bookmark.Actions;
			for (uint i = aList.Count - 1; i >= 0; i--)
			{
				if (aList[i].Type == typeGoTo)
				{
					IPXC_Action_Goto actGoTO = (IPXC_Action_Goto)aList[i];
					currentPage.Text = (actGoTO.get_Dest().nPageNum + 1).ToString();
					break;
				}
			}
			
		}

		private void sampleTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			UpdateCodeSample(e.Node);
			if ((e.X < e.Node.Bounds.Left) && (e.Node.Name.IndexOf('_') >= 0))
				InvokeMethod(e.Node);
		}

		private void splitter2_SplitterMoved(object sender, SplitterEventArgs e)
		{
			UpdatePreviewFromCurrentDocument();
		}

		private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
		{
			UpdatePreviewFromCurrentDocument();
		}

		private FormWindowState m_LastWndState = FormWindowState.Normal;
		private void Form1_Resize(object sender, EventArgs e)
		{
			if (WindowState != m_LastWndState)
			{
				UpdatePreviewFromCurrentDocument();
				m_LastWndState = WindowState;
			}

		}
		private void filterEdit_TextChanged(object sender, EventArgs e)
		{
			RefillTree();
		}

		private void toolStripButton1_Click(object sender, EventArgs e)
		{
			filterEdit.Text = "";
			RefillTree();
		}

		private void expandAll_Click(object sender, EventArgs e)
		{
			sampleTree.ExpandAll();
		}

		private void collapseAll_Click(object sender, EventArgs e)
		{
			sampleTree.CollapseAll();
		}
		private void toolStripButton2_Click(object sender, EventArgs e)
		{
			try
			{
				TreeNode curNode = sampleTree.SelectedNode;
				MethodInfo theMethod = GetCurrentMethod(curNode);
				if (theMethod == null)
					return;

				int fileline = GetMethodLine(theMethod.DeclaringType.Name, theMethod.Name);

				string filePath = m_sDirPath + theMethod.DeclaringType.Name + ".cs";

				EnvDTE.DTE dte = (EnvDTE.DTE)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE");
				dte.MainWindow.Activate();
				EnvDTE.Window w = dte.ItemOperations.OpenFile(filePath);
				((EnvDTE.TextSelection)dte.ActiveDocument.Selection).GotoLine(fileline, true);
			}
			catch (Exception err)
			{
				Console.Write(err.Message);
			}
		}

		private void toolStripButton3_Click(object sender, EventArgs e)
		{
			CloseDocument();

			UpdateControlsFromDocument();
			UpdatePreviewFromCurrentDocument();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			Document.SaveDocToFile(this);
		}

		private void btnOpen_Click(object sender, EventArgs e)
		{
			if (m_CurDoc == null)
				return;

			String FileName = Path.GetTempFileName();
			FileName = FileName.Replace(".tmp", ".pdf");
			m_CurDoc.WriteToFile(FileName);
			Process pr = Process.Start(FileName);
		}
	}
}

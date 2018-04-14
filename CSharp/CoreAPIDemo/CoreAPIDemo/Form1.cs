using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PDFXCoreAPI;

namespace CoreAPIDemo
{
	public partial class Form1 : Form
	{
		IPXC_Inst m_pxcInst = null;
		IPXC_Document m_CurDoc = null;
		bool m_bNeedToCloseDoc = false;
		public Form1()
		{
			m_pxcInst = new PXC_Inst();
			m_pxcInst.Init("");
			InitializeComponent();
		}

		private void UpdatePreviewFromCurrentDocument()
		{
			if (m_CurDoc == null)
				return;
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

		private void UpdateControlsFromDocument()
		{
			pagesCount.Text = "/0";
			if (m_CurDoc == null)
				return;
			pagesCount.Text = "/" + m_CurDoc.Pages.Count.ToString();
			int nPage = int.Parse(currentPage.Text) - 1;
			if (nPage >= m_CurDoc.Pages.Count)
			{
				nPage = (int)m_CurDoc.Pages.Count - 1;
				currentPage.Text = (nPage + 1).ToString();
			}
		}

		private void CloseDocument()
		{
			if ((m_CurDoc != null) && (m_bNeedToCloseDoc))
				m_CurDoc.Close();
			m_CurDoc = null;
			m_bNeedToCloseDoc = false;
		}

		private void runSample_Click(object sender, EventArgs e)
		{
			TreeNode node = sampleTree.SelectedNode;
			if (node == null)
			{
				MessageBox.Show("Please select a sample from the needed category in sample tree and click Run Sample to execute it.");
				return;
			}
			Type thisType = this.GetType();
			MethodInfo theMethod = thisType.GetMethod(node.Name);
			if (theMethod == null)
			{
				MessageBox.Show("Please select a sample from the needed category in sample tree and click Run Sample to execute it.");
				return;
			}
			theMethod.Invoke(this, null);
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
			int nPage = int.Parse(currentPage.Text) - 1;
			if (nPage > 0)
				currentPage.Text = (nPage).ToString();
		}

		private void nextPage_Click(object sender, EventArgs e)
		{
			int nPage = int.Parse(currentPage.Text) - 1;
			if (nPage < (m_CurDoc.Pages.Count - 1))
			{
				currentPage.Text = (nPage + 2).ToString();
			}
		}
		private void sampleTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			TreeNode node = sampleTree.SelectedNode;
			if (node == null)
				return;
			Type thisType = this.GetType();
			MethodInfo theMethod = thisType.GetMethod(node.Name);
			if (theMethod == null)
				return;
			theMethod.Invoke(this, null);
		}


		#region Method Implementations
		public void createNewDoc()
		{
			IPXC_Document coreDoc = m_pxcInst.NewDocument();
			PXC_Rect rc;
			rc.left = 0;
			rc.right = 600;
			rc.top = 800;
			rc.bottom = 0;
			IPXC_UndoRedoData urd;
			coreDoc.Pages.AddEmptyPages(0, 4, ref rc, null, out urd);
			CloseDocument();
			m_CurDoc = coreDoc;
			UpdateControlsFromDocument();
			UpdatePreviewFromCurrentDocument();
		}

		public void openDocFromStringPath()
		{
			string sPath = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\Documents\\FeatureChartEU.pdf";
			CloseDocument();
			m_CurDoc = m_pxcInst.OpenDocumentFromFile(sPath, null);
			m_bNeedToCloseDoc = true;
			UpdateControlsFromDocument();
			UpdatePreviewFromCurrentDocument();
		}

		public void openDocumentFromStream()
		{
			string sPath = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\Documents\\FeatureChartEU.pdf";
			CloseDocument();
			FileStream srcStream = new FileStream(sPath, FileMode.Open);
			if (srcStream != null)
			{
				IStreamWrapper srcIStream = new IStreamWrapper(srcStream);
				m_CurDoc = m_pxcInst.OpenDocumentFrom(srcIStream, null);
				m_bNeedToCloseDoc = true;
				UpdateControlsFromDocument();
				UpdatePreviewFromCurrentDocument();
			}
			srcStream.Close();
		}

		public class AuthCallback : IPXC_DocAuthCallback
		{
			public void AuthDoc(IPXC_Document pDoc, uint nFlags)
			{
				//If this method is called then the document is protected
				pDoc.AuthorizeWithPassword("111");
			}
		}

		public void openPasswordProtectedDocument()
		{
			string sPath = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\Documents\\PasswordProtected.pdf";
			IAFS_Inst fsInst = (IAFS_Inst)m_pxcInst.GetExtension("AFS");
			IAFS_Name destPath = fsInst.DefaultFileSys.StringToName(sPath); //Converting string to name
			CloseDocument();
			AuthCallback clbk = new AuthCallback();
			m_CurDoc = m_pxcInst.OpenDocumentFrom(destPath, clbk);
			m_bNeedToCloseDoc = true;
			UpdateControlsFromDocument();
			UpdatePreviewFromCurrentDocument();
		}

		public void saveDocumentToFile()
		{
			if (m_CurDoc == null)
				return;
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "PDF Documents (*.pdf)|*.pdf|All Files (*.*)|*.*";
			sfd.DefaultExt = "pdf";
			sfd.FilterIndex = 1;
			sfd.CheckPathExists = true;
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				m_CurDoc.WriteToFile(sfd.FileName);
			}
		}
		#endregion

	}
}

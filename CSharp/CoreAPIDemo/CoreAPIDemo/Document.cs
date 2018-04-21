using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using PDFXCoreAPI;

namespace CoreAPIDemo
{
	[Description("Document")]
	public class Document
	{
		[Description("Create new document")]
		static public void CreateNewDoc(Form1 Parent)
		{
			IPXC_Document coreDoc = Parent.m_pxcInst.NewDocument();
			PXC_Rect rc;
			rc.left = 0;
			rc.right = 600;
			rc.top = 800;
			rc.bottom = 0;
			IPXC_UndoRedoData urd;
			coreDoc.Pages.AddEmptyPages(0, 4, ref rc, null, out urd);
			Parent.CloseDocument();
			Parent.m_CurDoc = coreDoc;
		}

		[Description("Open document with open file dialog")]
		static public void OpenDocWithOpenDialog(Form1 Parent)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "PDF Documents (*.pdf)|*.pdf|All Files (*.*)|*.*";
			ofd.DefaultExt = "pdf";
			ofd.FilterIndex = 1;
			ofd.CheckPathExists = true;
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				Parent.CloseDocument();
				Parent.m_CurDoc = Parent.m_pxcInst.OpenDocumentFromFile(ofd.FileName, null);
			}
		}

		[Description("Open document from string path")]
		static public void OpenDocFromStringPath(Form1 Parent)
		{
			string sPath = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\Documents\\FeatureChartEU.pdf";
			Parent.CloseDocument();
			Parent.m_CurDoc = Parent.m_pxcInst.OpenDocumentFromFile(sPath, null);
		}

		[Description("Open document from IStream")]
		static public void OpenDocumentFromStream(Form1 Parent)
		{
			string sPath = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\Documents\\FeatureChartEU.pdf";
			Parent.CloseDocument();
			FileStream srcStream = new FileStream(sPath, FileMode.Open);
			if (srcStream != null)
			{
				IStreamWrapper srcIStream = new IStreamWrapper(srcStream);
				Parent.m_CurDoc = Parent.m_pxcInst.OpenDocumentFrom(srcIStream, null);
			}
		}

		private class AuthCallback : IPXC_DocAuthCallback
		{
			public void AuthDoc(IPXC_Document pDoc, uint nFlags)
			{
				//If this method is called then the document is protected
				pDoc.AuthorizeWithPassword("111");
			}
		}

		[Description("Open password protected document from IAFS_Name")]
		static public void OpenPasswordProtectedDocument(Form1 Parent)
		{
			string sPath = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\Documents\\PasswordProtected.pdf";
			IAFS_Inst fsInst = (IAFS_Inst)Parent.m_pxcInst.GetExtension("AFS");
			IAFS_Name destPath = fsInst.DefaultFileSys.StringToName(sPath); //Converting string to name
			Parent.CloseDocument();
			AuthCallback clbk = new AuthCallback();
			Parent.m_CurDoc = Parent.m_pxcInst.OpenDocumentFrom(destPath, clbk);
		}

		[Description("Save document to file")]
		static public void SaveDocumentToFile(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return;
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "PDF Documents (*.pdf)|*.pdf|All Files (*.*)|*.*";
			sfd.DefaultExt = "pdf";
			sfd.FilterIndex = 1;
			sfd.CheckPathExists = true;
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				Parent.m_CurDoc.WriteToFile(sfd.FileName);
			}
		}
	}
}

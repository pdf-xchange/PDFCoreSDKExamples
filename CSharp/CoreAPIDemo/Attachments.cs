using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using PDFXCoreAPI;

namespace CoreAPIDemo
{
	[Description("13. Attachments")]
	class Attachments
	{
		[Description("13.1. Add attachment as bookmark")]
		static public int AddAttachmentAsBookmark(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
			IPXC_Bookmark bookmark = null;
			if (Parent.SelectedBookmarkNode == null)
				bookmark = Parent.m_CurDoc.BookmarkRoot.AddNewChild(true);
			else
				bookmark = Parent.SelectedBookmarkNode.m_Bookmark.AddNewSibling(false);

			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			uint atomGoToE = pxsInst.StrToAtom("GoToE");
			IPXC_NameTree attachments = Parent.m_CurDoc.GetNameTree("EmbeddedFiles");
			IPXS_PDFVariant var = null;
			try
			{
				var = attachments.Lookup("FeatureChartEU.pdf");
			}
			catch (Exception)
			{
				string sFilePath = System.Environment.CurrentDirectory + "\\Documents\\FeatureChartEU.pdf";
				IPXC_FileSpec fileSpec = Parent.m_CurDoc.CreateEmbeddFile(sFilePath);
				IPXC_EmbeddedFileStream EFS = fileSpec.EmbeddedFile;
				EFS.UpdateFromFile2(sFilePath);
				var = fileSpec.PDFObject;
			}

			attachments.Add("FeatureChartEU.pdf", var);

			IPXC_Action_Goto actionGoToE = Parent.m_pxcInst.GetActionHandler(atomGoToE).CreateEmptyAction(atomGoToE, Parent.m_CurDoc) as IPXC_Action_Goto;

			IPXC_GoToETargetPath targetPath = actionGoToE.TargetPath;
			IPXC_GoToETargetItem targetItem = targetPath.InsertNew();
			targetItem.FileName = "FeatureChartEU.pdf";
			targetItem = targetPath.InsertNew();
			targetItem.FileName = "MyStamps.pdf";
			targetItem = targetPath.InsertNew();
			targetItem.AnnotIndex = 0;
			targetItem.PageNumber = 0;

			IPXC_ActionsList aList = Parent.m_CurDoc.CreateActionsList();
			bookmark.Title = "GoToE Action";
			bookmark.Style = PXC_BookmarkStyle.BookmarkFont_Normal;
			aList.Insert(0, actionGoToE);
			bookmark.Actions = aList;

			return (int)Form1.eFormUpdateFlags.efuf_Attachments | (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("13.2. Add attachment as annotation")]
		static public int AddAttachmentAsAnnotation(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IPXC_UndoRedoData urData = null;
			PXC_Rect rcPage = Parent.m_CurDoc.Pages[0].get_Box(PXC_BoxType.PBox_PageBox);
			IPXC_Page page = Parent.m_CurDoc.Pages.InsertPage(0, ref rcPage, out urData);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			//Getting File attachment annotation atom for the InsertNewAnnot method
			uint nText = pxsInst.StrToAtom("FileAttachment");

			double nCX = (rcPage.right - rcPage.left) / 2.0;
			double nCY = (rcPage.top - rcPage.bottom) / 2.0;
			PXC_Rect rcOut = new PXC_Rect();
			rcOut.left = nCX - 200;
			rcOut.bottom = nCY + 250;
			rcOut.right = nCX - 150;
			rcOut.top = nCY + 300;
			IPXC_Annotation annot = page.InsertNewAnnot(nText, ref rcOut);
			IPXC_AnnotData_FileAttachment aData = annot.Data as IPXC_AnnotData_FileAttachment;
			aData.Contents = "FileAttachment Annotation 1.";
			string sFilePath = System.Environment.CurrentDirectory + "\\Documents\\Hobbit.txt";
			IPXC_FileSpec fileSpec = Parent.m_CurDoc.CreateEmbeddFile(sFilePath);
			IPXC_EmbeddedFileStream EFS = fileSpec.EmbeddedFile;
			EFS.UpdateFromFile2(sFilePath);
			aData.FileAttachment = fileSpec;
			annot.Data = aData;

			return (int)Form1.eFormUpdateFlags.efuf_Annotations | (int)Form1.eFormUpdateFlags.efuf_Attachments;
		}

		[Description("13.3. Add attachment")]
		static public int AddAttachment(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IAFS_Inst afsInst = Parent.m_pxcInst.GetExtension("AFS");

			IPXC_NameTree attachments = Parent.m_CurDoc.GetNameTree("EmbeddedFiles");
			IPXS_PDFVariant var = null;
			string path = Environment.CurrentDirectory + "\\Documents\\FeatureChartEU.pdf";
			IPXC_FileSpec fileSpec = Parent.m_CurDoc.CreateEmbeddFile(path);
			IAFS_Name name = afsInst.DefaultFileSys.StringToName(path);
			FileInfo fileInfo = new FileInfo(path);

			IPXC_EmbeddedFileStream embeddedFileStream = fileSpec.EmbeddedFile;
			embeddedFileStream.CreationDate = fileInfo.CreationTime;
			embeddedFileStream.FileType = "pdf";
			embeddedFileStream.ModificationDate = fileInfo.LastWriteTime;
			//embeddedFileStream.co = fileInfo.Length;
			embeddedFileStream.UpdateFromFile2(path);
			var = embeddedFileStream.PDFObject;
			attachments.Add("FeatureChartEU.pdf", var);

			return (int)Form1.eFormUpdateFlags.efuf_Attachments;
		}

		[Description("13.4. Remove attachment")]
		static public int RemoveAttachment(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;

			if (Parent.AttachmentView.SelectedItems.Count == 0)
			{
				MessageBox.Show("Please, select attachment in list.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return 0;
			}

			Form1.ListItemAttachment currentAnnot = Parent.AttachmentView.SelectedItems[0] as Form1.ListItemAttachment;
			if (currentAnnot.SubItems[currentAnnot.SubItems.Count - 1].Text == "Embedded File Item")
			{
				IPXC_NameTree attachments = Parent.m_CurDoc.GetNameTree("EmbeddedFiles");
				attachments.Remove(currentAnnot.SubItems[0].Text);
				return (int)Form1.eFormUpdateFlags.efuf_Attachments;
			}

			Parent.m_CurDoc.Pages[(uint)(currentAnnot.m_nPageNumber)].RemoveAnnots((uint)currentAnnot.m_nIndexOnPage, 1);

			return (int)Form1.eFormUpdateFlags.efuf_Attachments;
		}
	}
}

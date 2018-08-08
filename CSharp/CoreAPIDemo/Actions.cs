using System;
using System.ComponentModel;
using System.Windows.Forms;
using PDFXCoreAPI;

namespace CoreAPIDemo
{
	[Description("12. Actions")]
	class Actions
	{
		[Description("12.1. Add GoTo action as a bookmark")]
		static public int AddActionsGoTo(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;
			IPXC_Bookmark bookmark = null;
			if (Parent.SelectedBookmarkNode == null)
				bookmark = Parent.m_CurDoc.BookmarkRoot.AddNewChild(true);
			else
				bookmark = Parent.SelectedBookmarkNode.m_Bookmark.AddNewSibling(false);
			IPXC_ActionsList aList = Parent.m_CurDoc.CreateActionsList();
			bookmark.Title = (Parent.CurrentPage + 1) + " page: GoTo";
			bookmark.Style = PXC_BookmarkStyle.BookmarkFont_Normal;
			PXC_Destination dest = new PXC_Destination();
			dest.nPageNum = Parent.CurrentPage;
			dest.nNullFlags = 12;
			dest.nType = PXC_DestType.Dest_XYZ;
			double[] point = { 20, 30, 0, 0 };
			dest.dValues = point;
			aList.AddGoto(dest);
			bookmark.Actions = aList;
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("12.2. Add GoToR action as a bookmark")]
		static public int AddActionsGoToR(Form1 Parent)
		{

			if (Parent.m_CurDoc == null)
				return 0;
			IPXC_Bookmark bookmark = null;
			if (Parent.SelectedBookmarkNode == null)
				bookmark = Parent.m_CurDoc.BookmarkRoot.AddNewChild(true);
			else
				bookmark = Parent.SelectedBookmarkNode.m_Bookmark.AddNewSibling(false);

			IPXC_ActionsList aList = Parent.m_CurDoc.CreateActionsList();
			bookmark.Title = (Parent.CurrentPage + 1) + " page: GoToR";
			bookmark.Style = PXC_BookmarkStyle.BookmarkFont_Normal;
			PXC_Destination dest = new PXC_Destination();
			dest.nPageNum = 2;
			dest.nNullFlags = 12;
			dest.nType = PXC_DestType.Dest_XYZ;
			double[] point = { 20, 30, 0, 0 };
			dest.dValues = point;
			string sFilePath = System.Environment.CurrentDirectory + "\\Documents\\FeatureChartEU.pdf";
			aList.AddGotoR(sFilePath, dest);
			bookmark.Actions = aList;
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("12.3. Add GoToE action as a bookmark")]
		static public int AddActionsGoToE(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;
			IPXC_Bookmark bookmark = null;
			if (Parent.SelectedBookmarkNode == null)
				bookmark = Parent.m_CurDoc.BookmarkRoot.AddNewChild(true);
			else
				bookmark = Parent.SelectedBookmarkNode.m_Bookmark.AddNewSibling(false);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			uint atomGoToE = pxsInst.StrToAtom("GoToE");
			IPXC_Action_Goto actionGoToE = Parent.m_pxcInst.GetActionHandler(atomGoToE).CreateEmptyAction(atomGoToE, Parent.m_CurDoc) as IPXC_Action_Goto;
			string sFilePath = System.Environment.CurrentDirectory + "\\Documents\\FeatureChartEU.pdf";
			IPXC_FileSpec fileSpec = Parent.m_CurDoc.CreateEmbeddFile(sFilePath);
			IPXC_EmbeddedFileStream EFS = fileSpec.EmbeddedFile;
			
			EFS.UpdateFromFile2(sFilePath);
			actionGoToE.Target = fileSpec;
			IPXC_GoToETargetPath targetPath = actionGoToE.TargetPath;
			IPXC_GoToETargetItem targetItem = targetPath.InsertNew();
			targetItem.AnnotIndex = 0;
			targetItem.PageNumber = 0;
			sFilePath = System.Environment.CurrentDirectory + "\\Documents\\MyStamps.pdf";
			targetItem.FileName = sFilePath;
			EFS.UpdateFromFile2(sFilePath);

			IPXC_ActionsList aList = Parent.m_CurDoc.CreateActionsList();
			bookmark.Title = (Parent.CurrentPage + 1) + " page: GoToE";
			bookmark.Style = PXC_BookmarkStyle.BookmarkFont_Normal;
			aList.Insert(0, actionGoToE);
			bookmark.Actions = aList;
			//EmbeddedFiles

			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("12.4. Add Launch action as a bookmark")]
		static public int AddActionLaunch(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;
			IPXC_Bookmark bookmark = null;
			if (Parent.SelectedBookmarkNode == null)
				bookmark = Parent.m_CurDoc.BookmarkRoot.AddNewChild(true);
			else
				bookmark = Parent.SelectedBookmarkNode.m_Bookmark.AddNewSibling(false);

			IPXC_ActionsList aList = Parent.m_CurDoc.CreateActionsList();
			bookmark.Title = (Parent.CurrentPage + 1) + " page: Launch";
			bookmark.Style = PXC_BookmarkStyle.BookmarkFont_Normal;
			string sFilePath = System.Environment.CurrentDirectory + "\\Documents\\FeatureChartEU.pdf";
			aList.AddLaunch(sFilePath);
			bookmark.Actions = aList;
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("12.5. Add URI action as a bookmark")]
		static public int AddActionURI(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;
			IPXC_Bookmark bookmark = null;
			if (Parent.SelectedBookmarkNode == null)
				bookmark = Parent.m_CurDoc.BookmarkRoot.AddNewChild(true);
			else
				bookmark = Parent.SelectedBookmarkNode.m_Bookmark.AddNewSibling(false);

			IPXC_ActionsList aList = Parent.m_CurDoc.CreateActionsList();
			bookmark.Title = (Parent.CurrentPage + 1) + " page: URI";
			bookmark.Style = PXC_BookmarkStyle.BookmarkFont_Normal;
			string sFilePath = "https://www.tracker-software.com";
			aList.AddURI(sFilePath);
			bookmark.Actions = aList;
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("12.6. Add JavaScript action as a bookmark")]
		static public int AddActionJS(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;
			IPXC_Bookmark bookmark = null;
			if (Parent.SelectedBookmarkNode == null)
				bookmark = Parent.m_CurDoc.BookmarkRoot.AddNewChild(true);
			else
				bookmark = Parent.SelectedBookmarkNode.m_Bookmark.AddNewSibling(false);

			IPXC_ActionsList aList = Parent.m_CurDoc.CreateActionsList();
			bookmark.Title = (Parent.CurrentPage + 1) + " page: Java Script";
			bookmark.Style = PXC_BookmarkStyle.BookmarkFont_Normal;
			string sJS = "app.alert(\"Hello world!\", 3);";
			aList.AddJavaScript(sJS);
			bookmark.Actions = aList;
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("12.7. Add Execute Command action as a bookmark")]
		static public int AddActionNamed(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;
			IPXC_Bookmark bookmark = null;
			if (Parent.SelectedBookmarkNode == null)
				bookmark = Parent.m_CurDoc.BookmarkRoot.AddNewChild(true);
			else
				bookmark = Parent.SelectedBookmarkNode.m_Bookmark.AddNewSibling(false);

			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			uint atomNamed = pxsInst.StrToAtom("Named");
			IPXC_Action_Named actionNamed = Parent.m_pxcInst.GetActionHandler(atomNamed).CreateEmptyAction(atomNamed, Parent.m_CurDoc) as IPXC_Action_Named;
			actionNamed.CmdName = "NextPage";
			IPXC_ActionsList aList = Parent.m_CurDoc.CreateActionsList();
			bookmark.Title = (Parent.CurrentPage + 1) + " page: Execute Command";
			bookmark.Style = PXC_BookmarkStyle.BookmarkFont_Normal;
			aList.Insert(0, actionNamed);
			bookmark.Actions = aList;

			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("12.8. Add Show/Hide action as a bookmark")]
		static public int AddActionHide(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("12.9. Add SubmitForm action as a bookmark")]
		static public int AddActionSubmitForm(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("12.10. Add ResetForm action as a bookmark")]
		static public int AddActionResetForm(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

	}
}
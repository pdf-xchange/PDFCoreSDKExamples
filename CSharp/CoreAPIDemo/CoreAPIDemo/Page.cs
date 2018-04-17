using System.ComponentModel;
using System.Windows.Forms;
using PDFXCoreAPI;

namespace CoreAPIDemo
{
	[Description("Page")]
	class Page
	{
		[Description("Insert Empty Pages Into The Beginning Of The Current Document")]
		static public void InsertEmptyPages(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.OpenDocFromStringPath(Parent);
			IPXC_Page firstPage = Parent.m_CurDoc.Pages[0];
			PXC_Rect rcMedia = firstPage.get_Box(PXC_BoxType.PBox_MediaBox);
			IPXC_UndoRedoData urd = null;
			//Adding pages with the size of the first page of the current document
			Parent.m_CurDoc.Pages.AddEmptyPages(0, 3, ref rcMedia, null, out urd);
		}

		[Description("Insert Page From Another Document Into The Beginning Of The Current Document")]
		static public void InsertPagesFromOtherDocument(Form1 Parent)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "PDF Documents (*.pdf)|*.pdf|All Files (*.*)|*.*";
			ofd.DefaultExt = "pdf";
			ofd.FilterIndex = 1;
			ofd.CheckPathExists = true;
			IPXC_Document srcDoc = null;
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				srcDoc = Parent.m_pxcInst.OpenDocumentFromFile(ofd.FileName, null);
			}
			if (srcDoc == null)
				return;
			if (Parent.m_CurDoc == null)
				Document.OpenDocFromStringPath(Parent);
			Parent.m_CurDoc.Pages.InsertPagesFromDoc(srcDoc, 0, 0, 1, (int)PXC_InsertPagesFlags.IPF_Annots_Copy | (int)PXC_InsertPagesFlags.IPF_Widgets_Copy);
		}

		[Description("Remove First Page From The Current Document")]
		static public void RemoveFirstPageFromTheDocument(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.OpenDocFromStringPath(Parent);
			IAUX_Inst auxInst = (IAUX_Inst)Parent.m_pxcInst.GetExtension("AUX");
			IBitSet bs = auxInst.CreateBitSet(1);
			bs.Set(0);
			IPXC_UndoRedoData urd = null;
			if (Parent.m_CurDoc.Pages.Count > 1)
				Parent.m_CurDoc.Pages.DeletePages(bs, null, out urd);
			else
				MessageBox.Show("The last page can't be removed from the document!");
		}

		[Description("Move First Page Of The Current Document Into The Last Page Position")]
		static public void MoveFirstPageToBack(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.OpenDocFromStringPath(Parent);
			IAUX_Inst auxInst = (IAUX_Inst)Parent.m_pxcInst.GetExtension("AUX");
			IBitSet bs = auxInst.CreateBitSet(1);
			bs.Set(0);
			IPXC_UndoRedoData urd = null;
			if (Parent.m_CurDoc.Pages.Count > 1)
				Parent.m_CurDoc.Pages.MovePages(bs, Parent.m_CurDoc.Pages.Count, null, out urd);
			else
				MessageBox.Show("Current document has one page - nothing to move!");
		}
	}
}

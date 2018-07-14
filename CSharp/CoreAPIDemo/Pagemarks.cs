using System.ComponentModel;
using System;
using PDFXCoreAPI;

namespace CoreAPIDemo
{
	[Description("7. Pagemarks")]
	class Pagemarks
	{
		[Description("7.1. Add Headers and Footers on pages")]
		static public void AddHeadersAndFootersOnPages(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			IPXC_HeaderAndFooterParams HeaderFooter = Parent.m_pxcInst.CreateHeaderAndFooterParams();
			IPXC_Font font = Parent.m_CurDoc.CreateNewFont("Arial", 0, 400);
			IColor color = auxInst.CreateColor(ColorType.ColorType_Gray);
			color.SetRGB(0, 0, 0);
			HeaderFooter.FillColor = color;
			HeaderFooter.Font = font;
			HeaderFooter.RightFooterText = "%[Page]";
			HeaderFooter.BottomMargin = 36.0f;
			HeaderFooter.TopMargin = 36.0f;
			HeaderFooter.RightMargin = 36.0f;
			HeaderFooter.LeftMargin = 36.0f;
			HeaderFooter.FontSize = 14.0f;
			IBitSet bitSet = auxInst.CreateBitSet(8);
			bitSet.SetSize(Parent.m_CurDoc.Pages.Count);
			bitSet.Set(0, Parent.m_CurDoc.Pages.Count, true);
			Parent.m_CurDoc.PlaceHeadersAndFooters(bitSet, HeaderFooter);
		}

		[Description("7.2. Add Watermarks on page")]
		static public void AddWatermarksOnPage(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			IPXC_WatermarkParams watermark = Parent.m_pxcInst.CreateWatermarkParams();
			IColor fillColor = auxInst.CreateColor(ColorType.ColorType_Gray);
			fillColor.SetRGB(0.7f, 0.7f, 0.7f);
			watermark.Text = "WATERMARK";
			watermark.HAlign = 50;
			watermark.VAlign = 50;
			watermark.FillColor = fillColor;
			watermark.Rotation = -45;
			watermark.FontSize = 200;
			watermark.StrokeWidth = 5.0f;
			IBitSet bitSet = auxInst.CreateBitSet(8);
			bitSet.SetSize(Parent.m_CurDoc.Pages.Count);
			bitSet.Set(0, Parent.m_CurDoc.Pages.Count, true);
			Parent.m_CurDoc.PlaceWatermark(bitSet, watermark);
		}

		[Description("7.3. Add Background on page")]
		static public void AddBackgroundOnPage(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			IPXC_BackgroundParams backgroundParams = Parent.m_pxcInst.CreateBackgroundParams();
			IColor fillColor = auxInst.CreateColor(ColorType.ColorType_Gray);
			fillColor.SetRGB(1.0f, 0.0f, 0.0f);
			IBitSet bitSet = auxInst.CreateBitSet(8);
			bitSet.SetSize(Parent.m_CurDoc.Pages.Count);
			bitSet.Set(0, Parent.m_CurDoc.Pages.Count, true);
			backgroundParams.FillColor = fillColor;

			Parent.m_CurDoc.PlaceBackgrounds(bitSet, backgroundParams);
		}

		[Description("7.4. Remove Headers and Footers from page")]
		static public void RemoveHeadersAndFooters(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return;
			
			//Get current page
			if (Parent.CurrentPage >= Parent.m_CurDoc.Pages.Count)
				return;
			
			IPXC_Page Page = Parent.m_CurDoc.Pages[Parent.CurrentPage];
			IPXC_DocumentProps docProps = Parent.m_CurDoc.Props;
			//docProps
			//Parent.m_CurDoc.
			//Remove all fields from page
			IPXC_Content content = Page.GetContent(PXC_ContentAccessMode.CAccessMode_FullClone);
			IPXC_ContentItems ContentItems = content.Items;
			ContentItems.DeleteItems(0);
#warning Remove all of the header and footer from page
		}

		[Description("7.5. Remove Watermarks from page")]
		static public void RemoveWatermarks(Form1 Parent)
		{
#warning Remove all of the watermarks from page
		}

		[Description("7.6. Remove Background from page")]
		static public void RemoveBackgroundOnPage(Form1 Parent)
		{
#warning Remove all of the background from page
		}
	}
}

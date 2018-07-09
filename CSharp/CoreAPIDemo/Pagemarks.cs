using System.ComponentModel;
using PDFXCoreAPI;
namespace CoreAPIDemo
{
	[Description("7. Pagemarks")]
	class Pagemarks
	{
		[Description("7.1. Add Headers and Footers on pages")]
		static public void AddHeadersAndFootersOnPages(Form1 Parent)
		{
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			IPXC_HeaderAndFooterParams HeaderFooter = Parent.m_pxcInst.CreateHeaderAndFooterParams();
			HeaderFooter.RightFooterText = "%[Page]";
			IBitSet bitSet = auxInst.CreateBitSet(8192);
			Parent.m_CurDoc.PlaceHeadersAndFooters(bitSet, HeaderFooter);
		}

		[Description("7.2. Add Watermarks on page")]
		static public void AddWatermarksOnPage(Form1 Parent)
		{
#warning Add one text watermark with custom styling and one image watermark on page
		}

		[Description("7.3. Add Background on page")]
		static public void AddBackgroundOnPage(Form1 Parent)
		{
#warning Add one background on the top of the page and one from file on the bottom of the page with different stylings
		}

		[Description("7.4. Remove Headers and Footers from page")]
		static public void RemoveHeadersAndFooters(Form1 Parent)
		{
#warning Remove all of the Headers and Footers on page
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

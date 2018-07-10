using System;
using System.IO;
using System.ComponentModel;
using PDFXCoreAPI;
using System.Diagnostics;
using System.Drawing;

namespace CoreAPIDemo
{
	[Description("8. Converters")]
	class Converters
	{
		[Description("8.1. Convert from PDF to image")]
		static public void ConvertToImage(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IIXC_Inst ixcInst = Parent.m_pxcInst.GetExtension("IXC");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			IPXC_Page Page = Parent.m_CurDoc.Pages[Parent.CurrentPage];
			double nHeight = 0.0;
			double nWidth = 0.0;
			Page.GetDimension(out nWidth, out nHeight);
			uint cx = (uint)(nWidth * 300 / 72.0);
			uint cy = (uint)(nHeight * 300 / 72.0);
			IIXC_Page ixcPage = ixcInst.Page_CreateEmpty(cx, cy, IXC_PageFormat.PageFormat_8ARGB, 0);
			IPXC_PageRenderParams param = Parent.m_pxcInst.CreateRenderParams();
			if (param != null)
			{
				param.RenderFlags |= ((uint)PXC_RenderFlags.RF_SmoothImages | (uint)PXC_RenderFlags.RF_SmoothLineArts);
				param.SetColor(PXC_RenderColor.RC_PageColor1, 0, 0, 0, 0);
				param.TextSmoothMode |= PXC_TextSmoothMode.TSM_Antialias;
			}
			tagRECT rc = new tagRECT();
			rc.right = (int)cx;
			rc.bottom = (int)cy;
			PXC_Matrix matrix = Page.GetMatrix(PXC_BoxType.PBox_PageBox);
			matrix = auxInst.MathHelper.Matrix_Scale(ref matrix, cx / nWidth, -cy / nHeight);
			matrix = auxInst.MathHelper.Matrix_Translate(ref matrix, 0, cy);
			Page.DrawToIXCPage(ixcPage, ref rc, ref matrix, param);
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_XDPI] = 300;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_YDPI] = 300;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_FORMAT] = (uint)IXC_ImageFileFormatIDs.FMT_PNG_ID;
			IIXC_Image ixcImg = ixcInst.CreateEmptyImage();
			ixcImg.InsertPage(ixcPage, 0);
			string sPath = Path.GetTempFileName();
			sPath = sPath.Replace(".tmp", ".png");
			ixcImg.Save(sPath, IXC_CreationDisposition.CreationDisposition_Overwrite);
			Process pr = Process.Start(sPath);

			ixcImg.RemovePageByIndex(0);
			ixcPage = ixcInst.Page_CreateEmpty(cx, cy, IXC_PageFormat.PageFormat_8Gray, 0);
			param = Parent.m_pxcInst.CreateRenderParams();
			if (param != null)
			{
				param.RenderFlags |= ((uint)PXC_RenderFlags.RF_OverrideBackgroundColor | (uint)PXC_RenderFlags.RF_SmoothImages | (uint)PXC_RenderFlags.RF_SmoothLineArts);
				param.TextSmoothMode |= PXC_TextSmoothMode.TSM_Antialias;
			}
			Page.DrawToIXCPage(ixcPage, ref rc, ref matrix, param);
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_XDPI] = 300;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_YDPI] = 300;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_FORMAT] = (uint)IXC_ImageFileFormatIDs.FMT_JPEG_ID;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_JPEG_QUALITY] = 100;
			ixcImg.InsertPage(ixcPage, 0);
			sPath = sPath.Replace(".png", ".jpg");
			ixcImg.Save(sPath, IXC_CreationDisposition.CreationDisposition_Overwrite);
			pr = Process.Start(sPath);

			ixcImg.RemovePageByIndex(0);
			ixcPage = ixcInst.Page_CreateEmpty(cx, cy, IXC_PageFormat.PageFormat_8RGB, 0);
			param = Parent.m_pxcInst.CreateRenderParams();
			if (param != null)
			{
				param.RenderFlags |= ((uint)PXC_RenderFlags.RF_OverrideBackgroundColor | (uint)PXC_RenderFlags.RF_SmoothImages | (uint)PXC_RenderFlags.RF_SmoothLineArts);
				param.TextSmoothMode |= PXC_TextSmoothMode.TSM_Antialias;
			}
			Page.DrawToIXCPage(ixcPage, ref rc, ref matrix, param);
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_XDPI] = 300;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_YDPI] = 300;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_BPC] = 1;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_BPP] = 1;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_DITHER] = 1;
			ixcPage.FmtInt[(uint)IXC_FormatFlags.FMTF_CAN_Encode] = 1;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_FORMAT] = (uint)IXC_ImageFileFormatIDs.FMT_TIFF_ID;
			ixcImg.InsertPage(ixcPage, 0);
			sPath = sPath.Replace(".jpg", ".tiff");
			ixcImg.Save(sPath, IXC_CreationDisposition.CreationDisposition_Overwrite);
			pr = Process.Start(sPath);

#warning Not implemented
		}

		[Description("8.2. Convert from image to PDF")]
		static public void ConvertToPDF(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
#warning Not implemented
		}

		[Description("8.3. Convert from PDF to txt file")]
		static public void ConvertToTXT(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
#warning Not implemented
		}
	}
}

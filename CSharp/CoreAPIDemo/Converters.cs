using System;
using System.IO;
using System.ComponentModel;
using PDFXCoreAPI;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;

namespace CoreAPIDemo
{
	[Description("8. Converters")]
	class Converters
	{
		[Description("8.1. Convert from PDF to image")]
		static public void ConvertToImage(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.OpenDocFromStringPath(Parent);

			IIXC_Inst ixcInst = Parent.m_pxcInst.GetExtension("IXC");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			IPXC_Page Page = Parent.m_CurDoc.Pages[Parent.CurrentPage];
			double nHeight = 0.0;
			double nWidth = 0.0;
			Page.GetDimension(out nWidth, out nHeight);
			uint cx = (uint)(nWidth * 150 / 72.0);
			uint cy = (uint)(nHeight * 150 / 72.0);
			IIXC_Page ixcPage = ixcInst.Page_CreateEmpty(cx, cy, IXC_PageFormat.PageFormat_8ARGB, 0);
			IPXC_PageRenderParams param = Parent.m_pxcInst.CreateRenderParams();
			if (param != null)
			{
				param.RenderFlags |= ((uint)PXC_RenderFlags.RF_SmoothImages | (uint)PXC_RenderFlags.RF_SmoothLineArts);
				param.SetColor(PXC_RenderColor.RC_PageColor1, 255, 255, 255, 0);
				param.TextSmoothMode |= PXC_TextSmoothMode.TSM_Antialias;
			}
			tagRECT rc = new tagRECT();
			rc.right = (int)cx;
			rc.bottom = (int)cy;
			PXC_Matrix matrix = Page.GetMatrix(PXC_BoxType.PBox_PageBox);
			matrix = auxInst.MathHelper.Matrix_Scale(ref matrix, cx / nWidth, -cy / nHeight);
			matrix = auxInst.MathHelper.Matrix_Translate(ref matrix, 0, cy);
			Page.DrawToIXCPage(ixcPage, ref rc, ref matrix, param);
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_XDPI] = 150;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_YDPI] = 150;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_INTERLACE] = 1;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_FILTER] = 5;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_COMP_LEVEL] = 5;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_FORMAT] = (uint)IXC_ImageFileFormatIDs.FMT_PNG_ID;
			IIXC_Image ixcImg = ixcInst.CreateEmptyImage();
			ixcImg.InsertPage(ixcPage, 0);
			string sPath = Path.GetTempFileName();
			sPath = sPath.Replace(".tmp", ".png");
			ixcImg.Save(sPath, IXC_CreationDisposition.CreationDisposition_Overwrite);
			Process.Start(sPath);

			ixcImg.RemovePageByIndex(0);
			ixcPage = ixcInst.Page_CreateEmpty(cx, cy, IXC_PageFormat.PageFormat_8Gray, 0);
			if (param != null)
				param.SetColor(PXC_RenderColor.RC_PageColor1, 255, 255, 255, 255);
			Page.DrawToIXCPage(ixcPage, ref rc, ref matrix, param);
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_XDPI] = 150;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_YDPI] = 150;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_YSUBSAMPLING] = 20;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_FORMAT] = (uint)IXC_ImageFileFormatIDs.FMT_JPEG_ID;
			ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_JPEG_QUALITY] = 100;
			ixcImg.InsertPage(ixcPage, 0);
			sPath = sPath.Replace(".png", ".jpg");
			ixcImg.Save(sPath, IXC_CreationDisposition.CreationDisposition_Overwrite);
			Process.Start(sPath);

			ixcImg.RemovePageByIndex(0);
			ixcPage = ixcInst.Page_CreateEmpty(cx, cy, IXC_PageFormat.PageFormat_8RGB, 0);
			for (int i = 0; i < Parent.m_CurDoc.Pages.Count; i++)
			{
				Page = Parent.m_CurDoc.Pages[(uint)i];
				Page.DrawToIXCPage(ixcPage, ref rc, ref matrix, param);
				ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_XDPI] = 150;
				ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_YDPI] = 150;
				ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_ITYPE] = 1;
				ixcPage.FmtInt[(uint)IXC_FormatParametersIDS.FP_ID_FORMAT] = (uint)IXC_ImageFileFormatIDs.FMT_TIFF_ID;
				ixcImg.InsertPage(ixcPage);
				ixcPage = ixcInst.Page_CreateEmpty(cx, cy, IXC_PageFormat.PageFormat_8Gray, 0);
			}
			sPath = sPath.Replace(".jpg", ".tiff");
			ixcImg.Save(sPath, IXC_CreationDisposition.CreationDisposition_Overwrite);
			Process.Start(sPath);
		}

		[Description("8.2. Convert from image to PDF")]
		static public void ConvertToPDF(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IIXC_Inst ixcInst = Parent.m_pxcInst.GetExtension("IXC");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			IPXC_Page Page = Parent.m_CurDoc.Pages[0];
			double nHeight = 0.0;
			double nWidth = 0.0;
			Page.GetDimension(out nWidth, out nHeight);
			IIXC_Image img = ixcInst.CreateEmptyImage();
			img.Load(System.Environment.CurrentDirectory + "\\Images\\Editor_welcome.png");
			IIXC_Page ixcPage = img.GetPage(0);
			IPXC_Image pxcImg = Parent.m_CurDoc.AddImageFromIXCPage(ixcPage);
			IPXC_ContentCreator CC = Parent.m_CurDoc.CreateContentCreator();
			PXC_Rect rcImg = new PXC_Rect();

			CC.SaveState();
			{
				//Proportional resize rectangle calculation
				{
					double k1 = nWidth / nHeight;
					double k2 = (double)pxcImg.Width / pxcImg.Height;
					if (k1 >= k2)
					{
						rcImg.top = nHeight;
						rcImg.right = nWidth / 2.0 + rcImg.top * k2 / 2.0;
						rcImg.left = nWidth / 2.0 - rcImg.top * k2 / 2.0;
					}
					else
					{
						rcImg.right = nWidth;
						rcImg.top = nHeight / 2.0 + rcImg.right / k2 / 2.0;
						rcImg.bottom = nHeight / 2.0 - rcImg.right / k2 / 2.0;
					}
				}
				//Moving the image rectangle to the center

				PXC_Rect rcImage = new PXC_Rect();
				rcImage.right = 1;
				rcImage.top = 1;
				PXC_Matrix matrix = Page.GetMatrix(PXC_BoxType.PBox_PageBox);
				matrix = auxInst.MathHelper.Matrix_RectToRect(ref rcImage, ref rcImg);
				CC.ConcatCS(ref matrix);
				CC.PlaceImage(pxcImg);
			}
			CC.RestoreState();
			Page.PlaceContent(CC.Detach());
		}

		[Description("8.3. Convert from PDF to txt file")]
		static public void ConvertToTXT(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.OpenDocFromStringPath(Parent);

			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			IPXC_Page Page = Parent.m_CurDoc.Pages[Parent.CurrentPage];
			IPXC_PageText Text = Page.GetText(null, false);

			string writePath = Path.GetTempFileName();
			writePath = writePath.Replace(".tmp", ".txt");
			StreamWriter stream = new StreamWriter(writePath);

			List<PXC_TextLineInfo> textsLineInfo = new List<PXC_TextLineInfo>();

			for (int i = 0; i < Text.LinesCount; i++)
			{
				PXC_TextLineInfo pxcTLI = Text.LineInfo[(uint)i];
				textsLineInfo.Add(pxcTLI);
			}

			textsLineInfo.Sort(delegate (PXC_TextLineInfo firstTLI, PXC_TextLineInfo secondTLI)
			{
				PXC_RectF rcFirst = firstTLI.rcBBox;
				PXC_RectF rcSecond = secondTLI.rcBBox;
				auxInst.MathHelper.Rect_TransformDF(ref firstTLI.Matrix, ref rcFirst);
				auxInst.MathHelper.Rect_TransformDF(ref secondTLI.Matrix, ref rcSecond);
				if (rcFirst.top < rcSecond.top)
					return 1;
				if (rcFirst.top > rcSecond.top)
					return -1;

				if (rcFirst.left < rcSecond.left)
					return -1;
				if (rcFirst.left > rcSecond.left)
					return 1;

				return 0;
			});

			for (int i = 0; i < Text.LinesCount; i++)
			{
				stream.Write(Text.GetChars(textsLineInfo[i].nFirstCharIndex, textsLineInfo[i].nCharsCount));
				if (i < Text.LinesCount - 1)
					stream.Write((textsLineInfo[i].rcBBox.top == textsLineInfo[i + 1].rcBBox.top) ? " ": "\r\n");
			}
			stream.Close();
			Process.Start(writePath);
		}
	}
}

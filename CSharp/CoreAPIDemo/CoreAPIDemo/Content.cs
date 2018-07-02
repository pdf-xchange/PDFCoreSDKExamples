using System.ComponentModel;
using System;
using System.Windows.Forms;
using PDFXCoreAPI;
using System.Runtime.InteropServices;

namespace CoreAPIDemo
{
	[Description("Content")]
	public class Content
	{

		delegate void DrawTitle(IPXC_Document Doc, IPXC_ContentCreator ContCrt, double cx, double baseLineY, string sText, double fontSize, uint argbFillColor = 0x00000000);
		delegate IPXC_Pattern CreateImagePattern(string str, IPXC_Document Doc, IIXC_Inst g_ImgCore);
		delegate void DrawArrLine(IPXC_ContentCreator CC, double xfrom, double yfrom, double xto, double yto, double linewidth, bool bDashed);
		delegate void DrawCS(IPXC_ContentCreator CC, double x0, double y0, double w, double h);
		delegate void DrawN(IPXC_ContentCreator CC, double cx, double baseLineY);
		delegate void FillByGradient(IPXC_Document Doc, IPXC_ContentCreator CC, PXC_Rect rect);
		delegate void CrossArrLine(IPXC_Document Doc, IPXC_ContentCreator CC, PXC_Point p);

		[Description("Add Text with different Text Rendering Mode to the current document")]
		static public void DrawTextRenderingModesOnPage(Form1 Parent)
		{
			const uint argbDarkLime = 0x00008888;
			const uint argbBlack = 0x00000000;
			//delegate void DrawTitle(IPXC_Document Doc, IPXC_ContentCreator ContCrt, double cx, double baseLineY, string sText, double fontSize);
			DrawTitle drawTitle = (Doc, ContCrt, cx, baseLineY, sText, fontSize, color) =>
			{
				IPXC_Font defFont = Doc.CreateNewFont("Arial", 0, 400);
				ContCrt.SaveState();
				ContCrt.SetFillColorRGB(color);
				ContCrt.SetFont(defFont);
				double Width = 0;
				double Height = 0;
				ContCrt.CalcTextSize(fontSize, sText, out Width, out Height, -1);
				ContCrt.SetFontSize(fontSize);
				ContCrt.ShowTextLine(cx - Width / 2.0, baseLineY, sText, -1, (uint)PXC_ShowTextLineFlags.STLF_Default | (uint)PXC_ShowTextLineFlags.STLF_AllowSubstitution);
				ContCrt.RestoreState();
			};
			//delegate IPXC_Pattern CreateImagePattern(string str, IPXC_Document Doc, IIXC_Inst g_ImgCore);
			CreateImagePattern crtImgPat = (str, doc, ImgCore) =>
			{
				IPXC_Pattern Patt = null;
				IPXC_Image Img = doc.AddImageFromFile(str);
				PXC_Rect bbox;
				bbox.left = 0;
				bbox.bottom = 0;
				bbox.right = Img.Width * 72.0 / 96.0;
				bbox.top = Img.Height * 72.0 / 96.0;
				IPXC_ContentCreator ContCrt = doc.CreateContentCreator();

				PXC_Matrix im = new PXC_Matrix();
				im.a = bbox.right;
				im.b = 0;
				im.c = 0;
				im.d = bbox.top;
				im.e = 0;
				im.f = 0;
				ContCrt.SaveState();
				ContCrt.ConcatCS(im);
				ContCrt.PlaceImage(Img);
				ContCrt.RestoreState();
				Patt = doc.CreateTilePattern(ref bbox);
				IPXC_Content content = ContCrt.Detach();
				content.set_BBox(ref bbox);
				Patt.SetContent(content, (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
				return Patt;
			};
			//delegate void FillByGradient(IPXC_Document Doc, IPXC_ContentCreator CC, PXC_Rect rect);
			FillByGradient fillByGradient = (Doc, ConCret, rect) =>
			{
				IPXC_GradientStops Stops;
				Stops = Doc.CreateShadeStops();

				Stops.AddStopRGB(0.0, 0x00ff0000);
				Stops.AddStopRGB(1.0, 0x0000ffff);

				IPXC_Shading Shade;
				PXC_Point p0, p1;
				p0.x = rect.left; p0.y = rect.top;
				p1.x = rect.left; p1.y = rect.bottom;

				Shade = Doc.CreateLinearShade(ref p0, ref p1, Stops, 3);

				ConCret.SaveState();
				ConCret.SetShadeAsPattern(Shade, true);
				ConCret.SetStrokeColorRGB(argbBlack);
				ConCret.Rect(rect.left, rect.bottom, rect.right, rect.top);
				ConCret.FillPath(true, false, PXC_FillRule.FillRule_Winding);
				ConCret.RestoreState();
			};

			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
			PXC_Rect rc;
			rc.left = 0;
			rc.right = 600;
			rc.top = 800;
			rc.bottom = 0;

			IPXC_UndoRedoData urData;
			IPXC_ContentCreator CC = Parent.m_CurDoc.CreateContentCreator();
			IPXC_Page Page = Parent.m_CurDoc.Pages.InsertPage(0, ref rc, out urData);

			double x = 1 * 72.0;
			double y = rc.top - 1 * 72.0;
			double xs = 3.5 * 72.0;
			double ys = 1 * 72.0;
			double ts = 0.45 * 72.0;

			IPXC_Font Font;
			IPXC_Font Font2;
			Font = Parent.m_CurDoc.CreateNewFont("Impact", 0, 0);
			double fntsize = 25.0;
			string text = "Text Rendering Mode";
			double nWidth;
			double nHight;

			CC.SaveState();
			CC.SetFont(Font);
			CC.SetFontSize(fntsize);
			CC.CalcTextSize(fntsize, text, out nWidth, out nHight, -1);

			CC.SetFillColorRGB(argbDarkLime);
			CC.SaveState();
			CC.SetTextRenderMode(PXC_TextRenderingMode.TRM_Fill);
			CC.ShowTextLine(x, y, text, -1, 0);
			CC.RestoreState();
			drawTitle(Parent.m_CurDoc, CC, x + nWidth / 2, y - ts, "TRM_Fill", 15);

			x += xs;
			CC.SaveState();
			CC.SetTextRenderMode(PXC_TextRenderingMode.TRM_Stroke);
			CC.ShowTextLine(x, y, text, -1, 0);
			CC.RestoreState();
			drawTitle(Parent.m_CurDoc, CC, x + nWidth / 2, y - ts, "TRM_Stroke", 15);

			x -= xs; y -= ys;
			CC.SaveState();
			CC.SetTextRenderMode(PXC_TextRenderingMode.TRM_FillStroke);
			CC.ShowTextLine(x, y, text, -1, 0);
			CC.RestoreState();
			drawTitle(Parent.m_CurDoc, CC, x + nWidth / 2, y - ts, "TRM_FillStroke", 15);

			x += xs;
			CC.SaveState();
			CC.SetTextRenderMode(PXC_TextRenderingMode.TRM_None);
			CC.ShowTextLine(x, y, text, -1, 0);
			CC.RestoreState();
			drawTitle(Parent.m_CurDoc, CC, x + nWidth / 2, y - ts, "TRM_None", 15);

			x -= xs;
			y -= ys;
			double w = 3.2 * 72.0;
			double h = 1 * 72.0;
			//
			text = "ABC";
			Font2 = Parent.m_CurDoc.CreateNewFont("Arial Black", 0, 0);
			fntsize = 50;
			CC.SetFontSize(fntsize);
			CC.SetFont(Font2);

			PXC_Rect pr;
			pr.left = x;
			pr.right = pr.left + w;
			pr.top = y;
			pr.bottom = pr.top - h;

			fillByGradient(Parent.m_CurDoc, CC, pr);

			CC.SaveState();
			CC.SetCharSpace(2.0);
			CC.SetTextScale(150.0);
			CC.SetTextRenderMode(PXC_TextRenderingMode.TRM_Stroke);
			CC.ShowTextLine(x + 0.35 * 72.0, y, text, -1, 0);
			CC.RestoreState();
			drawTitle(Parent.m_CurDoc, CC, x + w / 2, y - 1.1 * 72.0, "TRM_Stroke", 15);

			x += xs;
			pr.left += xs;
			pr.right += xs;

			CC.SaveState();
			CC.SetCharSpace(2.0);
			CC.SetTextScale(150.0);
			CC.SetTextRenderMode(PXC_TextRenderingMode.TRM_Clip_Stroke);
			CC.ShowTextLine(x + 0.35 * 72.0, y, text, -1, 0);
			fillByGradient(Parent.m_CurDoc, CC, pr);
			CC.RestoreState();
			drawTitle(Parent.m_CurDoc, CC, x + w / 2, y - 1.1 * 72.0, "TRM_Clip_Stroke", 15);

			text = "FILL";
			CC.SetFont(Font);
			CC.SetFontSize(120);


			x = rc.right / 4; y -= 1.4 * 72.0;
			pr.left += xs; pr.right += xs;
			CC.SaveState();
			CC.SetFillColorRGB(argbDarkLime);
			CC.SetCharSpace(2.0);
			CC.SetTextScale(150.0);
			CC.SetTextRenderMode(PXC_TextRenderingMode.TRM_FillStroke);
			CC.ShowTextLine(x + 0.35 * 72.0, y, text, -1, 0);
			CC.RestoreState();
			drawTitle(Parent.m_CurDoc, CC, rc.right / 2 + 15, y - 1.8 * 72.0, "SOLID COLOR", 15);

			x = rc.right / 4; y -= 1.8 * 72.0;
			pr.left += xs; pr.right += xs;
			CC.SaveState();
			IPXC_Pattern Pat = Parent.m_CurDoc.GetStdTilePattern((PXC_StdPatternType)4);
			CC.SetPatternRGB(Pat, true, argbDarkLime);

			CC.SetCharSpace(2.0);
			CC.SetTextScale(150.0);
			CC.SetTextRenderMode(PXC_TextRenderingMode.TRM_FillStroke);
			CC.ShowTextLine(x + 0.35 * 72.0, y, text, -1, 0);
			CC.RestoreState();
			drawTitle(Parent.m_CurDoc, CC, rc.right / 2 + 15, y - 1.8 * 72.0, "HatchType_Horizontal PATTERN", 15);


			x = rc.right / 4;
			y -= 1.8 * 72.0;
			pr.left += xs;
			pr.right += xs;

			CC.SaveState();
			IIXC_Inst Ixc_Inst = (IIXC_Inst)Parent.m_pxcInst.GetExtension("IXC");
			Pat = crtImgPat(System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\Images\\CoreAPI_32.ico", Parent.m_CurDoc, Ixc_Inst);
			CC.SetPatternRGB(Pat, true, argbDarkLime);
			CC.SetCharSpace(2.0);
			CC.SetTextScale(150.0);
			CC.SetTextRenderMode(PXC_TextRenderingMode.TRM_FillStroke);
			CC.ShowTextLine(x + 0.35 * 72.0, y, text, -1, 0);
			Pat = null;
			CC.RestoreState();
			drawTitle(Parent.m_CurDoc, CC, rc.right / 2 + 15, y - 1.8 * 72.0, "IMAGE PATTERN FOR STROKE", 15);

			Page.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
		}

		[Description("Add Text with different Char and Word Spacing to the current document")]
		static public void DrawTextWithDifferentSpacingOnPage(Form1 Parent)
		{
			const uint argbDarkLime = 0x00008888;
			//delegate void DrawTitle(IPXC_Document Doc, IPXC_ContentCreator ContCrt, double cx, double baseLineY, string sText, double fontSize);
			DrawTitle drawTitle = (Doc, ContCrt, cx, baseLineY, sText, fontSize, color) =>
			{
				IPXC_Font defFont = Doc.CreateNewFont("Arial", 0, 1000);
				ContCrt.SaveState();
				ContCrt.SetFillColorRGB(color);
				ContCrt.SetFont(defFont);
				double Width = 0;
				double Height = 0;
				ContCrt.CalcTextSize(fontSize, sText, out Width, out Height, -1);
				ContCrt.SetFontSize(fontSize);
				ContCrt.ShowTextLine(cx - Width / 2.0, baseLineY, sText, -1, (uint)PXC_ShowTextLineFlags.STLF_Default | (uint)PXC_ShowTextLineFlags.STLF_AllowSubstitution);
				ContCrt.RestoreState();
			};

			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
			PXC_Rect rc;
			rc.left = 0;
			rc.right = 600;
			rc.top = 800;
			rc.bottom = 0;

			IPXC_UndoRedoData urData;
			IPXC_ContentCreator CC = Parent.m_CurDoc.CreateContentCreator();
			IPXC_Page Page = Parent.m_CurDoc.Pages.InsertPage(0, ref rc, out urData);

			double x = 1.8 * 72.0;
			double y = rc.top - 0.5 * 72.0;
			double xs = 2.5 * 72.0;
			double ys = 0.85 * 72.0;

			string[] txts = new string[]
				{
					"-2 PT",
					"DEFAULT",
					"2 PT",
					"-10 PT",
					"DEFAULT",
					"10 PT"
				};
			for (int i = 0; i < 6; i++)
			{
				drawTitle(Parent.m_CurDoc, CC, x, y - i * ys - 5, txts[i], 15);
			}

			CC.SaveState();
			CC.SetTextRenderMode(PXC_TextRenderingMode.TRM_FillStroke);

			CC.SetFillColorRGB(argbDarkLime);
			string text = "Character Spacing";
			CC.SetCharSpace(-2.0);
			drawTitle(Parent.m_CurDoc, CC, x + xs, y + 2, text, 25, argbDarkLime);

			y -= ys;
			CC.SetCharSpace(0);
			drawTitle(Parent.m_CurDoc, CC, x + xs, y + 2, text, 25, argbDarkLime);

			y -= ys;
			CC.SetCharSpace(2.0);
			drawTitle(Parent.m_CurDoc, CC, x + xs, y + 2, text, 25, argbDarkLime);
			CC.SetCharSpace(0);

			y -= ys;
			text = "Word Spacing";
			CC.SetWordSpace(-10);
			drawTitle(Parent.m_CurDoc, CC, x + xs - 25, y + 2, text, 25, argbDarkLime);

			y -= ys;
			CC.SetWordSpace(0);
			drawTitle(Parent.m_CurDoc, CC, x + xs - 25, y + 2, text, 25, argbDarkLime);

			y -= ys;
			CC.SetWordSpace(10);
			drawTitle(Parent.m_CurDoc, CC, x + xs - 25, y + 2, text, 25, argbDarkLime);
			CC.SetWordSpace(0);

			CC.RestoreState();

			Page.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
		}

		[Description("Add Text with different Scaling, Subscript and Superscript to the current document")]
		static public void DrawTextWithScaleSubSuperscriptOnPage(Form1 Parent)
		{
			const uint argbDarkLime = 0x00008888;
			//delegate void DrawTitle(IPXC_Document Doc, IPXC_ContentCreator ContCrt, double cx, double baseLineY, string sText, double fontSize);
			DrawTitle drawTitle = (Doc, ContCrt, cx, baseLineY, sText, fontSize, color) =>
			{
				IPXC_Font defFont = Doc.CreateNewFont("Arial", 0, 1000);
				ContCrt.SaveState();
				ContCrt.SetFillColorRGB(color);
				ContCrt.SetFont(defFont);
				double Width = 0;
				double Height = 0;
				ContCrt.CalcTextSize(fontSize, sText, out Width, out Height, -1);
				ContCrt.SetFontSize(fontSize);
				ContCrt.ShowTextLine(cx - Width / 2.0, baseLineY, sText, -1, (uint)PXC_ShowTextLineFlags.STLF_Default | (uint)PXC_ShowTextLineFlags.STLF_AllowSubstitution);
				ContCrt.RestoreState();
			};

			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
			PXC_Rect rc;
			rc.left = 0;
			rc.right = 600;
			rc.top = 800;
			rc.bottom = 0;

			IPXC_UndoRedoData urData;
			IPXC_ContentCreator CC = Parent.m_CurDoc.CreateContentCreator();
			IPXC_Page Page = Parent.m_CurDoc.Pages.InsertPage(0, ref rc, out urData);

			double x = 1.8 * 72.0;
			double y = rc.top - 0.5 * 72.0;
			double xs = 2.5 * 72.0;
			double ys = 0.85 * 72.0;


			string[] txts = new string[]
				{
					"80%",
					"DEFAULT (100%)",
					"120%",
					"+10 PT",
					"-10 PT",
					"±10 PT"
				};

			for (int i = 0; i < 6; i++)
			{
				drawTitle(Parent.m_CurDoc, CC, x, y - i * ys - 5, txts[i], 15);
			}

			CC.SaveState();
			CC.SetTextRenderMode(PXC_TextRenderingMode.TRM_FillStroke);
			CC.SetFillColorRGB(argbDarkLime);
			string text = "Horizontal Scaling";
			CC.SetTextScale(80);
			drawTitle(Parent.m_CurDoc, CC, x + xs, y + 2, text, 25, argbDarkLime);

			y -= ys;
			CC.SetTextScale(100);
			drawTitle(Parent.m_CurDoc, CC, x + xs, y + 2, text, 25, argbDarkLime);

			y -= ys;
			CC.SetTextScale(120);
			drawTitle(Parent.m_CurDoc, CC, x + xs, y + 2, text, 25, argbDarkLime);
			CC.SetTextScale(100);

			y -= ys;
			text = "This text is ";
			drawTitle(Parent.m_CurDoc, CC, x + xs - 40, y + 2, text, 25, argbDarkLime);
			CC.SetTextRise(10.0);
			drawTitle(Parent.m_CurDoc, CC, x + xs + 110, y + 2, "superscripted", 25, argbDarkLime);

			CC.SetTextRise(0.0);
			y -= ys;
			drawTitle(Parent.m_CurDoc, CC, x + xs - 40, y + 2, text, 25, argbDarkLime);
			CC.SetTextRise(-10);
			drawTitle(Parent.m_CurDoc, CC, x + xs + 100, y + 2, "subscripted", 25, argbDarkLime);

			CC.SetTextRise(0.0);
			y -= ys;
			drawTitle(Parent.m_CurDoc, CC, x + xs - 82, y + 2, "This", 25, argbDarkLime);
			CC.SetTextRise(-10.0);
			drawTitle(Parent.m_CurDoc, CC, x + xs - 28, y + 2, "text", 25, argbDarkLime);
			CC.SetTextRise(10.0);
			drawTitle(Parent.m_CurDoc, CC, x + xs + 41, y + 2, "moves", 25, argbDarkLime);
			CC.SetTextRise(0.0);
			drawTitle(Parent.m_CurDoc, CC, x + xs + 128, y + 2, "around", 25, argbDarkLime);

			CC.RestoreState();

			Page.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
		}

		[Description("Add Arcs with different Styles to the current document")]
		static public void DrawArcsOnPage(Form1 Parent)
		{
			const uint argbBlack = 0x00000000;
			//delegate void DrawTitle(IPXC_Document Doc, IPXC_ContentCreator ContCrt, double cx, double baseLineY, string sText, double fontSize);
			DrawTitle drawTitle = (Doc, ContCrt, cx, baseLineY, sText, fontSize, color) => 
			{
				IPXC_Font defFont = Doc.CreateNewFont("Arial", 0, 400);
				ContCrt.SaveState();
				ContCrt.SetFillColorRGB(color);
				ContCrt.SetFont(defFont);
				double nWidth = 0;
				double nHeight = 0;
				ContCrt.CalcTextSize(fontSize, sText, out nWidth, out nHeight, -1);
				ContCrt.SetFontSize(fontSize);
				ContCrt.ShowTextLine(cx - nWidth / 2.0, baseLineY, sText, -1, (uint)PXC_ShowTextLineFlags.STLF_Default | (uint)PXC_ShowTextLineFlags.STLF_AllowSubstitution);
				ContCrt.RestoreState();
			};

			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
			PXC_Rect rc;
			rc.left = 0;
			rc.right = 600;
			rc.top = 800;
			rc.bottom = 0;

			IPXC_UndoRedoData urData;
			IPXC_ContentCreator CC = Parent.m_CurDoc.CreateContentCreator();
			IAUX_Inst auxInst = (IAUX_Inst)Parent.m_pxcInst.GetExtension("AUX");
			IPXC_Page firstPage = Parent.m_CurDoc.Pages.InsertPage(0, ref rc, out urData);
			double rx = 1 * 72.0;
			double ry = 1 * 72.0;
			double X = 2.3 * 72.0;
			double Y = rc.top - 2.5 * 72.0;

			CC.SetLineWidth(0.5);
			CC.SetStrokeColorRGB(argbBlack);
			IColor clr = auxInst.CreateColor(ColorType.ColorType_Gray);
			clr.SetGray(0.94f);
			CC.SetColor(clr, null);

			PXC_Rect rect;
			rect.left = X - rx;
			rect.top = Y + ry;
			rect.right = X + rx;
			rect.bottom = Y - ry;

			CC.Ellipse(rect.left, rect.bottom, rect.right, rect.top);
			CC.FillPath(false, true, PXC_FillRule.FillRule_Winding);
			drawTitle(Parent.m_CurDoc, CC, (rect.left + rect.right) / 2.0, rect.bottom - 0.2 * 72.0, "ELLIPSE", 15);

			rect.left += 4 * 72.0;
			rect.right += 4 * 72.0;
			drawTitle(Parent.m_CurDoc, CC, (rect.left + rect.right) / 2.0, rect.bottom - 0.2 * 72.0, "ARC", 15);
			CC.Arc(rect, 0, 270, true);
			CC.StrokePath(false);

			rect.left -= 4 * 72.0;
			rect.right -= 4 * 72.0;
			rect.top -= 3 * 72.0;
			rect.bottom -= 3 * 72.0;

			drawTitle(Parent.m_CurDoc, CC, (rect.left + rect.right) / 2.0, rect.bottom - 0.2 * 72.0, "CLOSED ARC", 15);
			CC.SetStrokeColorRGB(argbBlack);
			CC.Arc(rect, 0, 270.0, true);
			CC.FillPath(true, true, PXC_FillRule.FillRule_Winding);

			rect.left += 4 * 72.0;
			rect.right += 4 * 72.0;

			drawTitle(Parent.m_CurDoc, CC, (rect.left + rect.right) / 2.0, rect.bottom - 0.2 * 72.0, "PIE", 15);
			CC.Pie(rect, 0, 270.0);
			CC.FillPath(true, true, PXC_FillRule.FillRule_Winding);

			rect.left -= 4 * 72.0;
			rect.right -= 4 * 72.0;
			rect.top -= 3 * 72.0;
			rect.bottom -= 3 * 72.0;

			drawTitle(Parent.m_CurDoc, CC, (rect.left + rect.right) / 2.0, rect.bottom - 0.2 * 72.0, "CHORD", 15);
			CC.SetStrokeColorRGB((uint)(200 << 0) + (200 << 8) + (200 << 16));
			CC.SetDash(3, 3, 0);
			CC.Arc(rect, 0, 270.0, true);
			CC.StrokePath(false);

			CC.NoDash();
			CC.SetStrokeColorRGB(argbBlack);
			CC.Chord(rect, 0, 270.0);
			CC.StrokePath(false);

			rect.left += 4 * 72.0;
			rect.right += 4 * 72.0;

			PXC_Point pnt;
			pnt.x = (rect.left + rect.right) / 2;
			pnt.y = (rect.top + rect.bottom) / 2;
			drawTitle(Parent.m_CurDoc, CC, (rect.left + rect.right) / 2.0, rect.bottom - 0.2 * 72.0, "CIRCLE", 15);
			CC.Circle(pnt.x, pnt.y, ry);
			CC.FillPath(false, true, PXC_FillRule.FillRule_Winding);

			firstPage.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);


			IPXC_Page secondPage = Parent.m_CurDoc.Pages.InsertPage(0, ref rc, out urData);
			PXC_Point center;
			double r0 = 1.5 * 72.0;
			double r = r0;
			double dr = 6.0;
			int i = 0;

			center.x = 2.3 * 72.0;
			center.y = rc.top - 2.5 * 72.0;

			drawTitle(Parent.m_CurDoc, CC, center.x, center.y - r - 18.0, "FILLED, STROKE", 15);
			CC.SetLineWidth(0.5);
			CC.SetStrokeColorRGB(argbBlack);

			for (i = 360; i > 0; i -= 30)
			{
				CC.MoveTo(center.x, center.y);
				CC.CircleArc(center.x, center.y, r, 0.0, i, false);
				uint c = (uint)Math.Round(i * 240.0 / 360.0);
				CC.SetFillColorRGB(((c << 0) + (c << 8) + (255 << 16)));
				CC.FillPath(true, true, PXC_FillRule.FillRule_Winding);
				r -= dr;
			}

			r = r0;
			center.x += 4 * 72.0;

			drawTitle(Parent.m_CurDoc, CC, center.x, center.y - r - 18.0, "FILLED, NO STROKE", 15);
			for (i = 360; i > 0; i -= 30)
			{
				CC.MoveTo(center.x, center.y);
				CC.CircleArc(center.x, center.y, r, 0.0, i, false);
				uint c = (uint)Math.Round(i * 240.0 / 360.0);
				CC.SetFillColorRGB(((c << 0) + (c << 8) + (255 << 16)));
				CC.FillPath(true, false, PXC_FillRule.FillRule_Winding);
				r -= dr;
			}

			r = r0;
			center.x -= 4 * 72.0;
			center.y -= 4.5 * 72.0;

			drawTitle(Parent.m_CurDoc, CC, center.x, center.y - r - 18.0, "STROKE, NOT CLOSED PATH", 15);

			CC.SetStrokeColorRGB(argbBlack);
			for (i = 360; i > 0; i -= 30)
			{
				CC.CircleArc(center.x, center.y, r, 0, i, false);
				CC.StrokePath(false);
				r -= dr;
			}

			r = r0;
			center.x += 4 * 72.0;

			drawTitle(Parent.m_CurDoc, CC, center.x, center.y - r - 18.0, "STROKE, CLOSED PATH", 15);

			for (i = 360; i > 0; i -= 30)
			{
				CC.MoveTo(center.x, center.y);
				CC.CircleArc(center.x, center.y, r, 0, i, false);
				CC.StrokePath(true);
				r -= dr;
			}

			secondPage.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
			Parent.UpdateControlsFromDocument();
			Parent.UpdatePreviewFromCurrentDocument();
		}

		[Description("Add Polygons and Curves with different Styles to the current document")]
		static public void DrawPolygonsAndCurvesOnPage(Form1 Parent)
		{
			const uint argbBlack = 0x00000000;
			const uint argbDarkLime = 0x00008888;
			//delegate void DrawTitle(IPXC_Document Doc, IPXC_ContentCreator ContCrt, double cx, double baseLineY, string sText, double fontSize);
			DrawTitle drawTitle = (Doc, ContCrt, cx, baseLineY, sText, fontSize, color) =>
			{
				IPXC_Font defFont = Doc.CreateNewFont("Arial", 0, 400);
				ContCrt.SaveState();
				ContCrt.SetFillColorRGB(color);
				ContCrt.SetFont(defFont);
				double nWidth = 0;
				double nHeight = 0;
				ContCrt.CalcTextSize(fontSize, sText, out nWidth, out nHeight, -1);
				ContCrt.SetFontSize(fontSize);
				ContCrt.ShowTextLine(cx - nWidth / 2.0, baseLineY, sText, -1, (uint)PXC_ShowTextLineFlags.STLF_Default | (uint)PXC_ShowTextLineFlags.STLF_AllowSubstitution);
				ContCrt.RestoreState();
			};


			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			PXC_Rect rc;
			rc.left = 0;
			rc.right = 600;
			rc.top = 800;
			rc.bottom = 0;

			IPXC_UndoRedoData urData;
			IPXC_ContentCreator CC = Parent.m_CurDoc.CreateContentCreator();
			IPXC_Page Page = Parent.m_CurDoc.Pages.InsertPage(0, ref rc, out urData);

			double x = 2.2 * 72.0;
			double y = rc.top - 2.5 * 72.0;
			double r = 1.2 * 72.0;

			double b = 1.3333333;
			double br = b * r;
			double[] p = new double[12];
			p[0] = p[8] = p[10] = x - r;
			p[1] = p[3] = y + br;
			p[2] = p[4] = p[6] = x + r;
			p[5] = p[11] = y;
			p[7] = p[9] = y - br;

			drawTitle(Parent.m_CurDoc, CC, x, y - r - 0.1 * 72.0, "FAST-BEZIER \"CIRCLE\"", 15);

			CC.SetLineWidth(1.0);
			CC.SetStrokeColorRGB(argbBlack);
			CC.MoveTo(x - r, y);
			CC.PolyCurveSA(p, false);
			CC.StrokePath(false);

			x = 6.2 * 72.0;
			y = rc.top - 2.5 * 72.0;
			double rr = r / 2.0;
			double a = 0.05 * 72.0;

			PXC_Point center;
			center.x = x;
			center.y = y;
			PXC_Point p1 = center;
			PXC_Point p2 = center;

			p1.y -= rr;
			p2.y += rr;

			drawTitle(Parent.m_CurDoc, CC, x, y - r - 0.1 * 72.0, "CHINA MONAD", 15);

			CC.SetLineWidth(1.0);
			CC.SetColorRGB(argbBlack);

			CC.CircleArc(center.x, center.y, r, 90.0, -90.0, true);
			CC.CircleArc(p1.x, p1.y, rr, 270.0, 90.0, true);
			CC.CircleArc(p2.x, p2.y, rr, -90.0, 90.0, true);
			CC.Circle(p1.x, p1.y, 0.1 * 72.0);
			CC.Circle(p2.x, p2.y, 0.1 * 72.0);
			CC.FillPath(false, true, PXC_FillRule.FillRule_EvenOdd);
			CC.CircleArc(center.x, center.y, r, 90.0, 270.0, true);
			CC.StrokePath(false);

			const int ncnt = 8;
			x = 2.2 * 72.0;
			y = rc.top - 6 * 72.0;
			r = 1.3 * 72.0;
			double[] xy = new double[ncnt * 2];

			drawTitle(Parent.m_CurDoc, CC, x, y - r - 0.1 * 72.0, "POLYGON", 15);

			a = -90;
			for (int i = 0; i < ncnt; i++)
			{
				xy[i * 2] = x + r * Math.Cos(a * Math.PI / 180.0);
				xy[i * 2 + 1] = y - r * Math.Sin(a * Math.PI / 180.0);
				a += 360.0 / ncnt;
			}
			CC.PolygonSA(xy, true);
			CC.SetFillColorRGB(argbDarkLime);
			CC.FillPath(true, true, PXC_FillRule.FillRule_Winding);

			double xorig = 6.2 * 72.0;
			double yorig = rc.top - 6 * 72.0;
			r = 1.2 * 72.0;

			drawTitle(Parent.m_CurDoc, CC, rc.right - x, y - r - 0.2 * 72.0, "LISSAJOUS FIGURE", 15);

			CC.SetFillColorRGB(argbDarkLime);
			CC.SetStrokeColorRGB(argbBlack);
			for (int i = 0; i < 200; i++)
			{
				double ang = Math.PI * i / 100.0;
				x = xorig + r * Math.Cos(3 * ang);
				y = yorig - r * Math.Sin(5 * ang);
				if (i > 0)
					CC.LineTo(x, y);
				else
					CC.MoveTo(x, y);
			}
			CC.FillPath(true, true, PXC_FillRule.FillRule_Winding);

			x = 2.2 * 72.0;
			y = rc.top - 9 * 72.0;
			double w = 2.5 * 72.0;
			double h = 1.2 * 72.0;

			drawTitle(Parent.m_CurDoc, CC, x, y - r - 0.1 * 72.0, "RECTANGLE", 15);

			CC.SetFillColorRGB(argbDarkLime);
			CC.SetStrokeColorRGB(argbBlack);
			CC.Rect(x - w / 2, y - h / 2, x + w / 2, y + h / 2);
			CC.FillPath(true, true, PXC_FillRule.FillRule_Winding);

			x = 6.2 * 72.0;
			y = rc.top - 9 * 72.0;
			w = 2.5 * 72.0;
			h = 1.2 * 72.0;
			double ew = w / 5.0;
			double eh = h / 5.0;

			PXC_Rect rect;
			rect.left = x - w / 2;
			rect.top = y + h / 2;
			rect.right = x + w / 2;
			rect.bottom = y - h / 2;

			drawTitle(Parent.m_CurDoc, CC, x, y - r - 0.1 * 72.0, "ROUND RECTANGLE", 15);

			CC.SetFillColorRGB(argbDarkLime);
			CC.SetStrokeColorRGB(argbBlack);
			CC.RoundRect(rect.left, rect.bottom, rect.right, rect.top, ew, eh);
			CC.FillPath(true, true, PXC_FillRule.FillRule_Winding);
			CC.SetStrokeColorRGB(((255 << 0) + (255 << 8) + (255 << 16)));
			CC.SetLineWidth(0.0);
			CC.SetDash(1, 1, 0);
			rect.right = rect.left + ew;
			rect.bottom = rect.top - eh;
			CC.Ellipse(rect.left, rect.bottom, rect.right, rect.top);
			CC.StrokePath(false);

			Page.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
		}

		[Description("Add Fillings with different Styles to the current document")]
		static public void DrawFillingsOnPage(Form1 Parent)
		{
			const uint argbBlack = 0x00000000;
			const uint argbDarkLime = 0x00008888;
			//delegate IPXC_Pattern CreateImagePattern(string str, IPXC_Document Doc, IIXC_Inst g_ImgCore);
			CreateImagePattern crtImgPat = (str, doc, ImgCore) =>
			{
				IPXC_Pattern Patt = null;
				IPXC_Image Img = doc.AddImageFromFile(str);
				PXC_Rect bbox;
				bbox.left = 0;
				bbox.bottom = 0;
				bbox.right = Img.Width * 72.0 / 96.0;
				bbox.top = Img.Height * 72.0 / 96.0;
				IPXC_ContentCreator ContCrt = doc.CreateContentCreator();

				PXC_Matrix im = new PXC_Matrix();
				im.a = bbox.right;
				im.b = 0;
				im.c = 0;
				im.d = bbox.top;
				im.e = 0;
				im.f = 0;
				ContCrt.SaveState();
				ContCrt.ConcatCS(im);
				ContCrt.PlaceImage(Img);
				ContCrt.RestoreState();
				Patt = doc.CreateTilePattern(ref bbox);
				IPXC_Content content = ContCrt.Detach();
				content.set_BBox(ref bbox);
				Patt.SetContent(content, (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
				return Patt;
			};
			//delegate void DrawTitle(IPXC_Document Doc, IPXC_ContentCreator ContCrt, double cx, double baseLineY, string sText, double fontSize);
			DrawTitle drawTitle = (Doc, ContCrt, cx, baseLineY, sText, fontSize, color) =>
			{
				IPXC_Font defFont = Doc.CreateNewFont("Arial", 0, 400);
				ContCrt.SaveState();
				ContCrt.SetFillColorRGB(color);
				ContCrt.SetFont(defFont);
				double nWidth = 0;
				double nHeight = 0;
				ContCrt.CalcTextSize(fontSize, sText, out nWidth, out nHeight, -1);
				ContCrt.SetFontSize(fontSize);
				ContCrt.ShowTextLine(cx - nWidth / 2.0, baseLineY, sText, -1, (uint)PXC_ShowTextLineFlags.STLF_Default | (uint)PXC_ShowTextLineFlags.STLF_AllowSubstitution);
				ContCrt.RestoreState();
			};
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			PXC_Rect rc;
			rc.left = 0;
			rc.right = 600;
			rc.top = 800;
			rc.bottom = 0;

			IPXC_UndoRedoData urData;
			IPXC_ContentCreator CC = Parent.m_CurDoc.CreateContentCreator();
			IPXC_Page firstPage = Parent.m_CurDoc.Pages.InsertPage(0, ref rc, out urData);

			double x = 2.0 * 72.0;
			double y = rc.top - 2.0 * 72.0;
			double r = 1.0 * 72.0;
			double rr;

			string[] RuleTitles = { "NONZERO WINDING NUMBER RULE", "EVEN-ODD RULE" };
			PXC_FillRule[] rules = { PXC_FillRule.FillRule_Winding, PXC_FillRule.FillRule_EvenOdd };

			for (int i = 0; i < 2; i++)
			{
				x = 2.0 * 72.0;

				PXC_FillRule rule = rules[i];
				drawTitle(Parent.m_CurDoc, CC, (rc.right + rc.left) / 2, y - r - 15, RuleTitles[i], 15);

				const int num = 5;
				double[] points = new double[num * 2];

				double a = -90;
				for (int j = 0; j < num; j++)
				{
					points[j * 2] = x + r * Math.Cos(a * Math.PI / 180.0);
					points[j * 2 + 1] = y - r * Math.Sin(a * Math.PI / 180.0);
					a += 2.0 * (360 / num);
				}
				CC.PolygonSA(points, true);

				CC.SetStrokeColorRGB(argbBlack);
				CC.SetFillColorRGB(argbDarkLime);
				CC.FillPath(true, true, rule);

				x = (rc.right + rc.left) / 2;
				rr = r;
				PXC_Rect ps = new PXC_Rect();

				ps.left = x - rr;
				ps.bottom = y - rr;
				ps.right = x + rr;
				ps.top = y + rr;

				CC.Arc(ps, 0.0, 360.0, true);
				rr = r / 2;

				ps.left = x - rr;
				ps.bottom = y - rr;
				ps.right = x + rr;
				ps.top = y + rr;

				CC.Arc(ps, 0.0, 360.0, true);
				CC.FillPath(true, true, rule);

				x = rc.right - 2.0 * 72.0;

				ps.left = x - rr;
				ps.bottom = y - rr;
				ps.right = x + rr;
				ps.top = y + rr;

				CC.Arc(ps, 0.0, 360.0, true);
				rr = r / 2;

				ps.left = x - rr;
				ps.bottom = y - rr;
				ps.right = x + rr;
				ps.top = y + rr;

				CC.Arc(ps, 0.0, 360.0, true);
				CC.FillPath(true, true, rule);

				y -= 3.0 * 72.0;
			}
			firstPage.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
			IPXC_Page secondPage = Parent.m_CurDoc.Pages.InsertPage(0, ref rc, out urData);

			double w = (rc.right - rc.left - 3 * 72.0) / 2.0;
			double h = 1 * 72.0;
			y = rc.top - 1.0 * 72.0 - h;
			x = rc.left + 1.0 * 72.0;

			IPXC_GradientStops Stops = Parent.m_CurDoc.CreateShadeStops();

			Stops.AddStopRGB(0.0, 0x4bb5fd);
			Stops.AddStopRGB(0.5, 0x66b9ff);
			Stops.AddStopRGB(0.5, 0xffd900);
			Stops.AddStopRGB(1.0, 0xfff500);

			PXC_Point p0, p1;
			p0.x = x;
			p0.y = y + h;
			p1.x = x;
			p1.y = y;

			IPXC_Shading Shade = Parent.m_CurDoc.CreateLinearShade(p0, p1, Stops, 3);

			CC.SaveState();
			CC.Rect(x, y, x + w, y + h);
			CC.ClipPath(PXC_FillRule.FillRule_Winding, true);
			CC.Shade(Shade);
			CC.RestoreState();
			drawTitle(Parent.m_CurDoc, CC, x + w / 2, y - 0.1 * 72.0, "Linear Gradient", 15);

			x = rc.left + 5 * 72.0 + w / 2;
			r = h / 2;
			Shade = null;
			Stops.Reset();
			p0.x = x;
			p0.y = y + r;
			p1.x = p0.x - 0.5 * r;
			p1.y = p0.y + 0.5 * r;
			Stops.AddStopRGB(0.0, 0x5c5c5c);
			Stops.AddStopRGB(1.0, 0xf2f2f2);
			Shade = Parent.m_CurDoc.CreateRadialShade(p0, p1, h / 2, 0.0, Stops, 0);
			CC.Shade(Shade);
			drawTitle(Parent.m_CurDoc, CC, x, y - 0.1 * 72.0, "Radial Gradient", 15);

			secondPage.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);

			IPXC_Page thirdPage = Parent.m_CurDoc.Pages.InsertPage(0, ref rc, out urData);
			double[,] TRIVERTEXxy = new double[4,2];
			uint[] TRIVERTEXcolor = new uint[4];
			uint[,] GRADIENT_TRIANGLE = new uint[3, 3];

			rc.left = 72.0;
			rc.top = 800 - 72.0;
			rc.right = 600 - 72.0;
			rc.bottom = 800 - 3.5 * 72.0;
			h = rc.bottom - rc.top;

			drawTitle(Parent.m_CurDoc, CC, 600 / 2, rc.bottom - 0.2 * 72.0, "GRADIENT FILL (GRADIENT_FILL_RECT_H)", 15);
			IPXC_GradientStops stops;
			stops = Parent.m_CurDoc.CreateShadeStops();

			stops.AddStopRGB(0.0, 0x00000000);
			stops.AddStopRGB(0.33, 0x000000ff);
			stops.AddStopRGB(0.66, 0x0000ff00);
			stops.AddStopRGB(1.0, 0x00ff0000);

			IPXC_Shading shade;
			PXC_Point point0, point1;
			point0.x = rc.left;
			point0.y = rc.bottom;
			point1.x = rc.right;
			point1.y = rc.bottom;
			shade = Parent.m_CurDoc.CreateLinearShade(ref point0, ref point1, stops, 6);

			double[] xy = new double[16];
			xy[0] = rc.left;
			xy[1] = rc.top;
			xy[2] = rc.left + (rc.right - rc.left) / 3;
			xy[3] = rc.top;
			xy[4] = rc.left + (rc.right - rc.left) / 3;
			xy[5] = rc.top - (rc.top - rc.bottom) / 8;
			xy[6] = rc.right;
			xy[7] = rc.top - (rc.top - rc.bottom) / 8;
			xy[8] = rc.right;
			xy[9] = rc.bottom;
			xy[10] = rc.left + ((rc.right - rc.left) / 3) * 2;
			xy[11] = rc.bottom;
			xy[12] = rc.left + ((rc.right - rc.left) / 3) * 2;
			xy[13] = rc.bottom + (rc.top - rc.bottom) / 8;
			xy[14] = rc.left;
			xy[15] = rc.bottom + (rc.top - rc.bottom) / 8;

			CC.SaveState();
			CC.SetShadeAsPattern(shade, true);
			CC.PolygonSA(xy, true);
			CC.SetStrokeColorRGB(argbBlack);
			CC.FillPath(true, false, PXC_FillRule.FillRule_Winding);
			CC.RestoreState();

			string text = "PDF-XCHANGE";
			IPXC_Font Font = Parent.m_CurDoc.CreateNewFont("Impact", 0, 0);
			CC.SetFont(Font);
			CC.SetFontSize(120);

			stops = Parent.m_CurDoc.CreateShadeStops();

			stops.AddStopRGB(0.0, 0x00000000);
			stops.AddStopRGB(0.33, 0x000000ff);
			stops.AddStopRGB(0.66, 0x0000ff00);
			stops.AddStopRGB(1.0, 0x00ff0000);

			rc.top = 6.94 * 72.0;
			rc.bottom = 5.63 * 72.0;

			point0.x = rc.left;
			point0.y = rc.top;
			point1.x = rc.left;
			point1.y = rc.bottom;

			shade = Parent.m_CurDoc.CreateLinearShade(ref point0, ref point1, stops, 3);

			CC.SaveState();
			CC.SetShadeAsPattern(shade, true);
			CC.SetTextScale(70.0);
			CC.SetTextRenderMode(PXC_TextRenderingMode.TRM_Fill);
			CC.ShowTextLine(72, y - 1.8 * 72.0, text, -1, 0);
			CC.RestoreState();
			drawTitle(Parent.m_CurDoc, CC, 600 / 2, y - 3.8 * 72.0, "GRADIENT FILL (GRADIENT_FILL_RECT_V)", 15);

			thirdPage.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
		}

		[Description("Add Patterns with different Styles to the current document")]
		static public void DrawPatternsOnPage(Form1 Parent)
		{
			const uint argbBlack = 0x00000000;
			const uint argbDarkLime = 0x00008888;
			//delegate IPXC_Pattern CreateImagePattern(string str, IPXC_Document Doc, IIXC_Inst g_ImgCore);
			CreateImagePattern crtImgPat = (str, doc, ImgCore) =>
			{
				IPXC_Pattern Patt = null;
				IPXC_Image Img = doc.AddImageFromFile(str);
				PXC_Rect bbox;
				bbox.left = 0;
				bbox.bottom = 0;
				bbox.right = Img.Width * 72.0 / 96.0;
				bbox.top = Img.Height * 72.0 / 96.0;
				IPXC_ContentCreator ContCrt = doc.CreateContentCreator();

				PXC_Matrix im = new PXC_Matrix();
				im.a = bbox.right;
				im.b = 0;
				im.c = 0;
				im.d = bbox.top;
				im.e = 0;
				im.f = 0;
				ContCrt.SaveState();
				ContCrt.ConcatCS(im);
				ContCrt.PlaceImage(Img);
				ContCrt.RestoreState();
				Patt = doc.CreateTilePattern(ref bbox);
				IPXC_Content content = ContCrt.Detach();
				content.set_BBox(ref bbox);
				Patt.SetContent(content, (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
				return Patt;
			};
			//delegate void DrawTitle(IPXC_Document Doc, IPXC_ContentCreator ContCrt, double cx, double baseLineY, string sText, double fontSize);
			DrawTitle drawTitle = (Doc, ContCrt, cx, baseLineY, sText, fontSize, color) =>
			{
				IPXC_Font defFont = Doc.CreateNewFont("Arial", 0, 400);
				ContCrt.SaveState();
				ContCrt.SetFillColorRGB(color);
				ContCrt.SetFont(defFont);
				double nWidth = 0;
				double nHeight = 0;
				ContCrt.CalcTextSize(fontSize, sText, out nWidth, out nHeight, -1);
				ContCrt.SetFontSize(fontSize);
				ContCrt.ShowTextLine(cx - nWidth / 2.0, baseLineY, sText, -1, (uint)PXC_ShowTextLineFlags.STLF_Default | (uint)PXC_ShowTextLineFlags.STLF_AllowSubstitution);
				ContCrt.RestoreState();
			};

			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			PXC_Rect rc;
			rc.left = 0;
			rc.right = 600;
			rc.top = 800;
			rc.bottom = 0;

			IPXC_UndoRedoData urData;
			IPXC_ContentCreator CC = Parent.m_CurDoc.CreateContentCreator();
			IIXC_Inst Ixc_Inst = (IIXC_Inst)Parent.m_pxcInst.GetExtension("IXC");
			IPXC_Page Page = Parent.m_CurDoc.Pages.InsertPage(0, ref rc, out urData);

			double w = (rc.right - rc.left - 3 * 72.0) / 2.0;
			double h = 1 * 72.0;
			double Y = rc.top - 1 * 72.0 - h;
			double dy = 2 * 72.0;
			double[] X = { rc.left + 1.0 * 72.0, rc.left + 5 * 72.0 };

			CC.SetLineWidth(1.0);
			CC.SetStrokeColorRGB(argbBlack);
			CC.SetFillColorRGB(argbDarkLime);

			CC.Rect(X[0], Y, X[0] + w, Y + h);
			CC.StrokePath(false);
			drawTitle(Parent.m_CurDoc, CC, X[0] + w / 2, Y - 0.1 * 72.0, "STROKE, NO FILL", 15);

			CC.Rect(X[1], Y, X[1] + w, Y + h);
			CC.FillPath(false, false, PXC_FillRule.FillRule_Winding);
			drawTitle(Parent.m_CurDoc, CC, X[1] + w / 2, Y - 0.1 * 72.0, "FILL, NO STROKE", 15);

			Y -= dy;

			CC.Rect(X[0], Y, X[0] + w, Y + h);
			CC.FillPath(false, true, PXC_FillRule.FillRule_Winding);
			drawTitle(Parent.m_CurDoc, CC, X[0] + w / 2, Y - 0.1 * 72.0, "STROKE & FILL", 15);
			string[] PatternTitles =
			{
				"PATTERN FILL: CrossHatch",
				"PATTERN FILL: CrossDiagonal",
				"PATTERN FILL: DiagonalLeft",
				"PATTERN FILL: DiagonalRight",
				"PATTERN FILL: Horizontal",
				"PATTERN FILL: Vertical"
			};

			int k = 1;
			IPXC_Pattern Pat;
			for (int i = (int)PXC_StdPatternType.StdPattern_CrossHatch; i <= (int)PXC_StdPatternType.StdPattern_Vertical; i++)
			{
				Pat = Parent.m_CurDoc.GetStdTilePattern((PXC_StdPatternType)i);
				CC.SetPatternRGB(Pat, true, argbDarkLime);
				Pat = null;
				CC.Rect(X[k], Y, X[k] + w, Y + h);
				CC.FillPath(false, true, PXC_FillRule.FillRule_Winding);
				drawTitle(Parent.m_CurDoc, CC, X[k] + w / 2, Y - 0.1 * 72.0, PatternTitles[i], 15);
				k ^= 1;
				if (k == 0)
					Y -= dy;
			}
			Pat = crtImgPat(System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\Images\\CoreAPI_32.ico", Parent.m_CurDoc, Ixc_Inst);
			CC.SetPatternRGB(Pat, true, argbDarkLime);
			CC.Rect(X[k], Y, X[k] + w, Y + h);
			CC.FillPath(false, true, PXC_FillRule.FillRule_Winding);
			drawTitle(Parent.m_CurDoc, CC, X[1] + w / 2, Y - 0.1 * 72.0, "PATTERN FILL: Image", 15);

			Page.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
		}

		[Description("Add content with different Coordinate System Transformations (matrix usages)")]
		static public void AddCoordinateSystemTransformations(Form1 Parent)
		{
			uint argbBlack = 0x00000000;
			//delegate void CrossArrLine(IPXC_Document Doc, IPXC_ContentCreator CC, PXC_Point p);
			CrossArrLine crossArrLine = (Doc, ContCrt, point) =>
			{
				ContCrt.SaveState();
				ContCrt.SetLineWidth(1.0);
				ContCrt.SetStrokeColorRGB(argbBlack);
				ContCrt.NoDash();
				ContCrt.MoveTo(point.x - 0.74 * 72.0, point.y);
				ContCrt.LineTo(point.x + 0.74 * 72.0, point.y);

				ContCrt.MoveTo(point.x + 0.74 * 72.0, point.y);
				double[] pts = new double[6];
				double a = 0;
				for (int j = 0; j < 3; j++)
				{
					double xx = 2 * Math.Cos(a * Math.PI / 180.0);
					double yy = 2 * Math.Sin(a * Math.PI / 180.0);
					pts[j * 2 + 0] = point.x + 0.7 * 72.0 + xx;
					pts[j * 2 + 1] = point.y - yy;
					a += 120;
				}
				ContCrt.SetLineJoin(PXC_LineJoin.LineJoin_Miter);
				ContCrt.PolygonSA(pts, true);
				ContCrt.StrokePath(true);

				ContCrt.MoveTo(point.x, point.y + 0.74 * 72.0);
				ContCrt.LineTo(point.x, point.y - 0.74 * 72.0);

				ContCrt.SetFillColorRGB(argbBlack);
				ContCrt.Circle(point.x, point.y, 1);

				ContCrt.StrokePath(true);
				
				ContCrt.RestoreState();
			};


			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			PXC_Rect rc;
			rc.left = 0;
			rc.right = 600;
			rc.top = 800;
			rc.bottom = 0;

			IPXC_UndoRedoData urData;
			IPXC_ContentCreator CC = Parent.m_CurDoc.CreateContentCreator();
			IPXC_Page firstPage = Parent.m_CurDoc.Pages.InsertPage(0, ref rc, out urData);
			IAUX_Inst Aux_Inst = Parent.m_pxcInst.GetExtension("AUX");
			IMathHelper math = Aux_Inst.MathHelper;

			PXC_Point p;
			p.x = 2 * 72.0;
			p.y = rc.top - 3.5 * 72.0;
			crossArrLine(Parent.m_CurDoc, CC, p);


			firstPage.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);

#warning Implement this method
		}
	}
}

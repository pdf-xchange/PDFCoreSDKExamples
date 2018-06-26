using System.ComponentModel;
using System;
using System.Windows.Forms;
using PDFXCoreAPI;

namespace CoreAPIDemo
{
	[Description("Content")]
	public class Content
	{
		[Description("Add Text with different Text Rendering Mode to the current document")]
		static public void DrawTextRenderingModesOnPage(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

		}

		[Description("Add Text with different Char and Word Spacing to the current document")]
		static public void DrawTextWithDifferentSpacingOnPage(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
		}

		[Description("Add Text with different Scaling, Subscript and Superscript to the current document")]
		static public void DrawTextWithScaleSubSuperscriptOnPage(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
		}

		[Description("Add Arcs with different Styles to the current document")]
		static public void DrawArcsOnPage(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			PXC_Rect rc;
			rc.left = 0;
			rc.right = 600;
			rc.top = 800;
			rc.bottom = 0;

			IPXC_UndoRedoData urData;
			IPXC_Page page1 = Parent.m_CurDoc.Pages.InsertPage(0, ref rc, out urData);
			IPXC_ContentCreator CC = Parent.m_CurDoc.CreateContentCreator();

			PXC_Point center;
			double r0 = 1.5 * 72.0;
			double r = r0;
			double dr = 6.0;
			long i;

			center.x = 2.3 * 72.0;
			center.y = rc.top - 2.5 * 72.0;

			Parent.DrawTitle(Parent.m_CurDoc, CC, center.x, center.y - r - 18.0, "FILLED, STROKE", 15);
			CC.SetLineWidth(0.5);
			CC.SetStrokeColorRGB(0x00000000);

			for (i = 360; i > 0; i -= 30)
			{
				CC.MoveTo(center.x, center.y);
				CC.CircleArc(center.x, center.y, r, 0.0, i, false);
				ulong c = (ulong)Math.Round((double)i * 240 / 360);
				CC.SetFillColorRGB((uint)((c << 0) + (c << 8) + (255 << 16)));
				CC.FillPath(true, true, PXC_FillRule.FillRule_Winding);
				r -= dr;
			}

			r = r0;
			center.x += 4 * 72.0;

			Parent.DrawTitle(Parent.m_CurDoc, CC, center.x, center.y - r - 18.0, "FILLED, NO STROKE", 15);
			for (i = 360; i > 0; i -= 30)
			{
				CC.MoveTo(center.x, center.y);
				CC.CircleArc(center.x, center.y, r, 0.0, (double)i, false);
				ulong c = (ulong)Math.Round((double)i * 240 / 360);
				CC.SetFillColorRGB((uint)((c << 0) + (c << 8) + (255 << 16)));
				CC.FillPath(true, false, PXC_FillRule.FillRule_Winding);
				r -= dr;
			}

			r = r0;
			center.x -= 4 * 72.0;
			center.y -= 4.5 * 72.0;

			Parent.DrawTitle(Parent.m_CurDoc, CC, center.x, center.y - r - 18.0, "STROKE, NOT CLOSED PATH", 15);

			CC.SetStrokeColorRGB(0x00000000);
			for (i = 360; i > 0; i -= 30)
			{
				CC.CircleArc(center.x, center.y, r, 0, i, false);
				CC.StrokePath(false);
				r -= dr;
			}

			r = r0;
			center.x += 4 * 72.0;

			Parent.DrawTitle(Parent.m_CurDoc, CC, center.x, center.y - r - 18.0, "STROKE, CLOSED PATH", 15);

			for (i = 360; i > 0; i -= 30)
			{
				CC.MoveTo(center.x, center.y);
				CC.CircleArc(center.x, center.y, r, 0, i, false);
				CC.StrokePath(true);
				r -= dr;
			}

			page1.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
			IAUX_Inst auxInst = (IAUX_Inst)Parent.m_pxcInst.GetExtension("AUX");
			IPXC_Page page2 = Parent.m_CurDoc.Pages.InsertPage(0, ref rc, out urData);

			double rx = 1 * 72.0;
			double ry = 1 * 72.0;
			double X = 2.3 * 72.0;
			double Y = rc.top - 2.5 * 72.0;

			CC.SetLineWidth(0.5);
			CC.SetStrokeColorRGB(0x00000000);
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
			Parent.DrawTitle(Parent.m_CurDoc, CC, (rect.left + rect.right) / 2.0, rect.bottom - 0.2 * 72.0, "ELLIPSE", 15);

			rect.left += 4 * 72.0;
			rect.right += 4 * 72.0;
			Parent.DrawTitle(Parent.m_CurDoc, CC, (rect.left + rect.right) / 2.0, rect.bottom - 0.2 * 72.0, "ARC", 15);
			CC.Arc(rect, 0, 270, true);
			CC.StrokePath(false);

			rect.left -= 4 * 72.0; rect.right -= 4 * 72.0;
			rect.top -= 3 * 72.0; rect.bottom -= 3 * 72.0;
			Parent.DrawTitle(Parent.m_CurDoc, CC, (rect.left + rect.right) / 2.0, rect.bottom - 0.2 * 72.0, "CLOSED ARC", 15);
			CC.SetStrokeColorRGB(0x00000000);
			CC.Arc(rect, 0, 270.0, true);
			CC.FillPath(true, true, PXC_FillRule.FillRule_Winding);

			rect.left += 4 * 72.0; rect.right += 4 * 72.0;
			Parent.DrawTitle(Parent.m_CurDoc, CC, (rect.left + rect.right) / 2.0, rect.bottom - 0.2 * 72.0, "PIE", 15);
			CC.Pie(rect, 0, 270.0);
			CC.FillPath(true, true, PXC_FillRule.FillRule_Winding);

			rect.left -= 4 * 72.0; rect.right -= 4 * 72.0;
			rect.top -= 3 * 72.0; rect.bottom -= 3 * 72.0;
			Parent.DrawTitle(Parent.m_CurDoc, CC, (rect.left + rect.right) / 2.0, rect.bottom - 0.2 * 72.0, "CHORD", 15);
			CC.SetStrokeColorRGB((uint)(200 << 0) + (200 << 8) + (200 << 16));
			CC.SetDash(3, 3, 0);
			CC.Arc(rect, 0, 270.0, true);
			CC.StrokePath(false);

			CC.NoDash();
			CC.SetStrokeColorRGB(0x00000000);
			CC.Chord(rect, 0, 270.0);
			CC.StrokePath(false);

			rect.left += 4 * 72.0; rect.right += 4 * 72.0;
			PXC_Point pnt;
			pnt.x = (rect.left + rect.right) / 2;
			pnt.y = (rect.top + rect.bottom) / 2;
			Parent.DrawTitle(Parent.m_CurDoc, CC, (rect.left + rect.right) / 2.0, rect.bottom - 0.2 * 72.0, "CIRCLE", 15);
			CC.Circle(pnt.x, pnt.y, ry);
			CC.FillPath(false, true, PXC_FillRule.FillRule_Winding);

			page2.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
			Parent.UpdateControlsFromDocument();
			Parent.UpdatePreviewFromCurrentDocument();
		}

		[Description("Add Polygons and Curves with different Styles to the current document")]
		static public void DrawPolygonsAndCurvesOnPage(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
		}

		[Description("Add Fillings with different Styles to the current document")]
		static public void DrawFillingsOnPage(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
		}

		[Description("Add Patterns with different Styles to the current document")]
		static public void DrawPatternsOnPage(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
		}

		[Description("Add content with different Coordinate System Transformations (matrix usages)")]
		static public void AddCoordinateSystemTransformations(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
		}
	}
}

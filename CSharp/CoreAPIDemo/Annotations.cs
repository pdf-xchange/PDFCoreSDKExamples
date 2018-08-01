using System;
using System.ComponentModel;
using System.Windows.Forms;
using PDFXCoreAPI;

namespace CoreAPIDemo
{
	[Description("7. Annotations")]
	class Annotations
	{

		[Description("7.1. Add Text annotation")]
		static public int AddTextAnnotation(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IPXC_UndoRedoData urData = null;
			PXC_Rect rcPage = Parent.m_CurDoc.Pages[0].get_Box(PXC_BoxType.PBox_PageBox);
			IPXC_Page page = Parent.m_CurDoc.Pages.InsertPage(0, ref rcPage, out urData);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			//Getting Text annotation atom for the InsertNewAnnot method
			uint nText = pxsInst.StrToAtom("Text");
			
			double nCX = (rcPage.right - rcPage.left) / 2.0;
			double nCY = (rcPage.top - rcPage.bottom) / 2.0;
			PXC_Rect rcOut = new PXC_Rect();
			rcOut.left = nCX - 200;
			rcOut.bottom = nCY + 250;
			rcOut.right = nCX - 150;
			rcOut.top = nCY + 300;
			IPXC_Annotation annot = page.InsertNewAnnot(nText, ref rcOut);
			IPXC_AnnotData_Text aData = annot.Data as IPXC_AnnotData_Text;
			aData.Contents = "Text Annotation 1.";
			aData.Title = "Text Annotation 1";
			var color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.0f, 0.8f, 0.8f);
			aData.Color = color;
			annot.Data = aData;

			rcOut.bottom -= 100;
			rcOut.top -= 100;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_Text;
			aData.Contents = "Text Annotation 2.";
			aData.Title = "Text Annotation 2";
			color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.5f, 0.4f, 0.48f);
			aData.Color = color;
			annot.Data = aData;

			return (int)Form1.eFormUpdateFlags.efuf_Annotations;
		}

		[Description("7.2. Add Link annotation")]
		static public int AddLinkAnnotation(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IPXC_UndoRedoData urData = null;
			PXC_Rect rcPage = Parent.m_CurDoc.Pages[0].get_Box(PXC_BoxType.PBox_PageBox);
			IPXC_Page page = Parent.m_CurDoc.Pages.InsertPage(0, ref rcPage, out urData);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			//Getting Text annotation atom for the InsertNewAnnot method
			uint nText = pxsInst.StrToAtom("Link");

			double nCX = (rcPage.right - rcPage.left) / 2.0;
			double nCY = (rcPage.top - rcPage.bottom) / 2.0;
			PXC_Rect rcOut = new PXC_Rect();
			rcOut.left = nCX - 200;
			rcOut.bottom = nCY + 250;
			rcOut.right = nCX + 150;
			rcOut.top = nCY + 300;
			IPXC_Annotation annot = page.InsertNewAnnot(nText, ref rcOut);
			IPXC_AnnotData_Link aData = annot.Data as IPXC_AnnotData_Link;
			aData.Contents = "Link Annotation 1.";
			aData.HighlighMode = PXC_AnnotHighlightMode.AHM_Outline;

			var color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.0f, 0.8f, 0.8f);
			aData.Color = color;
			//Setting dashed border pattern
			var border = new PXC_AnnotBorder();
			border.nStyle = PXC_AnnotBorderStyle.ABS_Dashed;
			border.nWidth = 4.0f;
			border.DashArray = new float[10];
			border.DashArray[0] = border.DashArray[1] = 16.0f; //Width of dashes
			border.nDashCount = 2; //Number of dashes
			aData.set_Border(border);
			annot.Data = aData;

			rcOut.bottom -= 200;
			rcOut.top -= 200;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_Link;
			aData.Contents = "Link Annotation 2.";
			color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.5f, 0.4f, 0.48f);
			aData.Color = color;
			annot.Data = aData;

			return (int)Form1.eFormUpdateFlags.efuf_Annotations;
		}

		[Description("7.3. Add Free text annotation")]
		static public int AddFreeTextAnnotation(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IPXC_UndoRedoData urData = null;
			PXC_Rect rcPage = Parent.m_CurDoc.Pages[0].get_Box(PXC_BoxType.PBox_PageBox);
			IPXC_Page page = Parent.m_CurDoc.Pages.InsertPage(0, ref rcPage, out urData);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			//Getting Text annotation atom for the InsertNewAnnot method
			uint nText = pxsInst.StrToAtom("FreeText");

			double nCX = (rcPage.right - rcPage.left) / 2.0;
			double nCY = (rcPage.top - rcPage.bottom) / 2.0;
			PXC_Rect rcOut = new PXC_Rect();
			rcOut.left = nCX - 200;
			rcOut.bottom = nCY + 200;
			rcOut.right = nCX + 200;
			rcOut.top = nCY + 300;
			IPXC_Annotation annot = page.InsertNewAnnot(nText, ref rcOut);
			IPXC_AnnotData_FreeText aData = annot.Data as IPXC_AnnotData_FreeText;
			aData.Contents = "Free Text Annotation 1.";
			aData.Title = "Free Text Annotation 1.";
			IPXC_Font font = Parent.m_CurDoc.CreateNewFont("Arial", (uint)PXC_CreateFontFlags.CreateFont_Monospaced, 700);
			aData.DefaultFont = font;
			aData.DefaultFontSize = 40;
			var color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.7f, 0.7f, 0.7f);
			aData.DefaultTextColor = color;
			aData.Opacity = 0.5;
			aData.TextRotation = 90;
			aData.Subject = "Typewriter";
			//Setting dashed border pattern
			var border = new PXC_AnnotBorder();
			border.nStyle = PXC_AnnotBorderStyle.ABS_Beveled;
			border.nWidth = 4.0f;
			border.DashArray = new float[15];
			border.DashArray[0] = border.DashArray[1] = 5.0f; //Width of dashes
			border.nDashCount = 2; //Number of dashes
			aData.set_Border(border);
			annot.Data = aData;

			rcOut.bottom -= 200;
			rcOut.top -= 200;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_FreeText;
			aData.Contents = "Free Text Annotation 2.";
			aData.DefaultFontSize = 15.0;
			color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.0f, 0.0f, 0.0f);
			aData.SColor = color;
			aData.DefaultTextColor = color;
			//Setting dashed border pattern
			border = new PXC_AnnotBorder();
			border.nStyle = PXC_AnnotBorderStyle.ABS_Dashed;
			border.nWidth = 10.0f;
			border.DashArray = new float[20];
			border.DashArray[0] = border.DashArray[1] = 5.0f; //Width of dashes
			border.nDashCount = 4; //Number of dashes
			aData.set_Border(border);
			annot.Data = aData;

			rcOut.bottom -= 200;
			rcOut.top -= 200;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_FreeText;
			aData.Contents = "Free Text Annotation 3.";
			color = auxInst.CreateColor(ColorType.ColorType_RGB);
			aData.DefaultFontSize = 15.0;
			color.SetRGB(1.0f, 1.0f, 1.0f);
			aData.SColor = color;
			color.SetRGB(0.0f, 0.0f, 0.0f);
			aData.DefaultTextColor = color;
			annot.Data = aData;

			return (int)Form1.eFormUpdateFlags.efuf_Annotations;
		}

		[Description("7.4. Add Line annotation")]
		static public int AddLineAnnotation(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IPXC_UndoRedoData urData = null;
			PXC_Rect rcPage = Parent.m_CurDoc.Pages[0].get_Box(PXC_BoxType.PBox_PageBox);
			IPXC_Page page = Parent.m_CurDoc.Pages.InsertPage(0, ref rcPage, out urData);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			//Getting Text annotation atom for the InsertNewAnnot method
			uint nText = pxsInst.StrToAtom("Line");

			double nCX = (rcPage.right - rcPage.left) / 2.0;
			double nCY = (rcPage.top - rcPage.bottom) / 2.0;
			PXC_Rect rcOut = new PXC_Rect();
			rcOut.left = nCX - 200;
			rcOut.bottom = nCY + 200;
			rcOut.right = nCX + 200;
			rcOut.top = nCY + 300;
			PXC_Point startPoint = new PXC_Point();
			startPoint.x = rcPage.left + 50;
			startPoint.y = rcPage.top - 50;
			PXC_Point endPiont = new PXC_Point();
			endPiont.x = rcPage.right - 50;
			endPiont.y = startPoint.y;
			IPXC_Annotation annot = page.InsertNewAnnot(nText, ref rcOut);
			IPXC_AnnotData_Line aData = annot.Data as IPXC_AnnotData_Line;
			aData.Title = "Line annotation 1.";
			var color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.0f, 0.0f, 0.0f);
			aData.Color = color;
			aData.SetLinePoints(ref startPoint, ref endPiont);
			PXC_AnnotBorder border = new PXC_AnnotBorder();
			border.nStyle = PXC_AnnotBorderStyle.ABS_Solid;
			border.nWidth = 3.0f;
			aData.set_Border(border);
			annot.Data = aData;

			startPoint.y = startPoint.y - 50;
			endPiont.y = startPoint.y;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_Line;
			aData.SetLinePoints(ref startPoint, ref endPiont);
			aData.Title = "Line annotation 2.";
			aData.Color = color;
			aData.SetLineEndings(PXC_AnnotLineEndingStyle.LE_None, PXC_AnnotLineEndingStyle.LE_OpenArrow);
			aData.set_Border(border);
			annot.Data = aData;

			startPoint.y = startPoint.y - 50;
			endPiont.y = startPoint.y;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_Line;
			aData.SetLinePoints(ref startPoint, ref endPiont);
			aData.Title = "Line annotation 3.";
			aData.Color = color;
			aData.FColor = color;
			aData.SetLineEndings(PXC_AnnotLineEndingStyle.LE_ClosedArrow, PXC_AnnotLineEndingStyle.LE_ClosedArrow);
			aData.set_Border(border);
			annot.Data = aData;

			startPoint.y = startPoint.y - 50;
			endPiont.y = startPoint.y;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_Line;
			aData.SetLinePoints(ref startPoint, ref endPiont);
			aData.Title = "Line annotation 4.";
			aData.Color = color;
			aData.FColor = color;
			aData.SetLineEndings(PXC_AnnotLineEndingStyle.LE_Circle, PXC_AnnotLineEndingStyle.LE_None);
			aData.set_Border(border);
			annot.Data = aData;

			startPoint.y = startPoint.y - 50;
			endPiont.y = startPoint.y;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_Line;
			aData.SetLinePoints(ref startPoint, ref endPiont);
			aData.Title = "Line annotation 5.";
			aData.ShowCaption = true;
			aData.Contents = "Line annotation 5.";
			aData.Color = color;
			annot.Data = aData;

			startPoint.y = startPoint.y - 50;
			endPiont.y = startPoint.y;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_Line;
			aData.SetLinePoints(ref startPoint, ref endPiont);
			aData.Title = "Line annotation 6.";
			aData.Color = color;
			aData.FColor = color;
			aData.LeaderLine = 15.0;
			aData.LeaderLineExtension = 15.0;
			aData.set_Border(border);
			annot.Data = aData;

			return (int)Form1.eFormUpdateFlags.efuf_Annotations;
		}

		[Description("7.5. Add Square annotation")]
		static public int AddSquareAnnotation(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IPXC_UndoRedoData urData = null;
			PXC_Rect rcPage = Parent.m_CurDoc.Pages[0].get_Box(PXC_BoxType.PBox_PageBox);
			IPXC_Page page = Parent.m_CurDoc.Pages.InsertPage(0, ref rcPage, out urData);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			//Getting Text annotation atom for the InsertNewAnnot method
			uint nText = pxsInst.StrToAtom("Square");

			double nCX = (rcPage.right - rcPage.left) / 2.0;
			double nCY = (rcPage.top - rcPage.bottom) / 2.0;
			PXC_Rect rcOut = new PXC_Rect();
			rcOut.left = nCX - 200;
			rcOut.bottom = nCY + 200;
			rcOut.right = nCX + 200;
			rcOut.top = nCY + 300;
			IPXC_Annotation annot = page.InsertNewAnnot(nText, ref rcOut);
			IPXC_AnnotData_SquareCircle aData = annot.Data as IPXC_AnnotData_SquareCircle;
			aData.Title = "Square annotation 1.";
			annot.Data = aData;

			rcOut.bottom -= 150;
			rcOut.top -= 150;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_SquareCircle;
			aData.Title = "Square annotation 2.";
			IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.0f, 0.0f, 0.0f);
			aData.SColor = color;
			color.SetRGB(0.15f, 0.5f, 0.12f);
			aData.FColor = color;
			annot.Data = aData;

			rcOut.bottom -= 150;
			rcOut.top -= 150;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_SquareCircle;
			aData.Title = "Square annotation 3.";
			color.SetRGB(0.0f, 0.0f, 0.0f);
			aData.SColor = color;
			color.SetRGB(0.5f, 0.5f, 0.5f);
			aData.FColor = color;
			//Setting dashed border pattern
			PXC_AnnotBorder border = new PXC_AnnotBorder();
			border.nStyle = PXC_AnnotBorderStyle.ABS_Dashed;
			border.nWidth = 4.0f;
			border.DashArray = new float[] { 10f, 8f, 6f, 4f, 2f, 2f, 4f, 6f, 8f, 10f};//Width of dashes
			border.nDashCount = 4; //Number of dashes
			aData.set_Border(border);
			annot.Data = aData;

			rcOut.bottom -= 150;
			rcOut.top -= 150;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_SquareCircle;
			aData.Title = "Square annotation 4.";
			color.SetRGB(0.0f, 0.0f, 0.0f);
			aData.SColor = color;
			color.SetRGB(0.5f, 0.5f, 0.5f);
			aData.FColor = color;
			//Setting dashed border pattern
			border = new PXC_AnnotBorder();
			border.nStyle = PXC_AnnotBorderStyle.ABS_Solid;
			border.nWidth = 5.0f;
			aData.set_Border(border);
			annot.Data = aData;

			return (int)Form1.eFormUpdateFlags.efuf_Annotations;
		}

		[Description("7.6. Add Circle annotation")]
		static public int AddCircleAnnotation(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IPXC_UndoRedoData urData = null;
			PXC_Rect rcPage = Parent.m_CurDoc.Pages[0].get_Box(PXC_BoxType.PBox_PageBox);
			IPXC_Page page = Parent.m_CurDoc.Pages.InsertPage(0, ref rcPage, out urData);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			//Getting Text annotation atom for the InsertNewAnnot method
			uint nText = pxsInst.StrToAtom("Circle");

			double nCX = (rcPage.right - rcPage.left) / 2.0;
			double nCY = (rcPage.top - rcPage.bottom) / 2.0;
			PXC_Rect rcOut = new PXC_Rect();
			rcOut.left = nCX - 200;
			rcOut.bottom = nCY + 200;
			rcOut.right = nCX + 200;
			rcOut.top = nCY + 300;
			IPXC_Annotation annot = page.InsertNewAnnot(nText, ref rcOut);
			IPXC_AnnotData_SquareCircle aData = annot.Data as IPXC_AnnotData_SquareCircle;
			aData.Title = "Circle annotation 1.";
			annot.Data = aData;

			rcOut.bottom -= 150;
			rcOut.top -= 150;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_SquareCircle;
			aData.Title = "Circle annotation 2.";
			IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.0f, 0.0f, 0.0f);
			aData.SColor = color;
			color.SetRGB(0.15f, 0.5f, 0.12f);
			aData.FColor = color;
			annot.Data = aData;

			rcOut.bottom -= 150;
			rcOut.top -= 150;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_SquareCircle;
			aData.Title = "Circle annotation 3.";
			color.SetRGB(0.0f, 0.0f, 0.0f);
			aData.SColor = color;
			color.SetRGB(0.5f, 0.5f, 0.5f);
			aData.FColor = color;
			//Setting dashed border pattern
			PXC_AnnotBorder border = new PXC_AnnotBorder();
			border.nStyle = PXC_AnnotBorderStyle.ABS_Dashed;
			border.nWidth = 4.0f;
			border.DashArray = new float[] { 10f, 8f, 6f, 4f, 2f, 2f, 4f, 6f, 8f, 10f };//Width of dashes
			border.nDashCount = 4; //Number of dashes
			aData.set_Border(border);
			annot.Data = aData;

			rcOut.bottom -= 150;
			rcOut.top -= 150;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_SquareCircle;
			aData.Title = "Circle annotation 4.";
			color.SetRGB(0.0f, 0.0f, 0.0f);
			aData.SColor = color;
			color.SetRGB(0.5f, 0.5f, 0.5f);
			aData.FColor = color;
			//Setting dashed border pattern
			border = new PXC_AnnotBorder();
			border.nStyle = PXC_AnnotBorderStyle.ABS_Solid;
			border.nWidth = 5.0f;
			aData.set_Border(border);
			annot.Data = aData;

			return (int)Form1.eFormUpdateFlags.efuf_Annotations;
		}

		[Description("7.7. Add Polygon annotation")]
		static public int AddPolygonAnnotation(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IPXC_UndoRedoData urData = null;
			PXC_Rect rcPage = Parent.m_CurDoc.Pages[0].get_Box(PXC_BoxType.PBox_PageBox);
			IPXC_Page page = Parent.m_CurDoc.Pages.InsertPage(0, ref rcPage, out urData);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			//Getting Text annotation atom for the InsertNewAnnot method
			uint nText = pxsInst.StrToAtom("Polygon");

			double nCX = (rcPage.right - rcPage.left) / 2.0;
			double nCY = (rcPage.top - rcPage.bottom) / 2.0;
			PXC_Rect rcOut = new PXC_Rect();
			rcOut.left = nCX - 150;
			rcOut.bottom = nCY + 200;
			rcOut.right = nCX + 150;
			rcOut.top = nCY + 300;
			IPXC_Annotation annot = page.InsertNewAnnot(nText, ref rcOut);
			IPXC_AnnotData_Poly aData = annot.Data as IPXC_AnnotData_Poly;
			aData.Title = "Polygon annotation 1.";
			IPXC_PolygonSrcF poly = aData.Vertices;
			double r = 1.5 * 72.0;
			double a = -90.0;
			poly.Clear();
			for (int i = 0; i < 4; i++)
			{
				PXC_PointF pointF = new PXC_PointF();
				pointF.x = (float)(rcOut.left + r * Math.Cos(a * Math.PI / 180.0));
				pointF.y = (float)(rcOut.bottom - r * Math.Sin(a * Math.PI / 180.0));
				a += 360.0 / 4;
				poly.Insert(ref pointF);
			}
			aData.Vertices = poly;
			IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.5f, 0.2f, 0.2f);
			aData.FColor = color;
			color.SetRGB(0.0f, 0.0f, 0.0f);
			aData.SColor = color;
			
			annot.Data = aData;

			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_Poly;
			aData.Title = "Polygon annotation 2.";
			poly = aData.Vertices;
			poly.Clear();
			for (int i = 0; i < 6; i++)
			{
				PXC_PointF pointF = new PXC_PointF();
				pointF.x = (float)(nCX + r * Math.Cos(a * Math.PI / 180.0));
				pointF.y = (float)(rcOut.bottom - r * Math.Sin(a * Math.PI / 180.0));
				a += 360.0 / 6;
				poly.Insert(ref pointF);
			}
			aData.Vertices = poly;
			color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.2f, 0.5f, 0.2f);
			aData.FColor = color;
			color.SetRGB(0.0f, 0.0f, 0.0f);
			aData.SColor = color;
			PXC_AnnotBorder border = new PXC_AnnotBorder();
			border.nStyle = PXC_AnnotBorderStyle.ABS_Solid;
			border.nWidth = 5.0f;
			aData.set_Border(border);
			aData.BlendMode = PXC_BlendMode.BlendMode_Multiply;
			annot.Data = aData;


			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_Poly;
			aData.Title = "Polygon annotation 3.";
			poly = aData.Vertices;
			poly.Clear();
			for (int i = 0; i < 8; i++)
			{
				PXC_PointF pointF = new PXC_PointF();
				pointF.x = (float)(rcOut.right + r * Math.Cos(a * Math.PI / 180.0));
				pointF.y = (float)(rcOut.bottom - r * Math.Sin(a * Math.PI / 180.0));
				a += 360.0 / 8;
				poly.Insert(ref pointF);
			}
			aData.Vertices = poly;
			color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.2f, 0.2f, 0.5f);
			aData.FColor = color;
			color.SetRGB(0.0f, 0.0f, 0.0f);
			aData.SColor = color;
			border = new PXC_AnnotBorder();
			border.nStyle = PXC_AnnotBorderStyle.ABS_Dashed;
			border.nWidth = 3.0f;
			border.DashArray = new float[] { 10f, 8f, 6f, 4f, 2f, 2f, 4f, 6f, 8f, 10f };//Width of dashes
			border.nDashCount = 4; //Number of dashes
			aData.set_Border(border);
			aData.BlendMode = PXC_BlendMode.BlendMode_Multiply;
			annot.Data = aData;

			return (int)Form1.eFormUpdateFlags.efuf_Annotations;
		}

		[Description("7.8. Add Polyline annotation")]
		static public int AddPolylineAnnotation(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IPXC_UndoRedoData urData = null;
			PXC_Rect rcPage = Parent.m_CurDoc.Pages[0].get_Box(PXC_BoxType.PBox_PageBox);
			IPXC_Page page = Parent.m_CurDoc.Pages.InsertPage(0, ref rcPage, out urData);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			//Getting Text annotation atom for the InsertNewAnnot method
			uint nText = pxsInst.StrToAtom("PolyLine");

			double nCX = (rcPage.right - rcPage.left) / 2.0;
			double nCY = (rcPage.top - rcPage.bottom) / 2.0;
			PXC_Rect rcOut = new PXC_Rect();
			rcOut.left = nCX - 150;
			rcOut.bottom = nCY + 200;
			rcOut.right = nCX + 150;
			rcOut.top = nCY + 300;
			IPXC_Annotation annot = page.InsertNewAnnot(nText, ref rcOut);
			IPXC_AnnotData_Poly aData = annot.Data as IPXC_AnnotData_Poly;
			aData.Title = "PolyLine annotation 1.";
			IPXC_PolygonSrcF poly = aData.Vertices;
			double r = 1.5 * 72.0;
			double a = -90.0;
			poly.Clear();
			for (int i = 0; i < 5; i++)
			{
				PXC_PointF pointF = new PXC_PointF();
				pointF.x = (float)(rcOut.left + r * Math.Cos(a * Math.PI / 180.0));
				pointF.y = (float)(rcOut.bottom - r * Math.Sin(a * Math.PI / 180.0));
				a += 360.0 / 4;
				poly.Insert(ref pointF);
			}
			aData.Vertices = poly;
			IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.5f, 0.2f, 0.2f);
			aData.SColor = color;
			annot.Data = aData;

			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_Poly;
			aData.Title = "PolyLine annotation 2.";
			poly = aData.Vertices;
			poly.Clear();
			for (int i = 0; i < 7; i++)
			{
				PXC_PointF pointF = new PXC_PointF();
				pointF.x = (float)(nCX + r * Math.Cos(a * Math.PI / 180.0));
				pointF.y = (float)(rcOut.bottom - r * Math.Sin(a * Math.PI / 180.0));
				a += 360.0 / 6;
				poly.Insert(ref pointF);
			}
			aData.Vertices = poly;
			color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.2f, 0.5f, 0.2f);
			aData.SColor = color;
			PXC_AnnotBorder border = new PXC_AnnotBorder();
			border.nStyle = PXC_AnnotBorderStyle.ABS_Solid;
			border.nWidth = 5.0f;
			aData.set_Border(border);
			aData.BlendMode = PXC_BlendMode.BlendMode_Multiply;
			annot.Data = aData;


			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_Poly;
			aData.Title = "PolyLine annotation 3.";
			poly = aData.Vertices;
			poly.Clear();
			for (int i = 0; i < 9; i++)
			{
				PXC_PointF pointF = new PXC_PointF();
				pointF.x = (float)(rcOut.right + r * Math.Cos(a * Math.PI / 180.0));
				pointF.y = (float)(rcOut.bottom - r * Math.Sin(a * Math.PI / 180.0));
				a += 360.0 / 8;
				poly.Insert(ref pointF);
			}
			aData.Vertices = poly;
			color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.2f, 0.2f, 0.5f);
			aData.SColor = color;
			border = new PXC_AnnotBorder();
			border.nStyle = PXC_AnnotBorderStyle.ABS_Dashed;
			border.nWidth = 3.0f;
			border.DashArray = new float[] { 10f, 8f, 6f, 4f, 2f, 2f, 4f, 6f, 8f, 10f };//Width of dashes
			border.nDashCount = 4; //Number of dashes
			aData.set_Border(border);
			aData.BlendMode = PXC_BlendMode.BlendMode_Multiply;
			annot.Data = aData;

			return (int)Form1.eFormUpdateFlags.efuf_Annotations;
		}

		[Description("7.9. Add Highlight annotation")]
		static public int AddHighlightAnnotation(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IPXC_UndoRedoData urData = null;
			PXC_Rect rcPage = Parent.m_CurDoc.Pages[0].get_Box(PXC_BoxType.PBox_PageBox);
			IPXC_Page page = Parent.m_CurDoc.Pages.InsertPage(0, ref rcPage, out urData);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			IPXC_ContentCreator CC = Parent.m_CurDoc.CreateContentCreator();
			double nCX = (rcPage.right - rcPage.left) / 2.0;
			double nCY = (rcPage.top - rcPage.bottom) / 2.0;

			IPXC_Font font = Parent.m_CurDoc.CreateNewFont("Arial", (uint)PXC_CreateFontFlags.CreateFont_Monospaced, 700);
			CC.SetFontSize(30);
			CC.SetFont(font);
			CC.SetColorRGB(0x00000000);
			CC.ShowTextLine(nCX - 190, nCY, "This is a story of long ago.", -1, (uint)PXC_ShowTextLineFlags.STLF_Default | (uint)PXC_ShowTextLineFlags.STLF_AllowSubstitution);
			page.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
			IPXC_PageText Text = page.GetText(null, false);

			PXC_Rect rcOut = new PXC_Rect();
			rcOut.left = nCX - 150;
			rcOut.bottom = nCY - 100;
			rcOut.right = nCX + 150;
			rcOut.top = nCY + 100;
			//Getting Text annotation atom for the InsertNewAnnot method
			uint nText = pxsInst.StrToAtom("Highlight");
			IPXC_Annotation annot = page.InsertNewAnnot(nText, ref rcOut);
			IPXC_AnnotData_TextMarkup aData = annot.Data as IPXC_AnnotData_TextMarkup;
			aData.Title = "Highlight annotation 1.";
			IPXC_QuadsF quadsF = Parent.m_pxcInst.CreateQuads();
			uint afafaf = quadsF.Count;
			PXC_RectF rectF = new PXC_RectF();
			Text.GetTextQuads3(0, 7, quadsF, out rectF);
			aData.Quads = quadsF;
			annot.Data = aData;

			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_TextMarkup;
			aData.Title = "Highlight annotation 2.";
			Text.GetTextQuads3(19, 9, quadsF, out rectF);
			IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.74f, 0.74f, 0.74f);
			aData.Color = color;
			aData.Quads = quadsF;
			annot.Data = aData;

			return (int)Form1.eFormUpdateFlags.efuf_Annotations;
		}

		[Description("7.10. Add Underline annotation")]
		static public int AddUnderlineAnnotation(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IPXC_UndoRedoData urData = null;
			PXC_Rect rcPage = Parent.m_CurDoc.Pages[0].get_Box(PXC_BoxType.PBox_PageBox);
			IPXC_Page page = Parent.m_CurDoc.Pages.InsertPage(0, ref rcPage, out urData);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			IPXC_ContentCreator CC = Parent.m_CurDoc.CreateContentCreator();
			double nCX = (rcPage.right - rcPage.left) / 2.0;
			double nCY = (rcPage.top - rcPage.bottom) / 2.0;

			IPXC_Font font = Parent.m_CurDoc.CreateNewFont("Arial", (uint)PXC_CreateFontFlags.CreateFont_Monospaced, 700);
			CC.SetFontSize(30);
			CC.SetFont(font);
			CC.SetColorRGB(0x00000000);
			CC.ShowTextLine(nCX - 190, nCY, "This is a story of long ago.", -1, (uint)PXC_ShowTextLineFlags.STLF_Default | (uint)PXC_ShowTextLineFlags.STLF_AllowSubstitution);
			page.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
			IPXC_PageText Text = page.GetText(null, false);

			PXC_Rect rcOut = new PXC_Rect();
			rcOut.left = nCX - 150;
			rcOut.bottom = nCY - 100;
			rcOut.right = nCX + 150;
			rcOut.top = nCY + 100;
			//Getting Text annotation atom for the InsertNewAnnot method
			uint nText = pxsInst.StrToAtom("Underline");
			IPXC_Annotation annot = page.InsertNewAnnot(nText, ref rcOut);
			IPXC_AnnotData_TextMarkup aData = annot.Data as IPXC_AnnotData_TextMarkup;
			aData.Title = "Underline annotation 1.";
			IPXC_QuadsF quadsF = Parent.m_pxcInst.CreateQuads();
			uint afafaf = quadsF.Count;
			PXC_RectF rectF = new PXC_RectF();
			Text.GetTextQuads3(0, 7, quadsF, out rectF);
			aData.Quads = quadsF;
			annot.Data = aData;

			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_TextMarkup;
			aData.Title = "Underline annotation 2.";
			Text.GetTextQuads3(19, 9, quadsF, out rectF);
			IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(1.0f, 0.0f, 0.0f);
			aData.Color = color;
			aData.Quads = quadsF;
			annot.Data = aData;

			return (int)Form1.eFormUpdateFlags.efuf_Annotations;
		}

		[Description("7.11. Add Strikeout annotation")]
		static public void AddStrikeoutAnnotation(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IPXC_UndoRedoData urData = null;
			PXC_Rect rcPage = Parent.m_CurDoc.Pages[0].get_Box(PXC_BoxType.PBox_PageBox);
			IPXC_Page page = Parent.m_CurDoc.Pages.InsertPage(0, ref rcPage, out urData);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			IPXC_ContentCreator CC = Parent.m_CurDoc.CreateContentCreator();
			double nCX = (rcPage.right - rcPage.left) / 2.0;
			double nCY = (rcPage.top - rcPage.bottom) / 2.0;

			IPXC_Font font = Parent.m_CurDoc.CreateNewFont("Arial", (uint)PXC_CreateFontFlags.CreateFont_Monospaced, 700);
			CC.SetFontSize(30);
			CC.SetFont(font);
			CC.SetColorRGB(0x00000000);
			CC.ShowTextLine(nCX - 190, nCY, "This is a story of long ago.", -1, (uint)PXC_ShowTextLineFlags.STLF_Default | (uint)PXC_ShowTextLineFlags.STLF_AllowSubstitution);
			page.PlaceContent(CC.Detach(), (uint)PXC_PlaceContentFlags.PlaceContent_Replace);
			IPXC_PageText Text = page.GetText(null, false);

			PXC_Rect rcOut = new PXC_Rect();
			rcOut.left = nCX - 150;
			rcOut.bottom = nCY - 100;
			rcOut.right = nCX + 150;
			rcOut.top = nCY + 100;
			//Getting Text annotation atom for the InsertNewAnnot method
			uint nText = pxsInst.StrToAtom("Squiggly");
			IPXC_Annotation annot = page.InsertNewAnnot(nText, ref rcOut);
			IPXC_AnnotData_TextMarkup aData = annot.Data as IPXC_AnnotData_TextMarkup;
			aData.Title = "Strikeout annotation 1.";
			IPXC_QuadsF quadsF = Parent.m_pxcInst.CreateQuads();
			uint afafaf = quadsF.Count;
			PXC_RectF rectF = new PXC_RectF();
			Text.GetTextQuads3(0, 7, quadsF, out rectF);
			aData.Quads = quadsF;
			annot.Data = aData;

			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_TextMarkup;
			aData.Title = "Strikeout annotation 2.";
			Text.GetTextQuads3(19, 9, quadsF, out rectF);
			IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(1.0f, 0.0f, 0.0f);
			aData.Color = color;
			aData.Quads = quadsF;
			annot.Data = aData;
		}

		[Description("7.12. Add Popup annotation")]
		static public int AddPopupAnnotation(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IPXC_UndoRedoData urData = null;
			PXC_Rect rcPage = Parent.m_CurDoc.Pages[0].get_Box(PXC_BoxType.PBox_PageBox);
			IPXC_Page page = Parent.m_CurDoc.Pages.InsertPage(0, ref rcPage, out urData);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			//Getting Text annotation atom for the InsertNewAnnot method
			uint nText = pxsInst.StrToAtom("Popup");

			double nCX = (rcPage.right - rcPage.left) / 2.0;
			double nCY = (rcPage.top - rcPage.bottom) / 2.0;
			PXC_Rect rcOut = new PXC_Rect();
			rcOut.left = nCX - 200;
			rcOut.bottom = nCY + 250;
			rcOut.right = nCX - 150;
			rcOut.top = nCY + 300;
			IPXC_Annotation annot = page.InsertNewAnnot(nText, ref rcOut);
			IPXC_AnnotData_Popup aData = annot.Data as IPXC_AnnotData_Popup;
			aData.Contents = "Popup Annotation 1.";
			var color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.0f, 0.8f, 0.8f);
			aData.Color = color;
			annot.Data = aData;

			rcOut.bottom -= 100;
			rcOut.top -= 100;
			annot = page.InsertNewAnnot(nText, ref rcOut);
			aData = annot.Data as IPXC_AnnotData_Popup;
			aData.Contents = "Popup Annotation 2.";
			color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.5f, 0.4f, 0.48f);
			aData.Color = color;
			annot.Data = aData;

			return (int)Form1.eFormUpdateFlags.efuf_Annotations;
		}

		[Description("7.13. Add File attachment annotation")]
		static public int AddFile_AttachmentAnnotation(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			IPXC_UndoRedoData urData = null;
			PXC_Rect rcPage = Parent.m_CurDoc.Pages[0].get_Box(PXC_BoxType.PBox_PageBox);
			IPXC_Page page = Parent.m_CurDoc.Pages.InsertPage(0, ref rcPage, out urData);
			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			//Getting Text annotation atom for the InsertNewAnnot method
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
			IPXC_FileSpec fileSpec = Parent.m_CurDoc.CreateEmbeddFile(System.Environment.CurrentDirectory + "\\Documents\\Hobbit.txt");
			aData.FileAttachment = fileSpec;
			annot.Data = aData;

			return (int)Form1.eFormUpdateFlags.efuf_Annotations;
		}

		[Description("7.14. Add Sound annotation")]
		static public void AddSoundAnnotation(Form1 Parent)
		{
#warning Implement this
		}

		[Description("7.15. Add Redact annotation")]
		static public void AddRedactAnnotation(Form1 Parent)
		{
#warning Implement this
		}

		[Description("7.16. Remove all of the annotations from the current page")]
		static public void RemoveAnnotations(Form1 Parent)
		{
#warning Implement this
		}
	}
}
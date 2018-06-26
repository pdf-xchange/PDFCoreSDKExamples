using System.ComponentModel;
using System.IO;
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

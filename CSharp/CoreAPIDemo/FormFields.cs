using PDFXCoreAPI;
using System.ComponentModel;

namespace CoreAPIDemo
{
	[Description("5. Form Fields")]
	class FormFields
	{
		[Description("5.1. Add Text Fields on page")]
		static public void AddTextFieldsOnPage(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
			PXC_Rect rc = new PXC_Rect();
			rc.top = 800;
			rc.right = 600;

			IPXC_UndoRedoData urD = null;
			IPXC_Page firstPage = Parent.m_CurDoc.Pages.InsertPage(0, rc, out urD);
			PXC_Rect textRC = new PXC_Rect();
			textRC.top = rc.top - 1.0 * 72.0;
			textRC.left = 1.0 * 72.0;
			textRC.bottom = rc.top - 2.0 * 72.0;
			textRC.right = rc.right - 1.0 * 72.0;

			//Ordinary text field
			IPXC_FormField firstTextBOX = Parent.m_CurDoc.AcroForm.CreateField("Text1", PXC_FormFieldType.FFT_Text, 0, textRC);
			firstTextBOX.SetValueText("Ordinary text field");

			//Read-only and locked text field with custom style
			textRC.top = rc.top - 3.0 * 72.0;
			textRC.bottom = rc.top - 4.0 * 72.0;
			IPXC_FormField secondTextBOX = Parent.m_CurDoc.AcroForm.CreateField("Text2", PXC_FormFieldType.FFT_Text, 0, textRC);
			secondTextBOX.SetValueText("Read-only and locked text field with custom style");
			IAUX_Inst auxInst = Parent.m_pxcInst.GetExtension("AUX");
			IPXC_Annotation annot = secondTextBOX.Widget[0];
			IPXC_AnnotData_Widget WData = (IPXC_AnnotData_Widget)annot.Data;
			IColor color = auxInst.CreateColor(ColorType.ColorType_RGB);
			color.SetRGB(0.9f, 0.9f, 0.6f);
			WData.FColor = color;
			color.SetRGB(0.6f, 0.9f, 0.9f);
			WData.SColor = color;
			PXC_AnnotBorder border = new PXC_AnnotBorder();
			border.nStyle = PXC_AnnotBorderStyle.ABS_Dashed;
			border.DashArray = new float[10];
			border.DashArray[0] = border.DashArray[1] = 16.0f; //Width of dashes
			border.nDashCount = 2; //Number of dashes
			border.nWidth = 5.0f;
			WData.set_Border(ref border);
			annot.Data = WData;
			annot.Flags |= (uint)PXC_AnnotFlag.AF_Locked;
			secondTextBOX.SetFlags((uint)PXC_FormFieldFlag.FFF_ReadOnly, (uint)PXC_FormFieldFlag.FFF_ReadOnly);

			//90 degree orientation text field with multiline option enabled
			textRC.top = rc.top - 5.0 * 72.0;
			textRC.bottom = rc.top - 7.0 * 72.0;
			IPXC_FormField thirdTextBOX = Parent.m_CurDoc.AcroForm.CreateField("Text3", PXC_FormFieldType.FFT_Text, 0, textRC);
			thirdTextBOX.SetFlags((uint)PXC_FormFieldFlag.TFF_MultiLine, (uint)PXC_FormFieldFlag.TFF_MultiLine);
			thirdTextBOX.SetValueText("90 degree orientation text field with multiline option enabled");
			annot = thirdTextBOX.Widget[0];
			WData = (IPXC_AnnotData_Widget)annot.Data;
			WData.ContentRotation = 90;
			annot.Data = WData;
			
			//Time formatted text field with custom JS that gives current time
			textRC.top = rc.top - 8.0 * 72.0;
			textRC.bottom = rc.top - 9.0 * 72.0;
			IPXC_FormField fourthTextBOX = Parent.m_CurDoc.AcroForm.CreateField("Text4", PXC_FormFieldType.FFT_Text, 0, textRC);
			annot = fourthTextBOX.Widget[0];
			annot.Flags |= (uint)PXC_SignDocumentFlags.Sign_TX_Date;
			IPXC_ActionsList actionsList = Parent.m_CurDoc.CreateActionsList();
			//Set script to ActionList
			actionsList.AddJavaScript("var now = new Date();\n" +
						"this.getField(\"Text4\").value = now.getHours() + \":\" + now.getMinutes(); ");
			fourthTextBOX.Actions[PXC_TriggerType.Trigger_Format] = actionsList;
		}

		[Description("5.2. Add Button form field with icon and an Action link")]
		static public void AddButtonWithIconAndURI(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);

			PXC_Rect rc = new PXC_Rect();
			rc.top = 800;
			rc.right = 600;

			IPXC_UndoRedoData urD = null;
			IPXC_Page Page = Parent.m_CurDoc.Pages.InsertPage(0, rc, out urD);
			PXC_Rect rcPB = new PXC_Rect();
			rcPB.left = 1.0 * 72.0;
			rcPB.right = rcPB.left + 2.0 * 72.0;
			rcPB.top = rc.top - 1.0 * 72.0;
			rcPB.bottom = rcPB.top - 2.0 * 72.0; //top is greater then bottom (PDF Coordinate System)
			IPXC_FormField googleButton = Parent.m_CurDoc.AcroForm.CreateField("Button1", PXC_FormFieldType.FFT_PushButton, 0, ref rcPB);
			//Now we'll need to add the icon
			IPXC_Annotation annot = googleButton.Widget[0];
			IPXC_AnnotData_Widget WData = (IPXC_AnnotData_Widget)annot.Data;
			string sPath = System.Environment.CurrentDirectory + "\\Images\\gotoSource_24.png";
			IPXC_Image img = Parent.m_CurDoc.AddImageFromFile(sPath);
			float imgw = img.Width;
			float imgh = img.Height;
			IPXC_ContentCreator CC = Parent.m_CurDoc.CreateContentCreator();
			CC.SaveState();
			CC.ScaleCS(imgw, imgh); //the image will be scaled to the button's size
			CC.PlaceImage(img);
			CC.RestoreState();
			IPXC_Content content = CC.Detach();
			PXC_Rect rcBBox;
			rcBBox.left = 0;
			rcBBox.top = imgh;
			rcBBox.right = imgw;
			rcBBox.bottom = 0;
			content.set_BBox(rcBBox);
			IPXC_XForm xForm = Parent.m_CurDoc.CreateNewXForm(ref rcPB);
			xForm.SetContent(content);
			WData.ButtonTextPosition = PXC_WidgetButtonTextPosition.WidgetText_IconOnly;
			WData.SetIcon(PXC_AnnotAppType.AAT_Normal, xForm, true);
			WData.Contents = "http://www.google.com"; //tooltip
			annot.Data = WData;
			//Setting the annotation's URI action
			IPXC_ActionsList AL = Parent.m_CurDoc.CreateActionsList();
			AL.AddURI("https://www.google.com");
			annot.set_Actions(PXC_TriggerType.Trigger_Up, AL);


			rcPB.left +=  4.0 * 72.0;
			rcPB.right = rcPB.left + 2.0 * 72.0;
			IPXC_FormField nextButton = Parent.m_CurDoc.AcroForm.CreateField("Button2", PXC_FormFieldType.FFT_PushButton, 0, ref rcPB);
			//Now we'll need to add the icon
			annot = nextButton.Widget[0];
			WData = (IPXC_AnnotData_Widget)annot.Data;
			sPath = System.Environment.CurrentDirectory + "\\Images\\next_24.png";
			img = Parent.m_CurDoc.AddImageFromFile(sPath);
			imgw = img.Width;
			imgh = img.Height;
			CC.SaveState();
			CC.ScaleCS(imgw, imgh); //the image will be scaled to the button's size
			CC.PlaceImage(img);
			CC.RestoreState();
			content = CC.Detach();
			rcBBox.left = 0;
			rcBBox.top = imgh;
			rcBBox.right = imgw;
			rcBBox.bottom = 0;
			content.set_BBox(rcBBox);
			xForm = Parent.m_CurDoc.CreateNewXForm(ref rcPB);
			xForm.SetContent(content);
			WData.ButtonTextPosition = PXC_WidgetButtonTextPosition.WidgetText_IconOnly;
			WData.SetIcon(PXC_AnnotAppType.AAT_Normal, xForm, true);
			WData.Contents = "Next Page"; //tooltip
			annot.Data = WData;
			//Setting the annotation's Goto action
			PXC_Destination dest = new PXC_Destination();
			dest.nPageNum = Page.Number + 1;
			dest.nType = PXC_DestType.Dest_XYZ;
			dest.nNullFlags = 15;
			AL = Parent.m_CurDoc.CreateActionsList();
			AL.AddGoto(dest);
			annot.set_Actions(PXC_TriggerType.Trigger_Up, AL);


			rcPB.top = rc.top - 4.0 * 72.0;
			rcPB.bottom = rcPB.top - 2.0 * 72.0;
			rcPB.left -= 4.0 * 72.0;
			rcPB.right = rcPB.left + 2.0 * 72.0;
			IPXC_FormField openButton = Parent.m_CurDoc.AcroForm.CreateField("Button3", PXC_FormFieldType.FFT_PushButton, 0, ref rcPB);
			//Now we'll need to add the icon
			annot = openButton.Widget[0];
			WData = (IPXC_AnnotData_Widget)annot.Data;
			sPath = System.Environment.CurrentDirectory + "\\Images\\openWith_24.png";
			img = Parent.m_CurDoc.AddImageFromFile(sPath);
			imgw = img.Width;
			imgh = img.Height;
			CC.SaveState();
			CC.ScaleCS(imgw, imgh); //the image will be scaled to the button's size
			CC.PlaceImage(img);
			CC.RestoreState();
			content = CC.Detach();
			rcBBox.left = 0;
			rcBBox.top = imgh;
			rcBBox.right = imgw;
			rcBBox.bottom = 0;
			content.set_BBox(rcBBox);
			xForm = Parent.m_CurDoc.CreateNewXForm(ref rcPB);
			xForm.SetContent(content);
			WData.ButtonTextPosition = PXC_WidgetButtonTextPosition.WidgetText_IconOnly;
			WData.SetIcon(PXC_AnnotAppType.AAT_Normal, xForm, true);
			WData.Contents = "Next Page"; //tooltip
			annot.Data = WData;
			//Setting the annotation's Launch action
			AL = Parent.m_CurDoc.CreateActionsList();
			sPath = System.Environment.CurrentDirectory + "\\Documents\\FeatureChartEU.pdf";
			AL.AddLaunch(sPath);
			annot.set_Actions(PXC_TriggerType.Trigger_Up, AL);
		}
	}
}

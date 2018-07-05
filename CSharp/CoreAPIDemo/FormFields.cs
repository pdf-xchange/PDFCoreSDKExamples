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
			//Ordinary text field

			//Read-only and locked text field with custom style

			//90 degree orientation text field with multiline option enabled

			//Time formatted text field with custom JS that gives current time
		}

		[Description("5.2. Add Button form field with icon and an URI Action link")]
		static public void AddButtonWithIconAndURI(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				Document.CreateNewDoc(Parent);
			IPXC_Page firstPage = Parent.m_CurDoc.Pages[0];
			PXC_Rect rcPB = firstPage.get_Box(PXC_BoxType.PBox_PageBox);
			rcPB.left += 100;
			rcPB.right = rcPB.left + 200;
			rcPB.bottom += 200;
			rcPB.top = rcPB.bottom + 200; //top is greater then bottom (PDF Coordinate System)
			IPXC_FormField ff = Parent.m_CurDoc.AcroForm.CreateField("Button1", PXC_FormFieldType.FFT_PushButton, 0, ref rcPB);
			//Now we'll need to add the icon
			IPXC_Annotation annot = ff.Widget[0];
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
		}
	}
}

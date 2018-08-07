using System;
using System.ComponentModel;
using System.Windows.Forms;
using PDFXCoreAPI;

namespace CoreAPIDemo
{
	[Description("12. Actions")]
	class Actions
	{
		[Description("12.1. Add actions GoTo, GoToR and GoToE as bookmark")]
		static public int AddActionsGoTo_GoToR_GoToE(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}
	}
}
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

		[Description("12.2. Add action Launch as bookmark")]
		static public int AddActionLaunch(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("12.3. Add action URI as bookmark")]
		static public int AddActionURI(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("12.4. Add action JavaScript as bookmark")]
		static public int AddActionJS(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("12.4. Add action Named as bookmark")]
		static public int AddActionNamed(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("12.4. Add action Hide as bookmark")]
		static public int AddActionHide(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}
	}
}
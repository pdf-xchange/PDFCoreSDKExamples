using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreAPIDemo
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void sampleTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			Type thisType = this.GetType();
			MethodInfo theMethod = thisType.GetMethod(e.Node.Name);
			if (theMethod != null)
				theMethod.Invoke(this, null);
		}

		public void createNewDoc()
		{
			int a = 0;
		}
	}
}

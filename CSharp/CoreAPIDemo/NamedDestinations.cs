using System.ComponentModel;

namespace CoreAPIDemo
{
	[Description("10. Named Destinations")]
	class NamedDestinations
	{
		[Description("10.1. Add Named Destination after the currently selected item in the Named Destinations List")]
		static public int AddNewDestination(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_NamedDests;
		}

		[Description("10.2. Remove currently selected Named Destination from the Named Destinations List")]
		static public int RemoveNamedDest(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_NamedDests;
		}

		[Description("10.3. Sort Named Destination List by Name in ascending order")]
		static public int SortDestByNameAscending(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_NamedDests;
		}

		[Description("10.4. Sort Named Destination List by Name in descending order")]
		static public int SortDestByNameDescending(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_NamedDests;
		}

		[Description("10.5. Sort Named Destination List by Page in ascending order")]
		static public int SortDestByPageAscending(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_NamedDests;
		}

		[Description("10.6. Sort Named Destination List by Page in descending order")]
		static public int SortDestByPageDescending(Form1 Parent)
		{
#warning Implement this
			return (int)Form1.eFormUpdateFlags.efuf_NamedDests;
		}
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreAPIDemo
{
	[Description("9. Bookmarks")]
	class Bookmarks
	{
		[Description("9.1. Add Bookmark after the currently selected bookmark in the Bookmarks Tree")]
		static public int AddSiblingBookmark(Form1 Parent)
		{
#warning In case if there is no selected bookmarks - place the bookmark at the end of the tree (do not forget to select it)
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.2. Add Bookmark as a last child of the currently selected bookmark in the Bookmarks Tree")]
		static public int AddChildBookmark(Form1 Parent)
		{
#warning In case if there is no selected bookmarks - place the bookmark at the end of the tree (do not forget to select it)
#warning Also, if there is no children of the currently selected bookmark - add it as a first child
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.3. Remove currently selected bookmark in the Bookmarks Tree with it's children")]
		static public int RemoveSelectedBookmark(Form1 Parent)
		{
#warning In case if there is no selected bookmarks - display a message box that there are no selected bookmarks - place focus on the bookmarksTree control
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.4. Remove currently selected bookmark in the Bookmarks Tree without it's children")]
		static public int RemoveSelectedBookmarkWithoutChildren(Form1 Parent)
		{
#warning This should move the children one level up and then remove the original bookmark
#warning In case if there is no selected bookmarks - display a message box that there are no selected bookmarks - place focus on the bookmarksTree control
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.5. Move Up currently selected bookmark in the Bookmarks Tree")]
		static public int MoveUpSelectedBookmark(Form1 Parent)
		{
#warning This should move the currently selected bookmark on the previous index within it's siblings. If it is in 0 index - move it to the index before it's parent bookmark.
#warning In case if there is no selected bookmarks - display a message box that there are no selected bookmarks - place focus on the bookmarksTree control
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.6. Move Down currently selected bookmark in the Bookmarks Tree")]
		static public int MoveDownSelectedBookmark(Form1 Parent)
		{
#warning This should move the currently selected bookmark on the next index within it's siblings. If it is in last index - move it to the index after it's parent bookmark.
#warning In case if there is no selected bookmarks - display a message box that there are no selected bookmarks - place focus on the bookmarksTree control
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.7. Sort bookmarks by name in the current document")]
		static public int SortBookmarksByName(Form1 Parent)
		{
#warning This should sort the bookmarks by name within the bookmarks' levels. Meaning that each bookmark level should be sorted separately.
#warning In case if there is no selected bookmarks - display a message box that there are no selected bookmarks - place focus on the bookmarksTree control
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.8. Sort bookmarks by page in the current document")]
		static public int SortBookmarksByPage(Form1 Parent)
		{
#warning This should sort the bookmarks by page within the bookmarks' levels. Meaning that each bookmark level should be sorted separately. Note that the last GoTo action should be taken. The bookmarks without actions should be sorted by names and places after the bookmarks with pages
#warning In case if there is no selected bookmarks - display a message box that there are no selected bookmarks - place focus on the bookmarksTree control
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}
	}
}

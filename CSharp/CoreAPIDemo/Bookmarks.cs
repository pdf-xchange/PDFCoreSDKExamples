using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFXCoreAPI;
using System.Windows.Forms;

namespace CoreAPIDemo
{
	[Description("9. Bookmarks")]
	class Bookmarks
	{
		delegate void SortByAnything(SortByAnything sort, IPXC_Bookmark bookmark);

		[Description("9.1. Add Bookmark after the currently selected bookmark in the Bookmarks Tree")]
		static public int AddSiblingBookmark(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;
			IPXC_Bookmark bookmark = (Parent.SelectedBookmarkNode == null) 
				? Parent.m_CurDoc.BookmarkRoot.AddNewChild(true)
				: Parent.SelectedBookmarkNode.m_Bookmark.AddNewSibling(false);
			IPXC_ActionsList aList = Parent.m_CurDoc.CreateActionsList();
			bookmark.Title = (Parent.CurrentPage + 1) + " page";
			bookmark.Style = PXC_BookmarkStyle.BookmarkFont_Normal;
			PXC_Destination dest = new PXC_Destination();
			dest.nPageNum = Parent.CurrentPage;
			dest.nNullFlags = 15;
			dest.nType = PXC_DestType.Dest_Fit;
			aList.AddGoto(dest);
			bookmark.Actions = aList;
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.2. Add Bookmark as a last child of the currently selected bookmark in the Bookmarks Tree")]
		static public int AddChildBookmark(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;

			IPXC_Bookmark bookmark = (Parent.SelectedBookmarkNode == null)
				? Parent.m_CurDoc.BookmarkRoot.AddNewChild(true)
				: Parent.SelectedBookmarkNode.m_Bookmark.AddNewChild((Parent.SelectedBookmarkNode.m_Bookmark.ChildrenCount > 0) 
					? true 
					: false);
			IPXC_ActionsList aList = Parent.m_CurDoc.CreateActionsList();
			bookmark.Title = (Parent.CurrentPage + 1) + " page";
			bookmark.Style = PXC_BookmarkStyle.BookmarkFont_Normal;
			PXC_Destination dest = new PXC_Destination();
			dest.nPageNum = Parent.CurrentPage;
			dest.nNullFlags = 15;
			dest.nType = PXC_DestType.Dest_Fit;
			aList.AddGoto(dest);
			bookmark.Actions = aList;
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.3. Remove currently selected bookmark in the Bookmarks Tree with it's children")]
		static public int RemoveSelectedBookmark(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;

			if (Parent.SelectedBookmarkNode == null)
			{
				MessageBox.Show("There are no selected bookmarks - place focus on the bookmarksTree control.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return 0;
			}
			Parent.SelectedBookmarkNode.m_Bookmark.Unlink();
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.4. Remove currently selected bookmark in the Bookmarks Tree without it's children")]
		static public int RemoveSelectedBookmarkWithoutChildren(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;

			if (Parent.SelectedBookmarkNode == null)
			{
				MessageBox.Show("There are no selected bookmarks - place focus on the bookmarksTree control.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return 0;
			}
			if (Parent.SelectedBookmarkNode.m_Bookmark.ChildrenCount > 0)
			{
				while(Parent.SelectedBookmarkNode.m_Bookmark.ChildrenCount > 0)
				{
					IPXC_Bookmark childBookmark = Parent.SelectedBookmarkNode.m_Bookmark.FirstChild;
					Parent.SelectedBookmarkNode.m_Bookmark.FirstChild.Unlink();
					Parent.SelectedBookmarkNode.m_Bookmark.Parent.AddChild(childBookmark, true);
				}
			}
			Parent.GetBookmarkTree.Focus();
			Parent.SelectedBookmarkNode.m_Bookmark.Unlink();
			Parent.SelectedBookmarkNode.Remove();
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.5. Move Up currently selected bookmark in the Bookmarks Tree")]
		static public int MoveUpSelectedBookmark(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;

			if (Parent.SelectedBookmarkNode == null)
			{
				MessageBox.Show("There are no selected bookmarks - place focus on the bookmarksTree control.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return 0;
			}
			IPXC_Bookmark pxcBookmark = Parent.SelectedBookmarkNode.m_Bookmark;
			if (Parent.SelectedBookmarkNode.m_Bookmark == Parent.m_CurDoc.BookmarkRoot.FirstChild)
			{
				return 0;
			}
			if (Parent.SelectedBookmarkNode.m_Bookmark == Parent.SelectedBookmarkNode.m_Bookmark.Parent.FirstChild)
			{
				IPXC_Bookmark pervBookmark = pxcBookmark.Parent;
				pxcBookmark.Unlink();
				pervBookmark.AddSibling(pxcBookmark, true);
			}
			else
			{
				IPXC_Bookmark pervBookmark = pxcBookmark.Previous;
				pxcBookmark.Unlink();
				pervBookmark.AddSibling(pxcBookmark, true);
			}
			Parent.GetBookmarkTree.Focus();
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.6. Move Down currently selected bookmark in the Bookmarks Tree")]
		static public int MoveDownSelectedBookmark(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;
			if (Parent.SelectedBookmarkNode == null)
			{
				MessageBox.Show("There are no selected bookmarks - place focus on the bookmarksTree control.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return 0;
			}
			IPXC_Bookmark pxcBookmark = Parent.SelectedBookmarkNode.m_Bookmark;
			if (Parent.SelectedBookmarkNode.m_Bookmark == Parent.m_CurDoc.BookmarkRoot.LastChild)
			{
				return 0;
			}
			if (Parent.SelectedBookmarkNode.m_Bookmark == Parent.SelectedBookmarkNode.m_Bookmark.Parent.LastChild)
			{
				IPXC_Bookmark pervBookmark = pxcBookmark.Parent;
				pxcBookmark.Unlink();
				pervBookmark.AddSibling(pxcBookmark, false);
			}
			else
			{
				IPXC_Bookmark pervBookmark = pxcBookmark.Next;
				pxcBookmark.Unlink();
				pervBookmark.AddSibling(pxcBookmark, false);
			}
			Parent.GetBookmarkTree.Focus();
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.7. Sort bookmarks by name in the current document")]
		static public int SortBookmarksByName(Form1 Parent)
		{
			SortByAnything sortByAnything = (sort, root) => {
				List<IPXC_Bookmark> bookmarks = new List<IPXC_Bookmark>();
				while (root.ChildrenCount > 0)
				{
					bookmarks.Add(root.FirstChild);
					root.FirstChild.Unlink();
				}

				bookmarks.Sort(delegate (IPXC_Bookmark firstNode, IPXC_Bookmark secondNode)
				{
					return String.Compare(firstNode.Title, secondNode.Title);
				});

				foreach (IPXC_Bookmark bookmark in bookmarks)
				{
					if (bookmark.ChildrenCount > 0)
					{
						sort(sort, bookmark);
					}
					root.AddChild(bookmark, true);

				}
			};
			if (Parent.m_CurDoc == null)
				return 0;
			sortByAnything(sortByAnything, Parent.m_CurDoc.BookmarkRoot);
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.8. Sort bookmarks by page in the current document")]
		static public int SortBookmarksByPage(Form1 Parent)
		{
			
			//delegate void SortByAnything(SortByAnything sort, IPXC_Bookmark root);
			SortByAnything sortByAnything = (sort, root) => {
				List<Tuple<IPXC_Bookmark, PXC_Destination>> bookmarks = new List<Tuple<IPXC_Bookmark, PXC_Destination>>();
				IPXS_Inst pxcInst = Parent.m_pxcInst.GetExtension("PXS");
				uint typeGoTo = pxcInst.StrToAtom("GoTo");
				while(root.ChildrenCount > 0)
				{
					for (int i = (int)root.FirstChild.Actions.Count - 1; i >= 0; i--)
					{
						if (root.FirstChild.Actions[(uint)i].Type == typeGoTo)
						{
							int MAX_VALUE = int.MaxValue;

							PXC_Destination currDest = (root.FirstChild.Actions[(uint)i] as IPXC_Action_Goto).get_Dest();

							if ((bookmarks.Count == 0) || (currDest.nPageNum > bookmarks[bookmarks.Count - 1].Item2.nPageNum))
							{
								bookmarks.Add(Tuple.Create(root.FirstChild, currDest));
								break;
							}
							if (currDest.nPageNum < bookmarks[0].Item2.nPageNum)
							{
								bookmarks.Insert(0, Tuple.Create(root.FirstChild, currDest));
								break;
							}

							int first = 0;
							int last = bookmarks.Count;

							while (first < last)
							{
								int mid = first + (last - first) / 2;
								if (currDest.nPageNum == bookmarks[mid].Item2.nPageNum)
								{
									if ((MAX_VALUE == currDest.nPageNum) && (MAX_VALUE == bookmarks[mid].Item2.nPageNum))
									{
										if (String.Compare(root.FirstChild.Title, bookmarks[mid].Item1.Title) == 1)
										{
											last = mid;
										}
										else
										{
											first = mid + 1;
										}
									}
									else
									{
										if (currDest.dValues[1] > bookmarks[mid].Item2.dValues[1])
										{
											last = mid;
										}
										else
										{
											first = mid + 1;
										}
									}

								}
								else if (currDest.nPageNum < bookmarks[mid].Item2.nPageNum)
								{
									last = mid;
								}
								else
								{
									first = mid + 1;
								}
							}
							bookmarks.Insert(last, Tuple.Create(root.FirstChild, currDest));
						}

					}
					root.FirstChild.Unlink();
				}

				foreach(Tuple<IPXC_Bookmark, PXC_Destination> bookmark in bookmarks)
				{
					root.AddChild(bookmark.Item1, true);
					if (bookmark.Item1.ChildrenCount > 0)
					{
						sort(sort, bookmark.Item1);
					}				
				}
			};
			if (Parent.m_CurDoc == null)
				return 0;
			Parent.GetBookmarkTree.Nodes.Clear();
			sortByAnything(sortByAnything, Parent.m_CurDoc.BookmarkRoot);
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}
	}
}

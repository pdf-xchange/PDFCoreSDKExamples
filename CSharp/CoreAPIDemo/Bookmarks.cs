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
		delegate void AddDefaultBookmark();
		delegate void SortByAnything(SortByAnything sort, IPXC_Bookmark bookmark);

		[Description("9.1. Add Bookmark after the currently selected bookmark in the Bookmarks Tree")]
		static public int AddSiblingBookmark(Form1 Parent)
		{
			AddDefaultBookmark addDefaultBookmark = () =>
			{
				IPXC_Bookmark pxcBookmark = Parent.m_CurDoc.BookmarkRoot.AddNewChild(true);
				IPXC_ActionsList pxcAList = Parent.m_CurDoc.CreateActionsList();
				pxcBookmark.Title = "Last Page";
				pxcBookmark.Style = PXC_BookmarkStyle.BookmarkFont_Normal;
				PXC_Destination pxcDest = new PXC_Destination();
				pxcDest.nPageNum = Parent.m_CurDoc.Pages.Count - 1;
				pxcDest.nNullFlags = 15;
				pxcDest.nType = PXC_DestType.Dest_Fit;
				pxcAList.AddGoto(pxcDest);
				pxcBookmark.Actions = pxcAList;
				Form1.BookmarkNode bmLastNode = new Form1.BookmarkNode(pxcBookmark);
				bmLastNode.Name = "Bookmark " + (Parent.GetBookmarkTree.Nodes.Count + 1);
				bmLastNode.Text = pxcBookmark.Title;
				bmLastNode.ImageIndex = 0;
				bmLastNode.SelectedImageIndex = 0;
				Parent.GetBookmarkTree.Nodes.Add(bmLastNode);
				Parent.SelectedBookmarkNode = bmLastNode;
				Parent.GetBookmarkTree.Focus();
			};

			if (Parent.m_CurDoc == null)
				return 0;

			if (Parent.SelectedBookmarkNode == null)
			{
				addDefaultBookmark();
				return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
			}

			IPXC_Bookmark bookmark = Parent.SelectedBookmarkNode.m_Bookmark.Parent.AddNewChild(true);
			IPXC_ActionsList aList = Parent.m_CurDoc.CreateActionsList();
			bookmark.Title = (Parent.CurrentPage + 1) + " page";
			bookmark.Style = PXC_BookmarkStyle.BookmarkFont_Normal;
			PXC_Destination dest = new PXC_Destination();
			dest.nPageNum = Parent.CurrentPage;
			dest.nNullFlags = 15;
			dest.nType = PXC_DestType.Dest_Fit;
			aList.AddGoto(dest);
			bookmark.Actions = aList;
			Form1.BookmarkNode bmNode = new Form1.BookmarkNode(bookmark);
			bmNode.Name = Parent.SelectedBookmarkNode.Name + "." + (Parent.SelectedBookmarkNode.Nodes.Count + 1);
			bmNode.Text = bookmark.Title;
			bmNode.ImageIndex = 0;
			bmNode.SelectedImageIndex = 0;
			if (Parent.SelectedBookmarkNode.Parent == null)
				Parent.GetBookmarkTree.Nodes.Add(bmNode);
			else
				Parent.SelectedBookmarkNode.Parent.Nodes.Add(bmNode);
			Parent.SelectedBookmarkNode = bmNode;
			Parent.GetBookmarkTree.Focus();
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.2. Add Bookmark as a last child of the currently selected bookmark in the Bookmarks Tree")]
		static public int AddChildBookmark(Form1 Parent)
		{
			AddDefaultBookmark addDefaultBookmark = () =>
			{
				IPXC_Bookmark pxcBookmark = Parent.m_CurDoc.BookmarkRoot.AddNewChild(true);
				IPXC_ActionsList pxcAList = Parent.m_CurDoc.CreateActionsList();
				pxcBookmark.Title = "Last Page";
				pxcBookmark.Style = PXC_BookmarkStyle.BookmarkFont_Normal;
				PXC_Destination pxcDest = new PXC_Destination();
				pxcDest.nPageNum = Parent.m_CurDoc.Pages.Count - 1;
				pxcDest.nNullFlags = 15;
				pxcDest.nType = PXC_DestType.Dest_Fit;
				pxcAList.AddGoto(pxcDest);
				pxcBookmark.Actions = pxcAList;
				Form1.BookmarkNode bmLastNode = new Form1.BookmarkNode(pxcBookmark);
				bmLastNode.Name = "Bookmark " + (Parent.GetBookmarkTree.Nodes.Count + 1);
				bmLastNode.Text = pxcBookmark.Title;
				bmLastNode.ImageIndex = 0;
				bmLastNode.SelectedImageIndex = 0;
				Parent.GetBookmarkTree.Nodes.Add(bmLastNode);
				Parent.SelectedBookmarkNode = bmLastNode;
				Parent.GetBookmarkTree.Focus();
			};

			if (Parent.m_CurDoc == null)
				return 0;

			if (Parent.SelectedBookmarkNode == null)
			{
				addDefaultBookmark();
				return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
			}
			IPXC_Bookmark bookmark = Parent.SelectedBookmarkNode.m_Bookmark.AddNewChild(Parent.SelectedBookmarkNode.m_Bookmark.ChildrenCount > 0 ? true : false);
			IPXC_ActionsList aList = Parent.m_CurDoc.CreateActionsList();
			bookmark.Title = (Parent.CurrentPage + 1) + " page";
			bookmark.Style = PXC_BookmarkStyle.BookmarkFont_Normal;
			PXC_Destination dest = new PXC_Destination();
			dest.nPageNum = Parent.CurrentPage;
			dest.nNullFlags = 15;
			dest.nType = PXC_DestType.Dest_Fit;
			aList.AddGoto(dest);
			bookmark.Actions = aList;
			Form1.BookmarkNode bmNode = new Form1.BookmarkNode(bookmark);
			bmNode.Name = Parent.SelectedBookmarkNode.Name + "." + (Parent.SelectedBookmarkNode.Nodes.Count + 1);
			bmNode.Text = bookmark.Title;
			bmNode.ImageIndex = 0;
			bmNode.SelectedImageIndex = 0;
			Parent.SelectedBookmarkNode.Nodes.Add(bmNode);
			Parent.SelectedBookmarkNode = bmNode;
			Parent.GetBookmarkTree.Focus();
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
			Parent.SelectedBookmarkNode.Remove();
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
			Form1.BookmarkNode parentBookmark = Parent.SelectedBookmarkNode.Parent as Form1.BookmarkNode;
			TreeView treeView = Parent.SelectedBookmarkNode.TreeView;

			if (Parent.SelectedBookmarkNode.m_Bookmark.ChildrenCount > 0)
			{
				while(Parent.SelectedBookmarkNode.m_Bookmark.ChildrenCount > 0)
				{
					if (parentBookmark != null)
					{
						Form1.BookmarkNode cbNode = Parent.SelectedBookmarkNode.Nodes[0] as Form1.BookmarkNode;
						Parent.SelectedBookmarkNode.Nodes.RemoveAt(0);
						parentBookmark.Nodes.Add(cbNode);
						IPXC_Bookmark childBookmark = Parent.SelectedBookmarkNode.m_Bookmark.FirstChild;
						Parent.SelectedBookmarkNode.m_Bookmark.FirstChild.Unlink();
						Parent.SelectedBookmarkNode.m_Bookmark.Parent.AddChild(childBookmark, true);
					}
					else
					{
						Form1.BookmarkNode cbNode = Parent.SelectedBookmarkNode.Nodes[0] as Form1.BookmarkNode;
						Parent.SelectedBookmarkNode.Nodes.RemoveAt(0);
						treeView.Nodes.Add(cbNode);
						IPXC_Bookmark childBookmark = Parent.SelectedBookmarkNode.m_Bookmark.FirstChild;
						Parent.SelectedBookmarkNode.m_Bookmark.FirstChild.Unlink();
						Parent.SelectedBookmarkNode.m_Bookmark.Parent.AddChild(childBookmark, true);
					}
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
			Form1.BookmarkNode Node = Parent.SelectedBookmarkNode;
			Form1.BookmarkNode pervNode = Node.Parent as Form1.BookmarkNode;
			IPXC_Bookmark pxcBookmark = Node.m_Bookmark;
			if (Parent.SelectedBookmarkNode.m_Bookmark == Parent.SelectedBookmarkNode.m_Bookmark.Parent.FirstChild)
			{
				Form1.BookmarkNode pervPervNode = Node.Parent.Parent as Form1.BookmarkNode;
				TreeView view = Node.Parent.TreeView;

				int index = 0;
				if (pervPervNode == null)
				{
					index = view.Nodes.IndexOf(pervNode);
					pervNode.Nodes.RemoveAt(0);
					view.Nodes.Insert(index, Node);					
				}
				else
				{
					index = pervPervNode.Nodes.IndexOf(pervNode);
					pervNode.Nodes.RemoveAt(0);
					pervPervNode.Nodes.Insert(index, Node);
				}
				Parent.SelectedBookmarkNode = Node;
				IPXC_Bookmark pervBookmark = pxcBookmark.Parent;
				pxcBookmark.Unlink();
				pervBookmark.AddSibling(pxcBookmark, true);
			}
			else
			{
				TreeView view = Node.TreeView;

				int index = 0;
				if (pervNode == null)
				{
					index = view.Nodes.IndexOf(Node);
					view.Nodes.RemoveAt(index);
					view.Nodes.Insert(index - 1, Node);
				}
				else
				{
					index = pervNode.Nodes.IndexOf(Node);
					pervNode.Nodes.RemoveAt(index);
					pervNode.Nodes.Insert(index - 1, Node);
				}
				Parent.SelectedBookmarkNode = Node;
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
			Form1.BookmarkNode Node = Parent.SelectedBookmarkNode;
			Form1.BookmarkNode nextNode = Node.Parent as Form1.BookmarkNode;
			IPXC_Bookmark pxcBookmark = Node.m_Bookmark;
			if (Parent.SelectedBookmarkNode.m_Bookmark == Parent.SelectedBookmarkNode.m_Bookmark.Parent.LastChild)
			{
				Form1.BookmarkNode pervNextNode = Node.Parent.Parent as Form1.BookmarkNode;
				TreeView view = Node.Parent.TreeView;

				int index = 0;
				if (pervNextNode == null)
				{
					index = view.Nodes.IndexOf(nextNode);
					nextNode.Nodes.RemoveAt(nextNode.Nodes.Count - 1);
					view.Nodes.Insert(index + 1, Node);
					Parent.SelectedBookmarkNode = Node;
				}
				else
				{
					index = pervNextNode.Nodes.IndexOf(nextNode);
					nextNode.Nodes.RemoveAt(nextNode.Nodes.Count - 1);
					pervNextNode.Nodes.Insert(index + 1, Node);
					Parent.SelectedBookmarkNode = Node;
				}
				IPXC_Bookmark pervBookmark = pxcBookmark.Parent;
				pxcBookmark.Unlink();
				pervBookmark.AddSibling(pxcBookmark, false);
			}
			else
			{
				TreeView view = Node.TreeView;

				int index = 0;
				if (nextNode == null)
				{
					index = view.Nodes.IndexOf(Node);
					view.Nodes.RemoveAt(index);
					view.Nodes.Insert(index + 1, Node);
					Parent.SelectedBookmarkNode = Node;
				}
				else
				{
					index = nextNode.Nodes.IndexOf(Node);
					nextNode.Nodes.RemoveAt(index);
					nextNode.Nodes.Insert(index + 1, Node);
					Parent.SelectedBookmarkNode = Node;
				}
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

			Parent.UpdateControlsFromDocument(0xff);

#warning This should sort the bookmarks by name within the bookmarks' levels. Meaning that each bookmark level should be sorted separately.
#warning In case if there is no selected bookmarks - display a message box that there are no selected bookmarks - place focus on the bookmarksTree control
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.8. Sort bookmarks by page in the current document")]
		static public int SortBookmarksByPage(Form1 Parent)
		{
			//delegate void SortByAnything(SortByAnything sort, Form1.BookmarkNode node);
			SortByAnything sortByAnything = (sort, root) => {
				List<IPXC_Bookmark> bookmarks = new List<IPXC_Bookmark>();
				while(root.ChildrenCount > 0)
				{
					bookmarks.Add(root.FirstChild);
					root.FirstChild.Unlink();
				}

				bookmarks.Sort(delegate (IPXC_Bookmark firstNode, IPXC_Bookmark secondNode)
				{
					IPXS_Inst pxcInst = Parent.m_pxcInst.GetExtension("PXS");
					uint typeGoTo = pxcInst.StrToAtom("GoTo");
					IPXC_Action_Goto faGoTo = null;
					IPXC_Action_Goto saGoTo = null;
					for (uint i = firstNode.Actions.Count - 1; i >= 0; i--)
					{
						if (firstNode.Actions[i].Type == typeGoTo)
						{
							faGoTo = (IPXC_Action_Goto)firstNode.Actions[i];
							break;
						}
					}
					for (uint i = secondNode.Actions.Count - 1; i >= 0; i--)
					{
						if (secondNode.Actions[i].Type == typeGoTo)
						{
							saGoTo = (IPXC_Action_Goto)secondNode.Actions[i];
							break;
						}
					}
					if (faGoTo.get_Dest().nPageNum < saGoTo.get_Dest().nPageNum)
						return -1;
					if (faGoTo.get_Dest().nPageNum < saGoTo.get_Dest().nPageNum)
						return 1;
					return 0;
				});

				foreach(IPXC_Bookmark bookmark in bookmarks)
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

			Parent.UpdateControlsFromDocument(0xff);
#warning This should sort the bookmarks by page within the bookmarks' levels. Meaning that each bookmark level should be sorted separately. Note that the last GoTo action should be taken. The bookmarks without actions should be sorted by names and places after the bookmarks with pages
#warning In case if there is no selected bookmarks - display a message box that there are no selected bookmarks - place focus on the bookmarksTree control
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}
	}
}

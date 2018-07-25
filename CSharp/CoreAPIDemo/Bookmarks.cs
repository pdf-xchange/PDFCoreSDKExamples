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
		delegate void SortByAnything(SortByAnything sort, IPXC_Bookmark bookmark, uint actionType);
		delegate double[] GetXYFromDestination(IPXC_Bookmark bookmark, PXC_Destination dest);

		[Description("9.1. Add Bookmark after the currently selected bookmark in the Bookmarks Tree")]
		static public int AddSiblingBookmark(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;
			IPXC_Bookmark bookmark = null;
			if (Parent.SelectedBookmarkNode == null)
				bookmark = Parent.m_CurDoc.BookmarkRoot.AddNewChild(true);
			else
				bookmark = Parent.SelectedBookmarkNode.m_Bookmark.AddNewSibling(false);
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

			IPXC_Bookmark bookmark = null;
			if (Parent.SelectedBookmarkNode == null)
				bookmark = Parent.m_CurDoc.BookmarkRoot.AddNewChild(true);
			else
				bookmark = Parent.SelectedBookmarkNode.m_Bookmark.AddNewChild((Parent.SelectedBookmarkNode.m_Bookmark.ChildrenCount > 0));
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
				MessageBox.Show("There are no selected bookmarks - please select a bookmark from the Bookmarks Tree", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
				MessageBox.Show("There are no selected bookmarks - please select a bookmark from the Bookmarks Tree", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return 0;
			}
			IPXC_Bookmark selBookmark = Parent.SelectedBookmarkNode.m_Bookmark;

			while (selBookmark.ChildrenCount > 0)
			{
				IPXC_Bookmark childBookmark = selBookmark.FirstChild;
				selBookmark.FirstChild.Unlink();
				selBookmark.AddSibling(childBookmark, false);
			}
			selBookmark.Unlink();
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.5. Move Up currently selected bookmark in the Bookmarks Tree")]
		static public int MoveUpSelectedBookmark(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;

			if (Parent.SelectedBookmarkNode == null)
			{
				MessageBox.Show("There are no selected bookmarks - please select a bookmark from the Bookmarks Tree", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return 0;
			}
			IPXC_Bookmark pxcBookmark = Parent.SelectedBookmarkNode.m_Bookmark;
			if (pxcBookmark == Parent.m_CurDoc.BookmarkRoot.FirstChild)
				return 0;

			if (pxcBookmark == pxcBookmark.Parent.FirstChild)
			{
				IPXC_Bookmark parent = pxcBookmark.Parent;
				pxcBookmark.Unlink();
				parent.AddSibling(pxcBookmark, true);
			}
			else
			{
				IPXC_Bookmark prevBookmark = pxcBookmark.Previous;
				pxcBookmark.Unlink();
				prevBookmark.AddSibling(pxcBookmark, true);
			}
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.6. Move Down currently selected bookmark in the Bookmarks Tree")]
		static public int MoveDownSelectedBookmark(Form1 Parent)
		{
			if (Parent.m_CurDoc == null)
				return 0;
			if (Parent.SelectedBookmarkNode == null)
			{
				MessageBox.Show("There are no selected bookmarks - please select a bookmark from the Bookmarks Tree", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return 0;
			}
			IPXC_Bookmark pxcBookmark = Parent.SelectedBookmarkNode.m_Bookmark;
			if (pxcBookmark == Parent.m_CurDoc.BookmarkRoot.LastChild)
				return 0;

			if (pxcBookmark == pxcBookmark.Parent.LastChild)
			{
				IPXC_Bookmark parent = pxcBookmark.Parent;
				pxcBookmark.Unlink();
				parent.AddSibling(pxcBookmark, false);
			}
			else
			{
				IPXC_Bookmark nextBookmark = pxcBookmark.Next;
				pxcBookmark.Unlink();
				nextBookmark.AddSibling(pxcBookmark, false);
			}
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.7. Sort bookmarks by name in the current document")]
		static public int SortBookmarksByName(Form1 Parent)
		{
			SortByAnything sortByAnything = (sort, root, actionType) => {
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
						sort(sort, bookmark, actionType);
					}
					root.AddChild(bookmark, true);

				}
			};
			if (Parent.m_CurDoc == null)
				return 0;
			sortByAnything(sortByAnything, Parent.m_CurDoc.BookmarkRoot, 0);
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}

		[Description("9.8. Sort bookmarks by page in the current document")]
		static public int SortBookmarksByPage(Form1 Parent)
		{
			//delegate double[] GetXYFromDestination(IPXC_Bookmark bookmark, PXC_Destination dest);
			GetXYFromDestination getXYFromDestination = (IPXC_Bookmark book, PXC_Destination destination) => {
				PXC_DestType Type = destination.nType;
				double[] retValue = new double[2]; // retValue[0] - X, retValue[1] - Y
				PXC_Rect contentBBox = book.Document.Pages[destination.nPageNum].get_Box(PXC_BoxType.PBox_BBox);
				PXC_Rect pageBBox = book.Document.Pages[destination.nPageNum].get_Box(PXC_BoxType.PBox_PageBox);
				switch (Type)
				{
					case PXC_DestType.Dest_XYZ:
						{
							retValue[0] = destination.dValues[0];
							retValue[1] = destination.dValues[1];
							break;
						}
					case PXC_DestType.Dest_Fit:
						{
							retValue[0] = pageBBox.left;
							retValue[1] = pageBBox.top;
							break;
						}
					case PXC_DestType.Dest_FitH:
						{
							retValue[0] = pageBBox.left;
							retValue[1] = destination.dValues[1];
							break;
						}
					case PXC_DestType.Dest_FitV:
						{
							retValue[0] = destination.dValues[0];
							retValue[1] = pageBBox.top;
							break;
						}
					case PXC_DestType.Dest_FitR:
						{
							retValue[0] = destination.dValues[0];
							retValue[1] = destination.dValues[3];
							break;
						}
					case PXC_DestType.Dest_FitB:
						{
							retValue[0] = contentBBox.left;
							retValue[1] = contentBBox.top;
							break;
						}
					case PXC_DestType.Dest_FitBH:
						{
							retValue[0] = contentBBox.left;
							retValue[1] = destination.dValues[1];
							break;
						}
					case PXC_DestType.Dest_FitBV:
						{
							retValue[0] = destination.dValues[0];
							retValue[1] = contentBBox.top;
							break;
						}
					default:
						{
							retValue[0] = pageBBox.left;
							retValue[1] = pageBBox.top;
							break;
						}
				}
				return retValue;
			};
			//delegate void SortByAnything(SortByAnything sort, IPXC_Bookmark root);
			SortByAnything sortByAnything = (sort, root, actionType) => {
				List<Tuple<IPXC_Bookmark, PXC_Destination>> bookmarks = new List<Tuple<IPXC_Bookmark, PXC_Destination>>();
				while (root.ChildrenCount > 0)
				{
					for (int i = (int)root.FirstChild.Actions.Count - 1; i >= 0; i--)
					{
						if (root.FirstChild.Actions[(uint)i].Type == actionType)
						{
							int MAX_VALUE = int.MaxValue;
							IPXC_Action_Goto actionGoTo = root.FirstChild.Actions[(uint)i] as IPXC_Action_Goto;
							PXC_Destination currDest = actionGoTo.IsNamedDest
								? Parent.m_CurDoc.GetNamedDestination(actionGoTo.DestName)
								: actionGoTo.get_Dest();

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
										double[] currentBookmarkXY = getXYFromDestination(root.FirstChild, currDest);
										double[] bookmarkXY_FromList = getXYFromDestination(bookmarks[mid].Item1, bookmarks[mid].Item2);
										if (currentBookmarkXY[1] < bookmarkXY_FromList[1])
										{
											first = mid + 1;
										}
										else if (currentBookmarkXY[1] > bookmarkXY_FromList[1])
										{
											last = mid;
										}
										else
										{
											if (currentBookmarkXY[0] < bookmarkXY_FromList[0])
											{
												last = mid;
											}
											else
											{
												first = mid + 1;
											}
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
						sort(sort, bookmark.Item1, actionType);
					}				
				}
			};
			if (Parent.m_CurDoc == null)
				return 0;

			IPXS_Inst pxsInst = Parent.m_pxcInst.GetExtension("PXS") as IPXS_Inst;
			uint nGoTo = pxsInst.StrToAtom("GoTo");
			sortByAnything(sortByAnything, Parent.m_CurDoc.BookmarkRoot, nGoTo);
			return (int)Form1.eFormUpdateFlags.efuf_Bookmarks;
		}
	}
}

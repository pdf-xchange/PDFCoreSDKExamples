// PDFPreviewView.cpp : implementation file
//

#include "stdafx.h"
#include "MFCSample.h"
#include "PDFPreviewView.h"
#include "InitializeSDK.h"

static const LONG g_XGap = 5;
static const LONG g_YGap = 5;

// CPDFPreviewView

IMPLEMENT_DYNAMIC(CPDFPreviewView, CWnd)

CPDFPreviewView::CPDFPreviewView()
{
	m_nCurPage		= 0;
	m_ZoomLevel		= 100.0;
	m_nCoef			= 1.0;
	m_szPageSize.cx	=
	m_szPageSize.cy	= 0;
	m_ptOffset.x	=
	m_ptOffset.y	= 0;
	//
	m_rCachedRect.SetRectEmpty();
	m_bgColor		= GetSysColor(COLOR_APPWORKSPACE);
}

CPDFPreviewView::~CPDFPreviewView()
{
}

//////////////////////////////////////////////////////////////////////////

// CPDFPreviewView diagnostics

#ifdef _DEBUG
void CPDFPreviewView::AssertValid() const
{
	__super::AssertValid();
}

#ifndef _WIN32_WCE
void CPDFPreviewView::Dump(CDumpContext& dc) const
{
	__super::Dump(dc);
}
#endif
#endif //_DEBUG

//////////////////////////////////////////////////////////////////////////
BOOL CPDFPreviewView::Create(CWnd* pParentWnd, RECT& rc)
{
	const DWORD dwWndStyle = WS_CHILD | WS_VISIBLE | WS_VSCROLL | WS_HSCROLL;
	const DWORD dwExtStyle = 0;

	CString strMyClass;
	strMyClass = AfxRegisterWndClass(CS_VREDRAW | CS_HREDRAW,
									 ::LoadCursor(NULL, IDC_HAND),
									 (HBRUSH)::GetStockObject(WHITE_BRUSH),
									 0);

	BOOL bOK = __super::CreateEx(dwExtStyle, strMyClass, _T(""), dwWndStyle, rc, pParentWnd, 0, nullptr);
	return bOK;
}

void CPDFPreviewView::ReleaseCache()
{
	m_pCache = nullptr;
	m_rCachedRect.SetRectEmpty();
}

const int CacheStep	=	64;

bool CPDFPreviewView::EnsureCache(const CRect& pr)
{
	if (m_pCache != nullptr)
	{
		CRect ir;
		if (ir.IntersectRect(m_rCachedRect, pr) && ir.EqualRect(pr))
			return true;// m_rCachedRect contain pr
	}
	// todo: make more advanced cache
	// for now - rerender all
	m_pCache = nullptr;
	m_rCachedRect.SetRectEmpty();
	if (m_pDoc == nullptr)
		return false;
	CRect r = pr;
	r.InflateRect(CacheStep, CacheStep);
	r.IntersectRect(r, CRect(0, 0, m_szPageSize.cx, m_szPageSize.cy));
	CComPtr<PXC::IIXC_Page> temp;
	HRESULT hr = g_ImgCore->Page_CreateEmpty(r.Width(), r.Height(), PXC::PageFormat_8RGB, RGB(255, 255, 255), &temp);
	if (FAILED(hr))
		return false;
	CComPtr<PXC::IPXC_Pages> pages;
	CComPtr<PXC::IPXC_Page> page;
	hr = m_pDoc->get_Pages(&pages);
	if (FAILED(hr))
		return false;
	hr = pages->get_Item(m_nCurPage, &page);
	if (FAILED(hr))
		return false;
	hr = page->DrawToIXCPage(temp, r, &m_Matrix, nullptr, nullptr, nullptr);
	if (FAILED(hr))
		return false;
	m_pCache = temp;
	m_rCachedRect = r;
	return true;
}

bool CPDFPreviewView::FixupScrolls(CPoint& offs) const
{
	CRect cr;
	CSize ts = GetTotalSize();
	GetClientRect(cr);
	LONG mx = max(0, ts.cx - cr.Width());
	LONG my = max(0, ts.cy - cr.Height());
	if ((offs.x <= mx) && (offs.y <= my))
		return false;
	if (offs.x > mx)
		offs.x = mx;
	if (offs.y > my)
		offs.y = my;
	return true;
}

CRect CPDFPreviewView::GetPageRect() const
{
	CRect pr, cr;
	CSize ts = GetTotalSize();
	GetClientRect(cr);
	// X
	if (ts.cx <= cr.Width())
		pr.left = (cr.right + cr.left - m_szPageSize.cx) / 2;
	else
		pr.left = g_XGap - m_ptOffset.x;
	pr.right = pr.left + m_szPageSize.cx;
	// Y
	if (ts.cy <= cr.Height())
		pr.top = (cr.bottom + cr.top - m_szPageSize.cy) / 2;
	else
		pr.top = g_YGap - m_ptOffset.y;
	pr.bottom = pr.top + m_szPageSize.cy;
	return pr;
}

CSize CPDFPreviewView::GetTotalSize() const
{
	CSize sz = m_szPageSize;
	sz.cx += 2 * g_XGap;
	sz.cy += 2 * g_YGap;
	return sz;
}

void CPDFPreviewView::UpdateScrolls()
{
	CRect cr;
	CSize sz = GetTotalSize();
	GetClientRect(cr);
	SCROLLINFO si = {0};
	si.cbSize	= sizeof(si);
	si.fMask	= SIF_RANGE | SIF_PAGE | SIF_POS | SIF_DISABLENOSCROLL;
	si.nMin		= 0;
	// horizontal
	si.nMax		= sz.cx - 1;
	si.nPage	= cr.Width();
	si.nPos		= m_ptOffset.x;
	SetScrollInfo(SB_HORZ, &si);
	// vertical
	si.nMax		= sz.cy - 1;
	si.nPage	= cr.Height();
	si.nPos		= m_ptOffset.y;
	SetScrollInfo(SB_VERT, &si);
}

void CPDFPreviewView::SetDocument(PXC::IPXC_Document* pDoc)
{
	if (m_pDoc == pDoc)
		return;
	ReleaseCache();
	m_pDoc = pDoc;
	m_nCurPage = 0;
	m_ptOffset.x = m_ptOffset.y = 0;
	m_szPageSize = CalcPageSize();
	UpdateScrolls();
	Invalidate();
}

void CPDFPreviewView::SetCurPage(ULONG nPage)
{
	if (m_pDoc == nullptr)
		return;
	CComPtr<PXC::IPXC_Pages> pages;
	m_pDoc->get_Pages(&pages);
	ULONG nCount = 0;
	pages->get_Count(&nCount);
	if (nPage >= nCount)
		return;
	if (nPage == m_nCurPage)
		return;
	ReleaseCache();
	m_ptOffset.x = m_ptOffset.y = 0;
	m_szPageSize = CalcPageSize();
	UpdateScrolls();
	Invalidate();
}

void CPDFPreviewView::SetZoom(double nZoomLevel)
{
	CDC* pDC = GetWindowDC();
	LONG nDPI = pDC->GetDeviceCaps(LOGPIXELSY);
	double coef = (double)nDPI * nZoomLevel / 7200.0;
	ReleaseDC(pDC);
	if (coef == m_nCoef)
		return;
	ReleaseCache();
	LONG nx = (LONG)(((double)m_ptOffset.x / m_nCoef) * coef + 0.5);
	LONG ny = (LONG)(((double)m_ptOffset.y / m_nCoef) * coef + 0.5);
	m_nCoef = coef;
	m_ZoomLevel = nZoomLevel;
	m_szPageSize = CalcPageSize();
	m_ptOffset.x = nx;
	m_ptOffset.y = ny;
	FixupScrolls(m_ptOffset);
	UpdateScrolls();
	Invalidate();
}

CSize CPDFPreviewView::CalcPageSize()
{
	CSize sz;
	sz.cx = sz.cy = 0;
	if (m_pDoc == nullptr)
		return sz;
	CComPtr<PXC::IPXC_Pages> pages;
	CComPtr<PXC::IPXC_Page> page;
	m_pDoc->get_Pages(&pages);
	pages->get_Item(m_nCurPage, &page);
	double pw, ph;
	page->GetDimension(&pw, &ph);

	sz.cx = (LONG)(pw * m_nCoef + 0.5);
	sz.cy = (LONG)(ph * m_nCoef + 0.5);
	page->GetMatrix(PXC::PBox_PageBox, &m_Matrix);
	m_Matrix.a *= m_nCoef;
	m_Matrix.b *= -m_nCoef;
	m_Matrix.c *= m_nCoef;
	m_Matrix.d *= -m_nCoef;
	m_Matrix.e *= m_nCoef;
	m_Matrix.f = (double)sz.cy - m_Matrix.f * m_nCoef;

	return sz;
}

CSize CPDFPreviewView::GetPageSize()
{
	return m_szPageSize;
}

// CPDFPreviewView drawing
void CPDFPreviewView::OnInitialUpdate()
{
	SetZoom(100.0);
	CSize sizeTotal = GetPageSize();
//	SetScrollSizes(MM_TEXT, sizeTotal);
}

void CPDFPreviewView::OnDraw(CDC* pDC)
{
	if (m_pDoc == nullptr)
	{
		return;
	}
	//
	CComPtr<PXC::IPXC_Pages> pages;
	CComPtr<PXC::IPXC_Page> page;
	m_pDoc->get_Pages(&pages);
	pages->get_Item(m_nCurPage, &page);

	CSize psz = GetPageSize();
	CPoint sp = {0, 0};// GetScrollPosition();
	CRect cr;
	GetClientRect(cr);

	CRect dr;
	PXC::PXC_Matrix m;
	m.a = 1.0;
	m.b = 0.0;
	m.c = 0.0;
	m.d = 1.0;
	m.e = 0.0;
	m.f = 0.0;

	// scale
	m.a = m_nCoef;
	m.d = -m_nCoef;

	LONG cw = cr.Width();
	LONG ch = cr.Height();

	if (psz.cx < cw)
	{
		dr.left = (cw - psz.cx) / 2;
		dr.right = dr.left + psz.cx;
		m.e = dr.left;
	}
	else
	{
		dr.left = 0;
		dr.right = cw;
		m.e = -sp.x;
	}

	if (psz.cy < ch)
	{
		dr.top = (ch - psz.cy) / 2;
		dr.bottom = dr.top + psz.cy;
		m.f = dr.bottom;
	}
	else
	{
		dr.top = 0;
		dr.bottom = cr.Height();
		m.f = dr.bottom;
	}

	page->DrawToDevice((PXC::HANDLE_T)pDC->m_hDC, &dr, &m, nullptr, nullptr, nullptr);
}

// CPDFPreviewView message handlers
afx_msg BOOL CPDFPreviewView::OnEraseBkgnd(CDC* pDC)
{
	if (pDC == nullptr)
		return TRUE;
	CRect cr, r;
	GetClientRect(cr);
	r = cr;
	CRgn rgn;
	rgn.CreateRectRgn(r.left, r.top, r.right, r.bottom);
	int res = SIMPLEREGION;
	if (m_pDoc != nullptr)
	{
		CRect pr = GetPageRect();
		CRgn rgn2;
		rgn2.CreateRectRgn(pr.left, pr.top, pr.right, pr.bottom);
		res = rgn.CombineRgn(&rgn, &rgn2, RGN_DIFF);
		rgn2.DeleteObject();
	}
	if ((res != NULLREGION) && (res != ERROR))
	{
		CBrush br(m_bgColor);
		pDC->FillRgn(&rgn, &br);
	}
	return TRUE;
}

afx_msg int CPDFPreviewView::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (__super::OnCreate(lpCreateStruct) == -1)
		return -1;

	OnInitialUpdate();
	return 0;
}

afx_msg void CPDFPreviewView::OnSize(UINT nType, int cx, int cy)
{
	__super::OnSize(nType, cx, cy);
	UpdateScrolls();
}

void CPDFPreviewView::PaintRect(CDC* pDC, const CRect& paintRect)
{
	CRect pageRect = GetPageRect();
	CRect dr;
	dr.IntersectRect(pageRect, paintRect);
	if (dr.IsRectEmpty())
		return;
	// now lets calculate how dr maps to entire page in pixels
	CRect client;
	GetClientRect(client);
	client.OffsetRect(-pageRect.left, -pageRect.top);
	CRect cacheRect = dr;
	cacheRect.OffsetRect(-pageRect.left, -pageRect.top);
	if (EnsureCache(client))
	{
		// draw cached image
		m_pCache->DrawToDC((PXC::HANDLE_T)pDC->m_hDC, dr.left, dr.top, dr.Width(), dr.Height(),
						   cacheRect.left - m_rCachedRect.left, cacheRect.top - m_rCachedRect.top, 0);
	}
	else
	{
		pDC->FillSolidRect(&dr, RGB(255, 0, 0));
	}
}

afx_msg void CPDFPreviewView::OnPaint()
{
	// to have better performance we will redraw only portions that need to be redrawn
	// we have to get these regions before BeginPaint would be called

	CRect r;
	GetUpdateRect(&r);
	CRgn rgn;
	rgn.CreateRectRgnIndirect(&r);
	int reg = GetUpdateRgn(&rgn, FALSE);
	RGNDATA* data = nullptr;
	if (reg == COMPLEXREGION)
	{
		DWORD sz = GetRegionData(rgn, 0, NULL);
		if (sz)
		{
			data = (RGNDATA*)malloc(sz);
			GetRegionData(rgn, sz, data);
		}
		else
		{
			reg = ERROR;
		}
	}
	DeleteObject(rgn);
	//
	CPaintDC dc(this);
	if (reg == ERROR)
		return;
	if (data && (data->rdh.nCount > 0))
	{
		RECT* pr = (RECT*)data->Buffer;
		for (DWORD i = 0; i < data->rdh.nCount; i++)
		{
			PaintRect(&dc, pr[i]);
		}
	}
	else
	{
		if (r.IsRectEmpty())
			GetClientRect(r);
		PaintRect(&dc, r);
	}

	if (data != nullptr)
		free(data);
}

void CPDFPreviewView::DoScroll(LONG posX, LONG posY)
{
	CPoint newOffset(posX, posY);
	FixupScrolls(newOffset);
	if (newOffset == m_ptOffset)
		return;
	LONG dx = m_ptOffset.x - newOffset.x;
	LONG dy = m_ptOffset.y - newOffset.y;
	m_ptOffset = newOffset;

	if (dx != 0 || dy != 0)
	{
		UpdateScrolls();
		ScrollWindow(dx, dy);
	}
}

afx_msg void CPDFPreviewView::OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{
	LONG nNewPos = m_ptOffset.x;
	CRect cr;
	GetClientRect(cr);
	switch (nSBCode)
	{
	case SB_LEFT:
		nNewPos = 0;
		break;
	case SB_LINELEFT:
		nNewPos--;
		break;
	case SB_LINERIGHT:
		nNewPos++;
		break;
	case SB_PAGELEFT:
		nNewPos -= cr.Width();
		break;
	case SB_PAGERIGHT:
		nNewPos += cr.Width();
		break;
	case SB_RIGHT:
		nNewPos = GetTotalSize().cx;
		break;
	case SB_THUMBPOSITION:
	case SB_THUMBTRACK:
		nNewPos = (LONG)nPos;
		break;
	}
	DoScroll(nNewPos, m_ptOffset.y);
}

afx_msg void CPDFPreviewView::OnVScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{
	LONG nNewPos = m_ptOffset.y;
	CRect cr;
	GetClientRect(cr);
	switch (nSBCode)
	{
	case SB_TOP:
		nNewPos = 0;
		break;
	case SB_LINEUP:
		nNewPos--;
		break;
	case SB_LINEDOWN:
		nNewPos++;
		break;
	case SB_PAGEUP:
		nNewPos -= cr.Height();
		break;
	case SB_PAGEDOWN:
		nNewPos += cr.Height();
		break;
	case SB_BOTTOM:
		nNewPos = GetTotalSize().cy;
		break;
	case SB_THUMBPOSITION:
	case SB_THUMBTRACK:
		nNewPos = (LONG)nPos;
		break;
	}
	DoScroll(m_ptOffset.x, nNewPos);
}

BEGIN_MESSAGE_MAP(CPDFPreviewView, CWnd)
	ON_WM_ERASEBKGND()
	ON_WM_SIZE()
	ON_WM_PAINT()
	ON_WM_VSCROLL()
	ON_WM_HSCROLL()
END_MESSAGE_MAP()

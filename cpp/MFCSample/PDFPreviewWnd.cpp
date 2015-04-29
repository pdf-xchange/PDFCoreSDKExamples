// PreviewWnd.cpp : implementation file
//

#include "stdafx.h"
#include "MFCSample.h"
#include "PDFPreviewWnd.h"

// CPDFPreviewWnd

IMPLEMENT_DYNAMIC(CPDFPreviewWnd, CWnd)

BEGIN_MESSAGE_MAP(CPDFPreviewWnd, CWnd)
	ON_WM_NCDESTROY()
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_COMMAND(ID_VIEW_ZOOMIN, &CPDFPreviewWnd::OnViewZoomin)
	ON_COMMAND(ID_VIEW_ZOOMOUT, &CPDFPreviewWnd::OnViewZoomout)
	ON_COMMAND(ID_VIEW_TRANSP, &CPDFPreviewWnd::OnViewTransparency)
	ON_UPDATE_COMMAND_UI(ID_VIEW_TRANSP, &CPDFPreviewWnd::OnUpdateViewTransparency)
	ON_UPDATE_COMMAND_UI(ID_VIEW_ZOOMIN, &CPDFPreviewWnd::OnUpdateViewZoomin)
	ON_UPDATE_COMMAND_UI(ID_VIEW_ZOOMOUT, &CPDFPreviewWnd::OnUpdateViewZoomout)
END_MESSAGE_MAP()

CPDFPreviewWnd::CPDFPreviewWnd()
{
}

CPDFPreviewWnd::~CPDFPreviewWnd()
{
}

BOOL CPDFPreviewWnd::Create(const RECT& rect, CWnd* pParentWnd, CCreateContext* pContext)
{
	const DWORD dwWndStyle = WS_POPUPWINDOW | WS_CAPTION | WS_THICKFRAME | WS_VISIBLE | WS_CLIPSIBLINGS | WS_CLIPCHILDREN | WS_MAXIMIZEBOX;
	const DWORD dwExtStyle = 0;

	CString strMyClass;
	strMyClass = AfxRegisterWndClass(CS_VREDRAW | CS_HREDRAW,
									 ::LoadCursor(NULL, IDC_ARROW),
									 (HBRUSH)::GetStockObject(WHITE_BRUSH),
									 0);

	BOOL bRes = __super::CreateEx(dwExtStyle, strMyClass, _T("Preview"), dwWndStyle, rect, pParentWnd, 0, pContext);
	DWORD err = GetLastError();
	return bRes;
}

// CPDFPreviewWnd message handlers
void CPDFPreviewWnd::OnNcDestroy()
{
	__super::OnNcDestroy();
	delete this;
}

int CPDFPreviewWnd::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (__super::OnCreate(lpCreateStruct) == -1)
		return -1;
	//
	CRect rectDummy;
	rectDummy.SetRectEmpty();
	// view
	m_wndView.Create(this, rectDummy);
	// Load view images:
	m_wndToolbar.Create(this, AFX_DEFAULT_TOOLBAR_STYLE, IDR_PREVIEW_TOOLBAR);
	m_wndToolbar.LoadToolBar(IDR_PREVIEW_TOOLBAR, 0, 0, TRUE /* Is locked */);
	m_wndToolbar.CleanUpLockedImages();
	m_wndToolbar.LoadBitmap(IDR_PREVIEW_TOOLBAR, 0, 0, TRUE /* Locked */);
	m_wndToolbar.SetPaneStyle(m_wndToolbar.GetPaneStyle() | CBRS_TOOLTIPS | CBRS_FLYBY);
	m_wndToolbar.SetPaneStyle(m_wndToolbar.GetPaneStyle() & ~(CBRS_GRIPPER | CBRS_SIZE_DYNAMIC | CBRS_BORDER_TOP | CBRS_BORDER_BOTTOM | CBRS_BORDER_LEFT | CBRS_BORDER_RIGHT));

	m_wndToolbar.SetOwner(this);

	// All commands will be routed via this control , not via the parent frame:
	m_wndToolbar.SetRouteCommandsViaFrame(FALSE);

	//
	AdjustLayout();

	return 0;
}

void CPDFPreviewWnd::OnSize(UINT nType, int cx, int cy)
{
	__super::OnSize(nType, cx, cy);
	AdjustLayout();
}

void CPDFPreviewWnd::AdjustLayout()
{
	if (GetSafeHwnd() == NULL)
		return;
	CRect rectClient;
	GetClientRect(rectClient);
	int cyTlb = m_wndToolbar.CalcFixedLayout(FALSE, TRUE).cy;
	m_wndToolbar.SetWindowPos(nullptr, rectClient.left, rectClient.top, rectClient.Width(), cyTlb, SWP_NOACTIVATE | SWP_NOZORDER);
	rectClient.top += cyTlb;
	m_wndView.SetWindowPos(nullptr, rectClient.left, rectClient.top, rectClient.Width(), rectClient.Height(), SWP_NOACTIVATE | SWP_NOZORDER);
}

void CPDFPreviewWnd::OnViewZoomin()
{
	double curZoom = m_wndView.GetZoom();
	if (curZoom < 100.0)
		curZoom += 10.0;
	else
		curZoom += 100.0;
	if (curZoom > 800.0)
		curZoom = 800.0;
	m_wndView.SetZoom(curZoom);
}

void CPDFPreviewWnd::OnViewZoomout()
{
	double curZoom = m_wndView.GetZoom();
	if (curZoom <= 100.0)
		curZoom -= 10.0;
	else
		curZoom -= 100.0;
	if (curZoom < 10.0)
		curZoom = 10.0;
	m_wndView.SetZoom(curZoom);
}

void CPDFPreviewWnd::OnViewTransparency()
{

}

void CPDFPreviewWnd::OnUpdateViewZoomin(CCmdUI *pCmdUI)
{
	pCmdUI->Enable(m_wndView.GetZoom() < 800.0);
}

void CPDFPreviewWnd::OnUpdateViewZoomout(CCmdUI *pCmdUI)
{
	pCmdUI->Enable(m_wndView.GetZoom() > 10.0);
}

void CPDFPreviewWnd::OnUpdateViewTransparency(CCmdUI *pCmdUI)
{
}

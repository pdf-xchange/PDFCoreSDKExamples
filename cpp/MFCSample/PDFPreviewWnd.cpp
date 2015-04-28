// PreviewWnd.cpp : implementation file
//

#include "stdafx.h"
#include "MFCSample.h"
#include "PDFPreviewWnd.h"


// CPDFPreviewWnd

IMPLEMENT_DYNAMIC(CPDFPreviewWnd, CWnd)

CPDFPreviewWnd::CPDFPreviewWnd()
{
	m_wndView	= nullptr;
}

CPDFPreviewWnd::~CPDFPreviewWnd()
{
}

BOOL CPDFPreviewWnd::Create(const RECT& rect, CWnd* pParentWnd, CCreateContext* pContext)
{
	const DWORD dwWndStyle = WS_POPUPWINDOW | WS_CAPTION | WS_THICKFRAME | WS_VISIBLE | WS_CLIPSIBLINGS | WS_CLIPCHILDREN;
	const DWORD dwExtStyle = WS_EX_TOOLWINDOW;

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
	m_wndView = new CPDFPreviewView;
	if (m_wndView == nullptr)
		return -1;
	m_wndView->Create(this, rectDummy);
	// Load view images:
	m_wndToolbar.Create(this, AFX_DEFAULT_TOOLBAR_STYLE, IDR_MAINFRAME);
	m_wndToolbar.LoadToolBar(IDR_MAINFRAME, 0, 0, TRUE /* Is locked */);
	m_wndToolbar.CleanUpLockedImages();
	m_wndToolbar.LoadBitmap(IDR_MAINFRAME_256, 0, 0, TRUE /* Locked */);
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
	m_wndView->SetWindowPos(nullptr, rectClient.left, rectClient.top, rectClient.Width(), rectClient.Height(), SWP_NOACTIVATE | SWP_NOZORDER);
}


BEGIN_MESSAGE_MAP(CPDFPreviewWnd, CWnd)
	ON_WM_NCDESTROY()
	ON_WM_CREATE()
	ON_WM_SIZE()
END_MESSAGE_MAP()




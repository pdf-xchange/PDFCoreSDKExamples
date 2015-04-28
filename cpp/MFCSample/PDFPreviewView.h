#pragma once

// CPDFPreviewView view

// CScrollView

class CPDFPreviewView : public CWnd
{
	DECLARE_DYNAMIC(CPDFPreviewView)
public:
	CPDFPreviewView();           // protected constructor used by dynamic creation
	virtual ~CPDFPreviewView();
public:
#ifdef _DEBUG
	virtual void AssertValid() const;
#ifndef _WIN32_WCE
	virtual void Dump(CDumpContext& dc) const;
#endif
#endif
public:
	BOOL Create(CWnd* pParentWnd, RECT& rc);
public:
	void SetDocument(PXC::IPXC_Document* pDoc);
	void SetZoom(double nZoomLevel);
	void SetCurPage(ULONG nPage);
public:
	CSize GetPageSize();
protected:
	CSize CalcPageSize();
protected:
	void ReleaseCache();
	bool EnsureCache(const CRect& pr);
	void UpdateScrolls();
	bool FixupScrolls(CPoint& offs) const;
	// returns rect of the page (w/o gaps) relative to the client area
	CRect GetPageRect() const;
	// returns size of view area: page + gaps. in pixels
	CSize GetTotalSize() const;
	void PaintRect(CDC* pDC, const CRect& pr);
protected:
	virtual void OnDraw(CDC* pDC);
	virtual void OnInitialUpdate();
protected:
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnPaint();
	DECLARE_MESSAGE_MAP()
protected:
	CComPtr<PXC::IPXC_Document>		m_pDoc;
	DWORD							m_nCurPage;
	CComPtr<PXC::IIXC_Page>			m_pCache;
	CPoint							m_ptCacheOffset;	// offset of cache from page
	CSize							m_szPageSize;		// page size in pixels with current zoom
	CPoint							m_ptOffset;			// scroll offset
	double							m_nCoef;
	double							m_ZoomLevel;
	//
	COLORREF						m_bgColor;
};



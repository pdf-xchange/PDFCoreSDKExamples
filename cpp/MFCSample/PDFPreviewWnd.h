#pragma once

#include "PDFPreviewView.h"

class CPDFPreviewToolbar : public CMFCToolBar
{
public:
	virtual BOOL AllowShowOnList() const { return FALSE; }
};

// CPDFPreviewWnd
class CPDFPreviewWnd : public CWnd
{
	DECLARE_DYNAMIC(CPDFPreviewWnd)
public:
	CPDFPreviewWnd();
	virtual ~CPDFPreviewWnd();
public:
	BOOL Create(const RECT& rect, CWnd* pParentWnd, CCreateContext* pContext = NULL);
public:
	void SetDocument(PXC::IPXC_Document* pDoc)
	{
		if (m_wndView)
			m_wndView->SetDocument(pDoc);
	}

protected:
	void AdjustLayout();
public:
	CPDFPreviewToolbar		m_wndToolbar;
	CPDFPreviewView*		m_wndView;
protected:
	afx_msg void OnNcDestroy();
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	DECLARE_MESSAGE_MAP()
};



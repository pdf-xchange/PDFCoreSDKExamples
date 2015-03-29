#include "stdafx.h"
#include "Sample.h"
#include "resource.h"

//////////////////////////////////////////////////////////////////////////
class CBaseSampleDlg : public CDialogEx
{
public:
	enum { IDD = IDD_DLG_BASE };
	CBaseSampleDlg(CWnd* pParent);
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
// Implementation
protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnCmdRun();
public:
	CSDKSample*		m_pSample;
};

CBaseSampleDlg::CBaseSampleDlg(CWnd* pParent) : CDialogEx(IDD, pParent)
{
	m_pSample = nullptr;
}

void CBaseSampleDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

void CBaseSampleDlg::OnCmdRun()
{
	if (m_pSample)
		m_pSample->Perform();
}

BEGIN_MESSAGE_MAP(CBaseSampleDlg, CDialogEx)
	ON_BN_CLICKED(IDC_CMD_RUN, &CBaseSampleDlg::OnCmdRun)
END_MESSAGE_MAP()

//////////////////////////////////////////////////////////////////////////
CSDKSample::CSDKSample()
{
	m_pWindow	= nullptr;
}

CSDKSample::~CSDKSample()
{
	if (m_pWindow)
	{
		delete m_pWindow;
		m_pWindow = nullptr;
	}
}

CWnd* CSDKSample::GetDialog(CWnd* pParent)
{
	if (m_pWindow == nullptr)
	{
		CBaseSampleDlg* pDlg = new CBaseSampleDlg(pParent);
		pDlg->Create(CBaseSampleDlg::IDD, pParent);
		pDlg->m_pSample = this;
		m_pWindow = pDlg;
	}
	return m_pWindow;
}

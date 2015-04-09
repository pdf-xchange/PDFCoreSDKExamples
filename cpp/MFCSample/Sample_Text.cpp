#include "stdafx.h"
#include "Sample_Text.h"
#include "InitializeSDK.h"
#include "resource.h"

#define _USE_MATH_DEFINES
#include <math.h>

CSDKSample_Text::CSDKSample_Text()
{
}

CSDKSample_Text::~CSDKSample_Text()
{
}

LPCWSTR CSDKSample_Text::GetGroup()
{
	return L"Creation";
}

LPCWSTR CSDKSample_Text::GetTitle()
{
	return L"Text";
}

LPCWSTR CSDKSample_Text::GetDescription()
{
	return L"";
}

static
HRESULT FillByGradient(PXC::IPXC_Document* pDoc, PXC::IPXC_ContentCreator* pCC, const PXC::PXC_Rect& rect)
{
	CComPtr<PXC::IPXC_GradientStops> pStops;
	pDoc->CreateShadeStops(&pStops);

	pStops->AddStopRGB(0.0, RGB(255, 255, 0));
	pStops->AddStopRGB(1.0, RGB(0, 0, 255));

	CComPtr<PXC::IPXC_Shading> pShade;
	PXC::PXC_Point p0, p1;
	p0.x = rect.left; p0.y = rect.top;
	p1.x = rect.left; p1.y = rect.bottom;

	pDoc->CreateLinearShade(&p0, &p1, pStops, 3, &pShade);

	pCC->SaveState();
	pCC->SetShadeAsPattern(pShade, VARIANT_TRUE);
	pCC->SetStrokeColorRGB(RGB(0, 0, 0));
	pCC->Rect(rect.left, rect.bottom, rect.right, rect.top);
	pCC->FillPath(VARIANT_TRUE, VARIANT_FALSE, PXC::FillRule_Winding);
	pCC->RestoreState();
	return S_OK;
}

static
HRESULT Text_Ex1(PXC::IPXC_Document* pDoc, PXC::IPXC_ContentCreator* pCC, const PXC::PXC_Rect& pr)
{
	HRESULT hr = S_OK;
	//
	double x	= I2P(1);
	double y	= pr.top - I2P(1);
	double xs	= I2P(3.5);
	double ys	= I2P(1);
	double ts	= I2P(0.45);

	CComPtr<PXC::IPXC_Font> pFont;
	CComPtr<PXC::IPXC_Font> pFont2;
	hr = pDoc->CreateNewFont(L"Impact", 0, FW_HEAVY, &pFont);
	if (pFont == nullptr)
		return hr;
	double fntsize = 25.0;
	LPCWSTR text = L"Text Rendering Mode";
	double twidth;

	pCC->SaveState();

	pCC->SetFont(pFont);
	pCC->SetFontSize(fntsize);
	pCC->CalcTextSize(fntsize, (LPWSTR)text, &twidth, nullptr, -1);

	pCC->SetFillColorRGB(clrKhaki);
	pCC->SaveState();
		pCC->SetTextRenderMode(PXC::TRM_Fill);
		pCC->ShowTextLine(x, y, (LPWSTR)text, -1, 0);
	pCC->RestoreState();
	DrawTitle(pDoc, pCC, x + twidth / 2, y - ts, L"TRM_Fill", 15);

	x += xs;
	pCC->SaveState();
		pCC->SetTextRenderMode(PXC::TRM_Stroke);
		pCC->ShowTextLine(x, y, (LPWSTR)text, -1, 0);
	pCC->RestoreState();
	DrawTitle(pDoc, pCC, x + twidth / 2, y - ts, L"TRM_Stroke", 15);

	x -= xs; y -= ys;
	pCC->SaveState();
		pCC->SetTextRenderMode(PXC::TRM_FillStroke);
		pCC->ShowTextLine(x, y, (LPWSTR)text, -1, 0);
	pCC->RestoreState();
	DrawTitle(pDoc, pCC, x + twidth / 2, y - ts, L"TRM_FillStroke", 15);

	x += xs;
	pCC->SaveState();
		pCC->SetTextRenderMode(PXC::TRM_None);
		pCC->ShowTextLine(x, y, (LPWSTR)text, -1, 0);
	pCC->RestoreState();
	DrawTitle(pDoc, pCC, x + twidth / 2, y - ts, L"TRM_None", 15);

	x -= xs;
	y -= ys;
	double w = I2P(3.2);
	double h = I2P(1);
	//
	text = L"ABC";
	pDoc->CreateNewFont(L"Arial Black", 0, FW_BOLD, &pFont2);
	fntsize = 50;
	pCC->SetFontSize(fntsize);
	pCC->SetFont(pFont2);

	PXC::PXC_Rect rc;
	rc.left = x; rc.right = rc.left + w;
	rc.top = y; rc.bottom = rc.top - h;
	FillByGradient(pDoc, pCC, rc);

	pCC->SaveState();
		pCC->SetCharSpace(2.0);
		pCC->SetTextScale(150.0);
		pCC->SetTextRenderMode(PXC::TRM_Stroke);
		pCC->ShowTextLine(x + I2P(0.35), y, (LPWSTR)text, -1, 0);
	pCC->RestoreState();
	DrawTitle(pDoc, pCC, x + w / 2, y - I2P(1.1), L"TRM_Stroke", 15);

	x += xs;
	rc.left += xs; rc.right += xs;
	pCC->SaveState();
		pCC->SetCharSpace(2.0);
		pCC->SetTextScale(150.0);
		pCC->SetTextRenderMode(PXC::TRM_Clip_Stroke);
		pCC->ShowTextLine(x + I2P(0.35), y, (LPWSTR)text, -1, 0);
		FillByGradient(pDoc, pCC, rc);
	pCC->RestoreState();
	DrawTitle(pDoc, pCC, x + w / 2, y - I2P(1.1), L"TRM_Clip_Stroke", 15);

	pCC->RestoreState();

	return hr;
}

//
HRESULT CSDKSample_Text::Perform()
{
	HRESULT hr = S_OK;
	CComPtr<PXC::IPXC_Document> pDoc;
	do 
	{
		// creating document with one page
		CComPtr<PXC::IPXC_Page> pPage;
		CComPtr<PXC::IPXC_Content> pContent;
		CComPtr<PXC::IPXC_ContentCreator> pCC;

		PXC::PXC_Rect pr = letterSize;
		hr = CreateNewDocWithPage(pDoc, pPage, pr);
		if (FAILED(hr))
			break;
		// creating some conent
		hr = pDoc->CreateContentCreator(&pCC);
		BreakOnFailure(hr, L"Error creating conent creator");
		// page 1
		Text_Ex1(pDoc, pCC, pr);
		pCC->Detach(&pContent);
		hr = pPage->PlaceContent(pContent, PXC::PlaceContent_Replace);
		BreakOnFailure(hr, L"Error replacing page content");
		pContent = nullptr;
		//
		pCC = nullptr;
		pPage = nullptr;
		CStringW sFilename;
		hr = SaveDocument(pDoc, sFilename);
	} while (false);
	if (pDoc)
		pDoc->Close(0);
	return hr;
}

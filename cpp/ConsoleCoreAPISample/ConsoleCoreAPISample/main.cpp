#include "stdafx.h"
#include "InitSDK.h"
#include "LogUtils.h"
#include <iostream>

using namespace std;
using namespace PXC;

int main()
{
	HRESULT hr = S_OK;
	CComPtr<IAUX_Inst> pAUX;
	do
	{
		cout << "Initializing CoreAPI SDK..." << endl;
		hr = InitializeSDK();
		BreakOnFailure(hr);
		cout << "Core API initialized successfully!" << endl;

		CComPtr<IUnknown> pUnk;
		hr = g_Inst->GetExtension((LPWSTR)L"AUX", &pUnk);
		BreakOnFailure(hr);
		hr = pUnk->QueryInterface(&pAUX);

		//Creating new document and inserting pages
		CComPtr<IPXC_Document> pDoc;
		hr = g_Inst->NewDocument(&pDoc);
		BreakOnFailure(hr);
		CComPtr<IPXC_Pages> pPages;
		hr = pDoc->get_Pages(&pPages);
		BreakOnFailure(hr);
		PXC_Rect rcMedia {};
		rcMedia.right = 800;
		rcMedia.top = 600;
		hr = pPages->AddEmptyPages(0, 1, &rcMedia, nullptr, nullptr);
		BreakOnFailure(hr);
		ULONG_T nCnt = 0;
		pPages->get_Count(&nCnt);
		cout << "Newly created document contains " << nCnt << " pages." << endl;
		wchar_t buff[FILENAME_MAX]; //create string buffer to hold path
		_wgetcwd(buff, FILENAME_MAX);
		wstring sPath = buff;
		wstring sImagePath = sPath;
		sImagePath.append(L"\\Figuress.png");
		wcout << L"Opening image from path: " << sImagePath.c_str() << endl;

		CComPtr<IPXC_Image> pImage;
		hr = pDoc->AddImageFromFile((LPWSTR)sImagePath.c_str(), 0, 0, &pImage);
		BreakOnFailure(hr);
		ULONG_T nW = 0;
		ULONG_T nH = 0;
		pImage->get_Width(&nW);
		pImage->get_Height(&nH);
		PXC_Rect bbox;
		bbox.left = 0;
		bbox.bottom = 0;
		bbox.right = nW * 72.0 / 96.0;
		bbox.top = nH * 72.0 / 96.0;

		cout << "Creating content from image..." << endl;

		CComPtr<IPXC_ContentCreator> pCC;
		hr = pDoc->CreateContentCreator(&pCC);
		BreakOnFailure(hr);
		PXC_Matrix im;
		CComPtr<IMathHelper> pMH;
	
		BreakOnFailure(hr);
		hr = pAUX->get_MathHelper(&pMH);
		BreakOnFailure(hr);
		pMH->Matrix_Reset(&im);
		pMH->Matrix_Scale(&im, bbox.right, bbox.top, &im);
		pCC->SaveState();
		hr = pCC->ConcatCS(&im);
		BreakOnFailure(hr);
		pCC->PlaceImage(pImage);
		BreakOnFailure(hr);
		pCC->RestoreState();

		cout << "Placing content..." << endl;

		CComPtr<IPXC_Content> pC;
		pCC->Detach(&pC);
		CComPtr<IPXC_Page> pPage;
		hr = pPages->get_Item(0, &pPage);
		BreakOnFailure(hr);
		hr = pPage->PlaceContent(pC, PXC_PlaceContentFlags::PlaceContent_Replace);
		BreakOnFailure(hr);

		cout << "Placing content successful..." << endl;
		sPath.append(L"\\Test.pdf");
		wcout << L"Saving file to path: " << sPath.c_str() << endl;

		hr = pDoc->WriteToFile((LPWSTR)sPath.c_str(), nullptr, 0);
		BreakOnFailure(hr);

		cout << "File save successful..." << endl;
	} while (false);
	if (FAILED(hr))
	{
		cout << "Encountered failure..." << endl;
		if (pAUX)
		{
			CComBSTR bstr;
			pAUX->FormatHRESULT(hr, 0, &bstr);
			wcout << (LPCWSTR)bstr << endl;
		}
	}
	pAUX = nullptr;

	FinalizeSDK();
	cout << "Finalized SDK" << endl;
	cout << "Press any key to close..." << endl;
	int a = 0;
	cin >> a;
	return hr;
}
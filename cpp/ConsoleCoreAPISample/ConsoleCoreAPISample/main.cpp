#include "stdafx.h"
#include "InitSDK.h"
#include "LogUtils.h"
#include <iostream>

using namespace std;
using namespace PXC;

int main()
{
	HRESULT hr = S_OK;
	do
	{
		cout << "Initializing CoreAPI SDK..." << endl;
		hr = InitializeSDK();
		BreakOnFailure(hr);
		cout << "Core API initialized successfully!" << endl;

		//Creating new document and inserting pages
		CComPtr<IPXC_Document> pDoc;
		hr = g_Inst->NewDocument(&pDoc);
		BreakOnFailure(hr);
		CComPtr<IPXC_Pages> pPages;
		hr = pDoc->get_Pages(&pPages);
		BreakOnFailure(hr);
		PXC_Rect rcMedia {};
		rcMedia.right = 600;
		rcMedia.top = 800;
		hr = pPages->AddEmptyPages(0, 1, &rcMedia, nullptr, nullptr);
		BreakOnFailure(hr);
		ULONG_T nCnt = 0;
		pPages->get_Count(&nCnt);
		cout << "Newly created document contains " << nCnt << " pages." << endl;
		
	} while (false);
	FinalizeSDK();
	cout << "Finalized SDK" << endl;
	return hr;
}
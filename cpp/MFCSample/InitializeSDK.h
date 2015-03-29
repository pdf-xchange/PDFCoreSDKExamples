#pragma once


extern PXC::IPXC_Inst*		g_Inst;
extern PXC::IPXS_Inst*		g_COS;
extern PXC::IIXC_Inst*		g_ImgCore;
extern PXC::IAUX_Inst*		g_AUX;

HRESULT InitializeSDK();
HRESULT FinalizeSDK();

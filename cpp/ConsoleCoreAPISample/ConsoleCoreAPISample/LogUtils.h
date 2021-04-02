#pragma once

#define BreakFunc()		break;

#define BreakOnFailure(x, ...)				if (FAILED(x)) { BreakFunc(); }
#define BreakOnNull(p, x, e, ...)				if (nullptr == (p)) { x = e; BreakFunc(); }
#define BreakOnNullWithLastError(p, x, ...)	if (nullptr == (p)) { x = ::GetLastError(); x = HRESULT_FROM_WIN32(x); if (!FAILED(x)) { x = E_FAIL; } BreakFunc(); }

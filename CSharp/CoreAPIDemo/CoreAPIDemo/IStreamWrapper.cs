using System;
using System.IO;
using System.Runtime.InteropServices;
//using System.Runtime.InteropServices.ComTypes;
using MS.Internal;
using PDFXCoreAPI;


namespace CoreAPIDemo
{
	public class IStreamWrapper : IStream
	{
		private Stream		m_stream;
		private Int64		m_pos;
		private Object		m_sync;

		public enum STGTY : int
		{
			STGTY_STORAGE	= 1,
			STGTY_STREAM	= 2,
			STGTY_LOCKBYTES	= 3,
			STGTY_PROPERTY	= 4
		}

		public enum STGM : int
		{
			STGM_READ		= 0,
			STGM_WRITE		= 1,
			STGM_READWRITE	= 2,
		}
		
		public IStreamWrapper(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException("stream");
			m_stream = stream;
			m_pos = 0;
			m_sync = new Object();
		}

		protected IStreamWrapper(IStreamWrapper streamWrapper)
		{
			m_stream = streamWrapper.m_stream;
			m_pos = streamWrapper.m_pos;
			m_sync = streamWrapper.m_sync;
		}


#region IStream implementation

		void IStream.Clone(out IStream clone)
		{
			clone = new IStreamWrapper(this);
			if (clone == null)
				throw new ArgumentNullException("StreamWrapper");
		}

		void IStream.Commit(uint grfCommitFlags)
		{
		}

		void IStream.RemoteCopyTo(IStream pstm, _ULARGE_INTEGER cb, out _ULARGE_INTEGER pcbRead, out _ULARGE_INTEGER pcbWritten)
		{
			pcbRead.QuadPart = 0;
			pcbWritten.QuadPart = 0;
		}

		void IStream.LockRegion(_ULARGE_INTEGER libOffset, _ULARGE_INTEGER cb, uint dwLockType)
		{
		}

		unsafe void IStream.RemoteRead(out byte pv, uint cb, out uint pcbRead)
		{
			int cbRead = 0;
			pv = 0;
			fixed (byte* addressOfBuffer = &pv)
			{
				lock (m_sync)
				{
					try
					{
						m_stream.Seek(m_pos, SeekOrigin.Begin);
						byte[] buf = new byte[cb];
						cbRead = m_stream.Read(buf, 0, (int)cb);
						IntPtr outPtr = new IntPtr(addressOfBuffer);
						Marshal.Copy(buf, 0, outPtr, cbRead);
						if (cbRead > 0)
							m_pos += cbRead;
					}
					catch (System.Exception ex)
					{
						int a = ex.HResult;
					}
				}

			}
			pcbRead = (uint)cbRead;
		}
		
		void IStream.Revert()
		{
		}
		
		void IStream.RemoteSeek(_LARGE_INTEGER dlibMove, uint dwOrigin, out _ULARGE_INTEGER plibNewPosition)
		{
			
			m_pos = m_stream.Seek(dlibMove.QuadPart, (SeekOrigin)dwOrigin);
// 			if (newPos != IntPtr.Zero)
// 				Marshal.WriteInt64(newPos, m_pos);
			plibNewPosition.QuadPart = (ulong)m_pos;
		}
		
		void IStream.SetSize(_ULARGE_INTEGER libNewSize)
		{
		}
		
		void IStream.Stat(out tagSTATSTG pstatstg, uint grfStatFlag)
		{
			pstatstg = new tagSTATSTG();
			pstatstg.Type = (uint)STGTY.STGTY_STREAM;
			pstatstg.cbSize.QuadPart = (ulong)m_stream.Length;
			pstatstg.grfMode = 0; // default value for each flag will be false
			
			if (m_stream.CanRead && m_stream.CanWrite)
				pstatstg.grfMode |= (int)STGM.STGM_READWRITE;
			else if (m_stream.CanRead)
				pstatstg.grfMode |= (int)STGM.STGM_READ;
			else if (m_stream.CanWrite)
				pstatstg.grfMode |= (int)STGM.STGM_WRITE;

		}
		
		void IStream.UnlockRegion(_ULARGE_INTEGER libOffset, _ULARGE_INTEGER cb, uint dwLockType)
		{
		}

		void IStream.RemoteWrite(ref byte pv, uint cb, out uint pcbWritten)
		{
			lock (m_sync)
			{
				try
				{
					m_stream.Seek(m_pos, SeekOrigin.Begin);

					byte[] buf = new byte[cb];
					IntPtr outPtr = new IntPtr((long)pv);
					Marshal.Copy(outPtr, buf, 0, (int)cb);
					m_stream.Write(buf, 0, (int)cb);
					if (cb > 0)
						m_pos += cb;
				}
				catch { }
			}

// 			if (pcbWritten != 0)
// 				Marshal.WriteInt32(pcbWritten, cb);

			pcbWritten = cb;
		}

		public void RemoteRead(out byte pv, uint cb, out uint pcbRead)
		{
			throw new NotImplementedException();
		}

		public void RemoteWrite(ref byte pv, uint cb, out uint pcbWritten)
		{
			throw new NotImplementedException();
		}

		#endregion // IStream implementation

	}
}

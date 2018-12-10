/**************************************************************************
 *                                                                        *
 *  File:        HighResTimer.cs                                          *
 *  Description: Merge Sort multi-agent                                   *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/

using System;
using System.Runtime.InteropServices;

namespace MASMA.Common
{
    public class HighResTimer
    {
        [DllImport("kernel32", EntryPoint = "QueryPerformanceFrequency")]
        private static unsafe extern bool QueryPerformanceFrequency(Int64* lpPerformanceFreq);

        [DllImport("kernel32", EntryPoint = "QueryPerformanceCounter")]
        private static unsafe extern bool QueryPerformanceCounter(Int64* lpPerformanceCount);

        private static Int64 _t1, _t2, _htrFrecv;
        private static bool _htrInit;

        static HighResTimer()
        {
            bool result = InitCounter();
            if (!result)
            {
                throw new System.ComponentModel.Win32Exception();
            }
        }

        private static unsafe bool InitCounter()
        {
            _t1 = 0;
            _t2 = 0;
            _htrFrecv = 0;
            _htrInit = false;

            fixed (Int64* frecv = &_htrFrecv)
            {
                _htrInit = QueryPerformanceFrequency(frecv);
            }
            return _htrInit;
        }

        public unsafe void Start()
        {
            fixed (Int64* t1 = &_t1)
            {
                QueryPerformanceCounter(t1);
            }
        }

        public unsafe double Stop()
        {
            fixed (Int64* t2 = &_t2)
            {
                QueryPerformanceCounter(t2);
            }

            Int64 difCounts = _t2 - _t1;
            double difSeconds = (double)difCounts / (double)_htrFrecv;
            return difSeconds * 1000.0;
        }
    }
}

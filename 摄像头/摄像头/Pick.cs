﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace 摄像头
{
    class Pick
    {
        private const int WM_USER = 0x400;
        private const int WS_CHILD = 0x40000000;
        private const int WS_VISIBLE = 0x10000000;
        private const int WM_CAP_START = WM_USER;
        private const int WM_CAP_STOP = WM_CAP_START + 68;
        private const int WM_CAP_DRIVER_CONNECT = WM_CAP_START + 10;
        private const int WM_CAP_DRIVER_DISCONNECT = WM_CAP_START + 11;
        private const int WM_CAP_SAVEDIB = WM_CAP_START + 25;
        private const int WM_CAP_GRAB_FRAME = WM_CAP_START + 60;
        private const int WM_CAP_SEQUENCE = WM_CAP_START + 62;
        private const int WM_CAP_FILE_SET_CAPTURE_FILEA = WM_CAP_START + 20;
        private const int WM_CAP_SEQUENCE_NOFILE = WM_CAP_START + 63;
        private const int WM_CAP_SET_OVERLAY = WM_CAP_START + 51;
        private const int WM_CAP_SET_PREVIEW = WM_CAP_START + 50;
        private const int WM_CAP_SET_CALLBACK_VIDEOSTREAM = WM_CAP_START + 6;
        private const int WM_CAP_SET_CALLBACK_ERROR = WM_CAP_START + 2;
        private const int WM_CAP_SET_CALLBACK_STATUSA = WM_CAP_START + 3;
        private const int WM_CAP_SET_CALLBACK_FRAME = WM_CAP_START + 5;
        private const int WM_CAP_SET_SCALE = WM_CAP_START + 53;
        private const int WM_CAP_SET_PREVIEWRATE = WM_CAP_START + 52;
        private IntPtr hWndC;
        private bool bStat = false;
        private IntPtr mControlPtr;
        private int mWidth;
        private int mHeight;
        private int mLeft;
        private int mTop;

        ///  
        /// 初始化摄像头  
        ///  
        /// 控件的句柄  
        /// 开始显示的左边距  
        /// 开始显示的上边距  
        /// 要显示的宽度  
        /// 要显示的长度  
        public Pick(IntPtr handle, int left, int top, int width, int height)
        {
            mControlPtr = handle;
            mWidth = width;
            mHeight = height;
            mLeft = left;
            mTop = top;
        }
        [DllImport("avicap32.dll")]
        private static extern IntPtr capCreateCaptureWindowA(byte[] lpszWindowName, int dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, int nID);
        [DllImport("avicap32.dll")]
        private static extern int capGetVideoFormat(IntPtr hWnd, IntPtr psVideoFormat, int wSize);
        [DllImport("User32.dll")]
        private static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        [DllImport("User32.dll")]
        private static extern bool SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);
        [DllImport("avicap32.dll")]
        public static extern bool capGetDriverDescriptionA(short wDriver, byte[] lpszName, int cbName, byte[] lpszVer, int cbVer); 
        ///  
        /// 开始显示图像  
        ///  
        public void Start()
        {
            if (bStat)
                return;
            bStat = true;
            byte[] lpszName = new byte[100];
            byte[] lpszVer = new byte[100]; 
            capGetDriverDescriptionA(0, lpszName, 100, lpszVer, 100);
            hWndC = capCreateCaptureWindowA(lpszName, WS_CHILD | WS_VISIBLE, mLeft, mTop, mWidth, mHeight, mControlPtr, 0);
            if (hWndC.ToInt32() != 0)
            {
                SendMessage(hWndC, WM_CAP_SET_CALLBACK_VIDEOSTREAM, 0, 0);
                SendMessage(hWndC, WM_CAP_SET_CALLBACK_ERROR, 0, 0);
                SendMessage(hWndC, WM_CAP_SET_CALLBACK_STATUSA, 0, 0);
                SendMessage(hWndC, WM_CAP_DRIVER_CONNECT, 0, 0);
                SendMessage(hWndC, WM_CAP_SET_SCALE, 1, 0);
                SendMessage(hWndC, WM_CAP_SET_PREVIEWRATE, 66, 0);
                SendMessage(hWndC, WM_CAP_SET_OVERLAY, 1, 0);
                SendMessage(hWndC, WM_CAP_SET_PREVIEW, 1, 0);
                //SendMessage(hWndC, WM_CAP_SET_PREVIEW, true, 0); 
            }
            return;
        }
        ///  
        /// 停止显示  
        ///  
        public void Stop()
        {
            SendMessage(hWndC, WM_CAP_DRIVER_DISCONNECT, 0, 0);
            bStat = false;
        }
        ///  
        /// 抓图  
        ///  
        /// 要保存bmp文件的路径  
        public void GrabImage(string path)
        // public void GrabImage( )  
        {
            IntPtr hBmp = Marshal.StringToHGlobalAnsi(path);
            if (
            SendMessage(hWndC, WM_CAP_SAVEDIB, 0, hBmp.ToInt32()))
                MessageBox.Show("抓图成功");
            else
                MessageBox.Show("抓图失败");
        }

        ///  
        /// 录像  
        ///  
        /// 要保存avi文件的路径  
        public void Kinescope(string path)
        {
            IntPtr hBmp = Marshal.StringToHGlobalAnsi(path);
            SendMessage(hWndC, WM_CAP_FILE_SET_CAPTURE_FILEA, 0, hBmp.ToInt32());
            SendMessage(hWndC, WM_CAP_SEQUENCE, 0, 0);
        }
        ///  
        /// 停止录像  
        ///  
        public void StopKinescope()
        {
            SendMessage(hWndC, WM_CAP_STOP, 0, 0);
        }  
    }
}

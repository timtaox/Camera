using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace 摄像头
{
    public partial class Form1 : Form
    {
        Pick pick;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int left = 0;
            int top = 0;
            int width = 352;
            int w = pictureBox1.Width;
            int height = 288;
            int h = pictureBox1.Height;
            pick = new Pick(pictureBox1.Handle, left, top, w, h);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            pick.Stop();
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
            pick.Start();
        }

        private void btn_Capture_Click(object sender, EventArgs e)
        {
            pick.GrabImage("D:\\测试.jpg");
        }

        private void btn_Video_Click(object sender, EventArgs e)
        {
            if (btn_Video.Text == "录像")
            {
                pick.Kinescope("D:\\测试.avi");
                btn_Video.Text = "停止录像";
            }
            else
            {
                pick.StopKinescope();
                btn_Video.Text = "录像";
            }
        }
    }
}

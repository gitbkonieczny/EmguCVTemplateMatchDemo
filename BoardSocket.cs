using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
namespace TemplateMatchDemo
{
    class BoardSocket
    {
        public Point SocketStart { get; set; }
        public Size SocketSize { get; set; }

        public string TemplateValue { get; set; } //-None = empty socket

        public string Value { get; set; } //Found Value on socket

        public BoardSocket(Point SocketStart, Size SocketSize,string TemplateValue)
        {
            this.SocketStart = SocketStart;
            this.SocketSize = SocketSize;
            this.Value = "None";
            this.TemplateValue = TemplateValue;
        }
    }
}

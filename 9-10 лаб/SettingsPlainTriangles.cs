using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace belarusian_ornament
{
    class SettingsPlainTriangles
    {
        public Color BackgroundLinesColor = Color.FromArgb(220, 220, 220);
        public Color BackgroundColor = Color.White;
        public Color TriagleColor = Color.Red;

        public Graphics Graph;

        public int Scale { get; set; }
        public int IndentX { get; set; }
        public int IndentY { get; set; }
        public int WidthPlain { get; set; }
        public int HeightPlain { get; set; }
        public int CursorX { get; set; }
        public int CursorY { get; set; }
        public int TriagleRotation { get; set; }
        public int LineStartX { get; set; }
        public int LineStartY { get; set; }
        public bool LineStart { get; set; }
        public bool LineNotEnded { get; set; }
        public bool IndentChange { get; set; }
        public bool Changed { get; set; }


        public SettingsPlainTriangles()
        {
            BackgroundLinesColor = Color.FromArgb(220, 220, 220);
            BackgroundColor = Color.White;
            TriagleColor = Color.Red;

            Scale = 20;
            IndentX = 0;
            IndentY = 0;
            WidthPlain = 0;
            HeightPlain = 0;
            CursorX = 0;
            CursorY = 0;
            TriagleRotation = 0;
            LineStartX = 0;
            LineStartY = 0;

            LineStart = false;
            LineNotEnded = false;
            Changed = false;
            IndentChange = false;
        }
    }
}

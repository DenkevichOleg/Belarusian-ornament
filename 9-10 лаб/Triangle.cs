using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace belarusian_ornament
{
    class Triangle
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Rotation { get; private set; }
        public Color Color { get; private set; }
        public Triangle(int newCoordX, int newCoordY, int newRotation, Color newColor)
        {
            X = newCoordX;
            Y = newCoordY;
            Rotation = newRotation;
            Color = newColor;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace belarusian_ornament
{
    class DrawTriangle
    {
        public List<Triangle> listTriangle = new List<Triangle>();
        public List<Triangle> listRemoveTriangle = new List<Triangle>();

        public void DrawBackground(SettingsPlainTriangles settings)
        {
            Pen backgroundLinesPen = new Pen(settings.BackgroundLinesColor, 1);

            settings.Graph.Clear(settings.BackgroundColor);

            for (int i = 0; i <= settings.HeightPlain; i = i + settings.Scale)
            {
                settings.Graph.DrawLine(backgroundLinesPen, new Point(0, i), new Point(settings.WidthPlain, i));
            }

            for (int i = 0; i <= settings.WidthPlain; i = i + settings.Scale)
            {
                settings.Graph.DrawLine(backgroundLinesPen, new Point(i, 0), new Point(i, settings.HeightPlain));
            }
        }

        public void DrawTriagles(SettingsPlainTriangles settings)
        {
            Point[] P = new Point[3];

            for (int i = 0; i < listTriangle.Count(); i++)
            {
                int realX = (listTriangle[i].X + settings.IndentX) * settings.Scale;
                int realY = (listTriangle[i].Y + settings.IndentY) * settings.Scale;

                if (listTriangle[i].Rotation == 0)
                {
                    P[0] = new Point(realX, realY);
                    P[1] = new Point(realX + settings.Scale, realY + settings.Scale);
                    P[2] = new Point(realX, realY + settings.Scale);
                }
                if (listTriangle[i].Rotation == 1)
                {
                    P[0] = new Point(realX, realY);
                    P[1] = new Point(realX + settings.Scale, realY);
                    P[2] = new Point(realX, realY + settings.Scale);
                }
                if (listTriangle[i].Rotation == 2)
                {
                    P[0] = new Point(realX, realY);
                    P[1] = new Point(realX + settings.Scale, realY);
                    P[2] = new Point(realX + settings.Scale, realY + settings.Scale);
                }
                if (listTriangle[i].Rotation == 3)
                {
                    P[0] = new Point(realX, realY + settings.Scale);
                    P[1] = new Point(realX + settings.Scale, realY + settings.Scale);
                    P[2] = new Point(realX + settings.Scale, realY);
                }

                settings.Graph.DrawPolygon(new Pen(listTriangle[i].Color, 1), P);
                settings.Graph.FillPolygon(new SolidBrush(listTriangle[i].Color), P);
            }
        }

        public void DrawCursor(SettingsPlainTriangles settings, int x, int y)
        {
            Point[] P = new Point[3];
            int realX = x * settings.Scale;
            int realY = y * settings.Scale;

            if (settings.TriagleRotation == 0)
            {
                P[0] = new Point(realX, realY);
                P[1] = new Point(realX + settings.Scale, realY + settings.Scale);
                P[2] = new Point(realX, realY + settings.Scale);
            }
            if (settings.TriagleRotation == 1)
            {
                P[0] = new Point(realX, realY);
                P[1] = new Point(realX + settings.Scale, realY);
                P[2] = new Point(realX, realY + settings.Scale);
            }
            if (settings.TriagleRotation == 2)
            {
                P[0] = new Point(realX, realY);
                P[1] = new Point(realX + settings.Scale, realY);
                P[2] = new Point(realX + settings.Scale, realY + settings.Scale);
            }
            if (settings.TriagleRotation == 3)
            {
                P[0] = new Point(realX, realY + settings.Scale);
                P[1] = new Point(realX + settings.Scale, realY + settings.Scale);
                P[2] = new Point(realX + settings.Scale, realY);
            }
            settings.Graph.DrawPolygon(new Pen(settings.TriagleColor, 1), P);
            settings.Graph.FillPolygon(new SolidBrush(settings.TriagleColor), P);
        }

        public void RefreshImage(SettingsPlainTriangles settings)
        {
            DrawBackground(settings);
            DrawTriagles(settings);
            DrawCursor(settings, settings.CursorX, settings.CursorY);
        }

        public void RightRotation(SettingsPlainTriangles settings)
        {
            switch (settings.TriagleRotation)
            {
                case 0:
                    settings.TriagleRotation = 1;
                    break;
                case 1:
                    settings.TriagleRotation = 2;
                    break;
                case 2:
                    settings.TriagleRotation = 3;
                    break;
                case 3:
                    settings.TriagleRotation = 0;
                    break;
            }

            RefreshImage(settings);

            if (settings.LineStart && settings.LineNotEnded)
            {
                BresenhamLineTriangle(settings);
            }
        }

        public void LeftRotation(SettingsPlainTriangles settings)
        {
            switch (settings.TriagleRotation)
            {
                case 0:
                    settings.TriagleRotation = 3;
                    break;
                case 1:
                    settings.TriagleRotation = 0;
                    break;
                case 2:
                    settings.TriagleRotation = 1;
                    break;
                case 3:
                    settings.TriagleRotation = 2;
                    break;
            }

            RefreshImage(settings);

            if (settings.LineStart && settings.LineNotEnded)
            {
                BresenhamLineTriangle(settings);
            }
        }

        private void BresenhamLineTriangle(SettingsPlainTriangles settings)
        {
            int x = settings.LineStartX;
            int y = settings.LineStartY;
            int dx = Math.Abs(settings.CursorX - x);
            int dy = Math.Abs(settings.CursorY - y);
            int stepY = (y < settings.CursorY) ? 1 : -1;
            int stepX = (x < settings.CursorX) ? 1 : -1;
            int err = dx - dy;
            while (true)
            {
                if (settings.LineNotEnded == false)
                {
                    listTriangle.Add(new Triangle(x - settings.IndentX, y - settings.IndentY, settings.TriagleRotation, settings.TriagleColor));
                }
                else
                {
                    DrawCursor(settings, x, y);
                }
                if (x == settings.CursorX && y == settings.CursorY) break;
                if (err * 2 > -dy)
                {
                    err -= dy;
                    x += stepX;
                }
                if (x == settings.CursorX && y == settings.CursorY)
                {
                    if (settings.LineNotEnded == false)
                    {
                        listTriangle.Add(new Triangle(x - settings.IndentX, y - settings.IndentY, settings.TriagleRotation, settings.TriagleColor));
                    }
                    else
                    {
                        DrawCursor(settings, x, y);
                    }

                    break;
                }
                if (err * 2 < dx)
                {
                    err += dx;
                    y += stepY;
                }
            }
        }

        public void FixTriagle(SettingsPlainTriangles settings)
        {
            if (listRemoveTriangle.Count > 0)
            {
                listRemoveTriangle = new List<Triangle>();
            }

            if (settings.LineStart == false)
            {
                listTriangle.Add(new Triangle(settings.CursorX - settings.IndentX, settings.CursorY - settings.IndentY, settings.TriagleRotation, settings.TriagleColor));
                settings.Changed = true;
            }

            if (settings.LineStart && settings.LineNotEnded == false)
            {
                settings.LineStartX = settings.CursorX;
                settings.LineStartY = settings.CursorY;
                settings.LineNotEnded = true;
            }
            else
            {
                FixLineTriagle(settings);
            }
        }

        public void FixLineTriagle(SettingsPlainTriangles settings)
        {
            if (settings.LineStart && settings.LineNotEnded)
            {
                settings.Changed = true;
                settings.LineNotEnded = false;
                BresenhamLineTriangle(settings);
            }
        }

        public void SetNewCoordsCursor(SettingsPlainTriangles settings, int newX, int newY)
        {
            if (settings.IndentChange)
            {
                settings.IndentX = settings.IndentX + (newX - settings.CursorX);
                settings.IndentY = settings.IndentY + (newY - settings.CursorY);
            }

            if ((newX < settings.WidthPlain / settings.Scale) && (newX >= 0))
            {
                settings.CursorX = newX;
            }
            if ((newY < settings.HeightPlain / settings.Scale) && (newY >= 0))
            {
                settings.CursorY = newY;
            }

            RefreshImage(settings);

            if (settings.LineStart && settings.LineNotEnded)
            {
                BresenhamLineTriangle(settings);
            }
        }

        public bool DeleteTriangle(SettingsPlainTriangles settings, int x, int y)
        {
            int lastIndex = -1;
            for (int i = 0; i < listTriangle.Count; i++)
            {
                if (listTriangle[i].X == x - settings.IndentX && listTriangle[i].Y == y - settings.IndentY)
                {
                    lastIndex = i;
                }
            }
            if (lastIndex >= 0)
            {
                listRemoveTriangle.Add(listTriangle[lastIndex]);
                listTriangle.Remove(listTriangle[lastIndex]);
            }
            RefreshImage(settings);

            if (listRemoveTriangle.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool Undo(SettingsPlainTriangles settings)
        {
            if (listTriangle.Count > 0)
            {
                listRemoveTriangle.Add(listTriangle.Last());
                listTriangle.Remove(listTriangle.Last());
            }

            RefreshImage(settings);

            if (listTriangle.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool Redo(SettingsPlainTriangles settings)
        {
            if (listRemoveTriangle.Count > 0)
            {
                listTriangle.Add(listRemoveTriangle.Last());
                listRemoveTriangle.Remove(listRemoveTriangle.Last());
            }

            RefreshImage(settings);

            if (listRemoveTriangle.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

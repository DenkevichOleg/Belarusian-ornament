using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace belarusian_ornament
{
    public partial class Form1 : Form
    {
        DrawTriangle triangles = new DrawTriangle();
        SettingsPlainTriangles settingsTriangles = new SettingsPlainTriangles();

        public Form1()
        {
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(this_MouseWheel);
        }

        public Form1(StreamReader stream, string nameFile)
        {
            InitializeComponent();
            string realName = nameFile.Split('\\').Last().Remove(nameFile.Split('\\').Last().LastIndexOf('.'));
            this.Text = "Беларускі арнамет " + realName;
            while (stream.EndOfStream == false)
            {
                triangles.listTriangle.Add(new Triangle(
                    Convert.ToInt32(stream.ReadLine()),
                    Convert.ToInt32(stream.ReadLine()),
                    Convert.ToInt32(stream.ReadLine()),
                    Color.FromArgb(Convert.ToInt32(stream.ReadLine()))
                    ));
            }
            this.MouseWheel += new MouseEventHandler(this_MouseWheel);
        }

        private void refreshImage_Click()
        {
            triangles.RefreshImage(settingsTriangles);
            mainPictureBox.Invalidate();
        }
        private void pictureBox_Resize(object sender, EventArgs e)
        {
            int height = mainPictureBox.Height;
            int width = mainPictureBox.Width;

            if (width > 0 && height > 0)
            {
                Image im = new Bitmap(width, height);
                if (mainPictureBox.Image != null)
                    mainPictureBox.Image.Dispose();
                mainPictureBox.Image = im;

                Graphics graph = Graphics.FromImage(mainPictureBox.Image);
                if (settingsTriangles.Graph != null)
                    settingsTriangles.Graph.Dispose();
                settingsTriangles.Graph = graph;

                settingsTriangles.WidthPlain = width;
                settingsTriangles.HeightPlain = height;
                triangles.RefreshImage(settingsTriangles);
            }
        }

        private void commitPositionTriangle(object sender, EventArgs e)
        {
            triangles.FixTriagle(settingsTriangles);
            undo.Enabled = true;
            redo.Enabled = false;
            //textBox1.Text = " " + triangles.listTriangle.Count() + " | " + settingsTriangles.CursorX + "," + settingsTriangles.CursorX;
        }
        private void mainPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                triangles.FixTriagle(settingsTriangles);
                undo.Enabled = true;
                redo.Enabled = false;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                redo.Enabled = triangles.DeleteTriangle(settingsTriangles, e.X / settingsTriangles.Scale, e.Y / settingsTriangles.Scale);
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                settingsTriangles.IndentChange = true;
            }
        }
        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            triangles.FixLineTriagle(settingsTriangles);
            undo.Enabled = true;
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                settingsTriangles.IndentChange = false;
            }
            //textBox1.Text = " " + triangles.listTriangle.Count() + " | " + settingsTriangles.CursorX + "," + settingsTriangles.CursorY;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            int realX = e.X / settingsTriangles.Scale;
            int realY = e.Y / settingsTriangles.Scale;
            triangles.SetNewCoordsCursor(settingsTriangles, realX, realY);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                redo.Enabled = triangles.DeleteTriangle(settingsTriangles, e.X / settingsTriangles.Scale, e.Y / settingsTriangles.Scale);
            }
            mainPictureBox.Invalidate();
            //textBox1.Text = "X = " + settingsTriangles.IndentX + "Y = " + settingsTriangles.IndentY;
        }
        private void this_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                rightRotation(sender, e);
            }
            else
            {
                leftRotation(sender, e);
            }
        }

        private void leftRotation(object sender, EventArgs e)
        {
            switch (settingsTriangles.TriagleRotation)
            {
                case 0:
                    pictureBox4.Image = Properties.Resources.Icon2;
                    break;
                case 1:
                    pictureBox4.Image = Properties.Resources.Icon5;
                    break;
                case 2:
                    pictureBox4.Image = Properties.Resources.Icon1;
                    break;
                case 3:
                    pictureBox4.Image = Properties.Resources.Icon4;
                    break;
            }
            triangles.LeftRotation(settingsTriangles);
            mainPictureBox.Invalidate();
        }
        private void rightRotation(object sender, EventArgs e)
        {
            switch (settingsTriangles.TriagleRotation)
            {
                case 0:
                    pictureBox4.Image = Properties.Resources.Icon1;
                    break;
                case 1:
                    pictureBox4.Image = Properties.Resources.Icon4;
                    break;
                case 2:
                    pictureBox4.Image = Properties.Resources.Icon2;
                    break;
                case 3:
                    pictureBox4.Image = Properties.Resources.Icon5;
                    break;
            }
            triangles.RightRotation(settingsTriangles);
            mainPictureBox.Invalidate();
        }

        private void chooseTriagleCursor(object sender, EventArgs e)
        {
            settingsTriangles.LineStart = false;
            pictureBox4.BorderStyle = BorderStyle.Fixed3D;
            pictureBox3.BorderStyle = BorderStyle.FixedSingle;
        }
        private void chooseLineTriagleCursor(object sender, EventArgs e)
        {
            settingsTriangles.LineStart = true;
            pictureBox3.BorderStyle = BorderStyle.Fixed3D;
            pictureBox4.BorderStyle = BorderStyle.FixedSingle;
        }
        private void selectColor_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.ShowHelp = true;
            if (MyDialog.ShowDialog() == DialogResult.OK)
            {
                settingsTriangles.TriagleColor = MyDialog.Color;
                label2.ForeColor = MyDialog.Color;
            }
            refreshImage_Click();

            mainPictureBox.Invalidate();

            refreshImage_Click();
        }
        private void undo_Click(object sender, EventArgs e)
        {
            undo.Enabled = triangles.Undo(settingsTriangles);
            redo.Enabled = true;
            mainPictureBox.Invalidate();
        }
        private void redo_Click(object sender, EventArgs e)
        {
            redo.Enabled = triangles.Redo(settingsTriangles);
            undo.Enabled = true;
            mainPictureBox.Invalidate();
        }
        private void scale_ValueChanged(object sender, EventArgs e)
        {
            settingsTriangles.Scale = (int)Scale.Value;
            refreshImage_Click();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Z & e.Modifiers == Keys.Control)
            {
                undo_Click(sender, e);
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Y & e.Modifiers == Keys.Control)
            {
                redo_Click(sender, e);
                e.Handled = true;
            }

            if (e.KeyCode == Keys.W)
            {
                moveUp_Click(sender, e);
                e.Handled = true;
            }
            if (e.KeyCode == Keys.S)
            {
                moveDown_Click(sender, e);
                e.Handled = true;
            }
            if (e.KeyCode == Keys.D)
            {
                moveRight_Click(sender, e);
                e.Handled = true;
            }
            if (e.KeyCode == Keys.A)
            {
                moveLeft_Click(sender, e);
                e.Handled = true;
            }


            if (e.KeyCode == Keys.E)
            {
                rightRotation(sender, e);
                e.Handled = true;
            }

            if (e.KeyCode == Keys.Q)
            {
                leftRotation(sender, e);
                e.Handled = true;
            }

            if (e.KeyCode == Keys.Enter)
            {
                commitPositionTriangle(sender, e);
                e.Handled = true;
            }
        }
        private void moveRight_Click(object sender, EventArgs e)
        {
            triangles.SetNewCoordsCursor(settingsTriangles, settingsTriangles.CursorX + 1, settingsTriangles.CursorY);
            mainPictureBox.Invalidate();
        }
        private void moveDown_Click(object sender, EventArgs e)
        {
            triangles.SetNewCoordsCursor(settingsTriangles, settingsTriangles.CursorX, settingsTriangles.CursorY + 1);
            mainPictureBox.Invalidate();
        }
        private void moveLeft_Click(object sender, EventArgs e)
        {
            triangles.SetNewCoordsCursor(settingsTriangles, settingsTriangles.CursorX - 1, settingsTriangles.CursorY);
            mainPictureBox.Invalidate();
        }
        private void moveUp_Click(object sender, EventArgs e)
        {
            triangles.SetNewCoordsCursor(settingsTriangles, settingsTriangles.CursorX, settingsTriangles.CursorY - 1);
            mainPictureBox.Invalidate();
        }


        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (settingsTriangles.Changed)
                сохранитьToolStripMenuItem.Enabled = true;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (saveFileDialog1.FilterIndex == 1)
            {
                string mySuperString = "";
                for (int i = 0; i < triangles.listTriangle.Count(); i++)
                {
                    mySuperString += Convert.ToString(triangles.listTriangle[i].X) + "\n"
                        + Convert.ToString(triangles.listTriangle[i].Y) + "\n"
                        + Convert.ToString(triangles.listTriangle[i].Rotation) + "\n"
                        + triangles.listTriangle[i].Color.ToArgb() + "\n";
                }
                File.AppendAllText(saveFileDialog1.FileName, mySuperString);
            }
            if (saveFileDialog1.FilterIndex == 2)
            {
                triangles.DrawBackground(settingsTriangles);
                triangles.DrawTriagles(settingsTriangles);
                mainPictureBox.Invalidate();
                mainPictureBox.Image.Save(saveFileDialog1.FileName, ImageFormat.Png);
            }
            if (saveFileDialog1.FilterIndex == 3)
            {
                triangles.DrawBackground(settingsTriangles);
                triangles.DrawTriagles(settingsTriangles);
                mainPictureBox.Invalidate();
                mainPictureBox.Image.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader stream = File.OpenText(openFileDialog1.FileName);
                Form1 f2 = new Form1(stream, openFileDialog1.FileName);
                f2.Show();
            }
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 F1 = new Form1();
            F1.Show();
        }

        private void цветЛинийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.ShowHelp = true;
            if (MyDialog.ShowDialog() == DialogResult.OK)
                settingsTriangles.BackgroundLinesColor = MyDialog.Color;
        }

        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            MyDialog.ShowHelp = true;
            if (MyDialog.ShowDialog() == DialogResult.OK)
                settingsTriangles.BackgroundColor = MyDialog.Color;
            refreshImage_Click();
        }

        private void управлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Повернуть треугольник - Колесико мыши/Q/E \nЗакрепить - Левая кнопка мыши/Enter \nДвигать курсор вверх - Мышь вверх/E \nДвигать курсор вниз - Мышь вниз/S \nДвигать курсор вправо - Мышь вправо/D \nДвигать курсор влево - Мышь влево/A \nОтменить - Ctrl+Z \nПовторить - Ctrl+Y \nДвигать все изображение - Средняя кнопка мыши \nСтёреть треугольник - Правая кнопка мыши", "Управление");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = DialogResult.No;

            if (settingsTriangles.Changed)
            {
                result = MessageBox.Show("Хотите сохранить перед закрытием?", "Закрытие приложения",
               MessageBoxButtons.YesNoCancel);
            }

            switch (result)
            {
                case DialogResult.Yes:
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        e.Cancel = false;
                    else
                        e.Cancel = true;
                    break;

                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;

                case DialogResult.No:
                    e.Cancel = false;
                    break;

            }

        }
    }
}



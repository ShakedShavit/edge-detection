using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edge_Detection
{
    public partial class Form1 : Form
    {
        Bitmap image1;
        Bitmap image2;
        Color[,] copyArray;//copyies the image
        Color[,] copyArray2;//copyies the copy of the image

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string imageLocation = "";
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg file(*.jpg)|*.jpg| PNG files(*.png)|*.png| All Files(*.*)|*.*";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageLocation = dialog.FileName;

                    pictureBox1.ImageLocation = imageLocation;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void button2_Click(object sender, EventArgs e)
        {
            image1 = new Bitmap(pictureBox1.ImageLocation);
            image2 = new Bitmap(pictureBox1.ImageLocation);

            #region copying the image to an array
            copyArray = new Color[image1.Size.Width, image1.Size.Height];

            for (int x = 0; x < image1.Width; x++)
                for (int y = 0; y < image1.Height; y++)
                    copyArray[x, y] = image1.GetPixel(x, y);
            #endregion           

            for (int x = 0; x < image1.Width; x++)
                for (int y = 0; y < image1.Height; y++)
                {
                    int r1 = image1.GetPixel(x, y).R;
                    int g1 = image1.GetPixel(x, y).G;
                    int b1 = image1.GetPixel(x, y).B;
                    int r2;
                    int g2;
                    int b2;

                    for (int i = 0;  i < 4; i++)
                    {
                        if ((i == 0) && (x + 1 < image1.Width))
                        {
                            r2 = image1.GetPixel(x + 1, y).R;
                            g2 = image1.GetPixel(x + 1, y).G;
                            b2 = image1.GetPixel(x + 1, y).B;
                            Coloring_Borders(r1, r2, g1, g2, b1, b2, x, y, x + 1, y);
                        }
                        if ((i == 1) && (y + 1 < image1.Height))
                        {
                            r2 = image1.GetPixel(x, y + 1).R;
                            g2 = image1.GetPixel(x, y + 1).G;
                            b2 = image1.GetPixel(x, y + 1).B;
                            Coloring_Borders(r1, r2, g1, g2, b1, b2, x, y, x, y + 1);
                        }
                        else if ((i == 2) && (x + 1 < image1.Width) && (y + 1 < image1.Height))
                        {
                            r2 = image1.GetPixel(x + 1, y + 1).R;
                            g2 = image1.GetPixel(x + 1, y + 1).G;
                            b2 = image1.GetPixel(x + 1, y + 1).B;
                            Coloring_Borders(r1, r2, g1, g2, b1, b2, x, y, x + 1, y + 1);
                        }
                        else if ((x - 1 >= 0) && (y + 1 < image1.Height))
                        {
                            r2 = image1.GetPixel(x - 1, y + 1).R;
                            g2 = image1.GetPixel(x - 1, y + 1).G;
                            b2 = image1.GetPixel(x - 1, y + 1).B;
                            Coloring_Borders(r1, r2, g1, g2, b1, b2, x, y, x - 1, y + 1);
                        }
                    }
                }

            #region copying the array to a second array (array2 = array)
            copyArray2 = new Color[image1.Size.Width, image1.Size.Height];
            for (int x = 0; x < image1.Width; x++)
                for (int y = 0; y < image1.Height; y++)
                    copyArray2[x, y] = copyArray[x, y];
            #endregion

            #region Cluteter cleaning
            if (numericUpDown2.Value > 0)
            {
                for (int x = 0; x < image1.Width; x++)
                    for (int y = 0; y < image1.Height; y++)
                    {
                        if (copyArray[x, y].R == 0 && copyArray[x, y].G == 0 && copyArray[x, y].B == 0)
                        {
                            int EdgeNoiseCounter = 0;
                            for (int noise = 1; noise <= numericUpDown2.Value; noise++)
                            {
                                if (x + noise < image1.Width)
                                    if (copyArray[x + noise, y].R == 0 && copyArray[x + noise, y].G == 0 && copyArray[x + noise, y].B == 0)
                                        EdgeNoiseCounter++;

                                if (y + noise < image1.Height)
                                    if (copyArray[x, y + noise].R == 0 && copyArray[x, y + noise].G == 0 && copyArray[x, y + noise].B == 0)
                                        EdgeNoiseCounter++;

                                if (x - noise >= 0)
                                    if (copyArray[x - noise, y].R == 0 && copyArray[x - noise, y].G == 0 && copyArray[x - noise, y].B == 0)
                                        EdgeNoiseCounter++;

                                if (y - noise >= 0)
                                    if (copyArray[x, y - noise].R == 0 && copyArray[x, y - noise].G == 0 && copyArray[x, y - noise].B == 0)
                                        EdgeNoiseCounter++;

                                if (x + noise < image1.Width && y + noise < image1.Height)
                                    if (copyArray[x + noise, y + noise].R == 0 && copyArray[x + noise, y + noise].G == 0 && copyArray[x + noise, y + noise].B == 0)
                                        EdgeNoiseCounter++;

                                if (x + noise < image1.Width && y - noise >= 0)
                                    if (copyArray[x + noise, y - noise].R == 0 && copyArray[x + noise, y - noise].G == 0 && copyArray[x + noise, y - noise].B == 0)
                                        EdgeNoiseCounter++;

                                if (x - noise >= 0 && y + noise < image1.Height)
                                    if (copyArray[x - noise, y + noise].R == 0 && copyArray[x - noise, y + noise].G == 0 && copyArray[x - noise, y + noise].B == 0)
                                        EdgeNoiseCounter++;

                                if (x - noise >= 0 && y - noise >= 0)
                                    if (copyArray[x - noise, y - noise].R == 0 && copyArray[x - noise, y - noise].G == 0 && copyArray[x - noise, y - noise].B == 0)
                                        EdgeNoiseCounter++;
                            }
                            //if there are a lot of edge detections or a few edge detection, it erases the edge
                            if (EdgeNoiseCounter < numericUpDown3.Value || EdgeNoiseCounter > numericUpDown4.Value)
                                copyArray[x, y] = image1.GetPixel(x, y);
                        }
                    }
            }
            #endregion

            #region copying the image (image2 = array)
            for (int x = 0; x < image1.Width; x++)
                for (int y = 0; y < image1.Height; y++)
                    image2.SetPixel(x, y, copyArray[x, y]);
            #endregion

            pictureBox2.Image = image2;
        }

        void Coloring_Borders(int r1, int r2, int g1, int g2, int b1, int b2, int x1, int y1, int x2, int y2)
        {
            Color newColor = Color.FromArgb(0, 0, 0);
            //int change = (Math.Abs(r2 - r1) + Math.Abs(g2 - g1) + Math.Abs(b2 - b1)) / 3;//the average of the difference between each element of the colors of two following pixels
            double change = Math.Sqrt(Math.Pow(r2 - r1, 2) + Math.Pow(g2 - g1, 2) + Math.Pow(b2 - b1, 2));

            //regular version - draws border on the brighter side
            if (!checkBox1.Checked && !checkBox2.Checked && !checkBox3.Checked)
            {
                if ((int)change >= numericUpDown1.Value)
                {
                    if (r1 + g1 + b1 > r2 + g2 + b2)
                        copyArray[x1, y1] = newColor;
                    else
                        copyArray[x2, y2] = newColor;
                }
            }
            //opposite regular version - draws border on the darker side
            else if (!checkBox1.Checked && checkBox3.Checked && !checkBox2.Checked)
            {
                if ((int)change >= numericUpDown1.Value)
                {
                    if (r1 + g1 + b1 < r2 + g2 + b2)
                        copyArray[x1, y1] = newColor;
                    else
                        copyArray[x2, y2] = newColor;
                }
            }
            //another opposite regular version - draws border on both sides
            else if (checkBox1.Checked && !checkBox2.Checked)
            {
                if ((int)change >= numericUpDown1.Value)
                {
                    copyArray[x1, y1] = newColor;
                    copyArray[x2, y2] = newColor;
                }
            }
            //opposie version - it was a mistake but it looks cool so I made it feature - draws a border only if the right side (of the border) is darker
            else if (checkBox2.Checked)
            {
                change = ((r2 - r1) + (g2 - g1) + (b2 - b1)) / 3;
                if ((int)change >= numericUpDown1.Value)
                    copyArray[x1, y1] = newColor;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value * 6 <= numericUpDown4.Maximum)
                numericUpDown4.Value = numericUpDown2.Value * 6;
            if (numericUpDown2.Value / 6 <= numericUpDown4.Maximum)
                numericUpDown3.Value = numericUpDown2.Value / 6;
        }
    }
}

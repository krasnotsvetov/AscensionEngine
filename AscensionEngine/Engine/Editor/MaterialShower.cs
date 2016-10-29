using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ascension.Engine.Editor
{
    public class MaterialShower
    {
        public static List<Control> TextureShower(int count)
        {

            List<Control> controls = new List<Control>();
            int step = 200;
            for (int i = 0; i < count; i++)
            {
                Label label = new Label();
                label.Location = new System.Drawing.Point(10 + i * step, 10);
                label.Text = "Texture " + i + ":";
                label.Name = "TextureLabel" + i;

                ComboBox textureComboBox = new ComboBox();
                textureComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                textureComboBox.FormattingEnabled = true;
                textureComboBox.Location = new System.Drawing.Point(10 + i * step, 190);
                textureComboBox.Name = "TextureComboBox" + i;
                textureComboBox.Size = new System.Drawing.Size(140, 30);
                textureComboBox.Tag = i;

                PictureBox textureBox = new PictureBox();
                textureBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
                textureBox.BackgroundImageLayout = ImageLayout.Stretch;
                textureBox.Location = new System.Drawing.Point(10 + i * step, 40);
                textureBox.Name = "textureBox" + i;
                textureBox.Size = new System.Drawing.Size(140, 140);
                textureBox.TabStop = false;
                textureBox.Tag = i;

                controls.Add(label);
                controls.Add(textureComboBox);
                controls.Add(textureBox);
            }
            return controls;
        }
    }
}

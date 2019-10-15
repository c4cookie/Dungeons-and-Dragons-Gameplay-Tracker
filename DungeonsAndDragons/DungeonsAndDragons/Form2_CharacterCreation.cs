using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DungeonsAndDragons
{
    public partial class Form2_CharacterCreation : Form
    {

        Form mainForm;
        Bitmap bmpChooseColor;
        Color hoveredColor;
        Color assignedColor;

        public Form2_CharacterCreation(Form mainForm)
        {
            InitializeComponent();
            SetValues(mainForm);
        }

        /*****************************************************************
        Function: Private void SetValues
        Description: Initializes local variables
        ******************************************************************/
        private void SetValues(Form mainForm)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
                                                            
            bmpChooseColor = new Bitmap(pictureBox1.Image);
            assignedColor = Color.Orange;
            customPictureBoxCircle_chosenAvatar.BackColor = assignedColor;
            this.mainForm = mainForm;
        }

        /*****************************************************************
        Function: Private void Form2_CharacterCreation_FormClosing
        Description: When the form is closing, enable main form again
        ******************************************************************/
        private void Form2_CharacterCreation_FormClosing(object sender, FormClosingEventArgs e)
        {
            textBox_name.Text = "";
            mainForm.Enabled = true;
        }

        /*****************************************************************
        Function: Private void button1_Click
        Description: Creates a character
        ******************************************************************/
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_name.Text.Length > 0)
            {
                Program.players.Add(new Player(textBox_name.Text.ToString(), createAvatar(textBox_name.Text.ToString())));
                Form1.thisFormStatic.AddPlayer();
                Console.WriteLine(textBox_name.Text.ToString() + "debug");
                this.Close();
            }
        }

        private CustomPictureBoxCircle createAvatar(string picturebox_name)
        {
            CustomPictureBoxCircle returnavatar = new CustomPictureBoxCircle();
            returnavatar.Location = new Point(0, 0);
            returnavatar.Name = "picturebox_" + picturebox_name;
            returnavatar.Size = new Size(40, 40);
            returnavatar.BackColor = customPictureBoxCircle_chosenAvatar.BackColor;
            returnavatar.BackgroundImage = customPictureBoxCircle_chosenAvatar.BackgroundImage;
            returnavatar.BackgroundImageLayout = ImageLayout.Stretch;
            returnavatar.Tag = "avatar";
            returnavatar.DoubleClick += new EventHandler(Form1.thisFormStatic.Avatar_PlaceOnGrid);
            return returnavatar; 
        } 


        /*****************************************************************
        Function: Private void pictureBox1_MouseMove
        Description: when user moves mouse around the color pallet, assign
        the color to the little box in the top right
        ******************************************************************/
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                hoveredColor = bmpChooseColor.GetPixel(e.X, e.Y);
                customPictureBoxCircle_chosenAvatar.BackColor = hoveredColor;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
        }

        /*****************************************************************
        Function: Private void pictureBox1_MouseLeave
        Description: when users mouse leaves the color pallet, assign
        avatar color back to selected color
        ******************************************************************/
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            customPictureBoxCircle_chosenAvatar.BackColor = assignedColor;
        }

        /*****************************************************************
        Function: Private void pictureBox1_Click
        Description: when user clicks on color pallet, assign selected color
        ******************************************************************/
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            assignedColor = hoveredColor;
        }

        /*****************************************************************
        Function: Private void picturebox_click
        Description: when a user clicks on a picture box, assign
        its background image to the main avatar
        ******************************************************************/
        private void picturebox_click(object sender, EventArgs e)
        {
            CustomPictureBoxCircle clickedAvatar = sender as CustomPictureBoxCircle;
            customPictureBoxCircle_chosenAvatar.BackgroundImage = clickedAvatar.BackgroundImage;
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DungeonsAndDragons
{
    public partial class Form1 : Form
    {
        string tempstorage;
        string[] tempstoragearray;
        Point lastPoint;
        bool isMouseDown;
        Bitmap bmpChooseColor;
        Color hoveredColor;
        Color penColor;
        Cursor cursor_pencil;
        Form formCharacterCreation;
        public static Form1 thisFormStatic;
        GridSizeControl GridControl;
        List<TableLayoutPanel> TableLayout;

        int amount;
        int ysize;
        int xsize;
        Graphics g_pengraphics;

        private bool isDragging = false;
        private bool allowDrag = true;
        private int currentX, currentY;

        //enemyCreationVariables
        Bitmap bmpChooseColorEnemy;
        Color hoveredColorEnemy;
        Color assignedColorEnemy;

        //playerCreationVariables
        Bitmap bmpChooseColorPlayer;
        Color hoveredColorPlayer;
        Color assignedColorPlayer;

        public Form1()
        {
            InitializeComponent();
            SetValues();
            DrawGrid();

            //Main Page
            ChangeParent(label2, pictureBox1);
        }

        /*****************************************************************
        Function: SetValues
        Trigger: application startup
        Where: N/A
        Result: sets initial values
        ******************************************************************/
        private void SetValues()
        {
            //Objects
            formCharacterCreation = new Form2_CharacterCreation(this);
            GridControl = new GridSizeControl();
            thisFormStatic = this;
            TableLayout = new List<TableLayoutPanel>();

            //UI
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            trackBar_gridsize.Value = 2;
            trackBar_pensize.Value = 4;
            amount = 40;
            ysize = 20;
            xsize = 20;
            ChangeGridSize();

            //Tab Control, Hide tab control header
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;

            //Color
            lastPoint = Point.Empty;
            isMouseDown = new Boolean();
            bmpChooseColor = new Bitmap(pictureBox_choosecolor.Image);
            penColor = Color.Blue;
            customPictureBoxCircle_displaycolor.BackColor = penColor;
            cursor_pencil = new Cursor(Properties.Resources.CustomPencil.Handle);

            //enemy creation tab section
            bmpChooseColorEnemy = new Bitmap(picturebox_enemycreationColor.Image);
            assignedColorEnemy = Color.Orange;
            customPictureBoxCircle_enemycreation_chosenAvatar.BackColor = assignedColorEnemy;
            numericUpDown_enemy_health.Maximum = 999;
            numericUpDown_enemy_health.Minimum = 0;

            listView_display_names_enemy.View = View.List;

            //player creation tab section
            bmpChooseColorPlayer = new Bitmap(pictureBox_playercreation_color.Image);
            assignedColorPlayer = Color.Orange;
            customPictureBoxCircle_playercreation_chosenAvatar.BackColor = assignedColorPlayer;
            listView_display_names_player.View = View.List;
        }

        /*****************************************************************
        Function: ChangeParent
        Trigger: called via different functions
        Where: N/A
        Result: Changes the objects parent without moving its position to 0.0
        ******************************************************************/
        void ChangeParent(Control child, Control newParent)
        {
            child.Location = newParent.PointToClient(child.PointToScreen(Point.Empty));
            child.Parent = newParent;
        }

        /*****************************************************************
        Functions: AddPlayer
                   CreateTable
                   CreateAvatarPlayer
        Trigger: called via different functions
        Where: N/A
        Result: AddPlayer function called, creates a table for this player
                selecting its avatar, name and health
                then adds it to the layout list on the main page.
        ******************************************************************/
        public void AddPlayer()
        {
            CreateTable(Program.players[Program.players.Count - 1].ReturnName());
            flowLayoutPanel1.Controls.Add(TableLayout[TableLayout.Count - 1]);
            Console.WriteLine(Program.players[Program.players.Count - 1].ReturnName() + " Player Added");
        }
        private void CreateTable(string playername)
        {
            TableLayoutPanel PlayerPanel = new TableLayoutPanel();
            Label labelname = new Label();
            labelname.Anchor = AnchorStyles.None;
            labelname.Font = new Font("Arial", 8, FontStyle.Bold);
            labelname.Text = playername;

            PictureBox whosturn = new PictureBox();
            whosturn.Size = new Size(40, 40);
            whosturn.Tag = "whosturn";
            if (TableLayout.Count == 0)
                whosturn.BackgroundImage = Properties.Resources.myturn;
            else
                whosturn.BackgroundImage = null;
            whosturn.BackgroundImageLayout = ImageLayout.Stretch;

            PlayerPanel.Tag = "runtimetable";
            PlayerPanel.ColumnCount = 5;
            PlayerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            PlayerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            PlayerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            PlayerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            PlayerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            PlayerPanel.Controls.Add(Program.players[Program.players.Count - 1].ReturnAvatar(), 0, 0);
            PlayerPanel.Controls.Add(labelname, 1, 0);
            PlayerPanel.Controls.Add(whosturn, 4, 0); // add the arrow (whos turn image here)
            PlayerPanel.Location = new System.Drawing.Point(0, 0);
            PlayerPanel.Name = playername + "_table";
            PlayerPanel.RowCount = 1;
            PlayerPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            PlayerPanel.Size = new System.Drawing.Size(250, 60);
            PlayerPanel.TabIndex = 0;

            TableLayout.Add(PlayerPanel);
        }
        private CustomPictureBoxCircle createAvatarPlayer(string picturebox_name)
        {
            CustomPictureBoxCircle returnavatar = new CustomPictureBoxCircle();
            returnavatar.Location = new Point(0, 0);
            returnavatar.Name = "picturebox_" + picturebox_name;
            returnavatar.Size = new Size(40, 40);
            returnavatar.BackColor = customPictureBoxCircle_playercreation_chosenAvatar.BackColor;
            returnavatar.BackgroundImage = customPictureBoxCircle_playercreation_chosenAvatar.BackgroundImage;
            returnavatar.BackgroundImageLayout = ImageLayout.Stretch;
            returnavatar.Tag = "avatar";
            returnavatar.DoubleClick += new EventHandler(Form1.thisFormStatic.Avatar_PlaceOnGrid);
            return returnavatar;
        }

        /*****************************************************************
        Functions: AddEnemy
                   CreateTableEnemy
                   createAvatarEnemy
        Trigger: called via different functions
        Where: N/A
        Result: AddEnemy function called, creates a table for this enemy
                selecting its avatar, name and health
                then adds it to the layout list on the main page.
        ******************************************************************/
        public void AddEnemy()
        {
            CreateTableEnemy(Program.enemies[Program.enemies.Count - 1].ReturnName());
            flowLayoutPanel1.Controls.Add(TableLayout[TableLayout.Count - 1]);
            Console.WriteLine(Program.enemies[Program.enemies.Count - 1].ReturnName() + " Player Added");
        }
        private void CreateTableEnemy(string playername)
        {
            TableLayoutPanel PlayerPanel = new TableLayoutPanel();
            Label labelname = new Label();
            labelname.Anchor = AnchorStyles.None;
            labelname.Font = new Font("Arial", 8, FontStyle.Bold);
            labelname.Text = playername;

            TextBox healthe = new TextBox();
            healthe.Anchor = AnchorStyles.None;
            healthe.Text = Program.enemies[Program.enemies.Count - 1].ReturnHealth().ToString();

            PictureBox whosturn = new PictureBox();
            whosturn.Size = new Size(40, 40);
            whosturn.Tag = "whosturn";
            if (TableLayout.Count == 0)
                whosturn.BackgroundImage = Properties.Resources.myturn;
            else
                whosturn.BackgroundImage = null;
            whosturn.BackgroundImageLayout = ImageLayout.Stretch;

            PlayerPanel.Tag = "runtimetable";
            PlayerPanel.ColumnCount = 5;
            PlayerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            PlayerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            PlayerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            PlayerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            PlayerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            PlayerPanel.Controls.Add(Program.enemies[Program.enemies.Count - 1].ReturnAvatar(), 0, 0);
            PlayerPanel.Controls.Add(labelname, 1, 0);
            PlayerPanel.Controls.Add(healthe, 2, 0);
            PlayerPanel.Controls.Add(whosturn, 4, 0); // add the arrow (whos turn image here)
            PlayerPanel.Location = new System.Drawing.Point(0, 0);
            PlayerPanel.Name = playername + "_table";
            PlayerPanel.RowCount = 1;
            PlayerPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            PlayerPanel.Size = new System.Drawing.Size(250, 60);
            PlayerPanel.TabIndex = 0;

           // PlayerPanel.DoubleClick += new EventHandler(Form1.thisFormStatic.changeGameplayListOrder);
            TableLayout.Add(PlayerPanel);
        }
        private CustomPictureBoxCircle createAvatarEnemy(string picturebox_name)
        {
            CustomPictureBoxCircle returnavatar = new CustomPictureBoxCircle();
            returnavatar.Location = new Point(0, 0);
            returnavatar.Name = "picturebox_" + picturebox_name;
            returnavatar.Size = new Size(40, 40);
            returnavatar.BackColor = customPictureBoxCircle_enemycreation_chosenAvatar.BackColor;
            returnavatar.BackgroundImage = customPictureBoxCircle_enemycreation_chosenAvatar.BackgroundImage;
            returnavatar.BackgroundImageLayout = ImageLayout.Stretch;
            returnavatar.Tag = "avatar";
            returnavatar.DoubleClick += new EventHandler(Form1.thisFormStatic.Avatar_PlaceOnGrid);
            return returnavatar;
        }

        /*****************************************************************
        Function: GetPictureBoxClone
        Trigger: called via different functions
        Where: N/A
        Result: creates a copy of a circle picturebox and 'clones it'
        ******************************************************************/
        private CustomPictureBoxCircle GetPictureBoxClone(CustomPictureBoxCircle origionalp)
        {
            CustomPictureBoxCircle pb = new CustomPictureBoxCircle();

            pb.Size = origionalp.Size;
            // pb.Name = "gameplay_" +origionalp.Name;
            pb.Name = origionalp.Name;
            pb.Size = origionalp.Size;
            pb.BackColor = origionalp.BackColor;
            pb.BackgroundImage = origionalp.BackgroundImage;
            pb.BackgroundImageLayout = origionalp.BackgroundImageLayout;
            pb.Tag = origionalp.Tag;
            pb.MouseMove += new MouseEventHandler(Avatar_MouseMove);
            pb.MouseDown += new MouseEventHandler(Avatar_MouseDown);
            pb.MouseUp += new MouseEventHandler(Avatar_MouseUp);
            pb.MouseHover += new EventHandler(Form1.thisFormStatic.avatartooltip);

            return pb;
        }

        /*****************************************************************
        Functions: pictureBox_drawingbox_MouseDown
                   pictureBox_drawingbox_MouseMove
                   pictureBox_drawingbox_MouseUp
                   pictureBox_drawingbox_MouseLeave
                   pictureBox_drawingbox_MouseEnter
        Trigger: hold mouse down & move ontop of grid (drawing board picturebox)
        Where: mainpage
        Result: using a pen, draws with paint on gridboard
        ******************************************************************/
        private void pictureBox_drawingbox_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = e.Location;
            isMouseDown = true;
        }
        private void pictureBox_drawingbox_MouseMove(object sender, MouseEventArgs e)
        {
            DrawGrid();
            if (isMouseDown == true)
            {
                if (lastPoint != null)
                {
                    if (pictureBox_drawingbox.Image == null)
                    {
                        Bitmap bmp = new Bitmap(pictureBox_drawingbox.Width, pictureBox_drawingbox.Height);
                        pictureBox_drawingbox.Image = bmp;
                    }

                    using (g_pengraphics = Graphics.FromImage(pictureBox_drawingbox.Image))
                    {
                        g_pengraphics.DrawLine(new Pen(penColor, (trackBar_pensize.Value * 2)), lastPoint, e.Location);
                        g_pengraphics.SmoothingMode = SmoothingMode.AntiAlias;
                    }

                    lastPoint = e.Location;
                    pictureBox_drawingbox.Invalidate();
                }
            }
        }
        private void pictureBox_drawingbox_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            lastPoint = Point.Empty;
        }
        private void pictureBox_drawingbox_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_drawingbox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox_drawingbox.Cursor = Cursors.Default;;
        }
        private void pictureBox_drawingbox_MouseEnter(object sender, EventArgs e)
        {
            pictureBox_drawingbox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox_drawingbox.Cursor = Cursors.Cross;
        }

        /*****************************************************************
        Functions: pictureBox_choosecolor_MouseMove
                   pictureBox_choosecolor_Click
                   pictureBox_choosecolor_MouseLeave
        Trigger: hover and select a color on the colorwheel
        Where: main page
        Result: displays selected color on a small circle
        ******************************************************************/
        private void pictureBox_choosecolor_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                hoveredColor = bmpChooseColor.GetPixel(e.X, e.Y);
                customPictureBoxCircle_displaycolor.BackColor = hoveredColor;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
        }
        private void pictureBox_choosecolor_Click(object sender, EventArgs e)
        {
            penColor = hoveredColor;
        }
        private void pictureBox_choosecolor_MouseLeave(object sender, EventArgs e)
        {
            customPictureBoxCircle_displaycolor.BackColor = penColor;
        }

        /*****************************************************************
        Function: ChangeGridSize
        Trigger: called via a click event function
        Where: function ->
                trackbar_gridsize_scroll
        Result: adjusts the grid size of the play field
        ******************************************************************/
        private void ChangeGridSize()
        {
            string temp = GridControl.returnGridSize(trackBar_gridsize.Value);
            string[] temparray = temp.Split(',');
            ysize = int.Parse(temparray[0]);
            xsize = int.Parse(temparray[1]);

            ClearBoard();
        }

        /*****************************************************************
        Function: SnapToGrid
        Trigger: move an avatar on the gameplay field
        Where: N/A
        Result: snaps the avatar to the closed grid square
        ******************************************************************/
        private Point SnapToGrid(Point p)
        {
            double x = Math.Round((double)p.X / xsize) * xsize;
            double y = Math.Round((double)p.Y / ysize) * ysize;
            x += GridControl.returnSnapToGridSpacingX(trackBar_gridsize.Value);
            y += GridControl.returnSnapToGridSpacingY(trackBar_gridsize.Value);
            return new Point((int)x, (int)y);         
        }

        /*****************************************************************
        Function: DrawGrid
        Trigger: called via other functions
        Where: N/A
        Result: draws the square grid on the gameplay field
        ******************************************************************/
        private void DrawGrid()
        {
            if (pictureBox_drawingbox.Image == null)
            {
                Bitmap bmp = new Bitmap(pictureBox_drawingbox.Width, pictureBox_drawingbox.Height);
                pictureBox_drawingbox.Image = bmp;
            }

            var g = Graphics.FromImage(pictureBox_drawingbox.Image);

            int numOfCells = amount;
            Pen p = new Pen(Color.Black);

            for (int y = 0; y < numOfCells; ++y)
            {
                g.DrawLine(p, 0, y * ysize, numOfCells * ysize, y * ysize);
            }

            for (int x = 0; x < numOfCells; ++x)
            {
                g.DrawLine(p, x * xsize, 0, x * xsize, numOfCells * xsize);
            }
        }

        /*****************************************************************
        Function: ClearBoard
        Trigger: called via a click event function
        Where: click event functions ->
                change of grid size, reset scene, reset game
        Result: clears the gameplay board of drawings and avatars
        ******************************************************************/
        private void ClearBoard()
        {
            if (pictureBox_drawingbox.Image != null)
            {
                pictureBox_drawingbox.Image = null;
                Invalidate();
            }
            DrawGrid();

            List<Control> removeControls = new List<Control>();
            foreach (Control C in this.tabPage1.Controls)
            {
                if (C.GetType() == typeof(CustomPictureBoxCircle))
                {
                    try
                    {
                        if (C.Tag.Equals("avatar"))
                        {
                            removeControls.Add(C);
                            //C.Dispose();
                        }
                    }
                    catch { }                  
                }
            }        
            for (int i = 0; i < removeControls.Count; i++)
            {
                removeControls[i].Dispose();
            }
        }

        /*****************************************************************
        Function: trackBar_gridsize_Scroll
        Trigger: drag the trackbar left or right
        Where: settings -> gameplay
        Result: calls to change grid size
        ******************************************************************/
        private void trackBar_gridsize_Scroll(object sender, EventArgs e)
        {
            ChangeGridSize();
        }

        /*****************************************************************
        Functions: Avatar_MouseMove
                   Avatar_MouseDown
                   Avatar_MouseUp
        Trigger: Hold and drag an avatar on the play field
        Where: main page
        Result: Moves the avatar around the grid
        ******************************************************************/
        private void Avatar_MouseMove(object sender, MouseEventArgs e)
        {           
            CustomPictureBoxCircle circlebox = (CustomPictureBoxCircle)sender;

            if (isDragging && allowDrag)
            {
                circlebox.Top = circlebox.Top + (e.Y - currentY);
                circlebox.Left = circlebox.Left + (e.X - currentX);
            }
        }
        private void Avatar_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;

            currentX = e.X;
            currentY = e.Y;
        }
        private void Avatar_MouseUp(object sender, MouseEventArgs e)
        {
            CustomPictureBoxCircle circlebox = (CustomPictureBoxCircle)sender;
            isDragging = false;
            circlebox.Location = SnapToGrid(circlebox.Location);

        }

        /*****************************************************************
        Function: Avatar_PlaceOnGrid
        Trigger: double click on an avatar in the list of players & enemies
        Where: main page
        Result: Creates a duplicate avatar and places it on the grid
        ******************************************************************/
        public void Avatar_PlaceOnGrid(object sender, EventArgs e)
        {
            CustomPictureBoxCircle circlebox = (CustomPictureBoxCircle)sender;
            CustomPictureBoxCircle circleboxClone = GetPictureBoxClone(circlebox);
            int duplicate = 0;

            if (circlebox.Tag.Equals("avatar"))
            {                        
                foreach (var avatarObject in tabPage1.Controls.OfType<CustomPictureBoxCircle>())
                {
                    Console.WriteLine(avatarObject.Name + ": debug");
                    if (avatarObject.Name.Equals(circlebox.Name.ToString()))
                    {
                        duplicate++;
                    }
                }

                if (duplicate == 1)
                {
                    return;
                }
                else
                {
                    circleboxClone.Location = new Point(250, 20);
                    circleboxClone.Size = GridControl.returnAvatarSize(trackBar_gridsize.Value);
                   // this.Controls.Add(circleboxClone);
                    this.tabPage1.Controls.Add(circleboxClone);
                    //circlebox.Location = SnapToGrid(circleboxClone.Location);
                    circleboxClone.BringToFront();
                }
            }
        }

        /*****************************************************************
        Function: avatartooltip
        Trigger: Hover over avatar on gameplay field
        Where: main page
        Result: Displays a tooltip showing who the avatar is
        ******************************************************************/
        private void avatartooltip(object sender, EventArgs e)
        {
            CustomPictureBoxCircle circlebox = (CustomPictureBoxCircle)sender;
            tempstorage = circlebox.Name;
            tempstoragearray = tempstorage.Split('_');
            toolTip1.Show(tempstoragearray[1].ToString(),circlebox);
        }

        /*****************************************************************
        Functions: picturebox_enemycreationColor_MouseMove
                   picturebox_enemycreationColor_MouseLeave
                   picturebox_enemycreationColor_MouseClick
                   picturebox_avatarselection_enemy
        Trigger: moving mouse over color selection / select avatar
        Where: Enemy selection page
        Result: selects the chosen avatar and assigns the chosen color
        ******************************************************************/
        private void picturebox_enemycreationColor_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                hoveredColorEnemy = bmpChooseColorEnemy.GetPixel(e.X, e.Y);
                customPictureBoxCircle_enemycreation_chosenAvatar.BackColor = hoveredColorEnemy;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
        }
        private void picturebox_enemycreationColor_MouseLeave(object sender, EventArgs e)
        {
            customPictureBoxCircle_enemycreation_chosenAvatar.BackColor = assignedColorEnemy;
        }
        private void picturebox_enemycreationColor_MouseClick(object sender, MouseEventArgs e)
        {
            assignedColorEnemy = hoveredColorEnemy;
        }
        private void picturebox_avatarselection_enemy(object sender, EventArgs e)
        {
            PictureBox clickedAvatar = sender as PictureBox;
            customPictureBoxCircle_enemycreation_chosenAvatar.BackgroundImage = clickedAvatar.BackgroundImage;
        }

        /*****************************************************************
        Functions: pictureBox_playercreation_color_MouseClick
                   pictureBox_playercreation_color_MouseLeave
                   pictureBox_playercreation_color_MouseMove
                   picturebox_avatarselection_player
        Trigger: moving mouse over color selection / select avatar
        Where: player selection page
        Result: selects the chosen avatar and assigns the chosen color
        ******************************************************************/
        private void pictureBox_playercreation_color_MouseClick(object sender, MouseEventArgs e)
        {
            assignedColorPlayer = hoveredColorPlayer;
        }
        private void pictureBox_playercreation_color_MouseLeave(object sender, EventArgs e)
        {
            customPictureBoxCircle_playercreation_chosenAvatar.BackColor = assignedColorPlayer;
        }
        private void pictureBox_playercreation_color_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                hoveredColorPlayer = bmpChooseColorPlayer.GetPixel(e.X, e.Y);
                customPictureBoxCircle_playercreation_chosenAvatar.BackColor = hoveredColorPlayer;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
        }
        private void picturebox_avatarselection_player(object sender, EventArgs e)
        {
            PictureBox clickedAvatar = sender as PictureBox;
            customPictureBoxCircle_playercreation_chosenAvatar.BackgroundImage = clickedAvatar.BackgroundImage;
        }

        /*****************************************************************
        Function: button_enemy_createavatar_Click
        Trigger: Create avatar button clicked
        Where: Enemy selection page
        Result: Creates the chosen avatar
        ******************************************************************/
        private void button_enemy_createavatar_Click(object sender, EventArgs e)
        {
            if (textBox_enemy_name.Text.Length > 0)
            {
                for(int i=0; i <Program.enemies.Count; i++)
                {
                    if(textBox_enemy_name.Text.Equals(Program.enemies[i].ReturnName()))
                    {
                        listView_display_names_enemy.Items.Add("Oops!, an Enemy with that" +
                            "name already exists");
                        RemoveEnemyOrPlayerNameOffList(6, listView_display_names_enemy.Items.Count, "enemy");
                        return;
                    }
                }
                for (int i = 0; i < Program.players.Count; i++)
                {
                    if (textBox_enemy_name.Text.Equals(Program.players[i].ReturnName()))
                    {
                        listView_display_names_enemy.Items.Add("Oops!, a Player with that" +
                            "name already exists");
                        RemoveEnemyOrPlayerNameOffList(6, listView_display_names_enemy.Items.Count, "enemy");
                        return;
                    }
                }

                Program.enemies.Add(new Enemy(textBox_enemy_name.Text.ToString(), createAvatarEnemy(textBox_enemy_name.Text.ToString()), Decimal.ToInt32(numericUpDown_enemy_health.Value)));
                AddEnemy();

                listView_display_names_enemy.Items.Add("Added Enemy: Name[" +textBox_enemy_name.Text + "]"+ 
                    " Health[ " + numericUpDown_enemy_health.Value+"]");
            }
            else
            {
                listView_display_names_enemy.Items.Add("Oops!, name cannot be blank");
            }

            RemoveEnemyOrPlayerNameOffList(6, listView_display_names_enemy.Items.Count, "enemy");
        }

        /*****************************************************************
        Function: button_player_create_Click
        Trigger: Create avatar button clicked
        Where: Player selection page
        Result: Creates the chosen avatar
        ******************************************************************/
        private void button_player_create_Click(object sender, EventArgs e)
        {
            if (textBox_player_name.Text.Length > 0)
            {
                for (int i = 0; i < Program.enemies.Count; i++)
                {
                    if (textBox_player_name.Text.Equals(Program.enemies[i].ReturnName()))
                    {
                        listView_display_names_player.Items.Add("Oops!, an Enemy with that" +
                            "name already exists");
                        RemoveEnemyOrPlayerNameOffList(6, listView_display_names_player.Items.Count, "player");
                        return;
                    }
                }
                for (int i = 0; i < Program.players.Count; i++)
                {
                    if (textBox_player_name.Text.Equals(Program.players[i].ReturnName()))
                    {
                        listView_display_names_player.Items.Add("Oops!, a Player with that" +
                            "name already exists");
                        RemoveEnemyOrPlayerNameOffList(6, listView_display_names_player.Items.Count, "player");
                        return;
                    }
                }

                Program.players.Add(new Player(textBox_player_name.Text.ToString(), createAvatarPlayer(textBox_player_name.Text.ToString())));
                AddPlayer();

                listView_display_names_player.Items.Add("Added Player: Name[" + textBox_player_name.Text + "]");
            }
            else
            {
                listView_display_names_player.Items.Add("Oops!, name cannot be blank");
            }

            RemoveEnemyOrPlayerNameOffList(6, listView_display_names_player.Items.Count, "player");
        }

        /*****************************************************************
        Function: RemoveEnemyOrPlayerNameOffList
        Trigger: when the list reaches max capacity, remove one off the top
        Where: called via functions, designed for player / enemy selection
        Result: removes the top most text off the list
        ******************************************************************/
        public void RemoveEnemyOrPlayerNameOffList(int maxLength, int currentLength,string name)
        {
            if (currentLength >= maxLength)
            {
                if(name.Equals("enemy"))
                    listView_display_names_enemy.Items.RemoveAt(0);

                if (name.Equals("player"))
                    listView_display_names_player.Items.RemoveAt(0);
            }

        }

        /*****************************************************************
        Function: button_characterselection_cancel_Click
        Trigger: Back button is pressed
        Where: enemy selection page
        Result: Reset values and go back to main page
        ******************************************************************/
        private void button_characterselection_cancel_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            numericUpDown_enemy_health.Value = 0;
            textBox_enemy_name.Text = "";
            textBox_player_name.Text = "";
            listView_display_names_enemy.Clear();
            listView_display_names_player.Clear();
        }

        /*****************************************************************
        Function: button_nextturn_Click
        Trigger: next turn button is clicked 
        Where: main page
        Result: switch the active turn icon to the next player in line
        ******************************************************************/
        private void button_nextturn_Click(object sender, EventArgs e)
        {
            bool whosNext = false;
            //go through the list of flowlayout panels
            //check to see where the "active turn" icon is held
            //then switch it to the index value below
            foreach (Control C in flowLayoutPanel1.Controls)
            {             
                try
                {
                    if (C.Tag.Equals("runtimetable")) //grab the table first
                    {
                        foreach (Control CC in C.Controls) //grab the contents in the table (picture, & stuff)
                        {
                            if (CC.Tag != null)
                            {
                                if (CC.Tag.Equals("whosturn"))
                                {
                                    if (CC.BackgroundImage != null)
                                    {
                                        CC.BackgroundImage = null;
                                        whosNext = true;
                                    }
                                    else if (whosNext.Equals(true))
                                    {
                                        CC.BackgroundImage = Properties.Resources.myturn;
                                        whosNext = false;
                                    }
                                }
                            }
                            //check to see if its one of these controls turn
                            //if cc.control has tag "turn"
                            //& cc.control background image isnt null
                            //set it to null and make the next control in the list have the image
                            //if it is --> 

                        }
                    }
                    else
                    {
                        Console.WriteLine("other");
                    }
                }   
                catch { }
            }
        }


        private void changeGameplayListOrder(object sender, EventArgs e)
        {
            //dont really need this, adds a border cell when double clicked
            TableLayoutPanel t = (TableLayoutPanel)sender;
            t.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        }


        /************************************************************************************************
        Function: All the menu strips at the top of the application
        *************************************************************************************************/
        private void provideASuggestionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("http://sooni.com.au/contactus.html");
        }

        private void reportAProblemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("http://sooni.com.au/contactus.html");
        }

        private void gameplayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void button_settings_gameplay_back_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void addPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
        }

        private void resetSceneToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ClearBoard();
            ClearBoard();
        }

        private void resetGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableLayout.Clear();
            Program.players.Clear();
            Program.enemies.Clear();
            List<Control> removeControls = new List<Control>();

            foreach (Control c in flowLayoutPanel1.Controls)
            {
                removeControls.Add(c);
            }
            for (int i = 0; i < removeControls.Count; i++)
            {
                flowLayoutPanel1.Controls.Remove(removeControls[i]);
                removeControls[i].Dispose();
            }
            removeControls.Clear();

            ClearBoard();
            ClearBoard();

        }

        private void addEnemyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }
    }
}


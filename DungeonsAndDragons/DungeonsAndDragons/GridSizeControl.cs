using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsAndDragons
{
    class GridSizeControl
    {
        public GridSizeControl()
        {
        }

        /**********************************************
        Controls the sizing of the grid
        **********************************************/
        public string returnGridSize(int selectedScroll)
        {
            switch (selectedScroll)
            {
                case 0:
                    return "20,20";
                case 1:
                    return "25,25";
                case 2:
                    return "30,30";
                case 3:
                    return "40,40";
                case 4:
                    return "50,50";
                case 5:
                    return "55,55";
                case 6:
                    return "60,60";
                default:
                    return "30,30";
            }
        }

        /************************************************
        Snaps the players x position based on grid size
        ************************************************/
        public int returnSnapToGridSpacingX(int selectedScroll)
        {
            switch (selectedScroll)
            {
                case 0:
                    return 4;
                case 1:
                    return -5;
                case 2:
                    return 4;
                case 3:
                    return 4;
                case 4:
                    return -6;
                case 5:
                    return 24;
                case 6:
                    return 4;
                default:
                    return 0;
            }
        }

        /************************************************
         Snaps the players y position based on grid size
        ************************************************/
        public int returnSnapToGridSpacingY(int selectedScroll)
        {
            switch (selectedScroll)
            {
                case 0:
                    return -7;
                case 1:
                    return 7;
                case 2:
                    return 2;
                case 3:
                    return -8;
                case 4:
                    return -18;
                case 5:
                    return -22;
                case 6:
                    return -28;
                default:
                    return 0;
            }
        }

        /************************************************
         Size the avatar based on grid size
        ************************************************/
        public Size returnAvatarSize(int selectedScroll)
        {
            switch (selectedScroll)
            {
                case 0:
                    return new Size(20, 20); 
                case 1:
                    return new Size(25, 25);
                case 2:
                    return new Size(30, 30);
                case 3:
                    return new Size(40, 40);
                case 4:
                    return new Size(50, 50);
                case 5:
                    return new Size(55, 55);
                case 6:
                    return new Size(60, 60);
                default:
                    return new Size(30, 30);
            }
        }
    }
}

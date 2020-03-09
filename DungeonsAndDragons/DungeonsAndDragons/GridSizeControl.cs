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

        public int returnSnapToGridSpacingX(int selectedScroll)
        {
            switch (selectedScroll)
            {
                case 0:
                    return -6;
                case 1:
                    return 5;
                case 2:
                    return -6;
                case 3:
                    return -6;
                case 4:
                    return 4;
                case 5:
                    return 25;
                case 6:
                    return -5;
                default:
                    return 0;
            }
        }

        public int returnSnapToGridSpacingY(int selectedScroll)
        {
            switch (selectedScroll)
            {
                case 0:
                    return -6;
                case 1:
                    return -12;
                case 2:
                    return 14;
                case 3:
                    return 14;
                case 4:
                    return 14;
                case 5:
                    return 14;
                case 6:
                    return 14;
                default:
                    return 0;
            }
        }

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

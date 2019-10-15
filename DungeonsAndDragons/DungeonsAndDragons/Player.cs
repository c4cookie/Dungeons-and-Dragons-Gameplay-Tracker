using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsAndDragons
{
    class Player
    {
        private string name;
        private CustomPictureBoxCircle avatar;

        public Player(string name, CustomPictureBoxCircle avatar)
        {
            this.name = name;
            this.avatar = avatar;
            // test
        }

        public string ReturnName()
        {
            return name;
        }

        public CustomPictureBoxCircle ReturnAvatar()
        {
            return avatar;
        }
    }
}

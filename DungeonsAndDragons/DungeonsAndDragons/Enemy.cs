using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsAndDragons
{
    class Enemy
    {
        private string name;
        private CustomPictureBoxCircle avatar;
        private int health;

        public Enemy(string name, CustomPictureBoxCircle avatar, int health)
        {
            this.name = name;
            this.avatar = avatar;
            this.health = health;
        }

        public string ReturnName()
        {
            return name;
        }

        public int ReturnHealth()
        {
            return health;
        }

        public void UpdateHealth(int health)
        {
            this.health = health;
        }

        public CustomPictureBoxCircle ReturnAvatar()
        {
            return avatar;
        }
    }
}

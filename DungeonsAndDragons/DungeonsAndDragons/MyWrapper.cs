using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DungeonsAndDragons
{
    class MyWrapper
    {
        private Control control;

        public MyWrapper(Control control)
        {
            this.control = control;
        }

        public Control Control
        {
            get { return control; }
        }
    }
}

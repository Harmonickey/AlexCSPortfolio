using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hatchu
{
    class ShowOrder : System.Collections.CollectionBase
    {
        private readonly Form Hatchu;

        public ShowOrder(Form host)
        {
           Hatchu = host;
        }

        public int this[int Index]
        {
            get
            {
                return (int)this.List[Index];
            }
        }

        public void Add(int item)
        {
            this.List.Add(item);
        }
    }
}

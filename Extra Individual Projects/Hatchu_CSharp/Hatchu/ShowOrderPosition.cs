using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hatchu
{
    class ShowOrderPosition : System.Collections.CollectionBase
    {
        private readonly Form Hatchu;

        public ShowOrderPosition(Form host)
        {
           Hatchu = host;
        }

        public List<int> this[int Index]
        {
            get
            {
                return (List<int>)this.List[Index];
            }
        }

        public void Add(List<int> aList)
        {
            this.List.Add(aList);
        }
    }
}

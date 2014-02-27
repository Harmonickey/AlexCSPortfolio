using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hatchu
{
    class CastList : System.Collections.CollectionBase
    {
        private readonly System.Windows.Forms.Form Hatchu;

        public CastList(System.Windows.Forms.Form host)
        {
           Hatchu = host;
           this.AddNewCastMember("NULL");  //add a null cast member
        }

        public CastList this[int Index]
        {
            get
            {
                return (CastList)this.List[Index];
            }
        }

        public void AddNewCastMember(string name)
        {
            //add to the collection list
            this.List.Add(name);
        }

        public bool HasMatching(string secondName)
        {
            foreach (string name in this.List)
            {
                if (name == secondName && name != "NULL")
                    return true;
            }

            return false;
        }

        public bool HasMatching(CastList aCast)
        {
            foreach (string name in aCast.InnerList)
            {
                foreach (string secondName in this.List)
                {
                    if (name == secondName && name != "NULL")
                        return true;
                }
            }

            return false;
        }

        public bool IsEmpty()
        {
            if (List.Count == 1) //only contains NULL
                return true;
            else 
                return false;
        }
    }
}

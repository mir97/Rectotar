using System;
using System.Collections.Generic;

namespace Rectotarat.Calculations
{
    public class IndexComparer : IComparer<Element>
    {
        public int Compare(Element x, Element y)
        {
            // Compare y and x in direct order.
            return x.Index.CompareTo(y.Index);
        }
    }
    public class ReverseIndexComparer : IComparer<Element>
    {
        public int Compare(Element x, Element y)
        {
            // Compare y and x in reverse order.
            return y.Index.CompareTo(x.Index);
        }
    }

    public class ValueComparer : IComparer<Element>
    {
        public int Compare(Element x, Element y)
        {
            // Compare y and x in direct order.
            return x.Value.CompareTo(y.Value);
        }
    }
    public class ReverseValueComparer : IComparer<Element>
    {
        public int Compare(Element x, Element y)
        {
            // Compare y and x in reverse order.
            return y.Value.CompareTo(x.Value);
        }
    }




    public class Element : IComparable
    {

        public int Index { get; set; }
        public float Value { get; set; }


        public int CompareTo(object o)
        {
            Element p = o as Element;
            if (p != null)
                return Value.CompareTo(p.Value);
            else
                throw new Exception("Невозможно сравнить два объекта");
        }

 
    }

}












    





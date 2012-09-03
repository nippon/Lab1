using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab1.Model
{
    class Logger
    {
        private List<string> MyList = new List<string>();
        public void Log(string msg)
        {
            MyList.Insert(0, msg);
            if ( MyList.Count() > 10 )
                MyList.RemoveAt(10);
        }

        public override string ToString( )   
        {
            string result = "";
            foreach (string s in MyList)
                result += s + "\n";
            return result;
        }

    }
}

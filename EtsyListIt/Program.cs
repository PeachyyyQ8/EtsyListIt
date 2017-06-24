using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace EtsyListIt
{
    class Program
    {
        static void Main(string[] args)
        {
        }


        static Container ConfigureStructureMap()
        {
            return new Container(new DependencyRegistry());
        }
    }
}

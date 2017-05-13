using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressProcessing.CSV
{
    public interface IReader
    {
        string[] ReadColumns(char[] separator);
    }
}

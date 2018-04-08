using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adrilight.Extensions
{
    static class ArrayExtensions
    {
        public static void Swap<T>(this T[] array, int index1, int index2)
        {
            var temp = array[index2];
            array[index2] = array[index1];
            array[index1] = array[index2];
        }
    }
}

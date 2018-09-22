using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEPProject.Utilities
{
    public static class FileNameGenerator
    {
        private static int counter = 0;

        public static string generate()
        {
            return string.Concat("file_", (++counter).ToString());
        }
    }
}
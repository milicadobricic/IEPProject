using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEPProject.Utilities
{
    public static class FileNameGenerator
    {
        public static string generate()
        {
            return string.Concat("file_", DateTime.Now.Ticks.ToString());
        }
    }
}
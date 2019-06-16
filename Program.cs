/*
 * Author: Josef Fällman
 * Created: 2019-06-16
 * github: https://github.com/joseffallman/PBOer
 * 
 * License: GPLv3 se LICENSE file
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PBOSharp;

namespace PBOer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //new Orginal(args[0]);

            string folder, exportTo, pboName;
            switch (args.Length)
            {
                case 1:
                    folder = args[0];
                    new PBOSharpManager(new Configs.ConsolConfigs()).PackPBO(folder);
                    break;
                case 2:
                    folder = args[0];
                    exportTo = args[1];
                    new PBOSharpManager(new Configs.ConsolConfigs()).PackPBO(folder, exportTo);
                    break;
                case 3:
                    folder = args[0];
                    exportTo = args[1];
                    pboName = args[2];
                    new PBOSharpManager(new Configs.ConsolConfigs()).PackPBO(folder, exportTo, pboName);
                    break;
                default:
                    Console.WriteLine("Unknown arguments");
                    Console.WriteLine("Try with: \n'PBOer \"folderToPBO\" \"exportTO (optional)\" \"pboName (optional)\" '");
                    break;
            }
        }
    }
}

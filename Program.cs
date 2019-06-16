/*
 *  This file is part of PBOer.
 *  
 *  Copyright 2019 by Josef Fällman
 *  Licensed under GNU General Public License 3.0
 *  
 *  File created:    2019-06-16
 *  Github location: https://github.com/joseffallman/PBOer
 *
 *  PBOer is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  PBOer is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with PBOer.  If not, see<https://www.gnu.org/licenses/>.
 *  
 *  This program incorporates work covered by the following copyright and permission notice: 
 *  PBOSharp - Copyright 2017 by Paton7 <https://github.com/Paton7/PBOSharp>
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

        private void PrintCopyrightNotice()
        {
            Console.WriteLine(":          PBOer           :");
            Console.WriteLine("Copyright 2019 Josef Fällman");
            Console.WriteLine("Github location: https://github.com/joseffallman/PBOer");
            Console.WriteLine("");
            Console.WriteLine("PBOer is free software: you can redistribute it and/or modify");
            Console.WriteLine("it under the terms of the GNU General Public License as published by");
            Console.WriteLine("the Free Software Foundation, either version 3 of the License, or");
            Console.WriteLine("(at your option) any later version.");
            Console.WriteLine("");
            Console.WriteLine("You should have received a copy of the GNU General Public License");
            Console.WriteLine("along with PBOer.  If not, see<https://www.gnu.org/licenses/>.");
            Console.WriteLine("");
            Console.WriteLine("This program incorporates work covered by the following copyright and permission notice: ");
            Console.WriteLine("PBOSharp - Copyright 2017 by Paton7 <https://github.com/Paton7/PBOSharp>");
        }
    }
}

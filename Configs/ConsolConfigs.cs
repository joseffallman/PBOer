/*
 * Author: Josef Fällman
 * Created: 2019-06-16
 * github: https://github.com/joseffallman/PBOer
 * 
 * License: GPLv3 se LICENSE file
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;

//using System.Windows.Forms;

namespace PBOer.Configs
{
    class ConsolConfigs : Interface.IConfigs
    {
        public ConsolConfigs()
        {
            NameValueCollection options = ConfigurationManager.GetSection("appSettings") as NameValueCollection;
            Sqx = bool.Parse(options["SQX"]);
            TestFolder = bool.Parse(options["testFolder"]);
            HiddenFiles = bool.Parse(options["hiddenFiles"]);
            Tproj = bool.Parse(options["tproj"]);
            Git = bool.Parse(options["git"]);
        }

        public Boolean Sqx { get; }
        public Boolean TestFolder { get; }
        public Boolean HiddenFiles { get; }
        public Boolean Tproj { get; }
        public Boolean Git { get; }
    }
}
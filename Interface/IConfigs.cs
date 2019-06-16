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

namespace PBOer.Interface
{
    public interface IConfigs
    {
        Boolean Sqx { get; }
        Boolean TestFolder { get; }
        Boolean HiddenFiles { get; }
        Boolean Tproj { get; }
        Boolean Git { get; }
    }
}

/*
 *  This file is part of PBOer.
 *  
 *  Copyright 2019 by Josef Fällman
 *  Licensed under GNU General Public License 3.0
 *  
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

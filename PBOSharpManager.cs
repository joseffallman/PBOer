/*
 *  This file is part of PBOer.
 *  
 *  Copyright 2019 by Josef Fällman
 *  Licensed under GNU General Public License 3.0
 *  
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBOSharp.Enums;
using PBOSharp.Objects;
using PBOer.Interface;

namespace PBOer
{
    public class PBOSharpManager : PBOSharp.PBOSharpClient
    {

        public event PBOSharpEventHandler OnEvent;
        private IConfigs _configs;

        public PBOSharpManager(Interface.IConfigs configs)
        {
            _configs = configs;
        }

        /// <summary>
        /// Packs specified folder in to PBO format.
        /// Will output to the parent directory unless packDirectory is specified.
        /// File name will be (folderName.pbo) unless pboName is specified.
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="packDirectory"></param>
        /// <param name="pboName"></param>
        public new void PackPBO(string folder, string packDirectory = null, string pboName = null)
        {
            try
            {
                PushOnEvent("Starting PackPBO", EventType.Debug);

                //Get the correct pack directory depending on if packDirectory was passed 
                packDirectory = packDirectory ?? Directory.GetParent(folder).FullName;

                //Get the corrent pbo name depending on if pboName was passed 
                pboName = pboName ?? new DirectoryInfo(folder).Name;

                //Make sure the pboName has the correct extension
                if (new FileInfo(pboName).Extension != ".pbo")
                    pboName = $"{pboName}.pbo";

                //Get the final pbo file path
                string pboFilePath = Path.Combine(packDirectory, pboName);

                //make sure the pbo does not alread exist.
                if (File.Exists(pboFilePath))
                {
                    string backup = $"{pboFilePath}.bak";
                    if (File.Exists(backup))
                        File.Delete(backup);
                    File.Move(pboFilePath, backup);
                }


                //Create dir if it does not exist 
                if (!Directory.Exists(Path.GetDirectoryName(pboFilePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(pboFilePath));

                //Create the pbo file 
                FileStream fileStream = new FileStream(pboFilePath, FileMode.Create, FileAccess.Write);

                //Write the file content
                PBOWriterManager writer = new PBOWriterManager(fileStream, this, _configs);
                writer.WritePBO(folder, packDirectory, pboFilePath);

                fileStream.Dispose();

                PushOnEvent("Finished PackPBO", EventType.Debug);
            }
            catch (Exception ex)
            {
                PushOnEvent(ex.Message, EventType.Error);
            }
        }

        /// <summary>
        /// Push event to event handler 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        internal new void PushOnEvent(string message, EventType type)
            => OnEvent?.Invoke(new PBOSharpEventArgs(message, type));

    }
}

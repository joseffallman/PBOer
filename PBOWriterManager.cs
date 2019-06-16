/*
 * Author: Josef Fällman
 * Created: 2019-06-16
 * github: https://github.com/joseffallman/PBOer
 * 
 * License: GPLv3 se LICENSE file
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
    public class PBOWriterManager : PBOSharp.PBOWriter
    {
        private PBOSharpManager _client;
        private IConfigs _configs;

        public PBOWriterManager(Stream stream, PBOSharpManager client, IConfigs configs)
            : base(stream, client)
        {
            _client = client;
            _configs = configs;
        }

        public override void Write(string buffer)
        {
            base.Write(Encoding.ASCII.GetBytes(buffer));
            base.Write((byte)0);
        }

        /// <summary>
        /// Takes content of a folder and packs it in to PBO format
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="packDirectory"></param>
        /// <param name="pboName"></param>
        internal void WritePBO(string folder, string packDirectory, string pboName)
        {
            try
            {
                _client.PushOnEvent("Starting WritePBO", EventType.Debug);
                PBOPrefix prefix = null;
                List<PBOFile> PBOfiles = new List<PBOFile>();


                //Look for a prefix file 
                string[] possiblePrefixFiles = Directory.GetFiles(folder, "$*$", SearchOption.TopDirectoryOnly);
                if (possiblePrefixFiles.Length > 0)
                    prefix = new PBOPrefix(Path.GetFileName(possiblePrefixFiles[0]).Trim('$').ToLower(), new StreamReader(possiblePrefixFiles[0]).ReadLine());

                //Look for all the files 
                //foreach (string file in Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories))
                foreach (string file in GetFiles(folder))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    //Make sure we are not readding the prefix file
                    if (fileInfo.Name[0] != 36 && fileInfo.Name[fileInfo.Name.Length - 1] != 36)
                    {
                        PBOfiles.Add(new PBOFile(
                            fileInfo.FullName.Substring(folder.Length + 1),
                            PackingMethod.Uncompressed,
                            (int)fileInfo.Length,
                            0,
                            (int)(fileInfo.LastWriteTime - new DateTime(1970, 1, 1)).TotalSeconds,
                            (int)fileInfo.Length,
                            0L));
                        _client.PushOnEvent($"Found File {fileInfo.FullName}", EventType.Debug);
                    }
                }

                //Write signature 
                Write("");
                Write((int)PackingMethod.Product);
                for (int i = 0; i < 4; i++)
                    Write(0);


                //Write Prefix if it exists 
                if (prefix != null)
                {
                    Write(prefix.PrefixName);
                    Write(prefix.PrefixValue);
                }

                //Terminate sig and if it exists the prefix
                Write((byte)0);

                //Write all file structs 
                foreach (PBOFile file in PBOfiles)
                {
                    Write(file.FileName);
                    Console.WriteLine(file.FileName);
                    Write((int)file.PackingMethod);
                    Write(file.OriginalSize);
                    Write(file.Reserved);
                    Write(file.Timestamp);
                    Write(file.DataSize);
                    _client.PushOnEvent($"Wrote Header for: {file.FileName}", EventType.Info);
                }

                //Write final empty struct 
                Write("");
                Write((int)PackingMethod.Uncompressed);
                for (int i = 0; i < 4; i++)
                    Write(0);

                //Write the data for each file 
                foreach (PBOFile file in PBOfiles)
                {
                    using (BinaryReader br = new BinaryReader(new FileStream(Path.Combine(folder, file.FileName), FileMode.Open, FileAccess.Read)))
                        Write(br.ReadBytes((int)br.BaseStream.Length));
                    _client.PushOnEvent($"Wrote Data for: {file.FileName}", EventType.Info);
                }

                _client.PushOnEvent("Finished WritePBO", EventType.Debug);
            }
            catch (Exception ex)
            {
                _client.PushOnEvent($"WritePBO failed\n{ex.Message}", EventType.Error);
            }
        }


        /// <summary>
        /// Return the files that should be added to the pbo
        /// </summary>
        /// <param name="folder"></param>
        /// <returns>Array with string</returns>
        public string[] GetFiles(string folder)
        {
            // Create a new empty List.
            List<string> files = new List<string>();

            // Find all hidden folders
            //List<string> hiddenFolders = new List<string>();
            IEnumerable<string> hiddenFolders = null;
            try
            {
                /*
                foreach (string subFolder in Directory.GetDirectories(folder))
                {
                    string path = Path.Combine(folder, subFolder);
                    DirectoryInfo info = new DirectoryInfo(path);
                    if ((info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        hiddenFolders.Add(path);
                    }
                }
                DirectoryInfo f = new DirectoryInfo(folder);
                hiddenFolders.AddRange(f.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                    .Where(w => (w.Attributes & FileAttributes.Hidden) == 0));
                foreach (var directory in baseDirectory.GetDirectories("*.*", SearchOption.TopDirectoryOnly).Where(w => (w.Attributes & FileAttributes.Hidden) == 0))
                    fileInfos.AddRange(getNonHiddenFiles(directory));
                */
                hiddenFolders = getHidden(folder);
            }
            catch (DirectoryNotFoundException)
            {
            }

            // Find if there is a test directory.
            string pathTestFolder = Path.Combine(folder, "Tests");

            // Loop all files in folder to se what to export.
            foreach (string file in Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories))
            {
                FileInfo fileInfo = new FileInfo(file);

                // Exluce if file is...
                // ...a *.sxq file
                if (_configs.Sqx && fileInfo.Extension == ".sqx")
                {
                    // Console.WriteLine("Found a .sqx file " + fileInfo.Name);
                    continue;
                }
                // ...in Test folder
                if (_configs.TestFolder && fileInfo.FullName.StartsWith(pathTestFolder))
                {
                    // Console.WriteLine("File in Test folder " + fileInfo.Name);
                    continue;
                }
                // ... hidden or in hidden folder
                if (_configs.HiddenFiles) // && hiddenFolders.Any(file.StartsWith))
                {
                    DirectoryInfo info = new DirectoryInfo(file);
                    if (
                        (info.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden
                        || hiddenFolders.Any(file.StartsWith)
                    )
                    {
                        continue;
                    }
                    // Console.WriteLine("Hiddden file " + fileInfo.Name);
                }
                // ... tproj file
                if (_configs.Tproj && fileInfo.Extension == ".tproj")
                {
                    // Console.WriteLine("Found the *.tproj " + fileInfo.Name);
                    continue;
                }
                if (_configs.Git && fileInfo.FullName.Contains(".git"))
                {
                    // Console.WriteLine("Git file " + fileInfo.Name);
                    continue;
                }

                // Add file to list
                files.Add(file);
            }

            return files.ToArray();
        }

        private List<string> getHidden(string path)
        {
            List<string> filtered = new List<string>();
            string[] directory = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            foreach( string subDir in directory)
            {
                DirectoryInfo subDirInfo = new DirectoryInfo(subDir);
                if (subDirInfo.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    filtered.Add(subDir);
                }
            }

            return filtered;
        }
    }
}

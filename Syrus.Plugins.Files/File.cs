using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Syrus.Plugin;
namespace Syrus.Plugins.Files
{
    internal class FileEntity
    {
        /// <summary>
        /// Název souboru/složky
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Cesta k souboru/složce
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Velikost v bytech
        /// </summary>
        public long Size { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }

    internal class File : FileEntity
    {
        /// <summary>
        /// Koncovka souboru
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Název složky
        /// </summary>
        public string DirectoryName { get; set; }

        /// <summary>
        /// Cesta do složky, ve které se soubor nachází
        /// </summary>
        public string DirectoryPath { get; set; }

        internal static File FromFileInfo(FileInfo fileInfo) => new File()
        {
            Name = fileInfo.Name,
            FullPath = fileInfo.FullName,
            Size = fileInfo.Length,
            CreationDate = fileInfo.CreationTime,
            LastUpdateDate = fileInfo.LastWriteTime,
            Extension = fileInfo.Extension,
            DirectoryName = Path.GetDirectoryName(fileInfo.DirectoryName),
            DirectoryPath = fileInfo.DirectoryName
        };

        public static implicit operator Result(File f)
        {
            return new Result(){ 
                Text = f.Name
            };
        }
    }

    internal class Directory: FileEntity
    {
        /// <summary>
        /// Název nadřazené složky
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// Cesta do nadřazené složky
        /// </summary>
        public string ParentPath { get; set; }

        /// <summary>
        /// Název kořenové složky
        /// </summary>
        public string Root { get; set; }

        public static Directory FromDirectoryInfo(DirectoryInfo directoryInfo) => new Directory() { 
            Name = directoryInfo.Name,
            FullPath = directoryInfo.FullName,
            CreationDate = directoryInfo.CreationTime,
            LastUpdateDate = directoryInfo.LastWriteTime,
            ParentName = directoryInfo.Parent.Name,
            ParentPath = directoryInfo.Parent.FullName,
            Root = directoryInfo.Root.Name
        };
    }
}

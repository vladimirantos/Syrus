using System;
using System.Collections.Generic;
using System.Text;

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
        public int Size { get; set; }
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
        /// Kořenová složka
        /// </summary>
        public string Root { get; set; }
    }
}

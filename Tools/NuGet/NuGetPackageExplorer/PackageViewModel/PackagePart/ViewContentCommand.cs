﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using NuGet;

namespace PackageExplorerViewModel {
    internal class ViewContentCommand : ICommand {

        private readonly PackageFile _file;
        private readonly IPackageViewModel _showFileContentHandler;

        public ViewContentCommand(PackageFile file, IPackageViewModel showFileContentHandler) {
            _file = file;
            _showFileContentHandler = showFileContentHandler;
        }

        public bool CanExecute(object parameter) {
            return !IsBinaryFile(_file.Name);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter) {
            if (_showFileContentHandler != null) {
                _showFileContentHandler.ShowFile(_file.Name, ReadFileContent(_file));
            }
        }

        private string ReadFileContent(PackageFile file) {
            using (StreamReader reader = new StreamReader(file.GetStream())) {
                return reader.ReadToEnd();
            }
        }

        private static string[] BinaryFileExtensions = new string[] { 
            ".DLL", ".EXE", ".CHM", ".PDF", ".DOCX", ".DOC", ".JPG", ".PNG", ".GIF", ".RTF", ".PDB", ".ZIP", ".XAP", ".VSIX", ".NUPKG"
        };

        private bool IsBinaryFile(string path) {
            // TODO: check for content type of the file here
            string extension = Path.GetExtension(path).ToUpper();
            return BinaryFileExtensions.Any(p => p.Equals(extension));
        }
    }
}
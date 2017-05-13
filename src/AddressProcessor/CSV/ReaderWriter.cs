using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AddressProcessing.CSV
{
    public abstract class ReaderWriter: IReader, IWriter
    {
        [Flags]
        public enum Mode { Read = 1, Write = 2 };
        protected StreamReader _readerStream = null;
        protected StreamWriter _writerStream = null;
        /// <summary>
        /// Open the file with the FileOpen Mode passed
        /// </summary>
        /// <param name="fileName">string FileName</param>
        /// <param name="mode">FileMode to open the file</param>
        public void OpenFile(string fileName, Mode mode)
        {
            try
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (mode == Mode.Read)
                    {
                        _readerStream = File.OpenText(fileName);
                    }
                    else if (mode == Mode.Write)
                    {
                        FileInfo fileInfo = new FileInfo(fileName);
                        _writerStream = fileInfo.CreateText();
                    }
                    else
                    {
                        throw new Exception("Unknown file mode for " + fileName);
                        /// The hard coding of the constant "Unknown file mode for " can be moved to a resource file to support internationalization in the future. 
                        /// Same with other hard codings in the file. 
                    }
                }
            }
            catch (FileLoadException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (FileNotFoundException ex) // File not found exception also needs to be checked, in case if the file doesn't exist in the given location.
            {
                throw new Exception(ex.Message + "File not found in the given Location. Please check.");
            }
            catch (Exception) // This is required if there are any other errors, other than the above two.
            {
                throw; // Simply throw the exception, to retain the stack trace to the calling method. 
            }
        }

        public void Write(string separator, params string[] columns)
        {
            string outPut = "";

            for (int i = 0; i < columns.Length; i++)
            {
                outPut += columns[i];
                if ((columns.Length - 1) != i)
                {
                    outPut += separator;
                }
            }

            WriteLine(outPut);
        }
        private void WriteLine(string line)
        {
            _writerStream.WriteLine(line);
        }

        public string[] ReadColumns(char[] separator)
        {
            string line = ReadLine();

            if (line == null)
            {
                return new string[0] { };
            }
            return line.Split(separator);
        }
        private string ReadLine()
        {
            return _readerStream.ReadLine();
        }

        /// <summary>
        /// Read data from columns and set them to column1 and column2
        /// </summary>
        /// <param name="column1">string output column1</param>
        /// <param name="column2">string output column2</param>
        /// <param name="columns">string[] columns</param>
        /// <returns>True/False</returns>
        public bool SetColumn1AndColumn2(out string column1, out string column2, string[] columns)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            }
            else if (columns.Length == 1)
            {
                column1 = columns[FIRST_COLUMN];
                column2 = string.Empty;
                return true;
            }
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        public void Close()
        {
            if (_writerStream != null)
            {
                _writerStream.Close();
            }

            if (_readerStream != null)
            {
                _readerStream.Close();
            }
        }
    }
}

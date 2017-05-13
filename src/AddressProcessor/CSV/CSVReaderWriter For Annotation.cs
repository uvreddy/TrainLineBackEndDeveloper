using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        1) List three to five key concerns with this implementation that you would discuss with the junior developer. 

        Please leave the rest of this file as it is so we can discuss your concerns during the next stage of the interview process.
        
        1) File references are not disposed automatically if a obejct is referencing it. If users of CSVReaderWriter class forget to call Close() method, it results in memory leak. Causes major issues in terms of performance as well. The better way is to implement IDispose interface and call the close method from the destructor. 
        2) Opening the file in read or write mode is not the responsibility of this class. It is good if it is in a base class. There fore in the future, if we want to come up with a pipe separated reader writer or any new type, can create a new class inheriting ReaderWriter class, please see this class I have created. 
        3) This class can not be unit tested due to no dependency inversion considered. Can not be mocked. 
        4) File.OpenText does not refer to the current project folder by default.
        5) The Open(string fileName, Mode mode) method does not catch any exceptions. There are many reasons to it. File path may not exists, While opening in Write mode the file may have already been opened by another application etc. 
        6) public bool Read(string column1, string column2) method has never been used. It can be removed. 
        7) There is lot of duplicated code in Read(string column1, string column2) and Read(out string column1, out string column2). This code can be moved to a separate common method. 
        8) The code is hard coded to read only 2 columns. Modified to make it generic.
        9) Mode enumeration is part of a class. Enumerations should always be part of a name space, so that they are available to other objects as well, like interfaces. Actually I wanted to create a new IReaderWriter interface with definition of OpenFile method. But it can not be possible, due to AdddressFileProcessor is using the enum and I am not allowed to modify it. 
        10) Reading and writing a two different jobs. It is good to have CSVReader and CSVWriter separate classes with their own interfaces. Therefore the consumers can use whatever they need. I am not doing this now for maintaining the backward compatibility.
    */

    public class CSVReaderWriterForAnnotation
    {
        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public void Open(string fileName, Mode mode)
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
            }
        }

        public void Write(params string[] columns)
        {
            string outPut = "";

            for (int i = 0; i < columns.Length; i++)
            {
                outPut += columns[i];
                if ((columns.Length - 1) != i)
                {
                    outPut += "\t";
                }
            }

            WriteLine(outPut);
        }

        public bool Read(string column1, string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();
            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            }
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        public bool Read(out string column1, out string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line;
            string[] columns;

            char[] separator = { '\t' };

            line = ReadLine();

            if (line == null)
            {
                column1 = null;
                column2 = null;

                return false;
            }

            columns = line.Split(separator);

            if (columns.Length == 0)
            {
                column1 = null;
                column2 = null;

                return false;
            } 
            else
            {
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];

                return true;
            }
        }

        private void WriteLine(string line)
        {
            _writerStream.WriteLine(line);
        }

        private string ReadLine()
        {
            return _readerStream.ReadLine();
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

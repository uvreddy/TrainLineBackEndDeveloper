using System;
using System.IO;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    public class CSVReaderWriter : ReaderWriter, IDisposable
    {
        public void Open(string fileName, Mode mode)
        {
            base.OpenFile(fileName, mode);
        }

        public void Write(params string[] columns)
        {
            base.Write("\t", columns);
        }

        public bool Read(string column1, string column2)
        {
            string[] columns = ReadColumns();
            return base.SetColumn1AndColumn2(out column1, out column2, columns);
        }

        public bool Read(out string column1, out string column2)
        {
            string[] columns = ReadColumns();
            return base.SetColumn1AndColumn2(out column1, out column2, columns);
        }

        /// <summary>
        /// Reads line and returns columns
        /// </summary>
        /// <returns>string array of columns</returns>
        private string[] ReadColumns()
        {
            char[] separator = { '\t' };
            return base.ReadColumns(separator);
        }

        /// <summary>
        /// Read N number of columns
        /// </summary>
        /// <returns>Collection of all Columns</returns>
        public string[] Read()
        {
            return ReadColumns();
        }

        #region Disposition

        /// <summary>
        /// Destroy the object.
        /// </summary>
        ~CSVReaderWriter()
        {
            base.Close();
        }

        /// <summary>
        /// Dispose the object
        /// </summary>
        public void Dispose()
        {
            this.Close();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

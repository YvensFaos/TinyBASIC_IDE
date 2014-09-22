using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TinyBASICAnalizer.Persistence
{
    public class Logger
    {
        private string _path;
        /// <summary>
        /// Path do arquivo de log
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        private StreamWriter _fileWriter;
        /// <summary>
        /// Objeto responsável por escrever no arquivo
        /// </summary>
        public StreamWriter FileWriter
        {
            get { return _fileWriter; }
            set { _fileWriter = value; }
        }

        public Logger(string Path)
        {
            this.Path = Path;
            FileWriter = new StreamWriter(Path);
        }

        /// <summary>
        /// Método para gerar o nome do arquivo de log
        /// </summary>
        /// <returns></returns>
        public string LogFileName()
        {
            string logFileName = System.DateTime.Now.ToString().Split(' ')[0];
            logFileName += "(" + System.DateTime.Now.Month + ":" + System.DateTime.Now.Day + ")";
            logFileName += "log.txt";

            return logFileName;
        }

        /// <summary>
        /// Escreve uma linha no arquivo de log
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine(string line)
        {
            string logLine = System.DateTime.Now.ToString().Split(' ')[0];
            logLine += ": " + line;

            FileWriter.WriteLine(logLine);
        }

    }
}

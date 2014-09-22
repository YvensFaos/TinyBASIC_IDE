﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

//TODO documentar
namespace TinyBASICAnalizer.Persistence
{
    public class IOUtils
    {
        public static string TXT_FILTER = "Txt files (*.txt)|*.txt|All files (*.*)|*.*";
        public static string IMAGE_FILTER = "Jpeg Files (*.jpeg)|*.jpeg|Png Files (*.png)|*.png";

        /// <summary>
        /// Chama uma janela para abertura de arquivos e retorna o path do arquivo selecionado.\n
        /// Depende do filtro selecionado.
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string OpenFileDialogShow(Form caller, string filter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = filter;
            if (openFileDialog.ShowDialog(caller) == DialogResult.OK)
            {
                return openFileDialog.FileName;
            }

            // caso o DialogResult não seja OK.
            return "";
        }

        /// <summary>
        /// Chama uma janela para abertura de arquivos e retorna o path do arquivo selecionado.\n
        /// Independe do filtro selecionado, aceitando qualquer formato.
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string OpenFileDialogShow(Form caller)
        {
            return OpenFileDialogShow(caller, "");
        }

        /// <summary>
        /// Chama uma janela para salvamento de arquivos e retorna o path do arquivo selecionado.\n
        /// Depende do filtro selecionado.
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string SaveFileDialogShow(Form caller, string filter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = filter;
            if (saveFileDialog.ShowDialog(caller) == DialogResult.OK)
            {
                return saveFileDialog.FileName;
            }

            // caso o DialogResult não seja OK.
            return "";
        }

        /// <summary>
        /// Chama uma janela para salvamento de arquivos e retorna o path do arquivo selecionado.\n
        /// Independe do filtro selecionado, aceitando qualquer formato.
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string SaveFileDialogShow(Form caller)
        {
            return SaveFileDialogShow(caller, "");
        }

        /// <summary>
        /// Cria um arquivo de texto contendo o conteúdo da variável data
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public static void CreateTxtFile(string path, string data)
        {
            File.WriteAllLines(path, new string[] { data });
        }

        /// <summary>
        /// Realiza a leitura de um arquivo de texto e retorna um vetor de strings com suas linhas respectivas
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] ReadTxtFile(string path)
        {
            if (path == null || path.Equals(""))
            {
                return new string[] { };
            }
            return System.IO.File.ReadAllLines(path);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBASICAnalizer.Model;
using TinyBASICAnalizer.Persistence;

namespace TinyBASICAnalizer.Control
{
    public class LexicAnalizer
    {
        private int _position;
        /// <summary>
        /// Variável responsável por indicar a posição do leitor na lista de tokens
        /// </summary>
        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private string[] _file;
        /// <summary>
        /// Arquivo que será analizado
        /// </summary>
        public string[] File
        {
            get { return _file; }
            set 
            { 
                _file = value;
                //A lista de tokens é zerada sempre que um novo arquivo é setado para o Analizador Léxico
                _listToken = null;
            }
        }

        private List<Token> _listToken;
        /// <summary>
        /// Lista dos tokens provenientes da análise do arquivo, se a lista estiver vazia, o método de analíse é chamado\\
        /// e é gerada a lista de tokens para o arquivo que tinha sito setado para o analisador léxico
        /// </summary>
        public List<Token> ListToken
        {
            get 
            {
                if (_listToken == null || _listToken.Count == 0)
                {
                    if (Analize())
                    {
                        return _listToken;
                    }
                    else
                    {
                        return null;
                    }
                }
                return _listToken;            
            }
            set { _listToken = value; }
        }

        private Transducer _lexicTransducer;
        /// <summary>
        /// Transdutor do analizador léxico
        /// </summary>
        public Transducer LexicTransducer
        {
            get { return _lexicTransducer; }
            set { _lexicTransducer = value; }
        }

        private Logger _log;
        /// <summary>
        /// Classe responsável por manter um log das atividades
        /// </summary>
        public Logger Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public LexicAnalizer()
            : this(null)
        { }

        public LexicAnalizer(string[] File)
        {
            this.File = File;
            LexicTransducer = new Transducer();
            Position = -1;
        }

        /// <summary>
        /// Realiza a análise do arquivo
        /// </summary>
        /// <returns></returns>
        public bool Analize()
        {
            return Analize(File);
        }

        /// <summary>
        /// Realiza a análise do arquivo
        /// </summary>
        /// <returns></returns>
        public bool Analize(string[] file)
        {
            if (file == null || file.Length <= 0)
            {
                return false;
            }

            ListToken = LexicTransducer.getTokens(file);
            Position = 0;

            //Linha vazia
            if (ListToken == null)
            {
                return true;
            }

            //Procedimento para tirar as quebras de linhas extras no final do arquivo
            while (true)
            {
                if (ListToken != null && ListToken[ListToken.Count - 1].Type == TokenType.LINE_BREAK)
                {
                    ListToken.RemoveAt(ListToken.Count - 1);
                }
                else
                {
                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// Método para retornar o próximo token na lista de tokens
        /// </summary>
        /// <returns></returns>
        public Token NextToken()
        {
            if (Position == -1)
            {
                return null;
            }       

            if (!IsValidPosition())
            {
                return null;
            }

            return ListToken[Position++];
        }

        /// <summary>
        /// Verifica se o valor da variável Position é uma posição válida dentro da lista de tokens
        /// </summary>
        /// <returns></returns>
        public bool IsValidPosition()
        {
            return IsValidPosition(Position);
        }

        /// <summary>
        /// Verifica se o valor da posição passada é uma posição válida dentro da lista de tokens
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsValidPosition(int position)
        {
            if (ListToken == null || ListToken.Count == 0)
            {
                return false;
            }

            return position > -1 && position < ListToken.Count;
        }

        /// <summary>
        /// Verifica se há token inválido na lista de tokens, se houver seu valor será adicionado ao feedback
        /// </summary>
        /// <returns></returns>
        public bool HasInvalidToken(ref string feedback)
        {
            foreach (Token token in ListToken)
            {
                if (token.Type == TokenType.INVALID)
                {
                    feedback += token.Value;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Verifica se a lista de tokens tem pelo menos uma variável
        /// </summary>
        /// <returns></returns>
        public bool HasVariable()
        {
            foreach (Token token in ListToken)
            {
                if (token.Type == TokenType.VARIABLE)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

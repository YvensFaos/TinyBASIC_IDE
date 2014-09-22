using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBASICAnalizer.Control;
using TinyBASICAnalizer.Model;
using TinyBASICAnalizer.Persistence;

namespace TinyBASICAnalizer.Test
{
    [TestClass]
    public class SyntaxTest
    {
        private string syntaxAnalizerTestFile1 = "Test/Input/syntax01.txt";
        private string syntaxAnalizerTestFile2 = "Test/Input/syntax02.txt";
        private string syntaxAnalizerTestFile3 = "Test/Input/syntax03.txt";

        private LexicAnalizer lexic;

        [TestInitialize]
        public void Initialize()
        {
            lexic = new LexicAnalizer();
        }

        [TestCleanup]
        public void CleanUp()
        {
        }

        /*
         * Template de Teste para ser utilizado
         * 
        [TestMethod]
        public void TestTemplate()
        {
        }
         * */

        /// <summary>
        /// Método de teste do transdutor com o analisador sintático Recursivo Descendente
        /// </summary>
        [TestMethod]
        public void SyntaxAnalize_RecursiveDescendent_Test()
        {
            SyntaxAnalizer syntax = new SyntaxAnalizer(lexic, SyntaxAnalizerType.RECURSIVE_DESCENDENT);

            string grammarIdentifier = Symbol.GrammarIdentifier;
            Symbol.GrammarIdentifier = "9";

            SyntaxTestExecution(syntax, SyntaxAnalizerType.RECURSIVE_DESCENDENT);

            Symbol.GrammarIdentifier = grammarIdentifier;
        }

        /// <summary>
        /// Método de teste do transdutor com o analisador sintático Não Recursivo Descendente
        /// </summary>
        [TestMethod]
        public void SyntaxAnalize_NonRecursiveDescendent_Test()
        {
            SyntaxAnalizer syntax = new SyntaxAnalizer(lexic, SyntaxAnalizerType.NON_RECURSIVE_DESCENDENT);

            string grammarIdentifier = Symbol.GrammarIdentifier;
            Symbol.GrammarIdentifier = "9";

            string line;
            bool validation;
            string feedback;

            line = "123 LET I = 12";
            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line = "GOTO 123";
            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line = "IF (A-B) = C THEN LET C = C-1";
            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line = "IF (A-B) = C THEN IF (C-1) = 5 THEN GOTO 123";
            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line = "12 IF I <= 10 THEN GOTO 5";
            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line = "REM \"CODIGO EXEMPLO DE STRING\"";
            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            SyntaxTestExecution(syntax, SyntaxAnalizerType.NON_RECURSIVE_DESCENDENT);

            Symbol.GrammarIdentifier = grammarIdentifier;
        }

        /// <summary>
        /// Método de teste do transdutor com o analisador sintático ascendente
        /// </summary>
        [TestMethod]
        public void SyntaxAnalize_Ascendent_Test()
        {
            string grammarIdentifier = Symbol.GrammarIdentifier;
            Symbol.GrammarIdentifier = "10";

            SyntaxAnalizer syntax = new SyntaxAnalizer(lexic, SyntaxAnalizerType.ASCENDENT);

            string line1;
            bool validation;
            string feedback;
        
            #region SimpleTests
            SimpleTests_Ascendent(syntax, out line1, out validation, out feedback);
            #endregion

            line1 = "PRINT 1 + 2";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "PRINT 1 + 2 * 3";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "PRINT 1 + 2 * (3 + 1)";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "PRINT 1 + 2 * (3 + (1*2))";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "PRINT 1 + 2 * (3 + (1*))";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(!validation, feedback);

            line1 = "INPUT A";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "INPUT A, B";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "LET A = 12";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "LET A = (1 + 2)";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "REM \"oi\"";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "CLEAR";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "IF 1 < 2 THEN LET A = 1";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "IF 1 < 2 THEN IF 1 < 2 THEN LET A = 1";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "IF 1 < 2 THEN IF 1 < 2 THEN IF 1 < 2 THEN LET A = 1";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            Symbol.GrammarIdentifier = grammarIdentifier;
        }

        /// <summary>
        /// Método de testes simples para o analizador ascendente
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="line1"></param>
        /// <param name="validation"></param>
        /// <param name="feedback"></param>
        private static void SimpleTests_Ascendent(SyntaxAnalizer syntax, out string line1, out bool validation, out string feedback)
        {
            line1 = "PRINT \"oi\"";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "PRINT \"oi\", \"oi\"";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "PRINT \"oi\", \"oi\", \"oi\"";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "PRINT 1";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "PRINT 1, 2, 3";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "PRINT \"oi\", \"oi\", \"oi\", \"oi\"";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);

            line1 = "PRINT ( 1 )";

            validation = false;
            feedback = "";

            syntax.Lexic.File = new string[] { line1 };
            validation = syntax.Analize(ref feedback);
            Assert.IsTrue(validation, feedback);
        }

        /// <summary>
        /// Método de teste para a geração dos first e follow da gramática
        /// </summary>
        [TestMethod]
        public void FirstAndFollow_Test()
        {
            //Buscar alguns terminais quaisquer
            /*
            S:B c|D B
            B:a b|c S
            D:d|#
            */

            string grammarIdentifier = Symbol.GrammarIdentifier;
            Symbol.GrammarIdentifier = "3";

            //Gerar os firsts and follows
            NonRecursiveDealer nonRecursive = new NonRecursiveDealer();

            NonTerminal s = Symbol.NonTerminalHash["S"];
            //Testa todos os firsts de S
            Assert.IsTrue(s.FirstContains("a"));
            Assert.IsTrue(s.FirstContains("c"));
            Assert.IsTrue(s.FirstContains("d"));
            Assert.IsTrue(s.FirstContainsEmpty());
            //Testa todos os follows de S
            Assert.IsTrue(s.FollowContains("c"));
            Assert.IsTrue(s.FollowContainsInitial());

            s = Symbol.NonTerminalHash["B"];
            //Testa todos os firsts de B
            Assert.IsTrue(s.FirstContains("a"));
            Assert.IsTrue(s.FirstContains("c"));
            //Testa todos os follows de C
            Assert.IsTrue(s.FollowContains("c"));
            Assert.IsTrue(s.FollowContainsInitial());

            s = Symbol.NonTerminalHash["D"];
            //Testa todos os firsts de D
            Assert.IsTrue(s.FirstContains("d"));
            Assert.IsTrue(s.FirstContainsEmpty());
            ////Testa todos os follows de D
            Assert.IsTrue(s.FollowContains("a"));
            Assert.IsTrue(s.FollowContains("c"));

            Symbol.GrammarIdentifier = grammarIdentifier;
        }

        /// <summary>
        /// Método genérico de teste para análise sintática
        /// </summary>
        /// <param name="syntax"></param>
        /// <param name="type"></param>
        private void SyntaxTestExecution(SyntaxAnalizer syntax, SyntaxAnalizerType type)
        {
            /*
            * Todos esses inputs são corretos
            * Input:
            * 
               LET A = 4
               LET B = 5*6
               123 LET C = B/A
               PRINT 2+3, "Olá"
               INPUT D
               GOTO 123
               GOSUB (123)
               IF (A-B) = C THEN (C-1)
               LIST A,B,C
            */

            string[] input = IOUtils.ReadTxtFile(syntaxAnalizerTestFile1);
            bool validation = false;
            string feedback = "";

            //Validação do arquivo com todas as linhas corretas
            int index = 0;
            foreach (string line in input)
            {
                string lline = line;
                syntax.Lexic.File = new string[] { lline };
                feedback = "";
                validation = syntax.Analize(ref feedback);

                Assert.IsTrue(validation, "Validação \"" + lline + "\"" + index + " deu negativa, feedback: " + feedback);

                index++;
            }

            input = IOUtils.ReadTxtFile(syntaxAnalizerTestFile2);

            //Validação do arquivo com todas as linhas inválidas
            index = 0;
            foreach (string line in input)
            {
                string lline = line;
                syntax.Lexic.File = new string[] { lline };
                feedback = "";
                validation = syntax.Analize(ref feedback);

                Assert.IsTrue(!validation, "Validação " + index + " deu positiva, feedback: " + feedback);

                index++;
            }

            feedback = "";

            input = IOUtils.ReadTxtFile(syntaxAnalizerTestFile3);
            syntax.Lexic.File = input;
            validation = syntax.Analize(ref feedback);

            Assert.IsTrue(validation, "Validação " + index + " deu negativa, feedback: " + feedback);
        }
    }
}

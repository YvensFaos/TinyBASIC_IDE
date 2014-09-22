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
    public class LexicTest
    {
        private string lexicAnalizerTestFile = "Test/Input/lexicAnalizerTestFile.txt";

        [TestInitialize]
        public void Initialize()
        {
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
        /// Método de teste do transdutor
        /// </summary>
        [TestMethod]
        public void TransducerTest()
        {
            /*
             * Input:
             * 
                LET A = 4       ->RESERVED_WORD, VARIABLE, MATH_OPERATOR, VALUE
                LET B = 5*6     ->RESERVED_WORD, VARIABLE, MATH_OPERATOR, VALUE, MATH_OPERATOR, VALUE
                LET C = B/A     ->RESERVED_WORD, VARIABLE, MATH_OPERATOR, VARIABLE, MATH_OPERATOR, VARIABLE
                LET d = 1       ->RESERVED_WORD, INVALID,  MATH_OPERATOR, VALUE
                LET D=5         ->RESERVED_WORD, VARIABLE, MATH_OPERATOR, VALUE
                LET E=5+(3-4)   ->RESERVED_WORD, VARIABLE, MATH_OPERATOR, VALUE, MATH_OPERATOR, SEPARATOR, VALUE, MATH_OPERATOR, VALUE, SEPARATOR
             */
            Transducer transducer = new Transducer();

            string[] input = IOUtils.ReadTxtFile(lexicAnalizerTestFile);

            #region LET A = 4
            string line = input[0];
            List<Token> comparateList = new List<Token>() { new Token("LET", TokenType.RESERVED_WORD), new Token("A", TokenType.VARIABLE), new Token("=", TokenType.RELATIONAL_OPERATOR), new Token("4", TokenType.VALUE), new Token("\n", TokenType.LINE_BREAK) };
            List<Token> algorithmList = transducer.getTokens(line);            
            

            Assert.AreEqual(comparateList.Count, algorithmList.Count, "Lista de tamanhos diferentes. Teste 1!" + comparateList.Count + "!=" + algorithmList.Count);

            for (int i = 0; i < comparateList.Count; i++)
            {
                Token compareToken = comparateList[i];
                Token algorithmToken = algorithmList[i];

                Assert.AreEqual(compareToken, algorithmToken, "Tokens diferentes! '" + compareToken + "' !=" + "'"+algorithmToken+"'");
            }
            #endregion

            #region LET B = 5*6
            line = input[1];
            comparateList = new List<Token>() { new Token("LET", TokenType.RESERVED_WORD), new Token("B", TokenType.VARIABLE), new Token("=", TokenType.RELATIONAL_OPERATOR), new Token("5", TokenType.VALUE), new Token("*", TokenType.MATH_OPERATOR), new Token("6", TokenType.VALUE), new Token("\n", TokenType.LINE_BREAK) };
            algorithmList = transducer.getTokens(line);
            

            Assert.AreEqual(comparateList.Count, algorithmList.Count, "Lista de tamanhos diferentes. Teste 2!" + comparateList.Count + "!=" + algorithmList.Count);

            for (int i = 0; i < comparateList.Count; i++)
            {
                Token compareToken = comparateList[i];
                Token algorithmToken = algorithmList[i];

                Assert.AreEqual(compareToken, algorithmToken, "Tokens diferentes!" + compareToken + "!=" + algorithmToken);
            }
            #endregion

            #region LET C = B/A
            line = input[2];
            comparateList = new List<Token>() { new Token("LET", TokenType.RESERVED_WORD), new Token("C", TokenType.VARIABLE), new Token("=", TokenType.RELATIONAL_OPERATOR), new Token("B", TokenType.VARIABLE), new Token("/", TokenType.MATH_OPERATOR), new Token("A", TokenType.VARIABLE), new Token("\n", TokenType.LINE_BREAK) };
            algorithmList = transducer.getTokens(line);
            

            Assert.AreEqual(comparateList.Count, algorithmList.Count, "Lista de tamanhos diferentes. Teste 3!" + comparateList.Count + "!=" + algorithmList.Count);

            for (int i = 0; i < comparateList.Count; i++)
            {
                Token compareToken = comparateList[i];
                Token algorithmToken = algorithmList[i];

                Assert.AreEqual(compareToken, algorithmToken, "Tokens diferentes!" + compareToken + "!=" + algorithmToken);
            }
            #endregion

            #region LET d = 1
            line = input[3];
            comparateList = new List<Token>() { new Token("LET", TokenType.RESERVED_WORD), new Token("d", TokenType.INVALID), new Token("=", TokenType.RELATIONAL_OPERATOR), new Token("1", TokenType.VALUE), new Token("\n", TokenType.LINE_BREAK) };
            algorithmList = transducer.getTokens(line);
            

            Assert.AreEqual(comparateList.Count, algorithmList.Count, "Lista de tamanhos diferentes. Teste 4!" + comparateList.Count + "!=" + algorithmList.Count);

            for (int i = 0; i < comparateList.Count; i++)
            {
                Token compareToken = comparateList[i];
                Token algorithmToken = algorithmList[i];

                Assert.AreEqual(compareToken, algorithmToken, "Tokens diferentes!" + compareToken + "!=" + algorithmToken);
            }
            #endregion

            #region LET D=5
            line = input[4];
            comparateList = new List<Token>() { new Token("LET", TokenType.RESERVED_WORD), new Token("D", TokenType.VARIABLE), new Token("=", TokenType.RELATIONAL_OPERATOR), new Token("5", TokenType.VALUE), new Token("\n", TokenType.LINE_BREAK) };
            algorithmList = transducer.getTokens(line);
            

            Assert.AreEqual(comparateList.Count, algorithmList.Count, "Lista de tamanhos diferentes. Teste 5!" + comparateList.Count + "!=" + algorithmList.Count);

            for (int i = 0; i < comparateList.Count; i++)
            {
                Token compareToken = comparateList[i];
                Token algorithmToken = algorithmList[i];

                Assert.AreEqual(compareToken, algorithmToken, "Tokens diferentes!" + compareToken + "!=" + algorithmToken);
            }
            #endregion

            #region LET E=5+(3-4)
            line = input[5];
            comparateList = new List<Token>() { new Token("LET", TokenType.RESERVED_WORD), new Token("E", TokenType.VARIABLE), new Token("=", TokenType.RELATIONAL_OPERATOR), new Token("5", TokenType.VALUE),
                new Token("+", TokenType.MATH_OPERATOR), new Token("(", TokenType.SEPARATOR), new Token("3", TokenType.VALUE), new Token("-", TokenType.MATH_OPERATOR), new Token("4", TokenType.VALUE),
                new Token(")", TokenType.SEPARATOR), new Token("\n", TokenType.LINE_BREAK)};
            algorithmList = transducer.getTokens(line);
            

            Assert.AreEqual(comparateList.Count, algorithmList.Count, "Lista de tamanhos diferentes. Teste 6!" + comparateList.Count + "!=" + algorithmList.Count);

            for (int i = 0; i < comparateList.Count; i++)
            {
                Token compareToken = comparateList[i];
                Token algorithmToken = algorithmList[i];

                Assert.AreEqual(compareToken, algorithmToken, "Tokens diferentes!" + compareToken + "!=" + algorithmToken);
            }
            #endregion
        }
    }
}

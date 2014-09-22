using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBASICAnalizer.Control;
using TinyBASICAnalizer.Model;
using TinyBASICAnalizer.Model.Code;
using TinyBASICAnalizer.Persistence;

namespace TinyBASICAnalizer.Test
{
    [TestClass]
    public class CodeGeneratorTest
    {
        private CodeGenerator codeGenerator;
        private VirtualMachine virtualMachine;

        [TestInitialize]
        public void Initialize()
        {
            codeGenerator = new CodeGenerator();
            virtualMachine = new VirtualMachine();
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

        [TestMethod]
        public void CalculateExpressionsTest()
        {
            string organized = codeGenerator.AdjustParenthesis("1 + (3 + 12)");
            Assert.AreEqual(organized.Replace(" ", ""), "3+12+1", "Parse de expressão inválido");
            organized = codeGenerator.AdjustParenthesis("1 + (3 + (3 + 4))");
            Assert.AreEqual(organized.Replace(" ", ""), "3+4+3+1", "Parse de expressão inválido");
            organized = codeGenerator.AdjustParenthesis("(1 * 2) + (3 + (3 + 4))");
            Assert.AreEqual(organized.Replace(" ", ""), "1*2+3+4+3", "Parse de expressão inválido");
        }

        [TestMethod]
        public void VirtualMachineTest()
        {
            string[] file = null;

            file = new string[]
            {
                "LET A = 12",
                "REM \"O valor de A é\"",
                "PRINT A"
            };

            codeGenerator.File = file;
            codeGenerator.GenerateCode();
            virtualMachine.Code = codeGenerator.CodeByteStream();
            string[] output = virtualMachine.Result();

            Assert.AreEqual(output[0], "O valor de A é");
            Assert.AreEqual(output[1], "12");
        }
    }
}

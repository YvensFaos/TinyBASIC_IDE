using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyBASICAnalizer.ADS;

namespace TinyBASICAnalizer.Test
{
    [TestClass]
    public class StackTest
    {
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

        [TestMethod]
        public void StackSimpleTest()
        {
            SimpleStack<int> stack = new SimpleStack<int>();
            DateTime now = DateTime.Now;
            Random random = new Random((int)((13 * DateTime.DaysInMonth(now.Year, now.Month)) + now.Ticks));

            Assert.AreEqual(0, stack.Size, "Tamanho da pilha diferente de 0");

            stack.Push(random.Next(50));
            stack.Push(random.Next(50));
            stack.Push(random.Next(50));

            Assert.AreEqual(3, stack.Size, "Tamanho da pilha diferente de 3");

            int value = random.Next(50);
            stack.Push(value);

            Assert.AreEqual(4, stack.Size, "Tamanho da pilha diferente de 4");
            Assert.AreEqual(value, stack.Top(), "Valores diferentes para o topo e o valor: " + value + " != " + stack.Top());

            int topValue = stack.Pop();

            Assert.AreEqual(3, stack.Size, "Tamanho da pilha diferente de 3");
            Assert.AreEqual(value, topValue, "Valores diferentes para o topo e o valor: " + value + " != " + topValue);

            while (stack.Size > 0)
            {
                stack.Pop();
            }
            Assert.AreEqual(0, stack.Size, "Tamanho da pilha diferente de 0");

            stack.Pop();
            Assert.AreEqual(0, stack.Size, "Tamanho da pilha diferente de 0");

            Assert.AreEqual(true, stack.IsEmpty(), "Pilha não considerada vazia!");
        }
    }
}

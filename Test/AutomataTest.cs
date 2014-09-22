using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TinyBASICAnalizer.ADS;
using TinyBASICAnalizer.ADS.AutomataStructure;
using TinyBASICAnalizer.ADS.GrammarAutomata;
using TinyBASICAnalizer.Model;
using TinyBASICAnalizer.Persistence;

namespace TinyBASICAnalizer.Test
{
    [TestClass]
    public class AutomataTest
    {
        private string grammarIdentifier;

        [TestInitialize]
        public void Initialize()
        {
            grammarIdentifier = Symbol.GrammarIdentifier;
            Symbol.GrammarIdentifier = "6";
        }

        [TestCleanup]
        public void CleanUp()
        {
            Symbol.GrammarIdentifier = grammarIdentifier;
        }

        [TestMethod]
        public void TestAutomataAFND()
        {
            Automata<int, char> testAutomata = GenerateTestAutomataAFN();

            #region Trivial Tests
            State<int, char> state0 = testAutomata.InitialState;
            Assert.AreEqual(0, state0.StateValue, "Valor do estado 0 diferente de 0, valor = " + state0.StateValue);

            List<State<int, char>> list = state0.ReachableStatesBy('e');
            Assert.AreEqual(list.Count, 1, "Estados alcançáveis pelo inicial diferente de 1, valor = " + list.Count);

            //State 1
            State<int, char> stateAux = list[0];
            Assert.AreEqual(1, stateAux.StateValue, "Valor do estado 1 diferente de 1, valor = " + stateAux.StateValue);

            list = stateAux.ReachableStatesBy('e');
            Assert.IsNull(list, "Lista não nula para os estados alcançáveis a partir do vazio do estado 1");

            //State 2
            stateAux = stateAux.TransictionIn('a');
            Assert.AreEqual(2, stateAux.StateValue, "Valor do estado 2 diferente de 2, valor = " + stateAux.StateValue);

            list = stateAux.ReachableStatesBy('e');
            Assert.AreEqual(list.Count, 1, "Estados alcançáveis pelo estado 2 diferente de 1, valor = " + list.Count);

            //State 3
            stateAux = list[0];
            Assert.AreEqual(3, stateAux.StateValue, "Valor do estado 3 diferente de 3, valor = " + stateAux.StateValue);

            //State 4
            stateAux = stateAux.TransictionIn('b');
            Assert.AreEqual(4, stateAux.StateValue, "Valor do estado 4 diferente de 4, valor = " + stateAux.StateValue);
            Assert.IsTrue(stateAux.IsFinal, "Estado 4 não final!");
            #endregion

            GrammarNonDeterministicAutomata automata = new GrammarNonDeterministicAutomata();
            Assert.IsNotNull(automata, "Automato não determinístico com transição em vazio retornou nulo");

            //TODO mais testes
        }

        [TestMethod]
        public void TestAutomataAFD()
        {
            Automata<int, char> testAutomata = GenerateTestAutomataAFN();

            List<char> transictionList = new List<char>(new char[] { 'a','b','e' });

            testAutomata = Automata<int, char>.AFNtoAFD(testAutomata, transictionList, 'e');

            State<int, char> state0 = testAutomata.InitialState;
            Assert.AreEqual(0, state0.StateValue, "Valor do estado 0 diferente de 0, valor = " + state0.StateValue);

            State<int, char> stateAux = state0.Transictions[0].NextState;
            Assert.AreEqual(2, stateAux.StateValue, "Valor do estado 2 diferente de 2, valor = " + stateAux.StateValue);
            Assert.AreEqual(2, stateAux.Transictions.Count, "Valor do estado 2 diferente de 2, valor = " + stateAux.StateValue);

            stateAux = stateAux.Transictions[0].NextState;
            Assert.AreEqual(3, stateAux.StateValue, "Valor do estado 3 diferente de 3, valor = " + stateAux.StateValue);
            Assert.AreEqual(1, stateAux.Transictions.Count, "Valor do estado 1 diferente de 1, valor = " + stateAux.StateValue);

            stateAux = stateAux.Transictions[0].NextState;
            Assert.AreEqual(4, stateAux.StateValue, "Valor do estado 4 diferente de 4, valor = " + stateAux.StateValue);
            Assert.IsTrue(stateAux.IsFinal, "Estado 4 não é final!");

            GrammarDeterministicAutomata automata = new GrammarDeterministicAutomata();
            Assert.IsNotNull(automata, "Automato determinístico retornou nulo");

            //TODO mais testes
        }

        /// <summary>
        /// Método para gerar um automato simples e não determinístico para testes
        /// </summary>
        /// <returns></returns>
        private Automata<int, char> GenerateTestAutomataAFN()
        {
            State<int, char> state0 = new State<int, char>(0);
            State<int, char> state1 = new State<int, char>(1);
            State<int, char> state2 = new State<int, char>(2);
            State<int, char> state3 = new State<int, char>(3);
            State<int, char> state4 = new State<int, char>(4, true);

            List<State<int, char>> stateList = new List<State<int,char>>();
            stateList.Add(state0);
            stateList.Add(state1);
            stateList.Add(state2);
            stateList.Add(state3);
            stateList.Add(state4);

            List<char> transictionList = new List<char>();
            transictionList.Add('e');
            transictionList.Add('a');
            transictionList.Add('b');

            state0.AddTransiction('e', state1);
            state1.AddTransiction('a', state2);
            state2.AddTransiction('e', state3);
            state2.AddTransiction('a', state4);
            state3.AddTransiction('b', state4);

            return new Automata<int, char>(state0, stateList, transictionList, 'e');
        }

        /*
         * Template de Teste para ser utilizado
         * 
        [TestMethod]
        public void TestTemplate()
        {
        }
         * */
    }
}

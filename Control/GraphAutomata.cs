using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TinyBASICAnalizer.ADS.AutomataStructure;
using TinyBASICAnalizer.ADS.GrammarAutomata;

using Microsoft.Glee.Drawing;
using Microsoft.Glee.GraphViewerGdi;

namespace TinyBASICAnalizer.ADS
{
    public class GraphicAutomata
    {
        /// <summary>
        /// Gera a imagem de um automato em formato de grafo
        /// </summary>
        /// <typeparam name="T">Valor contido em cada estado.</typeparam>
        /// <typeparam name="E">Valor esperado para as transições de estados.</typeparam>
        /// <param name="automata">Automato passado</param>
        /// <param name="path">Caminho de destino da imagem</param>
        /// <returns></returns>
        public static bool AutomataToPNG<T, E>(Automata<T,E> automata, string path)
        {
            Graph graph = new Graph("");
            for (int i = 0; i < automata.StateList.Count; i++)
            {
                State<T,E> state = automata.StateList[i];
                graph.AddNode(state.StateValue.ToString());
                for (int j = 0; j < state.Transictions.Count; j++)
                {
                    graph.AddEdge(state.StateValue.ToString(), state.Transictions[j].Transiction.ToString(), state.Transictions[j].NextState.StateValue.ToString());
                }
            }

            return GenerateGraphImage(graph, path);
        }

        /// <summary>
        /// Gera a imagem do grafo e salva no path passado
        /// </summary>
        /// <param name="graph">Grafo</param>
        /// <param name="path">Caminho destino da imagem</param>
        private static bool GenerateGraphImage(Graph graph, string path)
        {
            GraphRenderer renderer = new GraphRenderer(graph);
            renderer.CalculateLayout();
            Bitmap bitmap = new Bitmap((int)(graph.Width * 1.2), (int)(graph.Height * 1.2), PixelFormat.Format32bppPArgb);
            renderer.Render(bitmap);

            if (!path.Equals(""))
            {
                bitmap.Save(path);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Converte um afd para uma imagem representando o gráfico
        /// </summary>
        public static bool AFDtoPNG(string path)
        {
            GrammarDeterministicAutomata afd = new GrammarDeterministicAutomata();
            Graph graph = new Graph("");
            for (int i = 0; i < afd.StateList.Count;i++)
            {
                GrammarState state = GrammarState.ConvertFrom(afd.StateList[i]);
                graph.AddNode(state.StateValue);
                for (int j = 0; j < state.Transictions.Count; j++)
                {
                    graph.AddEdge(state.StateValue, state.Transictions[j].Transiction.Value, state.Transictions[j].NextState.StateValue);
                }
            }

            return GenerateGraphImage(graph, path);
        }

        /// <summary>
        /// Converte um afnde para uma imagem representando o gráfico
        /// </summary>
        public static bool AFNDEtoPNG(string path)
        {
            GrammarNonDeterministicAutomata afnde = new GrammarNonDeterministicAutomata();
            Graph graph = new Graph("");
            for (int i = 0; i < afnde.StateList.Count; i++)
            {
                GrammarState state = GrammarState.ConvertFrom(afnde.StateList[i]);
                graph.AddNode(state.StateValue);
                for (int j = 0; j < state.Transictions.Count; j++)
                {
                    graph.AddEdge(state.StateValue, state.Transictions[j].Transiction.Value, state.Transictions[j].NextState.StateValue);
                }
            }

            return GenerateGraphImage(graph, path);
        }
    }
}

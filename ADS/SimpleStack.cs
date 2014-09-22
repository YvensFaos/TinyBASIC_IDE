using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBASICAnalizer.ADS
{
    class StackNode<E>
    {
        private E _value;
        /// <summary>
        /// Valor do node
        /// </summary>
        public E Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private StackNode<E> _next;
        /// <summary>
        /// Referência ao próximo node
        /// </summary>
        public StackNode<E> Next
        {
            get { return _next; }
            set { _next = value; }
        }

        /// <summary>
        /// Construtor default do node de pilha
        /// </summary>
        public StackNode() : this(default(E))
        { }

        /// <summary>
        /// Construtor setando o valor do Node e criando um Next igual a null
        /// </summary>
        /// <param name="value"></param>
        public StackNode(E value)
        {
            _value = value;
            Next = null;
        }
    }

    public class SimpleStack<E>
    {
        private int _size;
        /// <summary>
        /// Representa o tamanho da pilha
        /// </summary>
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        private int _limit;
        /// <summary>
        /// Representa o limite do tamanho da pilha
        /// </summary>
        public int Limit
        {
            get { return _limit; }
            set { _limit = value; }
        }

        private StackNode<E> _top;

        /// <summary>
        /// Construtor default com tamanho igual a 0
        /// </summary>
        public SimpleStack()
        {
            _top = new StackNode<E>();
            Size = 0;
            Limit = 1000; //DEFAULT
        }

        /// <summary>
        /// Construtor com um elemento inicial e tamanho igual a 1
        /// </summary>
        /// <param name="value"></param>
        public SimpleStack(E value) : this()
        {
            StackNode<E> node = new StackNode<E>(value);
            _top.Next = node;
            Limit = 1000; //DEFAULT
            Size++;
        }

        /// <summary>
        /// Método para inserir valor no topo da pilha
        /// </summary>
        /// <param name="value"></param>
        public void Push(E value)
        {
            if (Size >= Limit)
            {
                //Provisório
                throw new OutOfMemoryException();
            }

            if (!IsEmpty())
            {
                StackNode<E> node = new StackNode<E>(value);
                StackNode<E> auxiliar = _top.Next;

                _top.Next = node;
                node.Next = auxiliar;
                Size++;
            }
            else
            {
                StackNode<E> node = new StackNode<E>(value);
                _top.Next = node;
                Size++;
            }
        }

        /// <summary>
        /// Método para remover o valor do topo da pilha
        /// </summary>
        /// <returns></returns>
        public E Pop()
        {
            if (!IsEmpty())
            {
                StackNode<E> node = _top.Next;
                if (node.Next != null)
                {
                    _top.Next = node.Next;
                }
                else
                {
                    _top.Next = null;
                }
                Size--;

                return node.Value;
            }
            return default(E);
        }

        /// <summary>
        /// Método para verificar o valor do topo da pilha sem removê-lo
        /// </summary>
        /// <returns></returns>
        public E Top()
        {
            if (!IsEmpty())
            {
                E value = _top.Next.Value;
                return value;
            }
            return default(E);
        }

        /// <summary>
        /// Método para verificar se a pilha contém algum valor
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return (Size == 0);
        }

        /// <summary>
        /// Método para escrever a pilha no formato: "{N{1}, N{2}, ..., N{Size - 1}}"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string toString = "{";
            StackNode<E> node = _top;
            for (int i = 0; i < Size; i++)
            {
                node = node.Next;
                toString += node.Value.ToString() + ", ";
            }
            //Remoção do último ", "
            if (toString.Length > 2)
            {
                toString = toString.Remove(toString.Length - 2);
            }
            toString += "}";

            return toString;
        }
    }
}

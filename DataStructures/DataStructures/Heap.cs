using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace DataStructures
{
    public class Heap<T> : IEnumerable<T> where T : IComparable
    {
        #region Variables
        //Heap
        internal DoubleLinkedList<HeapNode<T>> heapArray;
        //Capacidad máxima del heap
        int capacity;


        #endregion

        #region Methods
        //Constructor
        public Heap(int L)
        {
            capacity = L;
            heapArray = new DoubleLinkedList<HeapNode<T>>();
        }
        //Metodo que obtiene el tamaño del heap
        public int Length()
        {
            return heapArray.Length;
        }
        
        //Obtiene el Nodo padre
        public int Parent(int index)
        {
            return (index - 1) / 2;
        }
        //Obtiene el hijo izquierdo
        public int Left(int index)
        {
            return 2 * index + 1;
        }
        //Obtiene el hijo derecho
        public int Right(int index)
        {
            return 2 * index + 2;
        }

        //Inserta un nuevo elemento al heap
        public bool insertKey(T value, string p)
        {
            //Si no ha llegado a su máxima capacidad, inserta el elemento
            if (Length() == capacity)
            {
                return false;
            }
            int i = Length();
            heapArray.Insert(new HeapNode<T>(value, p), i);

            while (i > 0 && heapArray.Get(i).CompareTo(heapArray.Get(Parent(i))) > 0)
            {
                heapArray.Swap(i, Parent(i));
                i = Parent(i);
            }
            return true;
        }
        //Obtiene el elemento más pequeño del heap
        public T getMin()
        {
            return heapArray.Get(0).value;
        }
        public T Extract()
        {
            return extractMin().value;
        }
        //Extrae el elemento más pequeño del heap
        internal HeapNode<T> extractMin()
        {
            //Si el heap no esta vació realiza la eliminación
            if (Length() <= 0)
            {
                return default;
            }
            else
            {
                //Elimina el primer elemento
                HeapNode<T> result = heapArray.Get(0);
                heapArray.Delete(0);
                if (Length() > 0)
                {
                    HeapNode<T> last = heapArray.Get(heapArray.Length);
                    heapArray.Delete(heapArray.Length);
                    heapArray.InsertAtStart(last);
                    MoveDown(0);
                }
                return result;
            }
        }
        //Borra un elemento en específico
        public void Delete(T value)
        {
            //Elimina si la heap no esta vacía
            if (Length() > 0)
            {
                DoubleLinkedList<HeapNode<T>> result = new DoubleLinkedList<HeapNode<T>>();
                for (int i = 0; heapArray.Length > 0; i++)
                {
                    HeapNode<T> x = extractMin();
                    result.Insert(x, i);
                }
                for (int i = 0; result.Length > 0; i++)
                {
                    HeapNode<T> temp = result.Get(0);
                    result.Delete(0);
                    if (temp.value.CompareTo(value) != 0)
                    {
                        insertKey(temp.value, temp.priority);
                    }
                }
            }
        }

        public void MoveDown(int position)
        {
            int lchild = Left(position);
            int rchild = Right(position);
            int smallest = position;
            if ((lchild < Length()) && (heapArray.Get(smallest).CompareTo(heapArray.Get(lchild)) < 0))
            {
                smallest = lchild;
            }
            if ((rchild < Length()) && (heapArray.Get(smallest).CompareTo(heapArray.Get(rchild)) < 0))
            {
                smallest = rchild;
            }
            if (smallest != position)
            {
                heapArray.Swap(position, smallest);
                MoveDown(smallest);
            }
        }

        //Devuelve todos los elementos del heap
        public IEnumerator<T> GetEnumerator()
        {
            var node = heapArray.First;
            while (node != null)
            {
                yield return node.value.value;
                node = node.next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
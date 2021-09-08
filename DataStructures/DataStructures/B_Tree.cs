using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    public class B_Tree<T> where T : IComparable
    {
        internal B_TreeNode<T> root;
        public int degree;

        public B_Tree(int d)
        {
            root = null;
            degree = d;
        }

        public void Insert(T k)
        {
            if (root == null)
            {
                root = new B_TreeNode<T>(degree, true);
                root.keys[0] = k;
                root.length = 1;
            }
            else
            {
                if (root.length == (degree - 1))
                {
                    B_TreeNode<T> newNode = new B_TreeNode<T>(degree, false);
                    newNode.childs[0] = root;
                    newNode.SplitChild(0, root);

                    int i = 0;
                    if (newNode.keys[0].CompareTo(k) == -1)
                    {
                        i++;
                    }
                    newNode.childs[i].InsertKey(k);

                    root = newNode;
                }
                else
                {
                    root.InsertKey(k);
                }
            }
        }

        public T search(T k)
        {
            if (root != null)
            {
                B_TreeNode<T> node = root.Search(k);
                if (node != null)
                {
                    return k;
                }
                else
                {
                    return default;
                }
            }
            else
            {
                return default;
            }
        }

        public T search(Func<T, int> k)
        {
            if (root != null)
            {
                B_TreeNode<T> node = root.Search(k);
                if (node != null)
                {
                    foreach(var key in node.keys)
                    {
                        if(k.Invoke(key) == 0)
                        {
                            return key;
                        }
                    }
                    return default;
                }
                else
                {
                    return default;
                }
            }
            else
            {
                return default;
            }
        }

        public void Remove(T k)
        {
            if (root != null)
            {
                root.remove(k);

                if (root.length == 0)
                {
                    B_TreeNode<T> tmp = root;
                    if (root.leaf)
                    {
                        root = null;
                    }
                    else
                    {
                        root = root.childs[0];
                    }
                }
            }
        }
        public string printTree()
        {
            string result = "";
            if (root != null)
            {
                root.print(ref result);
            }
            result = result.Remove(result.Length - 2);
            return result;
        }

        public DoubleLinkedList<T> inOrder()
        {
            DoubleLinkedList<T> result = new DoubleLinkedList<T>();

            if (this.root != null)
                this.root.inOrder(ref result);
            return result;
        }

        public DoubleLinkedList<T> preOrder()
        {
            DoubleLinkedList<T> result = new DoubleLinkedList<T>();

            if (this.root != null)
                this.root.preOrder(ref result);
            return result;
        }

        public DoubleLinkedList<T> postOrder()
        {
            DoubleLinkedList<T> result = new DoubleLinkedList<T>();

            if (this.root != null)
                this.root.postOrder(ref result);
            return result;
        }
    }
}

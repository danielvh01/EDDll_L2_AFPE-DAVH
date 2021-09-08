using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures
{
    internal class B_TreeNode<T> where T : IComparable
    {        
        public T[] keys;
        int maximun;
        int minimum;
        public B_TreeNode<T>[] childs;
        public int length;
        public bool leaf;

        public B_TreeNode(int degree, bool l)
        {
            maximun = degree - 1;
            minimum = degree / 2;
            leaf = l;
            keys = new T[maximun];
            childs = new B_TreeNode<T>[maximun + 1];
            length = 0;
        }

        public void InsertKey(T k)
        {
            int i = length - 1;

            if (leaf)
            {
                while (i >= 0 && keys[i].CompareTo(k) == 1)
                {
                    keys[i + 1] = keys[i];
                    i--;
                }
                keys[i + 1] = k;
                length++;
            }
            else
            {
                while (i >= 0 && keys[i].CompareTo(k) == 1)
                {
                    i--;
                }
                if (childs[i + 1].length == maximun)
                {
                    SplitChild(i + 1, childs[i + 1]);

                    if (keys[i + 1].CompareTo(k) == -1)
                    {
                        i++;
                    }
                }
                childs[i + 1].InsertKey(k);
            }
        }

        public void SplitChild(int i, B_TreeNode<T> y)
        {
            B_TreeNode<T> z = new B_TreeNode<T>(y.maximun + 1, y.leaf);
            z.length = minimum - 1;

            for (int j = 0; j < minimum - 1; j++)
            {
                z.keys[j] = y.keys[j + minimum + 1];
            }

            if (y.leaf == false)
            {
                for (int j = 0; j < minimum; j++)
                {
                    z.childs[j] = y.childs[j + minimum + 1];
                }
            }

            y.length = minimum;

            for (int j = length; j >= i + 1; j--)
            {
                childs[j + 1] = childs[j];
            }
            childs[i + 1] = z;

            for (int j = length-1; j >= i; j--)
            {
                keys[j + 1] = keys[j];
            }

            keys[i] = y.keys[minimum];
            length = length + 1;


        }

        public B_TreeNode<T> Search(T k)
        {
            int i = 0;
            while(i < length && k.CompareTo(keys[i]) == 1)
            {
                i++;
            }

            if (i < length && keys[i].CompareTo(k) == 0)
            {
                return this;
            }
            else if (leaf)
            {
                return null;
            }
            else
            {
                return childs[i].Search(k);
            }

        }

        public B_TreeNode<T> Search(Func<T, int> k)
        {
            int i = 0;
            while (i < length && k.Invoke(keys[i]) == 1)
            {
                i++;
            }

            if (i < length && k.Invoke(keys[i]) == 0)
            {
                return this;
            }
            else if (leaf)
            {
                return null;
            }
            else
            {
                return childs[i].Search(k);
            }

        }

        int findKey(T k)
        {
            int i = 0;
            while (i < length && keys[i].CompareTo(k) == -1)
            {
                ++i;
            }
            return i;
        }

        public void remove(T k)
        {
            int i = findKey(k);

            if (i < length && keys[i].CompareTo(k) == 0)
            {
                if (leaf)
                {
                    removeFromLeaf(i);
                }
                else
                {
                    removeFromNonLeaf(i);
                }
            }
            else
            {
                if (leaf)
                {
                    return;
                }

                bool flag = (i == length);

                if (childs[i].length < minimum)
                {
                    fill(i);
                }
                if (flag && i > length)
                {
                    childs[i - 1].remove(k);
                }
                else
                {
                    childs[i].remove(k);
                }
            }
        }

        void removeFromLeaf(int idx)
        {
            for (int i = idx + 1; i < length; i++)
            {
                keys[i - 1] = keys[i];
            }

            length--;
        }

        void removeFromNonLeaf(int idx)
        {
            T k = keys[idx];

            if (childs[idx].length >= minimum)
            {
                T pred = getPred(idx);
                keys[idx] = pred;
                childs[idx].remove(pred);
            }
            else if (childs[idx + 1].length >= minimum)
            {
                T succ = getSucc(idx);
                keys[idx] = succ;
                childs[idx + 1].remove(succ);
            }
            else
            {
                merge(idx);
                childs[idx].remove(k);
            }
        }

        T getPred(int idx)
        {
            B_TreeNode<T> cur = childs[idx];
            while (!cur.leaf)
            {
                cur = cur.childs[cur.length];
            }
            return cur.keys[cur.length - 1];
        }

        T getSucc(int idx)
        {
            B_TreeNode<T> cur = childs[idx + 1];
            while (!cur.leaf)
            {
                cur = cur.childs[0];
            }
            return cur.keys[0];
        }

        void fill(int idx)
        {
            if (idx != 0 && childs[idx - 1].length >= minimum)
            {
                borrowFromPrev(idx);
            }
            else if (idx != length && childs[idx + 1].length >= minimum)
            {
                borrowFromNext(idx);
            }
            else
            {
                if (idx != length)
                {
                    merge(idx);
                }
                else
                {
                    merge(idx - 1);
                }
            }
        }

        void borrowFromPrev(int idx)
        {
            B_TreeNode<T> child = childs[idx];
            B_TreeNode<T> sibling = childs[idx- 1];

            for (int i = child.length - 1; i >= 0; --i)
            {
                child.keys[i + 1] = child.keys[i];
            }

            if (!child.leaf)
            {
                for (int i = child.length; i >= 0; --i)
                {
                    child.childs[i + 1] = child.childs[i];
                }
            }

            child.keys[0] = keys[idx - 1];

            if (!child.leaf)
            {
                child.childs[0] = sibling.childs[sibling.length];
            }

            keys[idx - 1] = sibling.keys[sibling.length - 1];

            child.length += 1;
            sibling.length -= 1;
        }

        void borrowFromNext(int idx)
        {
            B_TreeNode<T> child = childs[idx];
            B_TreeNode<T> sibling = childs[idx + 1];

            child.keys[child.length] = keys[idx];

            if (!child.leaf)
            {
                child.childs[child.length + 1] = sibling.childs[0];
            }

            keys[idx] = sibling.keys[0];

            for (int i = 1; i < sibling.length; ++i)
            {
                sibling.keys[i - 1] = sibling.keys[i];
            }

            if (!sibling.leaf)
            {
                for (int i = 1; i <= sibling.length; ++i)
                {
                    sibling.childs[i - 1] = sibling.childs[i];
                }
            }

            child.length += 1;
            sibling.length -= 1;
        }

        void merge(int idx)
        {
            B_TreeNode<T> child = childs[idx];
            B_TreeNode<T> sibling = childs[idx + 1];

            child.keys[child.length - 1] = keys[idx];

            for (int i = 0; i < sibling.length; ++i)
            {
                child.keys[i + minimum] = sibling.keys[i];
            }

            if (!child.leaf)
            {
                for (int i = 0; i <= sibling.length; i++)
                {
                    child.childs[i + minimum] = sibling.childs[i];
                }
            }

            for (int i = idx + 1; i < length; ++i)
            {
                keys[i - 1] = keys[i];
            }

            for (int i = idx + 2; i <= length; ++i)
            {
                childs[i - 1] = childs[i];
            }

            child.length += sibling.length + 1;
            length--;
        }

        public void print(ref string body)
        {
            if(!leaf)
            {
                for(int i = 0; i < length; i++)
                {
                    childs[i].print(ref body);
                    body += keys[i] + ", ";
                }
                childs[length].print(ref body);
            }
            else
            {
                for(int i = 0; i < length; i++)
                {
                    body += keys[i] + ", ";
                }
            }
        }

        public void inOrder(ref DoubleLinkedList<T> result)
        {
            int index;
            for (index = 0; index < length; index++)
            {
                if (!leaf)
                {
                    childs[index].inOrder(ref result);
                }
                result.Insert(keys[index], result.Length);
            }
            if (!leaf)
            {
                childs[index].inOrder(ref result);
            }
        }
        public void preOrder(ref DoubleLinkedList<T> result)
        {
            int index;
            for (index = 0; index < length; index++)
            {
                result.Insert(keys[index], result.Length);
                
            }
            for (index = 0; index < length + 1; index++)
            {
                if (!leaf)
                {
                    childs[index].preOrder(ref result);
                }
            }
        }
        public void postOrder(ref DoubleLinkedList<T> result)
        {
            int index;
            for (index = 0; index < length + 1; index++)
            {
                if (!leaf)
                {
                    childs[index].preOrder(ref result);
                }
            }
            for(index = 0; index < length; index++)
            {
                result.Insert(keys[index], result.Length);
            }
        }
    }
}

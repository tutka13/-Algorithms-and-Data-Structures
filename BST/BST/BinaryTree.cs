using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BST
{
    public class BinaryTree
    {
        MainWindow main;
        Canvas g;
        private TreeNode root;
        public TreeNode Root
        {
            get { return root; }
        }
        public BinaryTree(Canvas C, MainWindow w)       //konstruktor
        {
            main = w;
            g = C;
        }

        public TreeNode Find(int input)
        {
            if (root != null)               //ak je koren != null, zavolame Find metodu na koren
            {
                return root.Find(input);
            }
            else                            //ak je koren = null, vratime null
            {
                return null;
            }
        }

        public void Insert(int input)
        {
            if (root != null)               //ak je koren != null, zavolame Insert metodu na koren
            {
                root.Insert(input);
            }
            else                            //ak je koren = null, tak vytvorime nnvy uzol, ktorym bude prave koren
            {
                root = new TreeNode(input, g.Width / 2, 80, g, main);
            }
        }

        public void Remove(int input)
        {
            //nastav current a parent na hodnotu korena (chcem pouzivat parent refernciu)
            TreeNode current = root;
            TreeNode parent = root;
            bool isLeftChild = false; //pomocna premenna na urcenie ktory syn ma byt zmazany

            //pripad prazdny strom, nedeje sa nic
            if (current == null)
            {
                return;
            }

            //hladanie uzla
            while (current != null && current.Data != input)
            {
                //nastavime current na parent a pozrieme sa na synov
                parent = current;

                //ak vkaldane data < data v current, tak lavy syn
                if (input < current.Data)
                {
                    current = current.LeftNode;
                    isLeftChild = true;                 //pomocna premenna
                }
                else //ak data >, tak pravy syn
                {
                    current = current.RightNode;
                    isLeftChild = false;                //pomocna premenna
                }
            }

            //ak sme uzol nenasli
            if (current == null)
            {
                return;
            }

            //ak hladany uzol je list
            if (current.RightNode == null && current.LeftNode == null)
            {
                //pripad koren
                if (current == root)
                {
                    root = null;
                }
                else
                {
                    //pripad nie je koren + odstranenie referencie
                    if (isLeftChild)
                    {
                        //odstranit nan odkaz
                        parent.LeftNode = null;
                    }
                    else
                    {   //pravy syn
                        parent.RightNode = null;
                    }
                }
            }
            else if (current.RightNode == null) //current ma iba laveho syna
            {
                //pripad koren, nastavi sa na laveho syna
                if (current == root)
                {
                    root = current.LeftNode;
                }
                else
                {
                    //current je pravy alebo lavy syn 
                    if (isLeftChild)
                    {
                        //current bol lavy, miesto neho pride jeho lavy syn
                        parent.LeftNode = current.LeftNode;
                    }
                    else
                    {   //current bol pravy, miesto neho pride jeho lavy syn
                        parent.RightNode = current.LeftNode;
                    }
                }
            }
            else if (current.LeftNode == null) //current ma iba praveho syna
            {
                //pripad koren, nastavi sa praveho syna
                if (current == root)
                {
                    root = current.RightNode;
                }
                else
                {
                    //analogicky
                    if (isLeftChild)
                    {
                        parent.LeftNode = current.RightNode;
                    }
                    else
                    {
                        parent.RightNode = current.RightNode;
                    }
                }
            }
            else                                    //current ma oboch synov
            {
                //najdeme nasledovnika
                TreeNode successor = GetSuccessor(root, current);

                //ak je current koren, novy koren je nasledovnik
                if (current == root)
                {
                    root = successor;
                }
                else if (isLeftChild)        //ak je lavy syn, tak parent.leftnode je nasledovik
                {
                    parent.LeftNode = successor;
                }
                else
                {
                    parent.RightNode = successor;
                }
            }
        }
        public void inOrderSuccessor(TreeNode root, TreeNode n, TreeNode successor)
        {
            if (root == null)
            {
                return;
            }

            inOrderSuccessor(root.LeftNode, n, successor);
            if (root.Data > n.Data && successor.LeftNode == null)
            {
                successor.LeftNode = root;
                return;
            }
            inOrderSuccessor(root.RightNode, n, successor);
        }
        public void inOrderPredecessor(TreeNode root, TreeNode n, TreeNode predecessor)
        {
            if (root == null)
            {
                return;
            }

            inOrderPredecessor(root.RightNode, n, predecessor);
            if (root.Data < n.Data && predecessor.RightNode == null)
            {
                predecessor.RightNode = root;
                return;
            }
            inOrderPredecessor(root.LeftNode, n, predecessor);
        }

        public TreeNode GetSuccessor(TreeNode root, TreeNode n)         //na hladanie nasledovnika pouzijeme inorder prehladavanie
        {
            TreeNode successor = new TreeNode(0);
            inOrderSuccessor(root, n, successor);
            return successor.LeftNode;
        }

        public TreeNode GetPredecessor(TreeNode root, TreeNode n)       //takisto na predchodcu
        {
            TreeNode predeccesor = new TreeNode(0);
            inOrderPredecessor(root, n, predeccesor);
            return predeccesor.RightNode;
        }

        public Nullable<int> Smallest()         //najde minimum v strome
        {
            if (root != null)                   //ak je koren != null, vojdeme do vnutra a hladame najmensiu hodnotu
            {
                return root.SmallestValue();
            }
            else                                 //inak neexistuje
            {
                return null;
            }
        }

        public Nullable<int> Largest()           //najde maximum v strome
        {
            if (root != null)                    //ak je koren != null, vojdeme do vnutra a hladame najvacsiu hodnotu
            {
                return root.LargestValue();
            }
            else                                 //inak neexistuje
            {
                return null;
            }
        }

        public void InOrderTraversal()              //prehladavanie stromu inorder (lavy-koren-pravy)
        {
            if (root != null) root.InOrderTraversal();
        }

        public void PreOrderTraversal()             //prehladavanie stromu preorder (koren-lavy-pravy)
        {
            if (root != null) root.PreOrderTraversal();
        }

        public void PostOrderTraversal()            //prehladavanie stromu postorder (lavy-pravy-koren)
        {
            if (root != null) root.PostOrderTraversal();
        }

        public int Height()
        {
            if (root == null) return 0;             //ak je koren = null, tak hlbka stromu je 0, inak vojdeme dnu
            return root.Height();
        }
    }
}

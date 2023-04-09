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
    public class TreeNode
    {
        MainWindow main;
        Canvas g;
        public double x;                            //pozicia x,y, kde sa bude treenode vykreslovat
        public double y;
        private List<double> Steps = new List<double> { 300, 150, 75, 37.5, 18.25, 9.125, 4.5 };       //vzialenosti pre spacing

        private Ellipse E;                          //kruh pre kazdy uzol
        private TextBlock TB;                       //cislo pre kazdy uzol

        private int data;
        public int Data         // hodnota v koreni
        {
            get { return data; }
        }

        private TreeNode rightNode;
        public TreeNode RightNode   //pravy syn
        {
            get { return rightNode; }
            set { rightNode = value; }
        }

        private TreeNode leftNode;
        public TreeNode LeftNode    //lavy syn
        {
            get { return leftNode; }
            set { leftNode = value; }
        }

        //TreeNode konstruktory
        public TreeNode(int value)          //prazdny treenode, len s hodnotou
        {
            data = value;
        }
        public TreeNode(MainWindow w)
        {
            main = w;
        }
        public TreeNode(int value, double X, double Y, Canvas C, MainWindow w)      //pri inserte pouzivame tento
        {
            data = value;
            x = X;
            y = Y;
            g = C;
            main = w;
            E = DrawEllipse();
            TB = DrawText();
        }
        public Ellipse DrawEllipse()      //vykresli kruh na pozicii x,y
        {
            Ellipse E = new Ellipse();
            E.Fill = new SolidColorBrush(Colors.DarkBlue);
            E.Width = 24;
            E.Height = 24;
            Canvas.SetLeft(E, x - 12);
            Canvas.SetTop(E, y - 12);
            Canvas.SetZIndex(E, 2);
            g.Children.Add(E);
            return E;
        }
        public TextBlock DrawText()        //vypise sa text - hodnota uzla
        {
            string S = Convert.ToString(data);
            TextBlock Cislo = new TextBlock();
            Cislo.Text = S;
            Cislo.FontSize = 12;
            Cislo.Foreground = new SolidColorBrush(Colors.White);

            if (S.Length > 2)
            {
                Canvas.SetLeft(Cislo, x - 10);
            }
            else if (S.Length > 1)
            {
                Canvas.SetLeft(Cislo, x - 6);
            }
            else
            {
                Canvas.SetLeft(Cislo, x - 4);
            }

            Canvas.SetTop(Cislo, y - 8);
            Canvas.SetZIndex(Cislo, 3);
            g.Children.Add(Cislo);
            return Cislo;
        }

        public TreeNode Find(int input)
        {
            if (input == data)                           //ak vstup = hodnota uzla, nasli sme uzol
            {
                return this;
            }
            else if (input < data && leftNode != null)  //ak je  vstup < hodnota uzla a lavy syn existuje, tak lavy syn
            {
                return leftNode.Find(input);
            }
            else if (rightNode != null)                 //pravy syn analogicky
            {
                return rightNode.Find(input);
            }
            else                                       //uzol sa nenasiel
            {
                return null;
            }
        }
        public void DrawLine(double x1, double y1, double x2, double y2)        //metoda na vykreslenie ciary medzi uzlami
        {
            Line L = new Line();
            L.Stroke = new SolidColorBrush(Colors.DarkBlue);
            L.StrokeThickness = 1;
            L.X1 = x1;
            L.Y1 = y1;
            L.X2 = x2;
            L.Y2 = y2;
            Canvas.SetZIndex(L, 0);
            g.Children.Add(L);
        }

        public void Insert(int input)               //vlozenie do stromu 
        {
            double step = 0;                        //vypocita sa vyska
            for (int i = 1; i < 7; i++)
            {
                if (y == 80 * i)
                {
                    step = Steps[i - 1];
                }
            }

            if (step > 4.5)                         //vizualizacia prebieha len do 7. urovne - potrebovali by sme "velky canvas" pre vsetky urovne
            {
                main.Message_TextBox.Text = "";

                if (input >= data)                  //ak je input >= data, tak pravy uzol
                {
                    if (rightNode == null)          //ak neexistuje, vytvorim ho
                    {
                        rightNode = new TreeNode(input, x + step, y + 80, g, main);
                        DrawLine(x, y, x + step, y + 80);

                    }
                    else                                //ak existuje, vojdem don
                    {
                        rightNode.Insert(input);
                    }
                }
                else                                    //lavy rovnako
                {
                    if (leftNode == null)
                    {
                        leftNode = new TreeNode(input, x - step, y + 80, g, main);
                        DrawLine(x, y, x - step, y + 80);
                    }
                    else
                    {
                        leftNode.Insert(input);
                    }
                }
            }
            else                                        //pre zvysne urovne vypiseme do text boxu hlasku
            {
                main.Message_TextBox.Text = "Dosiahli sme maxilmálnu hĺbku stromu pre vizualizáciu!";
            }
        }

        public Nullable<int> SmallestValue()                    //ak dosiahneme posledny lavy uzol, je najmensou hodnotou
        {
            if (leftNode == null)
            {
                return data;
            }
            else                                                //inak dalsi lavy uzol
            {
                return leftNode.SmallestValue();
            }
        }

        internal Nullable<int> LargestValue()                //ak dosiahneme posledny pravy uzol, je najvacsou hodnotou
        {
            if (rightNode == null)
            {
                return data;
            }
            else
            {
                return rightNode.LargestValue();
            }
        }

        public void InOrderTraversal()                       //prehladavanie stromu inorder (lavy-koren-pravy)
        {
            //najprv lavy uzol, az kym jeho deti nebudu = null
            if (leftNode != null) leftNode.InOrderTraversal();

            //koren
            main.Order_TextBox.Text += data + " ";

            //potom pravy uzol
            if (rightNode != null) rightNode.InOrderTraversal();
        }

        public void PreOrderTraversal()                     //prehladavanie stromu preorder (koren-lavy-pravy)
        {
            //najprv vypiseme koren 
            main.Order_TextBox.Text += data + " ";

            //lavy
            if (leftNode != null) leftNode.PreOrderTraversal();

            //pravy
            if (rightNode != null) rightNode.PreOrderTraversal();
        }

        public void PostOrderTraversal()                     //prehladavanie stromu postorder (lavy-pravy-koren)
        {
            //lavy
            if (leftNode != null) leftNode.PostOrderTraversal();

            //pravy
            if (rightNode != null) rightNode.PostOrderTraversal();

            //koren 
            main.Order_TextBox.Text += data + " ";
        }

        public int Height()
        {
            //vrati 1, ak nenajde list
            if (this.leftNode == null && this.rightNode == null)
            {
                return 1;
            }

            int left = 0;
            int right = 0;

            //rekurzivne cez podstrom
            if (this.leftNode != null) left = this.leftNode.Height();
            if (this.rightNode != null) right = this.rightNode.Height();

            //vrati vyssiu hodnotu
            if (left > right)
            {
                return (left + 1);
            }
            else
            {
                return (right + 1);
            }
        }
    }
}

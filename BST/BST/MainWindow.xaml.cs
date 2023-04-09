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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BinaryTree binaryTree;      //binarny strom
        Random rnd;
        Ellipse E;                  //kruznica, ktorou budeme kruzkovat
        public List<int> Values;    //list hodnot v uzloch

        public bool isCorrect;      //pomocna premenna pre chyby vstupov v textboxe
        public static int height;   //vyska/hlbka stromu

        public MainWindow()
        {
            InitializeComponent();                          //inicializacia komponentov
            binaryTree = new BinaryTree(g, this);
            Values = new List<int>();
            rnd = new Random();
            E = new Ellipse();
            Order_TextBox.Text = "Strom je prázdny, pre vypísanie usporiadania vyber možnosť.";
        }

        private void Insert_Click(object sender, RoutedEventArgs e)
        {
            string vstup = Insert_TextBox.Text;              //v stringu vstup si ulozim aktualny obsah textboxu
            int value;
            isCorrect = true;

            string cislo = "";
            for (int i = 0; i < vstup.Length; i++)              //v tomto cykle vytvaram cislo
            {
                if (Char.IsDigit(vstup[i]))
                {
                    cislo += Convert.ToString(vstup[i]);        //zretazim cisla ako text
                }
                else                                            //ak pouzivatel zada hocijaky iny znak do textboxu
                {
                    isCorrect = false;
                    MessageBox.Show("Vstup môže byť iba celé kladné číslo!");
                    break;
                }
            }

            if (vstup.Length == 0)
            {
                isCorrect = false;
                MessageBox.Show("Zadaj vstup!");
            }

            if (isCorrect)                                      //ak je vstup zadany spravne, hlada hodnotu
            {
                value = Convert.ToInt32(cislo);
                TreeNode w = binaryTree.Find(value);

                if (w == null)                                  //ak ju nenajde, vlozi, aj do listu, aj do stromu
                {
                    Values.Add(value);
                    binaryTree.Insert(value);
                    Update();
                }
                else                                            //ak ju najde, vyznaci v canvase
                {
                    DrawEllipse(w.x, w.y);
                    MessageBox.Show("Hodnota " + value + " sa už nachádza v strome!");
                    g.Children.Remove(E);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string vstup = Delete_TextBox.Text; ;                //v stringu vstup si ulozim aktualny obsah textboxu
            int value;
            isCorrect = true;

            string cislo = "";
            for (int i = 0; i < vstup.Length; i++)              //v tomto cykle vytvaram cislo
            {
                if (Char.IsDigit(vstup[i]))
                {
                    cislo += Convert.ToString(vstup[i]);        //zretazim cisla ako text
                }
                else                                            //ak pouzivatel zada hocijaky iny znak do textboxu
                {
                    isCorrect = false;
                    MessageBox.Show("Vstup môže byť iba celé kladné číslo!");
                    break;
                }
            }

            if (vstup.Length == 0)
            {
                isCorrect = false;
                MessageBox.Show("Zadaj vstup!");
            }

            if (isCorrect)                                      //ak je vstup zadany spravne
            {
                value = Convert.ToInt32(cislo);
                TreeNode w = binaryTree.Find(value);
                if (w == null)                                  //ak sa vstup nenachadza v strome, nemoze ho odstranit
                {
                    isCorrect = false;
                    MessageBox.Show("Vstup sa nenachádza v strome!");
                }
                else
                {
                    DrawEllipse(w.x, w.y);                      //ak sa nachadza, tak ho odstrani
                    binaryTree.Remove(Convert.ToInt32(cislo));
                    Values.Remove(w.Data);                      //odstrani odstranovanu hodnotu z listu
                    MessageBox.Show("Číslo " + Convert.ToString(cislo) + " bude odstránené!");

                    g.Children.Clear();
                    DrawBinaryTree();
                    Update();
                }
            }
        }
        public void DrawBinaryTree()                                        //vykresli strom
        {
            binaryTree = new BinaryTree(g, this);
            for (int i = 0; i < Values.Count; i++)
            {
                binaryTree.Insert(Values[i]);
            }
        }
        private void RandomTree_Click(object sender, RoutedEventArgs e)     //vygeneruje 10 nahodnych uzlov, pricom kontroluje podmienku, ci sa uz dana hodnota nachadza v strome
        {
            /*binaryTree.Insert(75);
            binaryTree.Insert(57);
            binaryTree.Insert(90);
            binaryTree.Insert(32);
            binaryTree.Insert(7);
            binaryTree.Insert(44);
            binaryTree.Insert(60);
            binaryTree.Insert(86);
            binaryTree.Insert(93);
            binaryTree.Insert(99);
            binaryTree.Insert(100);*/

            for (int i = 0; i < 10; i++)
            {
                int input = rnd.Next(101);

                if (binaryTree.Find(input) == null)
                {
                    Values.Add(input);
                    binaryTree.Insert(input);
                }
            }
            Update();
        }
        private void Update()                                           //aktualizuje hodnoty vysky, max, min a poradie vypisania
        {
            if (binaryTree.Root != null)
            {
                height = getHeight(binaryTree.Root);
                TreeHeightLabel.Content = getHeight(binaryTree.Root);
                Minimum_Label.Content = getMin(binaryTree.Root);
                Maximum_Label.Content = getMax(binaryTree.Root);
                Order_TextBox.Text = "Vyber možnosť usporiadania.";
            }
            Order();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)      //nastavi pociatocne hodnoty
        {
            g.Children.Clear();
            Insert_TextBox.Text = "";
            Delete_TextBox.Text = "";
            Search_TextBox.Text = "";
            Order_TextBox.Text = "Strom je prázdny.";
            Succ_TextBox.Text = "";
            Maximum_Label.Content = "";
            Minimum_Label.Content = "";
            TreeHeightLabel.Content = "";
            Message_TextBox.Text = "";

            Values.Clear();

            binaryTree = new BinaryTree(g, this);
        }

        public static int getHeight(TreeNode root)          //vyrata vysku
        {
            if (root == null)
            {
                return 0;
            }
            return Math.Max(getHeight(root.LeftNode), getHeight(root.RightNode)) + 1;
        }
        public Nullable<int> getMax(TreeNode root)      //maximum
        {
            return root.LargestValue();
        }
        public Nullable<int> getMin(TreeNode root)      //minimum
        {
            return root.SmallestValue();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string vstup = Search_TextBox.Text; ;                //v stringu vstup si ulozim aktualny obsah textboxu
            isCorrect = true;
            int value;

            string cislo = "";
            for (int i = 0; i < vstup.Length; i++)              //v tomto cykle vytvaram cislo
            {
                if (Char.IsDigit(vstup[i]))
                {
                    cislo += Convert.ToString(vstup[i]);        //zretazim cisla ako text
                }
                else                                            //ak pouzivatel zada hocijaky iny znak do textboxu
                {
                    isCorrect = false;
                    MessageBox.Show("Vstup môže byť iba celé kladné číslo!");
                    break;
                }
            }

            if (vstup.Length == 0)
            {
                isCorrect = false;
                MessageBox.Show("Zadaj vstup!");
            }

            if (isCorrect)                                      //ak je vstup zadany spravne
            {
                value = Convert.ToInt32(cislo);
                TreeNode w = binaryTree.Find(value);
                if (w == null)                                  //ak sa vstup nenachadza v strome
                {
                    isCorrect = false;
                    MessageBox.Show("Vstup sa nenachádza v strome!");
                }
                else                                            //ak sa nachadza, najde ho a vyznaci
                {
                    DrawEllipse(w.x, w.y);
                    MessageBox.Show("Našiel hodnotu " + value + "!");
                    g.Children.Remove(E);
                }
            }
        }
        private void Successor_Click(object sender, RoutedEventArgs e)
        {
            string vstup = Succ_TextBox.Text;              //v stringu vstup si ulozim aktualny obsah textboxu
            isCorrect = true;
            int value;

            string cislo = "";
            for (int i = 0; i < vstup.Length; i++)              //v tomto cykle vytvaram cislo
            {
                if (Char.IsDigit(vstup[i]))
                {
                    cislo += Convert.ToString(vstup[i]);        //zretazim cisla ako text
                }
                else                                            //ak pouzivatel zada hocijaky iny znak do textboxu
                {
                    isCorrect = false;
                    MessageBox.Show("Vstup môže byť iba celé kladné číslo!");
                    break;
                }
            }

            if (vstup.Length == 0)
            {
                isCorrect = false;
                MessageBox.Show("Zadaj vstup!");
            }

            if (isCorrect)                                      //ak je vstup zadany spravne, prekresli sa canvas
            {
                value = Convert.ToInt32(cislo);
                TreeNode w = binaryTree.Find(value);
                if (w == null)                                  //ak sa vstup nenachadza v strome
                {
                    isCorrect = false;
                    MessageBox.Show("Vstup sa nenachádza v strome!");
                }
                else                                            //ak sa nachazda, najde nasledovnika, ak neexistuje, vypise, ak existuje, vyznaci a vypise
                {
                    TreeNode v = binaryTree.GetSuccessor(binaryTree.Root, binaryTree.Find(value));
                    if (v == null)
                    {
                        MessageBox.Show("Nasledovník neexistuje.");
                    }
                    else
                    {
                        DrawEllipse(v.x, v.y);
                        MessageBox.Show("Nasledovník je " + v.Data + ".");
                        g.Children.Remove(E);
                    }
                }
            }
        }
        private void Predecessor_Click(object sender, RoutedEventArgs e)
        {
            string vstup = Succ_TextBox.Text;                   //v stringu vstup si ulozim aktualny obsah textboxu
            isCorrect = true;
            int value;

            string cislo = "";
            for (int i = 0; i < vstup.Length; i++)              //v tomto cykle vytvaram cislo
            {
                if (Char.IsDigit(vstup[i]))
                {
                    cislo += Convert.ToString(vstup[i]);        //zretazim cisla ako text
                }
                else                                            //ak pouzivatel zada hocijaky iny znak do textboxu
                {
                    isCorrect = false;
                    MessageBox.Show("Vstup môže byť iba celé kladné číslo!");
                    break;
                }
            }

            if (vstup.Length == 0)
            {
                isCorrect = false;
                MessageBox.Show("Zadaj vstup!");
            }

            if (isCorrect)                                      //ak je vstup zadany spravne, prekresli sa canvas
            {
                value = Convert.ToInt32(cislo);
                TreeNode w = binaryTree.Find(value);
                if (w == null)                                  //ak sa vstup nenachadza v strome
                {
                    isCorrect = false;
                    MessageBox.Show("Vstup sa nenachádza v strome!");
                }
                else                                            //ak sa nachazda, najde predchodcu, ak neexistuje, vypise, ak existuje, vyznaci a vypise
                {
                    TreeNode v = binaryTree.GetPredecessor(binaryTree.Root, binaryTree.Find(value));
                    if (v == null)
                    {
                        MessageBox.Show("Predchodca neexistuje.");
                    }
                    else
                    {
                        DrawEllipse(v.x, v.y);
                        MessageBox.Show("Predchodca je " + v.Data + ".");
                        g.Children.Remove(E);
                    }
                }
            }
        }
        private void DrawEllipse(double X, double Y)        //nakresli kruh pri hladani
        {
            E.Fill = new SolidColorBrush(Colors.Red);
            E.Width = 24.4;
            E.Height = 24.4;
            Canvas.SetLeft(E, X - 12.2);
            Canvas.SetTop(E, Y - 12.2);
            Canvas.SetZIndex(E, 2);
            g.Children.Add(E);
        }
        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)     //zmena moznosti, zavola Order metodu
        {
            Order();
        }

        private void Order()                                //metoda na vyber usporiadania
        {
            if (binaryTree.Root != null)
            {
                if (Combo.SelectedIndex == 0)
                {
                    Order_TextBox.Text = "";
                    binaryTree.Root.InOrderTraversal();
                }
                if (Combo.SelectedIndex == 1)
                {
                    Order_TextBox.Text = "";
                    binaryTree.Root.PreOrderTraversal();
                }
                if (Combo.SelectedIndex == 2)
                {
                    Order_TextBox.Text = "";
                    binaryTree.Root.PostOrderTraversal();
                }
            }
            else
            {
                Order_TextBox.Text = "Strom je prázdny.";
            }
        }
    }
}

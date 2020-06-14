using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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

namespace Miner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Rectangle rect;
        public Label val;     
        ForMiner game;        
        List<Rectangle> rects;
        List<Label> vals;
        bool MasterMode;
        List<int> problem_index_left ;
        List<int> problem_index_right ;
        List<int> problem_index_up ;
        List<int> problem_index_down ;
        List<int> WhereIWas;
        System.Windows.Threading.DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            retryButton.Visibility = Visibility.Hidden;
            time.Visibility = Visibility.Hidden;
            time1.Visibility = Visibility.Hidden;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            MasterMode = (bool)masterMode.IsChecked;
            game = new ForMiner(MasterMode);
            title.Visibility = Visibility.Hidden;
            masterMode.Visibility = Visibility.Hidden;
            startButton.Visibility = Visibility.Hidden;
            retryButton.Visibility = Visibility.Visible;
            time.Visibility = Visibility.Visible;
            time1.Visibility = Visibility.Visible;
            timer = new System.Windows.Threading.DispatcherTimer();

            timer.Tick += new EventHandler(timerTick);
            timer.Start();
            if (MasterMode)
            {
                easy.Visibility = Visibility.Hidden;
            } else
            {
                hard.Visibility = Visibility.Hidden;
            }
            newGame();

        }
        private void GridCtrl_MouseDown2(object sender, MouseButtonEventArgs e)//поставить-убрать знак вопроса
        {
            if (sender != null)
            {
                Rectangle current = sender as Rectangle;              
                int k = rects.IndexOf(current);
                if (vals[k].Content.ToString() == "")
                {
                    vals[k].Content= "?";
                } else if (vals[k].Content.ToString() == "?")
                {
                    vals[k].Content = "";
                }
            }
        }
        private void GridCtrl_MouseDown3(object sender, MouseButtonEventArgs e)//убрать знак вопроса
        {
            if (sender != null)
            {
                Label current = sender as Label;

                int k = vals.IndexOf(current);
                if (vals[k].Content.ToString() == "")
                {
                    vals[k].Content = "?";
                }
                else if (vals[k].Content.ToString() == "?")
                {
                    vals[k].Content = "";
                }
            }
        }
        private void GridCtrl_MouseDown1(object sender, MouseButtonEventArgs e)//вскрыть ячейку
        {
            if (sender != null)
            {
                Rectangle current = sender as Rectangle;              
                int i = rects.IndexOf(current);
                 WhereIWas = new List<int>();

                if (game.field[i].close)
                {
                    
                    if ((game.field[i].close) && (vals[i].Content.ToString() == ""))
                    {
                        
                        vals[i].Content = "";
                     
                        if (game.field[i].val > 0)
                        {
                            game.field[i].close = false;
                            GetColor(i);
                           
                        } else if (game.field[i].val == -1)
                        {
                            game.field[i].close = false;
                            current.Fill = new SolidColorBrush(Colors.LightGray);
                            vals[i].Content = "©";
                            vals[i].Foreground = new SolidColorBrush(Colors.Black);

                        }else if (game.field[i].val == 0) {
                            game.field[i].close = false;
                            int val = game.Num;
                            problem_index_left = new List<int>();
                            problem_index_right = new List<int>();
                            problem_index_up = new List<int>();
                            problem_index_down = new List<int>();
                            for (int j = 0; j < val * (val - 1) + 1; j += val)
                            {
                                problem_index_left.Add(j);
                            }
                            for (int j = val - 1; j < val * val; j += val)
                            {
                                problem_index_right.Add(j);
                            }
                            for (int j = 0; j < val; j++)
                            {
                                problem_index_up.Add(j);
                            }
                            for (int j = val * (val - 1); j < val * val; j++)
                            {
                                problem_index_down.Add(j);
                            }
                         
                            WhereIWas.Add(i);
                            OpenEmpty(i);
                            
                        }
                    }
                }
                if (isLose())
                {
                    timer.Stop();
                    if (MessageBox.Show("You Lose!\nTry again?", ":(", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        newGame();
                    } else
                    {
                        for (int h=0; h<game.Size; ++h)
                        {
                            if (game.field[h].val == -1)
                            {
                                game.field[h].close = false;
                                rects[h].Fill = new SolidColorBrush(Colors.LightGray);
                                vals[h].Content = "©";
                                vals[h].Foreground = new SolidColorBrush(Colors.Black);
                            } else if (game.field[i].val == 0)
                            {
                                game.field[h].close = false;
                                rects[h].Fill = new SolidColorBrush(Colors.LightGray);
                                vals[h].Content = "";                                
                            } else
                            {
                                GetColor(h);
                            }
                        }
                    }
                }
                if (isWin())
                {
                    timer.Stop();
                    MessageBox.Show("You Win!\nIn " + time.Content + " sec\nTry again?", ":}", MessageBoxButton.OK);

                    newGame();
                }
            }
        }
        private void GetColor(int i)//установить цвет
        {
            rects[i].Fill = new SolidColorBrush(Colors.LightGray);
            vals[i].Content = game.field[i].val;
            game.field[i].close = false;
            switch (game.field[i].val)
            {
                case 1:
                    vals[i].Foreground = new SolidColorBrush(Colors.RoyalBlue);
                    break;
                case 2:
                    vals[i].Foreground = new SolidColorBrush(Colors.Red);
                    break;
                case 3:
                    vals[i].Foreground = new SolidColorBrush(Colors.Green);
                    break;
                case 4:
                    vals[i].Foreground = new SolidColorBrush(Colors.DarkViolet);
                    break;
                case 5:
                    vals[i].Foreground = new SolidColorBrush(Colors.Tomato);
                    break;
                case 6:
                    vals[i].Foreground = new SolidColorBrush(Colors.HotPink);
                    break;
                case 7:
                    vals[i].Foreground = new SolidColorBrush(Colors.AliceBlue);
                    break;
                case 8:
                    vals[i].Foreground = new SolidColorBrush(Colors.Bisque);
                    break;
                case 0:
                    vals[i].Content = "";
                    break;
            }
        }
        private void GridCtrl_MouseDown4(object sender, MouseButtonEventArgs e)//вскрыть ячейку
        {
            if (sender != null)
            {
                Label current = sender as Label;

                int i = vals.IndexOf(current);
                WhereIWas = new List<int>();

                if (game.field[i].close)
                {

                    if ((game.field[i].close) && (vals[i].Content.ToString() == ""))
                    {
                        rects[i].Fill = new SolidColorBrush(Colors.LightGray);
                        vals[i].Content = "";
                        game.field[i].close = false;
                        if (game.field[i].val > 0)
                        {

                            game.field[i].close = false;
                            GetColor(i);
                        }
                        else if (game.field[i].val == -1)
                        {
                            game.field[i].close = false;
                            vals[i].Content = "©";
                            vals[i].Foreground = new SolidColorBrush(Colors.Black);

                        }
                        else if (game.field[i].val == 0)
                        {
                            game.field[i].close = false;
                            int val = game.Num;
                            problem_index_left = new List<int>();
                            problem_index_right = new List<int>();
                            problem_index_up = new List<int>();
                            problem_index_down = new List<int>();
                            for (int j = 0; j < val * (val - 1) + 1; j += val)
                            {
                                problem_index_left.Add(j);
                            }
                            for (int j = val - 1; j < val * val; j += val)
                            {
                                problem_index_right.Add(j);
                            }
                            for (int j = 0; j < val; j++)
                            {
                                problem_index_up.Add(j);
                            }
                            for (int j = val * (val - 1); j < val * val; j++)
                            {
                                problem_index_down.Add(j);
                            }

                            WhereIWas.Add(i);
                            OpenEmpty(i);

                        }
                    }
                }
                if (isLose())
                {
                    timer.Stop();
                    if (MessageBox.Show("You Lose!\nTry again?", ":(", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        newGame();
                    }
                    else
                    {
                        for (int h = 0; h < game.Size; ++h)
                        {
                            if (game.field[h].val == -1)
                            {
                                game.field[h].close = false;
                                rects[h].Fill = new SolidColorBrush(Colors.LightGray);
                                vals[h].Content = "©";
                                vals[h].Foreground = new SolidColorBrush(Colors.Black);
                            }
                            else if (game.field[i].val == 0)
                            {
                                game.field[h].close = false;
                                rects[h].Fill = new SolidColorBrush(Colors.LightGray);
                                vals[h].Content = "";
                            }
                            else
                            {
                                GetColor(h);
                            }
                        }
                    }

                }
                if (isWin())
                {
                    timer.Stop();
                    MessageBox.Show("You Win!\nIn " + time.Content + " sec\nTry again?", ":}", MessageBoxButton.OK);

                    newGame();
                }
            }
        }
        private void OpenEmpty(int i)//рек функция, для открытия смежных пустых ячеек при открытии одной
        {
            int val = game.Num;
            game.field[i].close = false;
            rects[i].Fill = new SolidColorBrush(Colors.LightGray);
            if (game.field[i].val != 0)
            {
                vals[i].Content = game.field[i].val;
            }
            if (!problem_index_up.Contains(i) && !WhereIWas.Contains(i-val)) //ячейка над i-той ячейкой
            {
                if (game.field[i - val].val == 0)
                {
                    WhereIWas.Add(i - val);
                    OpenEmpty(i - val);

                } else
                {
                    WhereIWas.Add(i - val);
                    GetColor(i - val);
                }
            }
            if (!problem_index_down.Contains(i) && !WhereIWas.Contains(i + val)) // ячейка под i-той ячейкой
            {
                if (game.field[i + val].val == 0)
                {
                    WhereIWas.Add(i + val);
                    OpenEmpty(i + val);
                }
                else
                {
                    WhereIWas.Add(i + val);
                    GetColor(i + val);
                }
            }
            if ((!problem_index_left.Contains(i)) && !WhereIWas.Contains(i - 1))//ячейка слева от i-той
            {
                if (game.field[i - 1].val == 0)
                {
                    WhereIWas.Add(i - 1);
                    OpenEmpty(i - 1);
                    
                }
                else
                {
                    WhereIWas.Add(i - 1);
                    GetColor(i-1);
                }
            }
            if (!problem_index_right.Contains(i) && !WhereIWas.Contains(i + 1))//ячейка справа от i-той
            {
                if (game.field[i + 1].val == 0)
                {
                    WhereIWas.Add(i + 1);
                    OpenEmpty(i + 1);
                }
                else
                {
                    WhereIWas.Add(i + 1);
                    GetColor(i + 1);
                }
            }
            if ((!problem_index_left.Contains(i)) && (!problem_index_up.Contains(i)) && !WhereIWas.Contains(i - val - 1))//ячейка слева сверху от i-той
            {
                if (game.field[i - val - 1].val == 0)
                {
                    WhereIWas.Add(i - val - 1);
                    OpenEmpty(i - val - 1);
                }
                else
                {
                    WhereIWas.Add(i - val-1);
                    GetColor(i-val-1);
                }
            }
            if ((!problem_index_left.Contains(i)) && (!problem_index_down.Contains(i)) && !WhereIWas.Contains(i + val - 1))//ячейка слева снизу от i-той
            {
                if (game.field[i + val - 1].val == 0)
                {
                    WhereIWas.Add(i +val- 1);
                    OpenEmpty(i + val - 1);
                }
                else
                {
                    WhereIWas.Add(i + val-1);
                    GetColor(i+val-1);
                }
            }
            if ((!problem_index_right.Contains(i)) && (!problem_index_up.Contains(i)) && !WhereIWas.Contains(i - val + 1))//ячейка справа сверху от i-той
            {
                if (game.field[i - val + 1].val == 0)
                {
                    WhereIWas.Add(i - val + 1);
                    OpenEmpty(i - val + 1);
                }
                else
                {
                    WhereIWas.Add(i - val+1);
                    GetColor(i-val+1);
                }
            }
            if ((!problem_index_right.Contains(i)) && (!problem_index_down.Contains(i)) && !WhereIWas.Contains(i + val + 1))//ячейка справа снизу от i-той
            {
                if (game.field[i + val + 1].val == 0)
                {
                    WhereIWas.Add(i + val + 1);
                    OpenEmpty(i + val + 1);
                }
                else
                {
                    WhereIWas.Add(i + val+1);
                    GetColor(i+val+1);
                }
            }
           
          
        }
        private void FirstUpdate(ForMiner new_game, bool isHard)
            //процедура первого обновления, заполняющая grid объектами классов Rectangle и Label

        {
            Grid current;
            if (isHard)
            {
                current = hard;
            }
            else
            {
                current = easy;
            }

            for (int i = 0; i < new_game.Size; i++)
            {
                
                current.Children.Add(rects[i]);
                Grid.SetColumn(rects[i], i % new_game.Num);
                Grid.SetRow(rects[i], i / new_game.Num);
                current.Children.Add(vals[i]);
                Grid.SetColumn(vals[i], i % new_game.Num);
                Grid.SetRow(vals[i], i / new_game.Num);
            }
        }
        private bool isLose()//мы проиграли?
        {
            bool res=false;
            foreach (var item in game.field)
            {
                if ((!item.close)&& (item.val == -1)){
                   res  = true;
                   break;
                }
            }
            return res;
        }
       
        private bool isWin()//мы выиграли?
        {
            bool res = true;
            foreach (var item in game.field)
            {
                if (item.val != -1)
                {
                    res = res && (!item.close);
                } else
                {
                    res = res && (item.close);
                }
            }
            return res;

        }

        private void newGame()
        {
            
           
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
            time.Content = "0";
            if (rects!=null)
                rects.Clear();
            if (game!= null)
            game.Clear();
            if (vals!=null)
            vals.Clear();
            game = new ForMiner(MasterMode);
            rects = new List<Rectangle>();
            vals = new List<Label>();
            SolidColorBrush brush = new SolidColorBrush(Colors.Gray);
            SolidColorBrush stroke = new SolidColorBrush(Colors.Black);
            for (int i = 0; i < game.Size; i++)
            {
                Label val = new Label();
                val.HorizontalAlignment = HorizontalAlignment.Center;
                val.VerticalAlignment = VerticalAlignment.Center;
                if (MasterMode) { val.FontSize = 22; }
                else
                {
                    val.FontSize = 45;
                }
                val.FontFamily = new System.Windows.Media.FontFamily("Symbol");
                val.FontWeight = FontWeights.Bold;
                val.Content = "";
                Rectangle empty_rec = new Rectangle();
                empty_rec.Fill = brush;
                empty_rec.Stroke = stroke;

                vals.Add(val);
                rects.Add(empty_rec);
            }
            FirstUpdate(game, MasterMode);
            for (int i = 0; i < game.Size; i++)
            {
                this.rects[i].MouseLeftButtonDown += new MouseButtonEventHandler(GridCtrl_MouseDown1);
                this.rects[i].MouseRightButtonDown += new MouseButtonEventHandler(GridCtrl_MouseDown2);
                this.vals[i].MouseRightButtonDown += new MouseButtonEventHandler(GridCtrl_MouseDown3);
                this.vals[i].MouseLeftButtonDown += new MouseButtonEventHandler(GridCtrl_MouseDown4);
            }

        }

        private void retryButton_Click(object sender, RoutedEventArgs e)
        {

            newGame();
            
        }
        private void timerTick(object sender, EventArgs e)
        {
            
            int t = Convert.ToInt32(time.Content);
            t += 1;
            time.Content = t.ToString() ;
        }

        private void retryButton_Click_1(object sender, RoutedEventArgs e)
        {

            newGame();
        }
    }
}

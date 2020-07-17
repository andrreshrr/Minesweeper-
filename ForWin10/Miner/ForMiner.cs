using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miner
{
    public class ForMiner
    {

        public int Num { get { return (int)Math.Sqrt(field.Count); } }//порядок поля
        public int Size { get { return field.Count; } } // размер поля
        public List<cell> field;
        Random random = new Random();

        public void Clear()
        {
            field.Clear();
        }

        public ForMiner(bool isHard)
        {
            int bombsCount;
            field = new List<cell>();
            if (isHard)
            {
                bombsCount = 40;
                for (int i = 0; i < 256; i++)
                {
                   
                    field.Add(new cell(0, true)); //ячейки создаются для сложного варианта
                } 
            } else
            {
                bombsCount = 10;
                for (int i = 0; i < 64; i++)
                {
                    field.Add(new cell(0, true)); //ячейки создаются для лёгкого варианта
                }
            }
            
                int bomb = 0;
                //автоматическая генерация бомб
                while (bomb < bombsCount)
                {

                    for (int i = 0; i < field.Count; ++i)
                    {
                        int p = random.Next(100);
                        if ((p > 80) && (field[i].val == 0) && (bomb < bombsCount))
                        {
                            field[i].val = -1;
                            bomb++;
                        }

                    }
                }
                //заполнение ячеек
                List<int> iters = new List<int>();
                List<int> problem_index_left = new List<int>();
                List<int> problem_index_right = new List<int>();
                List<int> problem_index_up = new List<int>();
                List<int> problem_index_down = new List<int>();
                int bombs_near = 0;
                for (int i = 0; i < field.Count; i++)
                {
                    if (field[i].val != -1)
                    {
                        int val = (int)Math.Sqrt(field.Count);//разрядность поля
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
                        if (!problem_index_up.Contains(i)) //ячейка над i-той ячейкой
                        {
                            iters.Add(i - val);
                        }
                        if (!problem_index_down.Contains(i)) // ячейка под i-той ячейкой
                        {
                            iters.Add(i + val);
                        }
                        if ((!problem_index_left.Contains(i)))//ячейка слева от i-той
                        {
                            iters.Add(i - 1);
                        }
                        if (!problem_index_right.Contains(i))//ячейка справа от i-той
                        {
                            iters.Add(i + 1);
                        }
                        if ((!problem_index_left.Contains(i)) && (!problem_index_up.Contains(i)))//ячейка слева сверху от i-той
                        {
                            iters.Add(i - val - 1);
                        }
                        if ((!problem_index_left.Contains(i)) && (!problem_index_down.Contains(i)))//ячейка слева снизу от i-той
                        {
                            iters.Add(i + val - 1);
                        }
                        if ((!problem_index_right.Contains(i)) && (!problem_index_up.Contains(i)))//ячейка справа сверху от i-той
                        {
                            iters.Add(i - val + 1);
                        }
                        if ((!problem_index_right.Contains(i)) && (!problem_index_down.Contains(i)))//ячейка справа снизу от i-той
                        {
                            iters.Add(i + val + 1);
                        }
                        bombs_near = 0;
                        foreach (var item in iters)
                        {
                            if (field[item].val == -1)
                            {
                                bombs_near++;
                            }
                        }
                        field[i].val = bombs_near;
                        iters.Clear();
                        problem_index_down.Clear();
                        problem_index_left.Clear();
                        problem_index_right.Clear();
                        problem_index_up.Clear();
                    }
                }
             
        }      
    }
    public class cell
    {
       public int val;
       public bool close;
       public cell(int val, bool close) { this.val = val; this.close = close; }
        public override string ToString()
        {
            return "val=" + this.val.ToString() +" close="+ this.close.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"input.txt");
            IList<Turn> result = new List<Turn>();
            foreach (var line in lines)
            {
                Turn t = new Turn();
                var data = line.Split('-');
                var timespan_start = data[0].Split(':');
                var timespan_end = data[1].Split(':');
                t.Start = new TimeSpan(int.Parse(timespan_start[0]), int.Parse(timespan_start[1]), int.Parse(timespan_start[2]),0);
                t.End = new TimeSpan(int.Parse(timespan_end[0]), int.Parse(timespan_end[1]), int.Parse(timespan_end[2]), 0);
                t.minute = t.End.Subtract(t.Start).Duration().TotalMinutes;
                result.Add(t);
            }

            //Turn t1 = new Turn();
            //t1.Start = new TimeSpan(0, 07, 50, 00);
            //t1.End = new TimeSpan(0, 10, 00, 00);
            //t1.minute = t1.End.Subtract(t1.Start).Duration().TotalMinutes;
            //result.Add(t1);
            //Turn t2 = new Turn();
            //t2.Start = new TimeSpan(0, 10, 10, 00);
            //t2.End = new TimeSpan(0, 12, 00, 00);
            //t2.minute = t2.Start.Subtract(t2.End).Duration().TotalMinutes;
            //result.Add(t2);

            //Turn t3 = new Turn();
            //t3.Start = new TimeSpan(0, 12, 50, 00);
            //t3.End = new TimeSpan(0, 15, 00, 00);
            //t3.minute = t3.Start.Subtract(t3.End).Duration().TotalMinutes;
            //result.Add(t3);

            //Turn t4 = new Turn();
            //t4.Start = new TimeSpan(0, 15, 10, 00);
            //t4.End = new TimeSpan(0, 17, 00, 00);
            //t4.minute = t4.Start.Subtract(t4.End).Duration().TotalMinutes;
            //result.Add(t4);

            //Turn t5 = new Turn();
            //t5.Start = new TimeSpan(0, 17, 30, 00);
            //t5.End = new TimeSpan(0, 20, 30, 00);
            //t5.minute = t5.Start.Subtract(t5.End).Duration().TotalMinutes;
            //result.Add(t5);

            var tttttt = (new SQLCreate()).GetResult(result);
            System.IO.File.WriteAllText(@"output.txt", tttttt,Encoding.UTF8);
        }
    }
}

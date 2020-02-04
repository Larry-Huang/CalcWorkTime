using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Turn
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public double minute { get; set; }

    }
    public class SQLCreate
    {
        public string GetResult(IList<Turn> result)
        {
            StringBuilder resultSQL = new StringBuilder();

            var totalwoTime = (int)result.Sum(s => s.minute);
            StringBuilder sbStart = new StringBuilder();
            sbStart.Append("(CASE ");
            foreach (var t in result)
            {
                totalwoTime = totalwoTime - (int)t.minute;
                string _start = t.Start.Hours.ToString().PadLeft(2, '0') + ":" + t.Start.Minutes.ToString().PadLeft(2, '0') + ":00";
                string _end = t.End.Hours.ToString().PadLeft(2, '0') + ":" + t.End.Minutes.ToString().PadLeft(2, '0') + ":00";
                var _firstStart = result.First().Start.Days;
                if (result.First().Start.Hours.ToString().PadLeft(2, '0') + ":" + result.First().Start.Minutes.ToString().PadLeft(2, '0') == "00:00")
                {
                    _firstStart = _firstStart - 1;
                }
                if (t.End.Days == _firstStart)
                {
                    sbStart.Append(FormatStart(_start, _end, totalwoTime, 0));
                }
                else if (t.End.Days != _firstStart)
                {
                    sbStart.Append(FormatStart(_start, _end, totalwoTime, (t.End.Days - _firstStart)));
                }

            }
            sbStart.Append(string.Format("ELSE {0} END)", totalwoTime));
            //var sssss = sbStart.ToString();
            resultSQL.Append(sbStart);
            //string sqlformattotal = string.Format(@"+ datediff(day,convert(date,DATEADD(day, 1, lotx.logon_date_min),112),convert(date, lotx.logoff_date_min, 112)) * {0} +", (int)result.Sum(s => s.minute));
            string sqlformattotal = string.Format(@"+ -1 * {0} +", (int)result.Sum(s => s.minute));
            resultSQL.Append(sqlformattotal);

            StringBuilder sbEnd = new StringBuilder();
            //var reverse = result.Reverse();
            var sumtotalwoTime = 0;
            sbEnd.Append("(CASE ");
            foreach (var t in result)
            {
                string _start = t.Start.Hours.ToString().PadLeft(2, '0') + ":" + t.Start.Minutes.ToString().PadLeft(2, '0') + ":00";
                string _end = t.End.Hours.ToString().PadLeft(2, '0') + ":" + t.End.Minutes.ToString().PadLeft(2, '0') + ":00";
                var _firstStart = result.First().Start.Days;
                if (result.First().Start.Hours.ToString().PadLeft(2, '0') + ":" + result.First().Start.Minutes.ToString().PadLeft(2, '0') == "00:00")
                {
                    _firstStart = _firstStart - 1;
                }
                if (t.End.Days == _firstStart)
                {
                    sbEnd.Append(FormatEnd(_start, _end, sumtotalwoTime, 0));
                }
                else if (t.End.Days != _firstStart)
                {
                    sbEnd.Append(FormatEnd(_start, _end, sumtotalwoTime, (t.End.Days - _firstStart)));
                }
                sumtotalwoTime = sumtotalwoTime + (int)t.minute;
            }
            sbEnd.Append(string.Format("ELSE {0} END)", (int)result.Sum(s => s.minute)));
            resultSQL.Append(sbEnd);
            return resultSQL.ToString();
        }
        private string FormatStart(string startTime, string endTime, int lifttotal, int addDay = 0)
        {
            var _s = int.Parse(startTime.Split(':')[0]);
            var _e = int.Parse(endTime.Split(':')[0]);
            string result = string.Empty;
            if (addDay != 0 && _e > _s)
            {
                //起始及結束都跨日 
                result = string.Format(@" WHEN (convert(varchar(10), logon_date_min, 126)+' '+convert(char(8), lotx.logon_date_min, 108)) <  (convert(varchar(10), DATEADD(DAY,{3},shift_date), 126)+ ' {1}') THEN {2} + datediff(MINUTE,(SELECT MAX(c) FROM (VALUES(logon_date_min),(CONVERT(datetime,CONVERT(char(10),DATEADD(DAY,{3},shift_date),20) + ' {0}',120))) T(c)),CONVERT(datetime,CONVERT(char(10), DATEADD(DAY,{3},shift_date), 20) +' {1}', 120)) "
                    ,startTime, endTime, lifttotal, addDay);
            }
            else if (addDay != 0 && _s > _e)
            {
                ////起始沒跨日 結束跨日
                result = string.Format(@" WHEN (convert(varchar(10), logon_date_min, 126)+' '+convert(char(8), lotx.logon_date_min, 108)) <  (convert(varchar(10), DATEADD(DAY,{3},shift_date), 126)+ ' {1}') THEN {2} + datediff(MINUTE,(SELECT MAX(c) FROM (VALUES(logon_date_min),(CONVERT(datetime,CONVERT(char(10),shift_date,20) + ' {0}',120))) T(c)),CONVERT(datetime,CONVERT(char(10), DATEADD(DAY,{3},shift_date), 20) +' {1}', 120)) "
                    , startTime, endTime, lifttotal, addDay);
            }
            else
            {
                //todo: 沒跨日
                result = string.Format(@" WHEN (convert(varchar(10), logon_date_min, 126)+' '+convert(char(8), lotx.logon_date_min, 108)) <  (convert(varchar(10), shift_date, 126)+ ' {1}') THEN {2} + datediff(MINUTE,(SELECT MAX(c) FROM (VALUES(logon_date_min),(CONVERT(datetime,CONVERT(char(10),shift_date,20) + ' {0}',120))) T(c)),CONVERT(datetime,CONVERT(char(10),shift_date, 20) +' {1}', 120)) "
                    , startTime, endTime, lifttotal);

            }
            return result;
        }



        private string FormatEnd(string startTime, string endTime, int lifttotal, int addDay = 0)
        {
            string result = string.Empty;
            var _s = int.Parse(startTime.Split(':')[0]);
            var _e = int.Parse(endTime.Split(':')[0]);
            if (addDay != 0 && _e > _s)
            {
            //起始及結束都跨日 
                result = string.Format(@" WHEN (convert(varchar(10), logoff_date, 126)+' '+convert(char(8), lotx.logoff_date_min, 108)) < (convert(varchar(10), DATEADD(DAY,{3},shift_date), 126)+ ' {1}') THEN {2} +datediff(MINUTE,CONVERT(datetime,CONVERT(char(10),DATEADD(DAY,{3},shift_date), 20) +' {0}',120),(SELECT MAX(c) FROM (VALUES(lotx.logoff_date_min),(CONVERT(datetime,CONVERT(char(10),DATEADD(DAY,{3},shift_date),20) + ' {0}',120))) T(c))) "
                    , startTime, endTime, lifttotal, addDay);

            }
            //起始沒跨日 結束跨日
            else if (addDay != 0 && _s > _e)
            {
                result = string.Format(@" WHEN (convert(varchar(10), logoff_date, 126)+' '+convert(char(8), lotx.logoff_date_min, 108)) < (convert(varchar(10), DATEADD(DAY,{3},shift_date), 126)+ ' {1}') THEN {2} +datediff(MINUTE,CONVERT(datetime,CONVERT(char(10),shift_date, 20) +' {0}',120),(SELECT MAX(c) FROM (VALUES(lotx.logoff_date_min),(CONVERT(datetime,CONVERT(char(10),shift_date,20) + ' {0}',120))) T(c))) "
                    , startTime, endTime, lifttotal, addDay);
            }
            else
            {
                //都沒跨日
                result = string.Format(@" WHEN (convert(varchar(10), logoff_date, 126)+' '+convert(char(8), lotx.logoff_date_min, 108)) < (convert(varchar(10), shift_date, 126)+ ' {1}') THEN {2} +datediff(MINUTE,CONVERT(datetime,CONVERT(char(10), shift_date, 20) +' {0}',120),(SELECT MAX(c) FROM (VALUES(lotx.logoff_date_min),(CONVERT(datetime,CONVERT(char(10),shift_date,20) + ' {0}',120))) T(c))) "
                    , startTime, endTime, lifttotal, addDay);
               
            }
            return result;
        }

    }

}

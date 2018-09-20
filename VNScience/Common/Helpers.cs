using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace VNScience.Common
{
    public class Flash
    {
        public string Title;
        public string Message;
        public MessageType Type;
    }

    public class Notification
    {
        public void Success(string message, HttpSessionStateBase Session, string title = "Thông báo")
        {
            if (Session["Flashes"] == null)
            {
                Session["Flashes"] = new List<Flash>()
                {
                    new Flash()
                    {
                        Message = message,
                        Type = MessageType.Success,
                        Title = title
                    }
                };
                return;
            }

            var flashes = (List<Flash>)Session["Flashes"];
            flashes.Add(new Flash()
            {
                Message = message,
                Type = MessageType.Success,
                Title = title
            });

            Session["Flashes"] = flashes;
        }

        public void Error(string message, HttpSessionStateBase Session, string title = "Thông báo")
        {
            if (Session["Flashes"] == null)
            {
                Session["Flashes"] = new List<Flash>()
                {
                    new Flash()
                    {
                        Message = message,
                        Type = MessageType.Error,
                        Title = title
                    }
                };
                return;
            }

            var flashes = (List<Flash>)Session["Flashes"];
            flashes.Add(new Flash()
            {
                Message = message,
                Type = MessageType.Error,
                Title = title
            });

            Session["Flashes"] = flashes;
        }

        public void Warning(string message, HttpSessionStateBase Session, string title = "Thông báo")
        {
            if (Session["Flashes"] == null)
            {
                Session["Flashes"] = new List<Flash>()
                {
                    new Flash()
                    {
                        Message = message,
                        Type = MessageType.Warning,
                        Title = title
                    }
                };
                return;
            }

            var flashes = (List<Flash>)Session["Flashes"];
            flashes.Add(new Flash()
            {
                Message = message,
                Type = MessageType.Warning,
                Title = title
            });

            Session["Flashes"] = flashes;
        }
        public void Info(string message, HttpSessionStateBase Session, string title = "Thông báo")
        {
            if (Session["Flashes"] == null)
            {
                Session["Flashes"] = new List<Flash>()
                {
                    new Flash()
                    {
                        Message = message,
                        Type = MessageType.Info,
                        Title = title
                    }
                };
                return;
            }

            var flashes = (List<Flash>)Session["Flashes"];
            flashes.Add(new Flash()
            {
                Message = message,
                Type = MessageType.Info,
                Title = title
            });

            Session["Flashes"] = flashes;
        }
    }

    public class StringHelper
    {
        public static string ToUnsignString(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            input = input.Replace(".", "-");
            input = input.Replace(" ", "-");
            input = input.Replace(",", "-");
            input = input.Replace(";", "-");
            input = input.Replace(":", "-");
            input = input.Replace("  ", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            while (str2.Contains("--"))
            {
                str2 = str2.Replace("--", "-").ToLower();
            }
            return str2;
        }

        public static string FilterWhiteSpaces(string input)
        {
            if (input == null)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (i == 0 || c != ' ' || (c == ' ' && input[i - 1] != ' '))
                    stringBuilder.Append(c);
            }
            return stringBuilder.ToString();
        }
    }

    public class DateTimeHelper
    {
        public const int MaxDay = 10;
        public static string FormatDate(DateTime dateTime, int maxDay = DateTimeHelper.MaxDay)
        {
            string suffix = "trước";
            string second = "giây";
            string minute = "phút";
            string hour = "giờ";
            string day = "ngày";
            string justNow = "Vừa xong";

            var interval = DateTime.Now - dateTime;

            if (Math.Floor(interval.TotalSeconds) == 0)
                return justNow;

            if (interval.TotalDays < 1)
            {
                //if less than a minute, display second
                if (interval.TotalMinutes < 1)
                {
                    return string.Join(" ", Math.Floor(interval.TotalSeconds).ToString(), second, suffix);
                }
                else if (interval.TotalMinutes < 60)
                {
                    return string.Join(" ", Math.Floor(interval.TotalMinutes), minute, suffix);
                }
                else
                {
                    return string.Join(" ", Math.Floor(interval.TotalHours), hour, suffix);
                }
            }
            else if (interval.TotalDays < maxDay)
            {
                return string.Join(" ", Math.Floor(interval.TotalDays), day, suffix);
            }
            else
            {
                return dateTime.ToString("dd/MM/yyyy HH:mm:ss a");
            }
        }
    }

    public class MySelectListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class MySelectList
    {
        public MySelectList()
        {
            Items = new List<MySelectListItem>();
            SelectedItems = new List<string>();
            FormElementName = "";
        }
        public List<MySelectListItem> Items { get; set; }
        public List<string> SelectedItems { get; set; }
        public string FormElementName { get; set; }
    }

    public enum DeleteType
    {
        WaitingForAccept = 1,
        Destroy = 2
    }

    public enum SearchMatchingType
    {
        FullyMatchTitle = 1,
        FullyMatchOther = 2,
        FullyMatchTitleButScrambled = 3,
        FullyMatchOtherButScrambled = 4,
        PartialMatch = 5
    }

    public enum MessageType
    {
        Success,
        Error,
        Warning,
        Info
    }

    public enum AdPosition
    {
        TopCenter = 1,
        TopLeft = 2,
        BottomLeft = 3
    }
}
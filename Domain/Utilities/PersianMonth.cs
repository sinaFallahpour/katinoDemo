using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Utilities
{
  public static  class PersianMonth
    {
	public	static string GetMonthName(this string number)
		{
			switch (number)
			{
				case "01":
					return "فروردین";
				case "02":
					return "اردیبهشت";
				case "03":
					return "خرداد";
				case "04":
					return "تیر";
				case "05":
					return "مرداد";
				case "06":
					return "شهریور";
				case "07":
					return "مهر";
				case "08":
					return "آبان";
				case "09":
					return "آذر";
				case "10":
					return "دی";
				case "11":
					return "بهمن";
				case "12":
					return "اسغند";
				default:
					return "";
			}
		}
	}
}

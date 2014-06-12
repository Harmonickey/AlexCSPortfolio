using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Problem19
{
    class Program
    {
        /*
            You are given the following information, but you may prefer to do some research for yourself.

                1 Jan 1900 was a Monday.
                Thirty days has September,
                April, June and November.
                All the rest have thirty-one,
                Saving February alone,
                Which has twenty-eight, rain or shine.
                And on leap years, twenty-nine.
                A leap year occurs on any year evenly divisible by 4, but not on a century unless it is divisible by 400.
            How many Sundays fell on the first of the month during the twentieth century (1 Jan 1901 to 31 Dec 2000)?
        */
        static void Main(string[] args)
        {

            int dayOfWeek = 1; //0 Sunday, 1 Monday, 2 Tuesday, 3 Wednesday, 4 Thursday, 5 Friday, 6 Saturday
            int howManySundays = 0;

            int day;
            int month;
            int year;
            int dayEnd;

            for (year = 1900; year < 2001; year++)
            {
                for (month = 1; month < 13; month++)
                {
                    //account for leap years and different months
                    if (month == 2 && isLeapYear(year)) 
                        dayEnd = 30;
                    else if (month == 2) 
                        dayEnd = 29;
                    else if (month == 4 || month == 6 || month == 9 || month == 11) 
                        dayEnd = 31;
                    else 
                        dayEnd = 32;

                    for (day = 1; day < dayEnd; day++)
                    {
                        //increment on day counting
                        dayOfWeek = (dayOfWeek + 1) % 7;

                        //only count how many sundays fall on the first day
                        if (dayOfWeek == 0 && day == 1 && year > 1900)
                        {
                            howManySundays++;
                        }
                    }
                }
            }

            Console.WriteLine(howManySundays);
            Console.ReadLine();
        }

        static public bool isLeapYear(int year)
        {
            if (year % 4 == 0) //if divisible by 4 then leap year!
            {
                if (year % 100 == 0) //but if divisible by 100, not a leap year, don't go to 29!
                {
                    if (year % 400 == 0) //however, if it is divisible by 400 then it's a leap year, go to 29!
                        return true;
                    else
                        return false; //it was divisible by 4 and 100 but not 400
                }
                else
                {
                    return true;  //it was divisible by 4 but not 100
                }
            }
            else
            {
                return false;  //it wasn't divisible by 4
            }
        }
    }
}

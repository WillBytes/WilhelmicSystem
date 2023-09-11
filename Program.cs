using System;

namespace WilhelmicSystem
{
    class Program
    {
        static byte[] monthDays = { 1, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28, 28 };

        static byte ntpHour, ntpMinute, ntpSecond, ntpDate, ntpMonth, leapDays, leapYearInd;
        static ushort tempDays;
        static uint epoch, ntpYear, daysSinceEpoch, dayOfYear, MHour, MMinute, MSecond, BSecondH, BSecondM, BSecond, BSecondS, MCSecond;

        static void CalculateImperialToMetricTime()
    {
        BSecondH = (uint)(ntpHour * 3600);
        BSecondM = (uint)(ntpMinute * 60);
        BSecondS = (uint)ntpSecond;
        BSecond = (uint)(BSecondS += BSecondM += BSecondH);
        MCSecond = (uint)(BSecond / 0.864);
        MSecond = (uint)(MCSecond % 100);
        MCSecond /= 100;
        MMinute = (uint)(MCSecond % 100);
        MCSecond /= 100;
        MHour = (uint)(MCSecond % 10);
        MCSecond /= 10;
    }


        static void PrintWeekday()
        {
            int weekday = (int)dayOfYear;
            if (leapYearInd == 1)
            {
                switch (weekday)
                {
                    case 1: Console.Write("\nNew Years"); break;
                    case 2: Console.Write("\nNullus Two"); break;
                    default:
                        weekday -= 2;
                        weekday %= 7;
                        Console.Write("\n");

                        switch (weekday)
                        {
                            case 1: Console.Write("Monday"); break;
                            case 2: Console.Write("Tuesday"); break;
                            case 3: Console.Write("Wednesday"); break;
                            case 4: Console.Write("Thursday"); break;
                            case 5: Console.Write("Friday"); break;
                            case 6: Console.Write("Saturday"); break;
                            case 7: Console.Write("Sunday"); break;
                        }
                        break;
                }
            }
            else
            {
                switch (weekday)
                {
                    case 1: Console.Write("\nNew Years"); break;
                    default:
                        weekday--;
                        weekday %= 7;
                        Console.Write("\n");

                        switch (weekday)
                        {
                            case 1: Console.Write("Monday"); break;
                            case 2: Console.Write("Tuesday"); break;
                            case 3: Console.Write("Wednesday"); break;
                            case 4: Console.Write("Thursday"); break;
                            case 5: Console.Write("Friday"); break;
                            case 6: Console.Write("Saturday"); break;
                            case 7: Console.Write("Sunday"); break;
                        }
                        break;
                }
            }
        }

        static void PrintMonth()
        {
            switch (ntpMonth)
            {
                case 1: Console.Write("Nullus"); break;
                case 2: Console.Write("Lupus"); break;
                case 3: Console.Write("Nix"); break;
                case 4: Console.Write("Mors"); break;
                case 5: Console.Write("Roseus"); break;
                case 6: Console.Write("Flos"); break;
                case 7: Console.Write("Fragium"); break;
                case 8: Console.Write("Tonitruum"); break;
                case 9: Console.Write("Rubrum"); break;
                case 10: Console.Write("Messis"); break;
                case 11: Console.Write("Venator"); break;
                case 12: Console.Write("Gelus"); break;
                case 13: Console.Write("Frigus"); break;
                case 14: Console.Write("Quercus"); break;
                default: break;
            }
        }

        static void ProcessEpoch()
        {
            DateTime currentDateTime = DateTime.UtcNow.AddHours(1); // UTC +1:00
             double epochSeconds = currentDateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            epoch = (uint)epochSeconds;

            ntpSecond = (byte)(epoch % 60);
            epoch /= 60;
            ntpMinute = (byte)(epoch % 60);
            epoch /= 60;
            ntpHour = (byte)(epoch % 24);
            epoch /= 24;

            daysSinceEpoch = epoch;
            ntpYear = 1970 + (daysSinceEpoch / 365);

            leapDays = 0;
            for (int i = 1972; i < ntpYear; i += 4)
            {
                if (((i % 4 == 0) && (i % 100 != 0)) || (i % 400 == 0))
                {
                    leapDays++;
                }
            }

            ntpYear = 1970 + ((daysSinceEpoch - leapDays) / 365);
            dayOfYear = ((daysSinceEpoch - leapDays) % 365) + 1;

            if (((ntpYear % 4 == 0) && (ntpYear % 100 != 0)) || (ntpYear % 400 == 0))
            {
                monthDays[0] = 2;
                leapYearInd = 1;
            }
            else
            {
                monthDays[0] = 1;
                leapYearInd = 0;
            }

            tempDays = 0;
            for (ntpMonth = 0; ntpMonth <= 13; ntpMonth++)
            {
                if (dayOfYear <= tempDays)
                {
                    break;
                }
                tempDays = (ushort)(tempDays + monthDays[ntpMonth]);
            }

            tempDays = (ushort)(tempDays - monthDays[ntpMonth - 1]);
            ntpDate = (byte)(dayOfYear - tempDays);
        }

        static void Main()
        {
            for (;;)
            {
                System.Threading.Thread.Sleep(1000);
                Console.Clear();
                ProcessEpoch();
                CalculateImperialToMetricTime();
                PrintWeekday();
                Console.Write(", ");
                PrintMonth();
                Console.WriteLine($" {ntpDate}, {ntpYear} ");
                Console.WriteLine($"Imperial Time = {ntpHour} : {ntpMinute} : {ntpSecond}");
                Console.WriteLine($"Metric Time = {MHour} : {MMinute} : {MSecond}");
                Console.WriteLine($"Days since Epoch: {daysSinceEpoch}");
                Console.WriteLine($"Day of year = {dayOfYear}");
                Console.WriteLine($"Is Year Leap? {leapYearInd}");
            }
        }
    }
}
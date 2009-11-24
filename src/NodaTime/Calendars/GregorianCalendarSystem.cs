#region Copyright and license information
// Copyright 2001-2009 Stephen Colebourne
// Copyright 2009 Jon Skeet
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;

namespace NodaTime.Calendars
{
    /// <summary>
    /// Original name: GregorianChronology
    /// </summary>
    public class GregorianCalendarSystem : BasicGJCalendarSystem
    {
        private const int DaysFrom0000To1970 = 719527;
        private const long AverageTicksPerGregorianYear = (long) (365.2425m * DateTimeConstants.TicksPerDay);

        private GregorianCalendarSystem(int minDaysInFirstWeek) : base(null, minDaysInFirstWeek)
        {
        }

        protected override void AssembleFields(NodaTime.Fields.FieldSet.Builder builder)
        {
            throw new NotImplementedException();
        }

        public static Chronology GetInstance(IDateTimeZone dateTimeZone)
        {
            throw new NotImplementedException();
        }

        protected override LocalInstant CalculateStartOfYear(int year)
        {
            // Initial value is just temporary.
            int leapYears = year / 100;
            if (year < 0)
            {
                // Add 3 before shifting right since /4 and >>2 behave differently
                // on negative numbers. When the expression is written as
                // (year / 4) - (year / 100) + (year / 400),
                // it works for both positive and negative values, except this optimization
                // eliminates two divisions.
                leapYears = ((year + 3) >> 2) - leapYears + ((leapYears + 3) >> 2) - 1;
            }
            else
            {
                leapYears = (year >> 2) - leapYears + (leapYears >> 2);
                if (IsLeapYear(year))
                {
                    leapYears--;
                }
            }

            return new LocalInstant((year * 365L + (leapYears - DaysFrom0000To1970)) * DateTimeConstants.TicksPerDay);
        }

        protected override bool IsLeapYear(int year)
        {
            return ((year & 3) == 0) && ((year % 100) != 0 || (year % 400) == 0);
        }

        public override long AverageTicksPerYear { get { return AverageTicksPerGregorianYear; } }
        public override long AverageTicksPerYearDividedByTwo { get { return AverageTicksPerGregorianYear / 2; } }
    }
}
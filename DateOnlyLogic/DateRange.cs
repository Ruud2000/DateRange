using System.Collections;

namespace DateOnlyLogic
{
    public readonly record struct DateRange : IEnumerable<DateOnly>
    {
        public DateRange(DateOnly from, DateOnly to)
        {
            From = from;
            To = to;
        }

        public DateOnly From { get; init; }

        public DateOnly To { get; init; }

        /// <summary>
        /// Check if this <see cref="DateRange"/> contains the given <see cref="DateOnly"/>.
        /// </summary>
        /// <param name="dateOnly">The <see cref="DateOnly"/> to check.</param>
        /// <returns><c>True</c> when this <see cref="DateRange"/> contains the given <see cref="DateOnly"/>, else <c>false</c>.</returns>
        public bool Contains(DateOnly dateOnly) 
        {
            return dateOnly >= From && dateOnly <= To;
        }

        /// <summary>
        /// Check if this <see cref="DateRange"/> overlaps the given <see cref="DateRange"/>.
        /// </summary>
        /// <param name="dateRange">The <see cref="DateRange"/> to check.</param>
        /// <returns><c>True</c> when this <see cref="DateRange"/> has at least a single day that overlaps with the given <see cref="DateRange"/>, else <c>false</c>.</returns>
        public bool Overlaps(DateRange dateRange)
        {
            return dateRange.From <= To && dateRange.To >= From;
        }

        /// <summary>
        /// Enumerate the dates within this date range.
        /// </summary>
        /// <returns>An enumerable of <see cref="DateOnly"/> representing the days in this <see cref="DateRange"/>.</returns>
        public IEnumerator<DateOnly> GetEnumerator()
        {
            return DaysInThisDateRange().GetEnumerator();
        }

        /// <summary>
        /// Returns the intersect of this <see cref="DateRange"/> and the given <see cref="DateRange"/>.
        /// </summary>
        /// <param name="dateRange">The <see cref="DateRange"/> to intersect.</param>
        /// <returns>A new <see cref="DateRange"/> with the intersect if possible.</returns>
        public DateRange? Intersect(DateRange dateRange)
        {
            if (dateRange.Overlaps(this))
            {
                var from = dateRange.From > From ? dateRange.From : From;
                var to = dateRange.To < To ? dateRange.To : To;
                return new DateRange(from, to);
            }

            return null;
        }

        /// <summary>
        /// Check if the given <see cref="DateRange"/> is a superset of this <see cref="DateRange"/>.
        /// </summary>
        /// <param name="dateRange">The <see cref="DateRange"/> to check.</param>
        /// <returns><c>True</c> when the given <see cref="DateRange"/> is a superset, else <c>false</c>.</returns>
        public bool IsSupersetOf(DateRange dateRange)
        {
            return From <= dateRange.From && To >= dateRange.To;
        }

        /// <summary>
        /// Subtract the given <see cref="DateRange"/> from the current <see cref="DateRange"/>.
        /// </summary>
        /// <param name="dateRangeToSubtract">The <see cref="DateRange"/> to subtract.</param>
        /// <returns>
        /// Returns an enumerable of <see cref="DateRange"/> resulting from the subtraction.
        /// The outcome can result in an empty enumerable of the given range overlaps the current <see cref="DateRange"/> completely.
        /// The outcome results in an enumerable with a single <see cref="DateRange"/> when the given range does not overlap, or overlaps partially at the beginning or end.
        /// The outcome results in an enumerable with two <see cref="DateRange"/> when the given range overlaps partially in the middle of the current <see cref="DateRange"/>.
        /// </returns>
        public IEnumerable<DateRange> Subtract(DateRange dateRangeToSubtract)
        {
            if (!dateRangeToSubtract.Overlaps(this))
            {
                return new[] { this };
            }

            if (dateRangeToSubtract.IsSupersetOf(this))
            {
                return Enumerable.Empty<DateRange>();
            }

            if (dateRangeToSubtract.From <= From)
            {
                return new[] { new DateRange(dateRangeToSubtract.To == DateOnly.MaxValue ? DateOnly.MaxValue : dateRangeToSubtract.To.AddDays(1), To) };
            }

            if (dateRangeToSubtract.To >= To)
            {
                return new[] { new DateRange(From, dateRangeToSubtract.From == DateOnly.MinValue ? DateOnly.MinValue : dateRangeToSubtract.From.AddDays(-1)) };
            }

            return new[]
            {
                new DateRange(From, dateRangeToSubtract.From.AddDays(-1)),
                new DateRange(dateRangeToSubtract.To.AddDays(1), To),
            };
        }

        /// <summary>
        /// Returns intersecting <see cref="DateRange"/> instances based on the given collections.
        /// </summary>
        /// <param name="left">The left collection to intersect with the right collection.</param>
        /// <param name="right">The right collection to intersect with the left collection.</param>
        /// <returns>A collection of intersected <see cref="DateRange"/> instances, if any.</returns>
        public static IEnumerable<DateRange> IntersectOverlappingDatesInRanges(IEnumerable<DateRange> left, IEnumerable<DateRange> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            foreach (var currentDateRange in left)
            {
                var overlapping = right.Where(x => x.Overlaps(currentDateRange));

                if (overlapping.Any())
                {
                    DateRange? intersectedDateRange = currentDateRange;

                    foreach (var overlappingDateRange in overlapping)
                    {
                        yield return overlappingDateRange.Intersect(currentDateRange)!.Value;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerable<DateOnly> DaysInThisDateRange()
        {
            var current = From;

            while (current < To)
            {
                yield return current;

                current = current.AddDays(1);
            }

            yield return current;
        }
    }
}
# DateRange

This repository holds a struct that is used to model a date range with a from and to value using the DateOnly type introduced in .NET 6.

It includes the following methods to easily work with two date ranges:
- Overlaps
- Intersect
- IsSupersetOf
- Subtract

Next it includes a static method IntersectOverlappingDatesInRanges which can be used to return all overlapping date ranges based on two given collections of date ranges.

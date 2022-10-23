using Xunit;

namespace DateOnlyLogic.Tests
{
    public class DateRangeTests
    {
        public class The_Contains_Method : DateRangeTests
        {
            [Theory]
            [InlineData(2022, 9, 9, false)]
            [InlineData(2022, 9, 10, true)]
            [InlineData(2022, 9, 11, true)]
            [InlineData(2022, 9, 12, true)]
            [InlineData(2022, 9, 13, false)]
            public void Should_Return_Expected_For_Given_DateOnly(int year, int month, int day, bool expectedResult)
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);
                
                var sut = new DateRange(from, to);

                // Act
                var result = sut.Contains(new DateOnly(year, month, day));

                // Assert
                Assert.Equal(expectedResult, result);
            }
        }

        public class The_Overlaps_Method : DateRangeTests
        {
            [Fact]
            public void Should_Return_True_When_Given_DateRange_Overlaps_Only_On_From_Date()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = sut.Overlaps(new DateRange(from, from));

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_True_When_Given_DateRange_Overlaps_Only_On_To_Date()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = sut.Overlaps(new DateRange(to, to));

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_True_When_Given_DateRange_Overlaps_Partially_At_Beginning()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = sut.Overlaps(new DateRange(new DateOnly(2022, 9, 1), new DateOnly(2022, 9, 11)));

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_True_When_Given_DateRange_Overlaps_Partially_At_End()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = sut.Overlaps(new DateRange(new DateOnly(2022, 9, 11), new DateOnly(2022, 9, 30)));

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_True_When_Given_DateRange_Overlaps_Exactly()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = sut.Overlaps(new DateRange(from, to));

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_True_When_Given_DateRange_Starts_Days_Before_From_And_Ends_Days_After_To()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = sut.Overlaps(new DateRange(new DateOnly(2022, 9, 1), new DateOnly(2022, 9, 30)));

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_When_Given_DateRange_Ends_Day_Before_From()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = sut.Overlaps(new DateRange(new DateOnly(2022, 9, 1), new DateOnly(2022, 9, 9)));

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_When_Given_DateRange_Ends_Many_Days_Before_From()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = sut.Overlaps(new DateRange(new DateOnly(1900, 1, 1), new DateOnly(2000, 1, 1)));

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_When_Given_DateRange_Start_Day_After_To()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = sut.Overlaps(new DateRange(new DateOnly(2022, 9, 13), new DateOnly(2022, 9, 30)));

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_When_Given_DateRange_Start_Many_Days_After_To()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = sut.Overlaps(new DateRange(new DateOnly(2077, 1, 1), new DateOnly(3000, 1, 1)));

                // Assert
                Assert.False(result);
            }
        }

        public class The_GetEnumerator_Method : DateRangeTests
        {
            [Fact]
            public void Should_Return_Expected_Days_In_Range_From_And_To_MinValue()
            {
                // Arrange
                var from = DateOnly.MinValue;
                var to = DateOnly.MinValue;

                var sut = new DateRange(from, to);

                var expectedResult = new List<DateOnly>
                {
                    DateOnly.MinValue,
                };

                // Act & Assert
                Assert.Equal(expectedResult, sut);
            }

            [Fact]
            public void Should_Return_Expected_Days_In_Range_From_And_To_MaxValue()
            {
                // Arrange
                var from = DateOnly.MaxValue;
                var to = DateOnly.MaxValue;

                var sut = new DateRange(from, to);

                var expectedResult = new List<DateOnly>
                {
                    DateOnly.MaxValue,
                };

                // Act & Assert
                Assert.Equal(expectedResult, sut);
            }

            [Fact]
            public void Should_Return_Expected_Days_In_Range_Single_Day()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 10);

                var sut = new DateRange(from, to);

                var expectedResult = new List<DateOnly>
                {
                    new DateOnly(2022, 9, 10),
                };

                // Act & Assert
                Assert.Equal(expectedResult, sut);
            }

            [Fact]
            public void Should_Return_Expected_Days_In_Range_Multiple_Days()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new List<DateOnly>
                {
                    new DateOnly(2022, 9, 10),
                    new DateOnly(2022, 9, 11),
                    new DateOnly(2022, 9, 12),
                };

                // Act & Assert
                Assert.Equal(expectedResult, sut);
            }
        }

        public class The_Intersect_Method
        {
            [Fact]
            public void Should_Return_Null_When_Given_DateRange_Ends_Before_From()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = sut.Intersect(new DateRange(new DateOnly(2022, 9, 1), new DateOnly(2022, 9, 9)));

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public void Should_Return_Null_When_Given_DateRange_Starts_After_To()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = sut.Intersect(new DateRange(new DateOnly(2022, 9, 13), new DateOnly(2022, 9, 30)));

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public void Should_Return_Expected_DateRange_When_Given_DateRange_Spans_Multiple_Days_And_Intersects_Only_From_Date()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new DateRange(from, from);

                // Act
                var result = sut.Intersect(new DateRange(new DateOnly(2022, 9, 1), new DateOnly(2022, 9, 10)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Expected_DateRange_When_Given_DateRange_Spans_Multiple_Days_And_Intersects_Only_To_Date()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new DateRange(to, to);

                // Act
                var result = sut.Intersect(new DateRange(new DateOnly(2022, 9, 12), new DateOnly(2022, 9, 30)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Expected_DateRange_When_Given_DateRange_Spans_Single_Day_And_Intersects_Only_From_Date()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new DateRange(from, from);

                // Act
                var result = sut.Intersect(new DateRange(new DateOnly(2022, 9, 10), new DateOnly(2022, 9, 10)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Expected_DateRange_When_Given_DateRange_Spans_Single_Day_And_Intersects_Only_To_Date()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new DateRange(to, to);

                // Act
                var result = sut.Intersect(new DateRange(new DateOnly(2022, 9, 12), new DateOnly(2022, 9, 12)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Expected_DateRange_When_Given_DateRange_Spans_Multiple_Days_And_Intersects_Multiple_Days_At_The_Beginning()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new DateRange(from, new DateOnly(2022, 9, 11));

                // Act
                var result = sut.Intersect(new DateRange(new DateOnly(2022, 9, 1), new DateOnly(2022, 9, 11)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Expected_DateRange_When_Given_DateRange_Spans_Multiple_Days_And_Intersects_Multiple_Days_At_The_End()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new DateRange(new DateOnly(2022, 9, 11), to);

                // Act
                var result = sut.Intersect(new DateRange(new DateOnly(2022, 9, 11), new DateOnly(2022, 9, 30)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Expected_DateRange_When_Given_DateRange_Spans_Multiple_Days_Within_The_Current_DateRange()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new DateRange(new DateOnly(2022, 9, 11), new DateOnly(2022, 9, 11));

                // Act
                var result = sut.Intersect(new DateRange(new DateOnly(2022, 9, 11), new DateOnly(2022, 9, 11)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Expected_DateRange_When_Given_DateRange_Spans_Exactly()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new DateRange(from, to);

                // Act
                var result = sut.Intersect(new DateRange(from, to));

                // Assert
                Assert.Equal(expectedResult, result);
            }
        }

        public class The_IsSupersetOf_Method
        {
            [Fact]
            public void Should_Return_True_When_Given_DateRange_Matches_Exactly_Current_DateRange()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = new DateRange(from, to).IsSupersetOf(sut);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_True_When_Given_DateRange_Begins_Before_Current_DateRange_And_Ends_On_Same_Date()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = new DateRange(new DateOnly(2022, 9, 1), to).IsSupersetOf(sut);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_True_When_Given_DateRange_Begins_At_Current_DateRange_And_Ends_After()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = new DateRange(from, new DateOnly(2022, 9, 15)).IsSupersetOf(sut);

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_When_Given_DateRange_Completely_Before_Current_DateRange()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = new DateRange(new DateOnly(2022, 9, 1), new DateOnly(2022, 9, 9)).IsSupersetOf(sut);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_When_Given_DateRange_Completely_After_Current_DateRange()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = new DateRange(new DateOnly(2022, 9, 13), new DateOnly(2022, 9, 15)).IsSupersetOf(sut);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_When_Given_DateRange_Only_Overlaps_From_Date()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = new DateRange(new DateOnly(2022, 9, 1), from).IsSupersetOf(sut);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_When_Given_DateRange_Partial_Overlaps_At_Beginning()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = new DateRange(new DateOnly(2022, 9, 1), new DateOnly(2022, 9, 11)).IsSupersetOf(sut);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_When_Given_DateRange_Only_Overlaps_To_Date()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = new DateRange(to, new DateOnly(2022, 9, 15)).IsSupersetOf(sut);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_When_Given_DateRange_Partial_Overlaps_At_End()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                // Act
                var result = new DateRange(new DateOnly(2022, 9, 11), new DateOnly(2022, 9, 15)).IsSupersetOf(sut);

                // Assert
                Assert.False(result);
            }
        }

        public class The_Subtract_Method
        {
            [Fact]
            public void Should_Return_Current_DateRange_When_DateRange_To_Subtract_Is_Many_Days_Before_Current_DateRange()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new List<DateRange> { sut };

                // Act
                var result = sut.Subtract(new DateRange(new DateOnly(1900, 1, 1), new DateOnly(2000, 1, 1)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Current_DateRange_When_DateRange_To_Subtract_Ends_Day_Before_Start_Current_DateRange()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new List<DateRange> { sut };

                // Act
                var result = sut.Subtract(new DateRange(new DateOnly(2022, 9, 1), new DateOnly(2022, 9, 9)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Current_DateRange_When_DateRange_To_Subtract_Is_Many_Days_After_Current_DateRange()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new List<DateRange> { sut };

                // Act
                var result = sut.Subtract(new DateRange(new DateOnly(2077, 1, 1), new DateOnly(3000, 1, 1)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Current_DateRange_When_DateRange_To_Subtract_Starts_Day_After_End_Current_DateRange()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new List<DateRange> { sut };

                // Act
                var result = sut.Subtract(new DateRange(new DateOnly(2022, 9, 13), new DateOnly(2022, 9, 30)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Empty_When_DateRange_To_Subtract_Is_Superset_Of_Current_DateRange_Exact_Match()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = Enumerable.Empty<DateRange>();

                // Act
                var result = sut.Subtract(new DateRange(from, to));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Empty_When_DateRange_To_Subtract_Is_Superset_Of_Current_DateRange_And_Begins_Before_Current_DateRange()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = Enumerable.Empty<DateRange>();

                // Act
                var result = sut.Subtract(new DateRange(new DateOnly(2022, 9, 1), to));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Empty_When_DateRange_To_Subtract_Is_Superset_Of_Current_DateRange_And_Ends_After_Current_DateRange()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = Enumerable.Empty<DateRange>();

                // Act
                var result = sut.Subtract(new DateRange(from, new DateOnly(2022, 9, 30)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Empty_When_DateRange_To_Subtract_Is_Superset_Of_Current_DateRange_And_Begins_Before_And_Ends_After_Current_DateRange()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = Enumerable.Empty<DateRange>();

                // Act
                var result = sut.Subtract(new DateRange(new DateOnly(2022, 9, 1), new DateOnly(2022, 9, 30)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Single_DateRange_When_Overlap_Single_Day_At_Beginning()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new []
                {
                    new DateRange(new DateOnly(2022, 9, 11), new DateOnly(2022, 9, 12)),
                };

                // Act
                var result = sut.Subtract(new DateRange(new DateOnly(2022, 9, 1), new DateOnly(2022, 9, 10)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Single_DateRange_When_Overlap_Multiple_Days_At_Beginning()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new[]
                {
                    new DateRange(new DateOnly(2022, 9, 12), new DateOnly(2022, 9, 12)),
                };

                // Act
                var result = sut.Subtract(new DateRange(new DateOnly(2022, 9, 1), new DateOnly(2022, 9, 11)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Single_DateRange_When_Overlap_Single_Day_At_End()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new[]
                {
                    new DateRange(new DateOnly(2022, 9, 10), new DateOnly(2022, 9, 11)),
                };

                // Act
                var result = sut.Subtract(new DateRange(new DateOnly(2022, 9, 12), new DateOnly(2022, 9, 30)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Single_DateRange_When_Overlap_Multiple_Days_At_End()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new[]
                {
                    new DateRange(new DateOnly(2022, 9, 10), new DateOnly(2022, 9, 10)),
                };

                // Act
                var result = sut.Subtract(new DateRange(new DateOnly(2022, 9, 11), new DateOnly(2022, 9, 30)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Single_DateRange_When_Overlap_Multiple_Days_At_End_And_From_Is_MinValue()
            {
                // Arrange
                var from = DateOnly.MinValue;
                var to = new DateOnly(2022, 9, 12);

                var sut = new DateRange(from, to);

                var expectedResult = new[]
                {
                    new DateRange(DateOnly.MinValue, new DateOnly(2022, 9, 10)),
                };

                // Act
                var result = sut.Subtract(new DateRange(new DateOnly(2022, 9, 11), new DateOnly(2022, 9, 30)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Expected_DateRanges_When_Overlap_Multiple_Days_At_End_And_To_Is_MaxValue()
            {
                // Arrange
                var from = new DateOnly(2022, 9, 10);
                var to = DateOnly.MaxValue;

                var sut = new DateRange(from, to);

                var expectedResult = new[]
                {
                    new DateRange(new DateOnly(2022, 9, 10), new DateOnly(2022, 9, 10)),
                    new DateRange(new DateOnly(2022, 10, 1), DateOnly.MaxValue),
                };

                // Act
                var result = sut.Subtract(new DateRange(new DateOnly(2022, 9, 11), new DateOnly(2022, 9, 30)));

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Expected_DateRanges_When_Overlap_Multiple_Days_And_From_Is_MinValue_And_To_Is_MaxValue()
            {
                // Arrange
                var from = DateOnly.MinValue;
                var to = DateOnly.MaxValue;

                var sut = new DateRange(from, to);

                var expectedResult = new[]
                {
                    new DateRange(DateOnly.MinValue, new DateOnly(2022, 9, 10)),
                    new DateRange(new DateOnly(2022, 10, 1), DateOnly.MaxValue),
                };

                // Act
                var result = sut.Subtract(new DateRange(new DateOnly(2022, 9, 11), new DateOnly(2022, 9, 30)));

                // Assert
                Assert.Equal(expectedResult, result);
            }
        }
    
        public class The_IntersectOverlappingDatesInRanges_Method
        {
            [Fact]
            public void Should_Return_Empty_When_No_Left_And_Right_Are_Empty()
            {
                // Arrange
                var left = Enumerable.Empty<DateRange>();
                var right = Enumerable.Empty<DateRange>();

                var expectedResult = Enumerable.Empty<DateRange>();

                // Act
                var result = DateRange.IntersectOverlappingDatesInRanges(left, right);

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Empty_When_Left_Is_Empty_And_Right_Not_Empty()
            {
                // Arrange
                var left = Enumerable.Empty<DateRange>();
                var right = new List<DateRange>
                {
                    new DateRange(DateOnly.MinValue, DateOnly.MaxValue),
                };
                var expectedResult = Enumerable.Empty<DateRange>();

                // Act
                var result = DateRange.IntersectOverlappingDatesInRanges(left, right);

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Empty_When_Left_Is_Not_Empty_And_Right_Is_Empty()
            {
                // Arrange
                var left = new List<DateRange>
                {
                    new DateRange(DateOnly.MinValue, DateOnly.MaxValue),
                };
                var right = Enumerable.Empty<DateRange>();

                var expectedResult = Enumerable.Empty<DateRange>();

                // Act
                var result = DateRange.IntersectOverlappingDatesInRanges(left, right);

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Expected_DateRange_With_Overlapping_Days_When_Left_And_Right_Contain_Same_DateRange()
            {
                // Arrange
                var left = new List<DateRange>
                {
                    new DateRange(new DateOnly(2022, 10, 20), new DateOnly(2022, 10, 25)),
                };
                var right = new List<DateRange>
                {
                    new DateRange(new DateOnly(2022, 10, 20), new DateOnly(2022, 10, 25)),
                };

                var expectedResult = new List<DateRange>
                {
                    new DateRange(new DateOnly(2022, 10, 20), new DateOnly(2022, 10, 25)),
                };

                // Act
                var result = DateRange.IntersectOverlappingDatesInRanges(left, right);

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Expected_DateRange_With_Overlapping_Days_When_Left_And_Right_Contain_Same_DateRange_And_Non_Intersecting_DateRange()
            {
                // Arrange
                var left = new List<DateRange>
                {
                    new DateRange(new DateOnly(2000, 10, 20), new DateOnly(2000, 10, 25)),
                    new DateRange(new DateOnly(2022, 10, 20), new DateOnly(2022, 10, 25)),
                };
                var right = new List<DateRange>
                {
                    new DateRange(new DateOnly(2022, 10, 20), new DateOnly(2022, 10, 25)),
                    new DateRange(new DateOnly(3000, 10, 20), new DateOnly(3000, 10, 25)),
                };

                var expectedResult = new List<DateRange>
                {
                    new DateRange(new DateOnly(2022, 10, 20), new DateOnly(2022, 10, 25)),
                };

                // Act
                var result = DateRange.IntersectOverlappingDatesInRanges(left, right);

                // Assert
                Assert.Equal(expectedResult, result);
            }

            [Fact]
            public void Should_Return_Expected_DateRanges_With_Overlapping_Days_When_Left_And_Right_Contain_Multiple_DateRanges_Where_Some_Match()
            {
                // Arrange
                var left = new List<DateRange>
                {
                    new DateRange(new DateOnly(2000, 10, 20), new DateOnly(2000, 10, 25)),
                    new DateRange(new DateOnly(2022, 10, 20), new DateOnly(2022, 10, 25)),
                    new DateRange(new DateOnly(2023, 10, 20), new DateOnly(2023, 10, 25)),
                };
                var right = new List<DateRange>
                {
                    new DateRange(new DateOnly(2022, 10, 20), new DateOnly(2022, 10, 25)),
                    new DateRange(new DateOnly(2023, 10, 10), new DateOnly(2023, 10, 30)),
                    new DateRange(new DateOnly(3000, 10, 20), new DateOnly(3000, 10, 25)),
                };

                var expectedResult = new List<DateRange>
                {
                    new DateRange(new DateOnly(2022, 10, 20), new DateOnly(2022, 10, 25)),
                    new DateRange(new DateOnly(2023, 10, 20), new DateOnly(2023, 10, 25)),
                };

                // Act
                var result = DateRange.IntersectOverlappingDatesInRanges(left, right);

                // Assert
                Assert.Equal(expectedResult, result);
            }
        }
    }
}
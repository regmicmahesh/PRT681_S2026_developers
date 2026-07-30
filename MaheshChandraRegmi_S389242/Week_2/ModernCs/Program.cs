int[] scores = [90, 71, 82, 93, 75, 82];

// Query Expression.
IEnumerable<int> scoreQuery =
    from score in scores
    where score > 80
    orderby score descending
    select score;

foreach (var testScore in scoreQuery)
{
    Console.WriteLine(testScore);
}

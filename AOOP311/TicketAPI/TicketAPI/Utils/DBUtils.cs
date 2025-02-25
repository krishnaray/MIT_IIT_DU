using MongoDB.Driver;

namespace Miths.Utils
{
    public static class DBUtils
    {
        public static async Task<(int totalPages, IReadOnlyList<TDocument> data)> AggregateByPage<TDocument>(this IMongoCollection<TDocument> collection,
            FilterDefinition<TDocument> filterDefinition, SortDefinition<TDocument> sortDefinition, int pageNumber, int contentCount)
        {
            var countFacet = AggregateFacet.Create("count",
                PipelineDefinition<TDocument, AggregateCountResult>.Create(new[]
                {PipelineStageDefinitionBuilder.Count<TDocument>()}));

            if(pageNumber < 1) pageNumber = 1;
            if(contentCount < 1) contentCount = 1;
            var dataFacet = AggregateFacet.Create("data",
                PipelineDefinition<TDocument, TDocument>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Sort(sortDefinition),
                    PipelineStageDefinitionBuilder.Skip<TDocument>((pageNumber - 1) * contentCount),
                    PipelineStageDefinitionBuilder.Limit<TDocument>(contentCount),
                }));


            var aggregation = await collection.Aggregate().Match(filterDefinition).Facet(countFacet, dataFacet).ToListAsync();

            var count = aggregation.First().Facets.First(x => x.Name == "count").Output<AggregateCountResult>()
                ?.FirstOrDefault()?.Count;
            var totalPages = 1;
            if(count is not null)
                totalPages = (int)Math.Ceiling((double)count / contentCount);

            var data = aggregation.First()
                .Facets.First(x => x.Name == "data")
                .Output<TDocument>();

            return (totalPages, data);
        }
    }
}

using Blog.Entities.DTO;
using Blog.Entities.Models;
using Blog.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Extensions
{
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services)
        {
            var url = "http://localhost:9200";
            var defaultIndex = "articles";

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex);

            AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton(client);

            CreateIndex(client, defaultIndex);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings
                .DefaultMappingFor<Article>(m => m
                .Ignore(p => p.Body)
                .Ignore(p => p.User)
                .Ignore(p => p.Votes)
            );
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<Article>(x => x.AutoMap())
                              .Map<ArticleDTO>(x => x.AutoMap())
            );
        }
    }
}

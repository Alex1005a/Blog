﻿using Blog.Domain;
using Blog.Models.DTO;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace Blog.Infrastructure.Extensions
{
    public static class ElasticsearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services)
        {
            var url = Environment.GetEnvironmentVariable("BONSAI_URL") ?? "http://localhost:9200";
            var defaultIndex = "articles";

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex).DisableDirectStreaming()
                                           .PrettyJson();

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

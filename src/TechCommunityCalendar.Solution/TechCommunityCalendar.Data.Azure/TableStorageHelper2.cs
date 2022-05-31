
using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTableStorageHelper.Library
{
    // https://docs.microsoft.com/en-us/azure/cosmos-db/table/create-table-dotnet?tabs=azure-portal%2Cvisual-studio

    public class TableService
    {
        private readonly TableClient _tableClient;

        public TableService(TableClient tableClient, string)
        {
            this._tableClient = tableClient;


        }
        public IEnumerable<T> GetAllRows<T>(Func<TableEntity, T> mapper)
        {
            Pageable<TableEntity> entities = _tableClient.Query<TableEntity>();

            return entities.Select(e => mapper(e));

            //return entities.Select(e => MapTableEntityToWeatherDataModel(e));
        }

        public IEnumerable<T> RetrieveManyByPartition<T>(string partitionKey, Func<TableEntity, T> mapper)
        {
            string filter = $"PartitionKey eq '{partitionKey}'";
            Pageable<TableEntity> entities = _tableClient.Query<TableEntity>(filter);

            return entities.Select(e => mapper(e));

        }

        public T RetrieveSingle<T>(string partitionKey, string rowKey, Func<TableEntity, T> mapper)
        {
            string filter = $"PartitionKey eq '{partitionKey}' and RowKey eq '{rowKey}'";
            Pageable<TableEntity> entities = _tableClient.Query<TableEntity>(filter);

            return entities.Select(e => mapper(e)).FirstOrDefault();
        }

        public void Insert<T>(T model, string partitionKey, string rowKey, Func<T, TableEntity> mapper)
        {
            TableEntity entity = mapper(model);



            _tableClient.AddEntity(entity);
        }

        public void Upsert<T>(T model, Func<T, TableEntity> mapper)
        {
            TableEntity entity = mapper(model);



            _tableClient.UpsertEntity(entity);
        }

    }

    public class TableStorageHelper2
    {
        private static TableClient GetTableClient(string connectionString, string tableName)
        {
            TableClient tableClient = new TableClient(connectionString, tableName);
            return tableClient;
        }

        /*
        public static async Task<TableResult> Insert(string connectionString, string tableName, TableEntity tableEntity, bool createIfNotExists = false)
        {
            var cloudTable = GetCloudTable(connectionString, tableName);

            if (createIfNotExists)
            {
                var cloudTableExists = await cloudTable.ExistsAsync();

                if (!cloudTableExists)
                    await cloudTable.CreateIfNotExistsAsync();
            }

            return await cloudTable.ExecuteAsync(TableOperation.Insert(tableEntity));
        }
        */
        /*
        public static async Task<IList<TableResult>> InsertBatch(string connectionString, string tableName, IEnumerable<TableEntity> tableEntities, bool createIfNotExists = false)
        {
            var cloudTable = GetCloudTable(connectionString, tableName);

            if (createIfNotExists)
            {
                var cloudTableExists = await cloudTable.ExistsAsync();

                if (!cloudTableExists)
                    await cloudTable.CreateIfNotExistsAsync();
            }

            TableBatchOperation batchOperation = new TableBatchOperation();

            foreach (var tableEntity in tableEntities)
                batchOperation.Insert(tableEntity);

            return await cloudTable.ExecuteBatchAsync(batchOperation);
        }
        */

        public static async Task<bool> TableExists(string connectionString, string cloudTableName)
        {
            /*
            var cloudTable = GetCloudTable(connectionString, cloudTableName);
            return await cloudTable.ExistsAsync();
            */

            return false;
        }

        public static async Task<TableResult> Update<T>(string connectionString, string tableName, TableEntity tableEntity) where T : TableEntity
        {
            /*
            var cloudTable = GetCloudTable(connectionString, tableName);
            var retrievedTableEntity = await RetrieveSingle<T>(connectionString, tableName, tableEntity.PartitionKey, tableEntity.RowKey);

            retrievedTableEntity = (T)tableEntity;

            TableOperation updateOperation = TableOperation.Replace(retrievedTableEntity);

            return await cloudTable.ExecuteAsync(updateOperation);
            */

            return null;
        }

        public static async Task<TableResult> Delete<T>(string connectionString, string tableName, string partitionKey, string rowKey) where T : TableEntity
        {
            /*
            var cloudTable = GetCloudTable(connectionString, tableName);
            TableResult retrievedResult = await cloudTable.ExecuteAsync(TableOperation.Retrieve<T>(partitionKey, rowKey));
            TableEntity deleteEntity = (T)retrievedResult.Result;

            if (deleteEntity == null)
                return null;

            TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

            return await cloudTable.ExecuteAsync(deleteOperation);
            */

            return null;
        }

        public static async Task<T> RetrieveSingle<T>(string connectionString, string tableName, string partitionKey, string rowKey) where T : TableEntity
        {
            /*
            var cloudTable = GetCloudTable(connectionString, tableName);
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult retrievedResult = await cloudTable.ExecuteAsync(retrieveOperation);

            if (retrievedResult.Result == null)
                return null;

            return (T)retrievedResult.Result;
            */

            return null;

        }

        public static async Task<IEnumerable<T>> RetrieveManyByPartition<T>(string connectionString, string tableName, string partitionKey)
        {
            string filter = "";
            Pageable<TableEntity> entities = _tableClient.Query<TableEntity>(filter);

            return entities.Select(e => MapTableEntityToWeatherDataModel(e));

            /*
            var cloudTable = GetCloudTable(connectionString, tableName);
            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            TableContinuationToken token = null;
            var entities = new List<T>();

            do
            {
                TableQuerySegment<T> resultSegment = await cloudTable.ExecuteQuerySegmentedAsync<T>(query, token);
                token = resultSegment.ContinuationToken;

                foreach (T entity in resultSegment.Results)
                {
                    entities.Add(entity);
                }
            }
            while (token != null);

            return entities;
            */

            return null;
        }

        public static async Task<bool> DropTable(string connectionString, string tableName)
        {
            /*
            var cloudTable = GetCloudTable(connectionString, tableName);
            return await cloudTable.DeleteIfExistsAsync();
            */

            return null;
        }
    }
}
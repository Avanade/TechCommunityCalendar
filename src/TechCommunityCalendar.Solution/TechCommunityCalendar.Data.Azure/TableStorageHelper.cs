using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureTableStorageHelper.Library
{
    public class TableStorageHelper
    {
        private static CloudTable GetCloudTable(string connectionstring, string tableName)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionstring);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            CloudTable cloudTable = cloudTableClient.GetTableReference(tableName);
            return cloudTable;
        }

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

        public static async Task<bool> TableExists(string connectionString, string cloudTableName)
        {
            var cloudTable = GetCloudTable(connectionString, cloudTableName);
            return await cloudTable.ExistsAsync();
        }

        public static async Task<TableResult> Update<T>(string connectionString, string tableName, TableEntity tableEntity) where T : TableEntity
        {
            var cloudTable = GetCloudTable(connectionString, tableName);
            var retrievedTableEntity = await RetrieveSingle<T>(connectionString, tableName, tableEntity.PartitionKey, tableEntity.RowKey);

            retrievedTableEntity = (T)tableEntity;

            TableOperation updateOperation = TableOperation.Replace(retrievedTableEntity);

            return await cloudTable.ExecuteAsync(updateOperation);
        }

        public static async Task<TableResult> Delete<T>(string connectionString, string tableName, string partitionKey, string rowKey) where T : TableEntity
        {
            var cloudTable = GetCloudTable(connectionString, tableName);
            TableResult retrievedResult = await cloudTable.ExecuteAsync(TableOperation.Retrieve<T>(partitionKey, rowKey));
            TableEntity deleteEntity = (T)retrievedResult.Result;

            if (deleteEntity == null)
                return null;

            TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

            return await cloudTable.ExecuteAsync(deleteOperation);
        }

        public static async Task<T> RetrieveSingle<T>(string connectionString, string tableName, string partitionKey, string rowKey) where T : TableEntity
        {
            var cloudTable = GetCloudTable(connectionString, tableName);
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult retrievedResult = await cloudTable.ExecuteAsync(retrieveOperation);

            if (retrievedResult.Result == null)
                return null;

            return (T)retrievedResult.Result;
        }

        public static async Task<IEnumerable<T>> RetrieveManyByPartition<T>(string connectionString, string tableName, string partitionKey) where T : TableEntity, new()
        {
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
        }

        public static async Task<bool> DropTable(string connectionString, string tableName)
        {
            var cloudTable = GetCloudTable(connectionString, tableName);
            return await cloudTable.DeleteIfExistsAsync();
        }
    }
}
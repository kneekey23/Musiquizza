using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Musiquizza_React
{
    public class SongService
    {

        private DynamoDBContext _dynamoContext;

        public SongService(IAmazonDynamoDB dynamoDbClient)
        {

            _dynamoContext = new DynamoDBContext(dynamoDbClient);
        }

        public async Task<Song> GetSong(int id)
        {
            
            Song song = await _dynamoContext.LoadAsync<Song>(id);
            return song;
        }

    }
}


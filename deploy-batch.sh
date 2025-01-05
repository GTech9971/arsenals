source .env
echo IP:$SERVER_IP
echo Port:$SERVER_PORT

scp -r -P $SERVER_PORT ./Arsenals.Infrastructure.Migration.Batch/bin/Release/net8.0/linux-x64/publish/ george@$SERVER_IP:/home/george/migration.batch.d
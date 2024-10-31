http://47.115.31.214:15672/api/index.html

```bash
# Get messages in queue
curl -u user:password -X POST -H "Content-Type: application/json" \
-d '{"count":1,"requeue": true,"encoding":"auto"}' \
http://localhost:15672/api/queues/%2F/{queueName}/get
```

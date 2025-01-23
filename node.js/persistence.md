# How to persist messages

- Declare **durable** exchanges
  ```javascript
  await channel.assertExchange(EXCHANGE_NAME, 'direct', { durable: true });
  ```
- Declare **durable** queues
  ```javascript
  await channel.assertQueue(QUEUE_NAME,  { durable: true });
  ```
- Push **persistent** messages
  ```javascript
  messages.forEach(msg => channel.publish(
    EXCHANGE_NAME,
    QUEUE_NAME,
    Buffer.from(JSON.stringify(msg)),
    { persistent: true }
  ));
  ```

Create RabbitMQ container with named **volume** and set **hostname**
- https://github.com/VIK2395/Rabbitmq/blob/master/CSharpProject/compose.yaml

This guarantees messages persistance across container recreation/deletion, ultill the volume is not deleted.

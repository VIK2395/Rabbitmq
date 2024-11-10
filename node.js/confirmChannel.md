Wait for confirms that all published messages were successfully delevered to RabbitMQ server.

```javascript
const connection = await amqp.connect(connectionString);

const channel = await connection.createChannel();
await channel.confirmSelect();
// or
const channel = await connection.createConfirmChannel();

channel.publish(exchangeName, routingKey, Buffer.from(message), {}, (err, ok) => {
  if (err) {
    return console.error('Message not confirmed by RabbitMQ server ', err?.toString());
  }
  console.log('Message confirmed by RabbitMQ server.');
});

await channel.waitForConfirms();
await channel.close();
await connection.close();
```

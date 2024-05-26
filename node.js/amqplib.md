__Create connection__
```javascript
const connection = await amqplib.connect(connectionString)
```

__Create channel__
```javascript
const channel = await connection.createChannel()
```

__Create exchange__
```javascript
await channel.assertExchange(exchangeName, exchangeType, exchangeOptions?)
```

__Create queue__
```javascript
await channel.assertQueue(queueName, queueOptions?)
```

__Bind queue__
```javascript
await channel.bindQueue(queueName, exchangeName, bindingKey, args?)
```

__Create consumer__
```javascript
await channel.consume(queueName, consumerFn, consumerOptions?)
```

```javascript
// Need to make consumerFn access the channel, with closure or so
consumerFn(msg) {
  const { content, properties, fields } = msg;
  const text = content.toString();
  const parsedContent = JSON.parse(text);
  ...
  channel.ack(msg, allUpTo?);
  // or
  channel.nack(msg, allUpTo?, requeue?);
}
```

__Publish msg__
```javascript
channel.publish(exchangeName, routingKey, msg, publishOptions?)
```

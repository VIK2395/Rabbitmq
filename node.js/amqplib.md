https://www.npmjs.com/package/amqplib

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

__Publish message__
```javascript
channel.publish(exchangeName, routingKey, msg, publishOptions?)
```
To default exchange
```javascript
channel.sendToQueue(queueName, msg, publishOptions?)
```

__Close channel__
```javascript
await channel.close()
```

__Close connection__
```javascript
await connection.close()
```
Closing a connection closes all its channels as well\
https://www.rabbitmq.com/docs/channels
![Screenshot from 2024-05-26 15-29-49](https://github.com/VIK2395/Rabbitmq/assets/50545334/81ca1532-824c-4bab-ace4-0c99dd949365)

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
No need to explicitly create the default exchange as it is always created by rabbitMq under the hood

__Create queue__
```javascript
await channel.assertQueue(queueName, queueOptions?)
```

__Bind queue__
```javascript
await channel.bindQueue(queueName, exchangeName, bindingKey, args?)
```
No need to do explicit bindings to the default exchange as all queues get bound to it under the hood

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
To default exchange (always has the direct type)
```javascript
channel.sendToQueue(queueName, msg, publishOptions?)
// or
channel.publish('', queueName, msg, publishOptions?)
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

__Notices__

Prefetch is set on channel
```javascript
channel.prefetch(1)
```
![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/3840b288-9774-4330-923d-96abe0ecc2ec)
https://amqp-node.github.io/amqplib/channel_api.html \
https://www.rabbitmq.com/docs/consumer-prefetch

We are not forbidden to crete multiple consumers per a channel. But is it a good practice?\
https://www.rabbitmq.com/docs/consumer-prefetch#independent-consumers


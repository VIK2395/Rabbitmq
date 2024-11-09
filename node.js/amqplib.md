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
const { exchange } = await channel.assertExchange(exchangeName, exchangeType, exchangeOptions?)
```
No need to explicitly create the default exchange as it is always created by rabbitMq under the hood

__Create queue__
```javascript
const { queue, messageCount, consumerCount } = await channel.assertQueue(queueName, queueOptions?)
```

__Bind queue__
```javascript
await channel.bindQueue(queueName, exchangeName, bindingKey, args?)
```
No need to do explicit bindings to the default exchange as all queues get bound to it under the hood

__Create consumer__
```javascript
const { consumerTag } = await channel.consume(queueName, consumerFn, consumerOptions?)
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
// where msg: Buffer.from(text)
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

__Prefetch__

- ___Channel Prefetch___
  ![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/3840b288-9774-4330-923d-96abe0ecc2ec)
  https://amqp-node.github.io/amqplib/channel_api.html#channel_prefetch

  ```javascript
  // Prefetch by channel
  // Each consumer on the channel is allowed to receive up to 5 unacknowledged messages at a time
  channel.prefetch(5)
  ```

- ___Consumer Prefetch___\
  https://www.rabbitmq.com/docs/consumer-prefetch

  ```javascript
  // Prefetch by consumer
  // In amqplib, the call below throws "channel.basicQos is not a function" error because no such method in amqplib.
  channel.basicQos(5);
  ```

In RabbitMQ, both `channel.basicQos(5)` and `channel.prefetch(5)` effectively set the **prefetch count** to 5, but the terms can sometimes cause confusion because of naming differences across client libraries. Here’s the breakdown:

1. **`channel.basicQos(5)`**:
   - This is the **AMQP protocol-level command** to set the QoS (Quality of Service) for message delivery, specifically setting the **prefetch count**.
   - It tells RabbitMQ to deliver a maximum of 5 unacknowledged messages to each consumer on the channel before waiting for acknowledgments.
   - This is the general approach in RabbitMQ’s **Java client** and many other clients that follow the AMQP standard closely.

2. **`channel.prefetch(5)`**:
   - This is the same concept but specific to certain client libraries, such as **`amqplib` in Node.js**, which uses `prefetch` as a more intuitive name for this setting.
   - **`channel.prefetch(5)` is effectively a wrapper around `basicQos(5)`, setting the maximum number of unacknowledged messages the consumer can handle at a time to 5.**

### Summary

- **Functionally Equivalent**: `channel.basicQos(5)` and `channel.prefetch(5)` both configure the prefetch limit to 5 unacknowledged messages per consumer.
- **Library-Specific Naming**: Some libraries expose it as `prefetch` for clarity, but they accomplish the same underlying QoS configuration.

We are not forbidden to crete multiple consumers per a channel. But is it a good practice?\
https://www.rabbitmq.com/docs/consumer-prefetch#independent-consumers

__Ack and nack__

Ack and nack are set on channel
```javascript
channel.ack(message: Message, allUpTo?: boolean): void;
channel.nack(message: Message, allUpTo?: boolean, requeue?: boolean): void;
```

__Dlx__

Dlx is set in assertQueue inside options.arguments. Queue-to-DlxEchange.
```javascript
channel.assertQueue(queue: string, options?: Options.AssertQueue): Promise<Replies.AssertQueue>;
````

__Alt__

Alt is set in assertExchange inside oprions.arguments. Exchange-to-AltExchange.
```javascript
channel.assertExchange(exchange: string, type: string, options?: Options.AssertExchange): Promise<Replies.AssertExchange>;
```

https://jstobigdata.com/rabbitmq/alternate-exchange-in-rabbitmq/

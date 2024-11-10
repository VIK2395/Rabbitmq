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

## Difference between publisher confirms and consumer confirms

**Publisher confirms** are different from the **consumer ack/nack** in RabbitMQ.

Here’s a breakdown of the differences:

### Publisher confirm:
- **Context**: Occurs between the **publisher** and RabbitMQ.
- **Purpose**: Confirms whether a message has been successfully received and processed by the broker (i.e., routed to the appropriate queues).
- **Types**:
  - **`confirm`**: The message was successfully received and processed by RabbitMQ.
  - **`Unconfirm`**: The message was not successfully processed (e.g., it was dropped or couldn't be routed to a queue). In this case, the publisher can choose to retry or log the failure.
  
### Consumer Acknowledgment (`ack`/`nack`):
- **Context**: Occurs between the **consumer** and RabbitMQ.
- **Purpose**: Confirms whether a **consumer** has successfully processed a message.
- **Types**:
  - **`ack`**: The consumer successfully processed the message, and RabbitMQ can safely remove it from the queue.
  - **`nack`**: The consumer did not process the message (e.g., due to a failure or timeout). The message can either be re-queued or discarded based on the consumer's configuration.

### Summary of the Key Differences:
| **Type**                | **Publisher `ack`/`nack`**                        | **Consumer `ack`/`nack`**                        |
|-------------------------|----------------------------------------------------|--------------------------------------------------|
| **Context**             | Publisher -> RabbitMQ                             | RabbitMQ -> Consumer                             |
| **Purpose**             | Confirms whether the message was successfully received by RabbitMQ | Confirms whether the consumer has successfully processed the message |
| **Actions**             | - `ack`: Message was received by RabbitMQ.        | - `ack`: Consumer has successfully processed the message. |
|                         | - `nack`: Message was not processed by RabbitMQ.  | - `nack`: Consumer failed to process the message. |
| **When it happens**     | After the message is published to RabbitMQ.       | After the message is received by the consumer.  |

## exclusive

```javascript
const queue = await channel.assertQueue('', { exclusive: true });
```
- Default `false`
- The queue is restricted to only one connection.
  - **Connection-specific:** Only the connection that created the queue can access it. Other connections won’t be able to consume messages from this queue.
  - **Automatic deletion:** When the connection that created the queue closes, the queue will be automatically deleted.
  - **Common use case:** This is often used for temporary, per-client queues, such as those created for direct response to specific clients in RPC-style messaging.
- The queue name is set to an empty string ('') to let RabbitMQ generate a unique name.

## noAck

```javascript
const { consumerTag } = await channel.consume('queue', (msg) => {}, { noAck: true });
```
- Default `false`
- Whether the consumer must acknowledge each message received.
  - When `false`, the consumer must explicitly acknowledge each message (typically by calling `channel.ack(message)` in the code).
  - When `true`, messages received will not require acknowledgment, so they are immediately marked as delivered and will not be re-queued if the consumer fails.

# Exchange options

## durable

```javascript
const { exchange } = await channel.assertExchange(exchangeName, exchangeType, { durable: true })
```
- Default `false`
- Whether the exchange will persist across RabbitMQ broker restarts.

# Queue options

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

## durable

```javascript
channel.assertQueue(queue, { durable: true });
```
- Default `false`
- Whether the queue will persist across RabbitMQ broker restarts.
  - This does not mean that messages in the queue are also persisted. Messages must be marked as persistent separately.
  - `false` is suitable for short-lived or temporary queues that don’t need to persist beyond the current broker session.
 
## autoDelete

```javascript
const queue = await channel.assertQueue('queue', { autoDelete: true });
```
- Default `false`
- Whether the queue will automatically be deleted when the last consumer unsubscribes.
- `durable: true` and `autoDelete: true` make the queue persistent across broker restarts but still deleted after the last consumer disconnects.

## arguments['x-message-ttl']

```javascript
const queue = await channel.assertQueue('queue', {arguments: { 'x-message-ttl': 5000 } });
```
- Default `unset`; meaning messages will remain in the queue indefinitely.
- How long in ms, messages remain in the queue.

 await channel.assertQueue(queue, {
        durable: true,
        arguments: { 'x-message-ttl': 5000 }  // Message TTL in milliseconds
    });

# Consumer options

## noAck

```javascript
const { consumerTag } = await channel.consume('queue', (msg) => {}, { noAck: true });
```
- Default `false`
- Whether the consumer must acknowledge each message received.
  - When `false`, the consumer must explicitly acknowledge each message (typically by calling `channel.ack(message)` in the code).
  - When `true`, messages received will not require acknowledgment, so they are immediately marked as delivered and will not be re-queued if the consumer fails.

## prefetch

```javascript
channel.prefetch(1);
```
- Default `unset`; meaning RabbitMQ to send as many messages as it can to a consumer until memory or network limitations are reached.
- How many unacknowledged messages, each consumer on the channel is allowed to receive at a time
 
# Publisher options

## persistent

```javascript
channel.publish(exchangeName, routingKey, Buffer.from(msg), { persistent: true });
```
- Default `false`
- Whether the message will persist across RabbitMQ broker restarts.
- Persistent messages will not be saved unless the queue itself is durable.

# What happens when `await channel.close()` with messages bieng curently in consumer?

When you call `await channel.close()` in RabbitMQ while there are messages currently being processed by a consumer, the following will happen:

1. **Active Consumers Stop Receiving Messages**:  
   - The `channel.close()` operation will stop the channel, which effectively halts any further message delivery to the consumers associated with that channel.

2. **Pending Acknowledgments**:  
   - If the consumer is in the middle of processing messages and has not yet acknowledged them (`ack`), the unacknowledged messages will be requeued back to the queue.
   - These unacknowledged messages will be eligible for redelivery to another active consumer, or to the same consumer if it reconnects later (depending on the queue's configuration, such as `auto-delete` or `exclusive`).

# When a message gets to consumer, is messages count in queue descreases immediatly or only after ack/nack?

In RabbitMQ, the message count in the queue **decreases immediately** when a message is delivered to a consumer, not when it is acknowledged (`ack`) or negatively acknowledged (`nack`). Here's the detailed behavior:

---

### **1. When the Message is Delivered**
- RabbitMQ considers the message "in-flight" as soon as it is sent to a consumer.
- The message is no longer part of the queue's count because it has left the queue and is waiting for the consumer to process it.
- If you check the queue's message count at this point (e.g., via the RabbitMQ Management UI or API), you will see the count decrease.

---

### **2. What Happens After Delivery**
- The message remains in the "in-flight" state until the consumer sends an acknowledgment (`ack`) or negative acknowledgment (`nack`).
- If the message is acknowledged (`ack`), it is removed permanently and considered processed.
- If the message is negatively acknowledged (`nack`) or the consumer disconnects without acknowledging:
  - Depending on your `autoAck` and queue settings, the message can be **requeued** (returned to the queue for redelivery) or **discarded**.

---

### **3. Relationship Between Queue Depth and In-Flight Messages**
- RabbitMQ distinguishes between:
  - **Ready messages**: Messages that are still in the queue, waiting to be delivered to a consumer.
  - **Unacknowledged messages**: Messages that have been delivered to a consumer but are waiting for acknowledgment.
  
  You can view these counts in the RabbitMQ Management UI:
  - **Ready**: Messages that are still in the queue.
  - **Unacked**: Messages that are in-flight but not yet acknowledged.

#  If consumer has not nacked the mesage, it won't get to DLX, but will be requeued, even though there is DLX configured?

If the consumer **does not negatively acknowledge** (`nack`) the message, and instead simply doesn't acknowledge it (i.e., the channel is closed with the message still unacknowledged), **the message will be requeued** rather than being sent to the dead-letter exchange (DLX), even if a DLX is configured.

Here's a more detailed breakdown:

### **How DLX Works**
- **Dead-letter exchanges (DLX)** are used to handle messages that cannot be processed normally. They typically come into play under conditions like:
  - A message is explicitly **nacked** with `requeue=false`.
  - A message **expires** (based on TTL settings).
  - The queue is **full** and the message cannot be delivered.

However, if the consumer does **not explicitly nack** or reject the message with `requeue=false`, and the message is still unacknowledged when the channel is closed, RabbitMQ will not automatically treat the message as "failed" or "dead." Instead:
- The message will be **requeued** to the same queue, where it can be redelivered to another consumer (or the same consumer if it reconnects).
  
### **Why DLX Is Not Triggered in This Case**
- **DLX is not triggered just because the channel is closed**. The message must be rejected, expired, or otherwise considered "failed" based on the queue’s settings.
- A message is only dead-lettered if it's explicitly rejected (`nack` with `requeue=false`), or if it meets other criteria (like TTL expiration or queue overflow).

---

### **Example Scenario:**

1. You have a queue `myQueue` with a DLX configured (`x-dead-letter-exchange`).
2. A consumer starts consuming messages from `myQueue`.
3. The consumer has not yet acknowledged the message, and before it does, the channel is closed (either due to an error, shutdown, or some other reason).
4. Since the message is still unacknowledged and the consumer hasn't nacked it, **the message is requeued** and made available for delivery again.
5. The message does **not go to the DLX** unless:
   - The message expires (based on TTL).
   - The consumer explicitly nacks it with `requeue=false`.
   - The queue has reached its maximum length and the message is rejected.

---

### **Conclusion**
If a consumer has not acknowledged (neither `ack` nor `nack`), and the channel is closed, **the message will be requeued** for redelivery to another consumer. **DLX will not be triggered** in this case unless there are other conditions (like TTL expiration or explicit rejection of the message).

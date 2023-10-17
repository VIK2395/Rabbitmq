1. https://www.youtube.com/playlist?list=PLalrWAGybpB-UHbRDhFsBgXJM1g6T4IvO
2. https://www.rabbitmq.com/getstarted.html

4b in 1* is "Hello world!" example in 2*. Just receive/consume a messages without a response;
If the message was not consummed by the consumer, it will wait in the queue.

We can never publish a message directly to the queue; it has to pass through an exchange.
Bindings - queues tied to exchnges.

Every producer or consumer should open a single tcp connection to our rebbitmq broker.
The connection can have multiple channels. By using a connection with multiple channels,
a producer might be able to produce and push messages to the broker using different threads.
But because each thread uses a different channel these messages are isolated from one another.
By using channels and not opening multiple connections, we can save a lot of resources. The same is true for consumers.

Exchange knows what to do with the message;

__Routing key__ "binds/routes" producers to exchnges;\
__Binding key__ binds exchnges to queues;

Exchange types:
- Direct (uses routing keys to push/route messages selectively to queues which has exact same binding key);
- Topic (uses routing key "dot" pattern as the routing keys; uses spesial pattern with charachers (* - any __one word__, # - 0 or more words) as binding keys; __checks for a match of routing key and binding key__);
- Fanout (broadcasts/pushes all the messages it receives to all the queues it knows);
- Headers;
- Default (exchange with name as empty string);
- Dead Letter;
- Alternate.

Messaging patterns:
- __Competing consumers (Work queue)__; By default, RabbitMQ sends messages in round-robin manner;\
  The idea is to have multiple __same__ consumers and distribute messages between them so that the idle consumers receive/process the messages;\
  https://medium.com/event-driven-utopia/competing-consumers-pattern-explained-b338d54eff2b \
  7 and 8b in 2*

  ![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/6fd6a5e4-5a52-4d44-bb14-e89bce803002)


- __Publish/Subscribe__;\
  The idea is to send the same message to multiple __different__ consumes. RabbitMQ doesn't store multiple message copies, it stores the original message and the queues store just the reference to the message.\
  In this pattern we can use "temporary" queues, so we don't have to explicitly declare the queues upfront. The consumers will take care of creating these temporary queues.\
  9 and 10b in 2*
  
  ![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/ff73c551-5854-4f76-a8f9-daf5e30296be)

- __Request-reply__


Routing using direct exchange and topic exchange:\
  11 and 12b in 2*

![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/16815790-29cf-4094-bffa-dff1742dca47)

![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/2df0f062-fec7-4e20-b4ad-ff6004430a0f)

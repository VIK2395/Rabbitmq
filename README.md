![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/42f2eac2-7880-41a5-a74a-dd2f06d81001)

![Screenshot from 2024-05-23 19-40-57](https://github.com/VIK2395/Rabbitmq/assets/50545334/51da38b1-716e-4e5e-9033-c511b2b60a98)

1. https://www.youtube.com/playlist?list=PLalrWAGybpB-UHbRDhFsBgXJM1g6T4IvO
2. https://www.rabbitmq.com/getstarted.html

https://github.com/delaneybrian/jumpstartCS-rabbitmq-csharp/tree/master

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

==============================================================================

Bindings are constrains established between queues and exchanges.

Routing key (BasicPublish) => while publishing a message to an exchange.\
Binding key (QueueBind) => while routing the message from the exchange to a appropriate queue.

Producer publishes to a specific exchange name and passes __routing key__; Based on match __routing key__ and __binding key__, a message gets to a spesific queue;

![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/834b064b-b7dd-4ba6-8a0d-04725c3ec236)

==============================================================================

__Exchange types:__\
https://www.rabbitmq.com/tutorials/amqp-concepts.html#:~:text=The%20default%20exchange%20is%20a,same%20as%20the%20queue%20name \
RabbitMq has 4 exchange types by default;

- Direct (uses routing keys to push/route messages selectively to queues which has exact same binding key);
- Topic (uses routing key "dot" pattern as the routing keys; uses spesial pattern with charachers (* - any __one word__, # - 0 or more words) as binding keys; __checks for a match of routing key and binding key__);
- Fanout (broadcasts/pushes all the messages it receives to all the queues it knows; ignores routing/binding keys);
- Headers (direct exchange on steroids; ignores the routing key attribute; x-match headers are used to route messages);

  ![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/51cf7469-eea4-46a0-bd9e-2464cdf66e99)
  
- Default (__direct exchange__ with no name (empty string) pre-declared by the broker; we don't have to declare this exchange explicitly. Every queue that is created is automatically bound to it with a routing key which is the same as the queue name);
https://www.rabbitmq.com/tutorials/amqp-concepts#exchange-default
  
  ![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/ff77c011-62dc-458f-9872-b85fad962a8f)

- Consistent Hashing (plugin installation needed) (kinda competing consumers pattern but with more options).

  ![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/a61f4305-3536-4f31-a941-606597476874)

- Alternate (can be any exchange type; exchange to route messages that have not match any routing key - binding key);

  ![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/09dd0299-cd8f-413c-8429-3d66da93bea1)
  
- Dead Letter (can be any exchange type; exchange to route expired or redjected messages);

  ![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/9193cc60-092d-4f1e-9909-671b61a4c47b)


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

- __Request-Reply__\
  The idea is simle, organaze a response/reply to a request.\
  Sending/publishing a request, the client needs to spesify its reply queue name which it listens to; with extra property __ReplyTo__ = QueueName;\
  And to know which reply is for which request, we can also use metadata __CorrelationId__/__MessageId__;\
  13 and 14b in 2*

  ![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/fb07639c-ee67-4358-998d-0db7552fd810)



Routing using direct exchange and topic exchange:\
  11 and 12b in 2*

![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/16815790-29cf-4094-bffa-dff1742dca47)

![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/2df0f062-fec7-4e20-b4ad-ff6004430a0f)

Routing using Exchange to Exchange routing (using Exchange to Exchange binding):\
  15 and 16b in 2*

![image](https://github.com/VIK2395/Rabbitmq/assets/50545334/12295cb5-fcab-40e6-85a0-163bef716819)

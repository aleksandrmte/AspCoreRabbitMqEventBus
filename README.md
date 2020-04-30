An example of using RabbitMQ to transfer messages between microservices (using asp core + dependency injection)

Error: 
Caused by: com.rabbitmq.client.AuthenticationFailureException: ACCESS_REFUSED - Login was refused using authentication mechanism PLAIN.
Solution:
I logged in to rabbitMQ admin page (http://localhost:15672/#/users) with the default user name and password which is guest/guest then added a new user and for that new user I enabled the permission to access it from virtual host and then used the new user name and password instead of default guest and that cleared the error.

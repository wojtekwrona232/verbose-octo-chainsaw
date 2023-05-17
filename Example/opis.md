Projekt `EmailConsumer` zawiera konsumera, który wysyła wiadomości email.
typ określony w obiekcie jest to tylko SmtpClient z wykorzystaniem MimeKit, można dodać więcej typów, ponieważ jest to enum,
dodatkowo trzeba dodać te opcję w switchu, który konsumuje same wiadomości. Żeby wiadmość została wysłana, należy a pliku `appsettings.Development.json` wpisać poprawne dane w sekcji `MailKit`. W celach demonstracyjnych zawartość wiadomości jest wypisana w konsoli.

Projekt `EmailProducer` zawiera producenta wiadomości email, za każdym razem, 
gdy odświeżymy stornę `/` zostanie wysłana nowa wiadomość email (wiadomość jest statycznie zapisana w kodzie w celach pokazania działania funkcji)

Projekt `SharedLibrary` zawiera wspólny kod, 
jest to głownie sama zawartość wiadomości, 
który jest wysłana przez RabbitMQ, oraz generic interface IProducer,
po którym możemy dziedziczyć i dodawać do niego dodatkową logikę, dodatkowo jest tutaj klasa odpowiadjąca za połączenie z Rabbitem i samo wysłanie wiadomości

RabbitMQ był uruchomiony lokalnie na Dockerze przy pomocy komendy `docker run -d --hostname my-rabbit --name some-rabbit -p 8080:15672 -p 5672:5672 rabbitmq:3-management`

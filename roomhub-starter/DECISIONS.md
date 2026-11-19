# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved
Pick one business rule (R1–R8). In which file did it live in the starter and in which file does it live now? Why there?

R8 , initial era in Models/Booking si cum clasa avea caracter anemic acum anularea rezervarii se poate face doar din interiorul clasei prin metodele create sau cancel 

## 2. Result instead of an exception
Why is `Result` a better choice than an exception for "the room is occupied"? What happens in your code when an `Error` is returned?

Result e mult mai bun pentru ca atunci cand arunc o exceptie se consuma ff multa memorie pentru ca trebuie sa faca tot stack trace ul

## 3. Who knows about EF Core?
Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?

Doar din Infrastructure si evdeint Program.cs iar restul din domain trebuie sa ramana izolate fata de baza de date 

## 4. The flow of a request
Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

_(your answer here)_

## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

Imi dadea o combinatie de arhitectura de tip controllere si dupa unele din ele le separa pe operatiuni unde erau definite clar command , handler , endpoitn , evident acolo unde aveam controllere am refuzat complet

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?

In primul rand as adauga versionare api pentru un mai bun management al api urilor daca dorim sa imbogatim arhitectura 

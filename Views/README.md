# SchoolPlanner
ASP.MVC .Net Core web application for planning school timetables

## Table of contents
* [About](#about)
* [Screenshots](#screenshots)
* [Technologies](#technologies)
* [Features](#features)
* [To-do](#to-do)

## About
Project made for the subject RAD Tools at the Warsaw University of Technology. 

## Screenshots
![Screenshot1](./img/screenshot1.png)
![Screenshot2](./img/screenshot2.png)
![Screenshot3](./img/screenshot3.png)
![Screenshot4](./img/screenshot4.png)
![Screenshot5](./img/screenshot5.png)
![Screenshot6](./img/screenshot6.png)
![Screenshot7](./img/screenshot7.png)
![Screenshot8](./img/screenshot8.png)
![Screenshot9](./img/screenshot9.png)

## Technologies
* C#
* ASP.MVC
* .Net Core
* Bootstrap
* MySQL
* Entity Framework
* Razor
* CSS
* HTML


## Features

* The user can select a classroom or teacher or class for which the data is presented.
* The user can click an entry (filled or empty) to add/edit/delete a lesson .
* The application only presents data that can be saved (e.g. when a class already has classes, it is not on the list).
* The application allows the user to save only non-conflicting data (e.g. the same class in different rooms at the same time).
* The application allows to modify classrooms, subjects, classes and teachers.
* It is not possible to delete e.g. a teacher who already has classes associated.
* Writing to the database includes overwriting verification - you cannot overwrite information that the user has not seen.


## To-do:
* Sharing views between screens

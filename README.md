Aim:

To develop an automated attendance marking scheme which would be used to solve the following problems of 
taking attendance during class:

1. Attendance, when taken by calling out students' names costs valuable class time.

2. If an attendance register is passed around, having direct access to such register without supervision 
has a well-exploited risk of giving "proxy" attendance.

3.  A biometric device's non-automated procedure is a distraction during class.  Sometimes, it also fails to read the 
fingerprint of particular students (even after updating its database).

Solution:

This application(Attendo) aims at solving the above problems through the use of Microsoft Visual API.

Attendo works by using face recognition API and SQl database (as provided by Microsoft).

Ideally, this app is to be fully automated.  Lack of proper hardware however, prevents this application.
A manual mode has been developed for this app for the sake of POC(proof of concept).

How to install:

1. Open Add-AppDevPackage using windows Powershell.

2.  Say yes to everything and you are done.

(We have cleared the database, so any insertion can be made).

Initially, photos of the class need to be given as input.  The database needs to be updated with the required people
inserted in the database.  

Manual mode:

There are a total of 5 buttons: "Add", "Help", "Detect", "Show", "Delete".

Help:

It will contain how to operate this app.
It is essentially a summary of this README file.  It is advised to read it before using any buttons.

Add:

This is used to add a person with a roll number b/w 1 to 10(this is a sample version for a class with 10 people).
Write the roll number of the person you want to add in the text box given and select an image using ImagePicker.
That image will be used to detect that person.  Many photos of the same person can be added to the given roll number.
Text box will show "succeeded" when photo is successfully added.

Detect:

It is used to detect the faces in an uploaded photo.  It will show the roll no. of person(s) detected in the textbox.
It will also show the face rectangle on the uploaded photo.  It will mark the attendance on the date on which the photo
is uploaded.  (Date cannot be uploaded, changed or tampered with).

(There is a known bug: If photo is in portrait mode, it will run correctly but the face rectangle will be in landscape mode.)

Show:

It takes a given date as input and show status on that date. (Default value is "absent" i.e. if a photo was not uploaded on
a given date, it will show all students to be absent.  Same applies to the case that there was no class on the input date.)

Delete:

It deletes a person from the data base.

(There is a known bug:  It doesnt delete the faces of that person or delete him from SQL database.  His data in previous classes
will be preserved though.  His roll number can be used again for another person.  His undeleted faces will not be linked to the newly 
added person with the same roll number as he had before he was deleted.)


-Team Nemesis

# Attendo

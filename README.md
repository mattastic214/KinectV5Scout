# KinectV5Scout
A Kinect Camera application to analyze body movement. This project leverages asynchronous and event-driven programming concepts.


## Contents:
[Features](#features)  
[Main Window](#main-window)  
[Kinect Database](#kinect-database)  
[Kinect Constants BGRA](kinect-constants-bgra)  

## Features
### Track movement data of default found person (developing for 6)
### Store tracking data to disk
### Record camera stream to disk
- _BodyIndex (Kinectv2 Reference)_
- _Infrared_
- _Skeletal, against a black background_

## Main Window
Contains the presentation XAML and C# logic to handle and present camera frames from the Kinect camera.

## Kinect Database
A library that fetches frame data from the Main Window camera stream and stores it to disk.

- _DatabaseController_  
Fetches frame data from the camera stream  

- _DataBaseAccess_  
Writes data to file/disk  

## Kinect Constants BGRA
